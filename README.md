# XamarinDemos

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
3. Right-click `AuthenticationDemo -> AuhtenticationDemo`, then choose *Add -> Add NuGet Packages*
4. Check the `Show pre-release packages` box, select PredixSDK then click *Add Package*

#### Running Authentication
1. Click `AuthenticationDemo` on the top-left and select `AuthenticationDemo.{iOS || Droid}`
2. Click the run button to start the demo.

