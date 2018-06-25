//MIT License

//Copyright(c) 2018 Antony Vitillo(a.k.a. "Skarredghost")

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
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ARBackground : MonoBehaviour {
    /// <summary>
    /// Proportional factor of the camera stream dimension (height/width), used to calculate a quad showing images with no warpings
    /// </summary>
    const float factor = 400.0f / 640.0f;

    /// <summary>
    /// Zoom factor with which you want the camera stream to be shown on the screen
    /// </summary>
    [Tooltip("Zoom factor with which you want the camera stream to be shown on the screen. A bigger zoom fills field of view of the user, but alters the depth perception")]
    public float zoom = 1.0f;

    private Mesh meshL, meshR;
    public Material material;
    private bool updated = true;
    private bool started = false;
    Texture2D nativeTexture = null;
    System.IntPtr textureid;
    private MeshRenderer meshrenderer;

    // Use this for initialization
    void Start()
    {
#if !UNITY_EDITOR
        meshL = CreateMesh(true);
        meshR = CreateMesh(false);    
#endif        
    }

    void Update()
    {
#if !UNITY_EDITOR
        if (updated && started)
        {
            updated = false;
            WaveVR_CameraTexture.instance.updateTexture((uint)textureid);
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
        material.mainTexture = nativeTexture;
    }

    /// <summary>
    /// Creates the mesh to show the camera stream for both eyes.
    /// At the moment it is a quad at 0.5m from the users eyes
    /// </summary>
    /// <param name="isLeft">True if we are creating the mesh for the left eye, false otherwise</param>
    /// <returns></returns>
    private Mesh CreateMesh(bool isLeft)
    {
        //calculate the mesh vertices coordinates, that is basically a quad at 0.5m from the users eyes. It will appear behind other objects in the scene thanks to the way we render it (through events)

        // ab
        // cd
        List<Vector3> vertices = new List<Vector3>();   

        vertices.Add(new Vector3(-1 * zoom, -factor * zoom, 0.5f)); // a
        vertices.Add(new Vector3(1 * zoom, -factor * zoom, 0.5f)); // b
        vertices.Add(new Vector3(-1 * zoom, factor * zoom, 0.5f)); // c
        vertices.Add(new Vector3(1 * zoom, factor * zoom, 0.5f)); // d              

        //calculate the UVs so that the texture with the camera stream from both eyes gets rendered correctly on the quad for the left and right eye

        List<Vector2> uvsL = new List<Vector2>();
        uvsL.Add(new Vector2(1, 1));
        uvsL.Add(new Vector2(0.5f, 1));
        uvsL.Add(new Vector2(1, 0));
        uvsL.Add(new Vector2(0.5f, 0));

        List<Vector2> uvsR = new List<Vector2>();
        uvsR.Add(new Vector2(0.5f, 1));
        uvsR.Add(new Vector2(0, 1));
        uvsR.Add(new Vector2(0.5f, 0));
        uvsR.Add(new Vector2(0, 0));

        //create the mesh and assign the UVs so that the quad for this eye will show the correct part of the texture with the stream relative to this eye.
        //Notice that the system doesn't give us a texture for the left and one for the right eye, but it gives us only one texture with images for both
        //eyes side-to-side. This is why we are messing around with UVs

        List<int> indices = new List<int>();
        indices.Add(0);
        indices.Add(2);
        indices.Add(1);

        indices.Add(1);
        indices.Add(2);
        indices.Add(3);

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        if (isLeft)
            mesh.SetUVs(0, uvsL);
        else
            mesh.SetUVs(0, uvsR);

        mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);

        //return the mesh with the right UVs
        return mesh;
    }

    void OnEnable()
    {
#if !UNITY_EDITOR
        //listen to rendering events and render left and right eye when requested.
        //We don't use traditional rendering methods (a quad in a scene with a material), but we intercept the event about rendering and 
        //explicitly render the quad from code: this way the object appears behind all the other objects of the scene
        WaveVR_Utils.Event.Listen(WaveVR_Utils.Event.RENDER_OBJECT_LEFT, RenderLeft);
        WaveVR_Utils.Event.Listen(WaveVR_Utils.Event.RENDER_OBJECT_RIGHT, RenderRight);

        //initialize all the stuff for getting the cameras stream
        WaveVR_CameraTexture.UpdateCameraCompletedDelegate += updateTextureCompleted;
        started = WaveVR_CameraTexture.instance.startCamera();
        nativeTexture = new Texture2D(1280, 400);
        textureid = nativeTexture.GetNativeTexturePtr();
        meshrenderer = GetComponent<MeshRenderer>();
        meshrenderer.material.mainTexture = nativeTexture;
#endif
    }

    void OnDisable()
    {
#if !UNITY_EDITOR
        //stop listening to rendering events
        WaveVR_Utils.Event.Remove(WaveVR_Utils.Event.RENDER_OBJECT_LEFT, RenderLeft);
        WaveVR_Utils.Event.Remove(WaveVR_Utils.Event.RENDER_OBJECT_RIGHT, RenderRight);

        //stop listening to the cameras stream
        WaveVR_CameraTexture.instance.stopCamera();
        WaveVR_CameraTexture.UpdateCameraCompletedDelegate -= updateTextureCompleted;

        started = false;
#endif
    }

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

    public void RenderLeft(params object[] args)
    {
        //render left camera stream
        if (material != null)
        {
            material.SetPass(0);
            Graphics.DrawMeshNow(meshL, Matrix4x4.identity);
        }
    }

    public void RenderRight(params object[] args)
    {
        //render right camera stream
        if (material != null)
        {
            material.SetPass(0);
            Graphics.DrawMeshNow(meshR, Matrix4x4.identity);
        }
    }
}
