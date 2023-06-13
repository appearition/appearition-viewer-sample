# **Appearition AR Viewer**

## Table of Contents

- [Technologies Used](#technologies-used)
- [Required Packages](#required-packages)
- [Installation](#installation)
- [Optional steps](#optional-steps)
- [Usage](#usage)

## Technologies Used

- Unity 2021.1.10f1
- Vuforia 9.8.5

## Required Packages
- Trilib 2
- Unity Logs Viewer(Nice to have for debugging)

## Installation

1. Clone the repository using your preferred mode of source control.
2. Install Unity 2021.1.10f1 with support for Android/iOS depending on your device.
3. Open the project in Unity 2021.1.10f1.
4. Please read and follow the instructions at http://docs.appearition.com/sdk/api-access/ to register an application and generate an API Token

## Optional Steps

Follow the following instructions to install the recommended UNity package for runtime loading of 3d Models in your applications
1. Install Trilib 2 using Unity Package Manager(You might have to purchase the required asset).
2. Extract the "ArDemo_Trilib2.0_Integration" Unity Package in the following location "Assets\Appearition\Examples\SampleProjects\ArDemo\Packages"

## Usage

1. Open the demo scene which can be found under Assets/Scenes/AppearitionViewer
2. Find the Gameobject 'AppearitionManager_Vuforia' and select it to view the properties in the inspector.
3. Under the AppearitionGate component found in the inspector, click the 'Create New' button and enter your credentials you've created from the Appearition Platform.
4. Hit the play button and your experiences will be fetched from the Appearition Platform and loaded.
5. Confirm the presence of your experiences.
6. Build and run the project on your Android/iOS device.
7. The app can also be tested within Unity by using your webcam to point to a marker or using Vuforia's ground plane for markerless

