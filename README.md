# PredixSDKForDotNet 

The Predix SDK is a .NET library designed to make developing IIoT applications for Predix for Windows and Mobile (Xamarin) fast and simple using a robust set of APIs that feel natural for a .NET developer

## Supported Platforms and Targets

- .NET Framework
- Xamarin

## Download

- [Alpha Release](https://github.build.ge.com/predix-mobile/PredixSDKForDotNet/releases)



## Xamarin Demos

Sample Xamarin using the .NET SDK. Download this repo to find several example projects demonstrating different aspects of functionality.

### Steps To Run

#### Getting Started
1. Install Visual Studio (Community Edition works fine)
2. Clone or download this repository
3. After opening Visual Studio, press `Open…` and find the `PredixSDKXamarinDemos.sln` file

#### Resolving Dependencies
Wait for packages/dependencies to restore, this may take a minute
	- Note: If on BLUESSO, restore may fail as we’re downloading external packages. 
    - You can manually restore packages clicking *Project -> Restore NuGet Packages*

1. To add the PredixSDK, go to (*Visual Studio -> Preferences -> NuGet -> Sources*)
2. Add a new source referencing `LocalNugetSource`
3. Right-click `SomeFeatureDemo -> SomeFeatureDemo`, then choose *Add -> Add NuGet Packages*
4. Check the `Show pre-release packages` box, select PredixSDK then click *Add Package*

#### Running a Demo
1. Click `SomeFeatureDemo` on the top-left and select `SomeFeatureDemo.{iOS || Droid}`
2. Click the run button to start the demo.

