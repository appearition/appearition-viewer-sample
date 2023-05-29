#ArDemo instructions
> Download the latest SDK version
> Open a new Unity project (2019LTS or later)
> Import in Unity via drag&drop or double click on the package
> Double click on the ArDemo package located under Appearition/Example/SampleProjects
> Navigate to Appearition\Examples\SampleProjects\ArDemo\Scenes and open the ArDemo scene
> Press play

#Vuforia Integration Instructions (Unity 2020.1 or above recommended)
> Open package manager
> Click on +, add package from git URL and paste git+https://git-packages.developer.vuforia.com/#9.8.5
> Wait until it's loaded and appears
> Install the AppearitionSDK_v0.7.4_ArDemo_v3.0 package located inside Appearition\Examples\SampleProjects\
> Install the ArDemo_Vuforia_v9.8.5_Integration package under Appearition/Example/SampleProjects/ArDemo/Packages
> Navigate to Appearition\Examples\SampleProjects\ArDemo\Scenes and open the ArDemo_Vuforia scene
> Press play
> You can test the Marker mode using the sample target images inside Appearition\Examples\SampleProjects\ArDemo\SampleImages

#ArFoundation Integration Instructions (Unity 2021.1 or above recommended)
> Open package manager
> Install XR Plugin Management (4.0.7)
> Install AR Foundation (4.1.7)
> Install either or both ARCore XR Plugin & ARKit XR Plugin (v4.1.7)
> Install the AppearitionSDK_v0.7.4_ArDemo_v3.0 package located inside Appearition\Examples\SampleProjects\
> Install the ArDemo_ArFoundation_v4.1.7_Integration package under Appearition/Example/SampleProjects/ArDemo/Packages
> Navigate to Appearition\Examples\SampleProjects\ArDemo\Scenes and open the ArDemo_ArFoundation scene
> Go to Player Settings
> Remove Vulkan from the list of Graphic APIs
> Set Minimum API Level to Android 7.0 API lvl 24
> Set API Compatibility Level to .NET 4.x
> Go to XR Plugin Management and select ArCore
> Make a build using a supported device and play
> You can test the Marker mode using the sample target images inside Appearition\Examples\SampleProjects\ArDemo\SampleImages

#Trilib 2 Integration Instructions (3D model loader)
> Install the AppearitionSDK_v0.7.4_ArDemo_v3.0 package located inside Appearition\Examples\SampleProjects\
> Add Trilib 2.0 package to Unity (purchased via the AssetStore)
> Double click on the ArDemo_Trilib2.0_Integration package under Appearition/Example/SampleProjects/ArDemo/Packages
> Create an experience on the EMS using Model mediatype
> You will need to creat an experience in the EMS in order to view it in the ArDemo. 
> Once created, you can see it in Standard Mode (or other AR Providers you've implemented) by entering the Test Asset Id of your new experience in the ArDemoUi GameObject on the ArDemo scenes.