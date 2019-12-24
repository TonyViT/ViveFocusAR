# ViveFocusAR
An experiment to offer augmented and mixed reality features on the Vive Focus device.

With this solution you will be able to create augmented and mixed reality applications for the Vive Focus, exploiting the two frontal cameras of the device. If you want to see something you can produce with it, check out [this cool Predator-vision video that I shot from my Vive Focus Plus on Youtube](https://www.youtube.com/watch?v=vUt7efTdwXw&feature=share)!

The solution is experimental, but it is a good start for experimenting with MR apps on the Focus.

## Getting Started

You can choose to use this project in three different ways:
* Just running the provided apks contained in the Bin folder to test AR and MR on the Focus;
* Getting the Unity package and using it in your project;
* Opening the Unity solution with the source code for this project and modify it as you wish;

### Running the app
If you plan to just use the APK to see how AR in the Focus works:
* Get one APK from the Bin folder of this project. If you plan to test AR features take ViveFocusAR.apk, to test MR take ViveFocusMR.apk. If you have an old device still running Wave runtime 2, take ViveFocusAR_Wave2.apk
* Connect your Vive Focus to the USB port of your PC
* Install the app on your Vive Focus device, using ADB, SideQuest or your method of choice
* Launch it from your Vive Focus. You can find it inside the menu Library -> On My Device (or Library -> Installed for old runtimes)
* Give the app the rights to access the cameras when requested
* You should see your camera stream and a little planet floating in the air. If you picked the AR version, the camera stream should be black and white, if instead you picked the MR version, you should have thermal vision.

### Using the Unity package
To use the AR/MR functionality in your project:
* Create a Unity project using Wave SDK plugin
* Import the package contained in the Package folder of this repository using Assets -> Import Package -> Custom Package... . Again, you have to choose ViveFocusAR.unitypackage if your project uses Wave SDK v3; otherwise for v2, import ViveFocusAR_Wave2.unitypackage
* The imported package should be in the Assets\ViveFocusMR of your project (or Assets\ViveFocusAR for the old plugin)
* Use the prefab WaveVR_MR Variant contained in Assets\ViveFocusMR\Prefabs to add AR camera background to your scene (AR_background is instead the name of the prefab for Wave 2)
* Make sure your project requests authorizations for the use of Cameras. If you don't know how to do that, look at how the sample project performs this or read this [post of mine on the topic](https://skarredghost.com/2018/04/23/how-to-ask-android-permissions-in-unity-for-a-vive-focus-app-vive-wave-sdk/). In the new version of the plugin, in the package there is already a manifest that is ready to be used to ask the authorization of the cameras. There is also a scene, Assets\ViveFocusMR\Scenes\PermissionsRequestScene.unity, that automatically asks for permissions and loads the next scene in the build settings when the permissions have been granted by the user. Have a look at the sample Unity project to understand how to use all of this.


### Using the Unity project
To use the sample Unity project:
* Download the repo and open the ViveFocusAR project
* Import the WaveVR Unity Plugin: you can download WaveVR SDK from this [link](https://developer.vive.com/resources/knowledgebase/wave-sdk/). Inside the downloaded ZIP there is a folder dedicated to the Unity plugins: SDK\plugins\unity. Import the WaveVR plugin into the project. NOTICE that you DON'T have to overwrite the file AndroidManifest.xml, or the permission request won't work!!
* If there is the need to update APIs, be brave and say Yes to that Unity dialog ("I made a backup, go ahead!")
* Open the Build Settings window and switch Unity platform to Android (File -> Build Settings -> tap on Android -> Switch Platform)
* If you see a pop up by Vive Wave asking you to set some settings, be lazy and select "Accept All"
* Check that the scene Assets\ViveFocusMR\Scenes\PermissionsRequestScene is the first scene to be built for the app and that the scene Assets\ViveFocusMR\Scenes\MR Demo is the second one (again you do this in the Build Settings dialog of Unity, in the "Scenes In Build" section)
* Hit Build and Run to build the app and execute it into your Focus. You should see a planet full of carrot and thermal vision of your surroundings
* If you select the scene Assets\ViveFocusMR\Scenes\AR Demo as the second scene in the build settings order and you build again, you should have just plain black&white augmented reality

### Understanding the code
The project is split in various folders whose names are self-explanatory. Its code is commented to help you in understanding it.

The main scenes are: PermissionsRequestScene, that is the scene that asks for camera permissions and then load the other scene, that is MR Demo, that displays a planet in MR

The core script is ARBackground, contained in ViveFocusMR\Scripts. It is a modification of one of the Vive Wave SDK samples and basically do the following:
* Gets a reference to the two quads that are below it in the hierarchy. One quad is responsible for showing the image to the left eye, the other to the right eye;
* It moves the two quads so that they always stay in front of the eyes of the user, offering so passthrough augmented reality. It also uses reprojection to reduce perceived latency;
* It requests the Vive Wave system to obtain the texture of the cameras stream. Whenever this stream is available, it receives a texture of the cameras and then renders it on the two quads;
* The two quads have an associated material. Depending on the shader of these materials, the visuals will be rendered in a different way. The scene AR Demo just uses a plain unlit shader, so the camera stream frames remain untouched, while the MR Demo uses a special colorizer shader, and that's why there is thermal vision.


## Prerequisites
If you want to use the project, you must have Unity editor installed. It has been tested with Unity 2018.3.6.f1.

The Vive Wave SDK version used to create this plugin is v3.0.2

To build the APK you should also have Android SDK installed and configured.  
  
## Known issues
This is an experimental project in its second version and so has various problems:
* The camera stream is in low resolution and only black and white (this is because of the cameras on the device, so can't be fixed);
* The camera stream is undistorted, but not in an optimal way;
* The shown stream appears as letterboxed, since it doesn't fill all the field of view of the user: this is because the camera frames have a landscape orientation, while the screen for each eyes is portrait. You can mitigate this issue by increasing the zoom factor in the ARBackground script, but this can lead to bad depth perception (objects appears as zoomed);
* The program is computational heavy;
* AR objects don't appear completely fixed in space;
* There may be issues when the program gets suspended and then resumed;

## Authors

* **Antony Vitillo (Skarredghost)** - [Blog](http://skarredghost.com)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

The third party project UnityMainThreadDispatcher is licensed under the Apache license - see the embedded license file for details.

The RenderPerEye shader is released under Wave SDK license.

## Acknowledgments

I have developed this plugin while developing the app Beat Reality with Enea Le Fons and the mixed reality fitness game [HitMotion:Reloaded](https://hitmotion.games) with New Technology Walkers.

I'm releasing this for free, to be helpful for the community. I would really appreciate whatever kind of support if you use this plugin in your project: a hug, a thank you, a subscription to the newsletter of my [blog](http://skarredghost.com), a mention in the credits of your project, a collaboration proposal for your XR project, a donation on my [Patreon account](https://www.patreon.com/skarredghost), the phone number of Scarlett Johansson, etc...

In case you need help, just [contact me](https://skarredghost.com/contact/). Being a consultant, if you need my help to develop an AR or MR application, I will be very happy to be part of your project. :)

Have fun with mixed reality! :)
