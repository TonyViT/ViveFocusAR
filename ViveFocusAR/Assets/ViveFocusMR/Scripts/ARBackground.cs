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

//The code is a modification of the one of the Vive Wave SDK samples, that was released under the following license:
// © 2017 HTC Corporation. All Rights Reserved.
//
// Unless otherwise required by copyright law and practice,
// upon the execution of HTC SDK license agreement,
// HTC grants you access to and use of the WaveVR SDK(s).
// You shall fully comply with all of HTC’s SDK license agreement terms and
// conditions signed by you and all SDK and API requirements,
// specifications, and documentation provided by HTC to You."

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wvr;
using WaveVR_Log;

/// <summary>
/// Shows the background of what the Focus cameras are seeing so that it is possible to create AR applications
/// </summary>
public class ARBackground : MonoBehaviour
{
    private bool updated = true;
    private bool started = false;
    private Texture2D nativeTexture = null;
    private System.IntPtr textureid;

    private Transform m_quadL, m_quadR;
    private Transform m_eyeL, m_eyeR; 

    /// <summary>
    /// Start method
    /// </summary>
    private void Awake()
    {
        //get references to useful objects
        m_quadL = transform.Find("Quad L");
        m_quadR = transform.Find("Quad R");

        m_eyeL = GameObject.Find("Eye Left").transform;
        m_eyeR = GameObject.Find("Eye Right").transform;

        m_quadR.localScale = new Vector3(33.0f, -20.0f, 23.5f);
        m_quadL.localScale = new Vector3(33.0f, -20.0f, 23.5f);
    }

    /// <summary>
    /// Update method
    /// </summary>
    private void Update()
    {        
#if !UNITY_EDITOR
        //if we have a new texture from the camera
        if (updated && started)
        {
            //get it
            updated = false;
            WaveVR_CameraTexture.instance.updateTexture((uint)textureid);        

            //move the quads, so that they are now in front of the eyes of the user
            m_quadR.SetPositionAndRotation(m_eyeR.TransformPoint(new Vector3(0, -2.0f, 27.0f)), m_eyeR.transform.rotation);
            m_quadL.SetPositionAndRotation(m_eyeL.TransformPoint(new Vector3(0, -2.0f, 27.0f)), m_eyeL.transform.rotation);         
        }       

#endif
    }

    /// <summary>
    /// Method called when the system has obtained a new image from the Focus cameras
    /// </summary>
    /// <param name="textureId"></param>
    void updateTextureCompleted(uint textureId)
    {
        updated = true;        
    }

    /// <summary>
    /// On Enable
    /// </summary>
    void OnEnable()
    {
#if !UNITY_EDITOR
        if(!started)
        {
            //initialize all the stuff for getting the cameras stream
            nativeTexture = new Texture2D(1280, 400);
            WaveVR_CameraTexture.UpdateCameraCompletedDelegate += updateTextureCompleted;
            started = WaveVR_CameraTexture.instance.startCamera();            
            textureid = nativeTexture.GetNativeTexturePtr();
            m_quadL.GetComponent<Renderer>().sharedMaterial.mainTexture = nativeTexture;
            m_quadR.GetComponent<Renderer>().sharedMaterial.mainTexture = nativeTexture;
        }
#endif
    }

    /// <summary>
    /// On Disable
    /// </summary>
    void OnDisable()
    {
#if !UNITY_EDITOR      
        
        if(started)
        {
            //stop listening to the cameras stream
            WaveVR_CameraTexture.instance.stopCamera();
            WaveVR_CameraTexture.UpdateCameraCompletedDelegate -= updateTextureCompleted;

            started = false;
        }
#endif
    }

    /// <summary>
    /// On application pause
    /// </summary>
    /// <param name="pauseStatus"></param>
    private void OnApplicationPause(bool pauseStatus)
    {
#if !UNITY_EDITOR
        //if the application gets paused (the user has removed the headset), stop everything, otherwise the program freezes
        if (pauseStatus)
        {
            this.OnDisable();
        }
        else
        {
            this.OnEnable();
        }
#endif
    }        
}
