# ViveFocusAR
An experiment to offer augmented reality features on the Vive Focus device.

With this solution you will be able to create augmented reality applications for the Vive Focus, exploiting the two frontal cameras of the device. 

The solution is experimental, but it is a good start form experimenting with MR apps on the Focus.

## Getting Started

You can choose to use this project in three different ways:
* Just running the provided apk contained in Bin folder to test AR on the Focus;
* Getting the Unity package and using it in your project;
* Opening the Unity solution with the source code for this project and modify it as you wish;

### Running the app
If you plan to just use the APK to see how AR in the Focus works:
* Get the APK from the Bin folder of this project
* Connect your Vive Focus to the USB port of your PC
* Install the app on your Vive Focus device
* Launch it from your Vive Focus. You can find it inside the menu Library -> Installed
* Give the app the rights to access the cameras when requested
* You should see your camera stream and a little planet floating in the air
* Point your controller to the planet to see it spinning
* Point your controller and press the touchpad with your thumb to drag the planet to a new position

### Using the Unity package
To use the AR functionality in your project:
* Make sure your project is a Vive Wave one
* Make sure your project requests authorizations for the use of Cameras. If you don't know how to do that, look at how the sample project performs this or read this [post of mine on the topic](https://skarredghost.com/2018/04/23/how-to-ask-android-permissions-in-unity-for-a-vive-focus-app-vive-wave-sdk/).
* Import the package using Assets -> Import Package -> Custom Package... 
* The imported package should be in the Assets\ViveFocusAR of your project
* Use the prefab AR_background contained in Assets\ViveFocusAR\Prefabs to add AR camera background to your scene

### Using the Unity project
To use the sample Unity project:
* Download the repo and open the ViveFocusAR project
* Import the WaveVR Unity Plugin: you can download WaveVR SDK from this [link](https://developer.vive.com/resources/knowledgebase/wave-sdk/). Inside the downloaded ZIP there is a folder dedicated to the Unity plugins: SDK\plugins\unity. Import the WaveVR plugin into the project.
* If there is the need to update APIs, be brave and say Yes to that Unity dialog ("I made a backup, go ahead!")
* Open the Build Settings window and switch Unity platform to Android (File -> Build Settings -> tap on Android -> Switch Platform)
* If you see a pop up by Vive Wave asking you to set some settings, be lazy and select "Accept All"
* Always in the Build Settings window, change the Build System to Internal
* Check that the scene Assets\ViveFocusAR\Scenes\PermissionsRequestScene is the first scene to be built for the app and that the scene Assets\ViveFocusAR\Scenes\FocusAR_Test is the second one (again you do this in the Build Settings of Unity)
* Open the file Plugins\Android\AndroidManifest.xml and add the following two lines after the &lt;/application&gt; tag:

&lt;uses-permission android:name="android.permission.CAMERA" /&gt;

&lt;uses-feature android:name="android.hardware.camera" /&gt;

* Hit Build and Run to build the app and execute it into your Focus

### Understanding the code
The project is split in various folders whose names are self-explanatory. Its code is commented to help you in understanding it.

The main scenes are: PermissionsRequestScene, that is the scene that asks for camera permissions and then load the other scene, that is FocusAR_Test, that displays a planet in AR

The core script is ARBackground, contained in ViveFocusAR\Scripts. It is a modification of one of the Vive Wave SDK samples and basically do the following:
* During startup, creates at runtime a quad in front of the user's eyes. This quad is a bit special because:
** Thanks to the RenderPerEye shader, it will be rendered differently with both eyes;
** It has UVs different for each eyes: this is necessary because the Vive Wave returns a texture containing the frame of both cameras side-by-side. This means that when rendering for the left eye, the quad should show the left half of the texture, while for the right eye it should show the right half;
** It doesn't get rendered in the standard way, but it gets rendered registering to the rendering events of the system and then triggering the rendering explicitly from code. This, together with the shader, makes sure that the quad appears always behind all the other objects of the scene;
* It requests the Vive Wave system to obtain the texture of the cameras stream. Whenever this stream is available, it receives a texture of the cameras and then renders it;

## Prerequisites
If you want to use the project, you must have Unity editor installed. It has been tested with Unity 2017.3.1f1.
To build the APK you should also have Android SDK installed and configured.  
  
## Known issues
This is an experimental project in its first version and so has various problems:
* The camera stream is in low resolution and only black and white (this is because of the cameras on the device, so can't be fixed);
* The camera stream is distorted: undistortion, like the one used in the passthrough, should be applied;
* The shown stream appears as letterboxed, since it doesn't fill all the field of view of the user: this is because the camera frames have a landscape orientation, while the screen for each eyes is portrait. You can mitigate this issue by increasing the zoom factor in the ARBackground script, but this can lead to bad depth perception (objects appears as zoomed);
* The program is computational heavy;
* AR objects don't appear completely fixed in space;
* There may be issues when the program gets suspended and then resumed;
* There may be problems when showcasing UI elements in your application;
* At time of writing, Vive Wave programs don't work with Unity 2018.

## Authors

* **Antony Vitillo (Skarredghost)** - [Blog](http://skarredghost.com)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.
The third party project UnityMainThreadDispatcher is licensed under the Apache license - see the embedded license file for details.
The RenderPerEye shader is released under Wave SDK license.

## Acknowledgments

In case you need help, just [contact me](https://skarredghost.com/contact/).
Have fun :)
