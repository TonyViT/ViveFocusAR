//MIT License

//Copyright(c) 2018-2019 Antony Vitillo(a.k.a. "Skarredghost")

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Request Android permissions
/// </summary>
public class RequestPermissions : MonoBehaviour
{
    /// <summary>
    /// The scene to go when the permissions have been obtained
    /// </summary>
    public string ToGoScene;

    /// <summary>
    /// Is the new scene additive or not?
    /// </summary>
    public LoadSceneMode ToGoSceneMode = LoadSceneMode.Single;

    /// <summary>
    /// Instance of the permissions manager
    /// </summary>
    private WaveVR_PermissionManager pmInstance = null;

    private void Awake()
    {
        //if no to-go scene has been specified, get the n.1 of scenes build list
        if (string.IsNullOrEmpty(ToGoScene))
        {
            string pathToScene = SceneUtility.GetScenePathByBuildIndex(1);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(pathToScene);
            ToGoScene = sceneName;
        }
    }

    private void Start()
    {
        //call the permission manager only if we're running the application on an Android device (the Focus), otherwise just go the main scene of the game
        if (Application.platform != RuntimePlatform.Android)
        {
            GoToMainScene();
        }
        else
        {
            pmInstance = WaveVR_PermissionManager.instance;//get the permission manger
            StartCoroutine(PermissionManagerWait());
        }
    }

    /// <summary>
    /// Waits the permission manager to be ready and then asks for a permission
    /// </summary>
    /// <returns></returns>
    private IEnumerator PermissionManagerWait()
    {
        //list of the permissions necessary for this application. Here i just need the microphone
        string[] tmpStr =
        {
            "android.permission.CAMERA",
            "android.permission.WRITE_EXTERNAL_STORAGE"
        };

        //wait for the permissions manager to be initialized, without blocking the app
        while (!pmInstance.isInitialized())
        {
            yield return new WaitForSeconds(0.33f);
        }

        //Major function to request permission
        pmInstance.requestPermissions(tmpStr, requestDoneCallback);
    }

    /// <summary>
    /// Callback called when the permissions manager has had the results of its requests
    /// </summary>
    /// <param name="results"></param>
    private void requestDoneCallback(List<WaveVR_PermissionManager.RequestResult> results)
    {
        //if the user has granted the mic request, go to main scene, otherwise log the failure
        if (results[0].Granted)
            UnityMainThreadDispatcher.Instance().Enqueue(() => { GoToMainScene(); });
        else
            UnityMainThreadDispatcher.Instance().Enqueue(() => { Debug.Log("WTF user"); });
    }

    /// <summary>
    /// Go to the game main scene
    /// </summary>
    void GoToMainScene()
    {
        SceneManager.LoadSceneAsync(ToGoScene, ToGoSceneMode); //we're ready for the real game action
    }
}