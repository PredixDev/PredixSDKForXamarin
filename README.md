# XamarinDemos

Sample Xamarin using the .NET SDK. Download this repo to find several example projects demonstrating different aspects of functionality.

### Steps To Run

1. Install Visual Studio (Community Edition works fine)
2. Clone or download this repository
3. After opening Visual Studio, press `Open…` and find the `PredixSDKXamarinDemos.sln` file
4. Wait for packages/dependencies to restore, this may take a minute
	- Note: If on BLUESSO, restore may fail as we’re downloading external packages.
5. Click `AuthenticationDemo` on the top-left and select `AuthenticationDemo.iO`
6. Hit the run button on.

-----

#### How to import a .NET Standard 2.0 NuGet package into a Xamarin.Forms project (locally).

~~You'll first have to convert a fresh Xamarin.Forms PCL project (the default with VS for Mac) into a .NET Standard based project.~~

The latest version of VS for Mac finally introduces a .NET Standard project template for Xamarin.Forms. 
You may follow the below steps if you're converting an existing PCL based Forms project, or cannot update VS for Mac to create a fresh project.

1. (optional) Read this blog post on converting a X.Forms PCL project to .NET Standard [Building Xamarin.Forms Apps with .NET Standard](https://blog.xamarin.com/building-xamarin-forms-apps-net-standard)
2. Follow the step-by-step instructions here [.NET Standard 2.0 Support in Xamarin.Forms - Xamarin](https://developer.xamarin.com/guides/xamarin-forms/under-the-hood/net-standard/)
3. Add the NuGet package (DotNetSDK e.g.) to all projects
    * Go to the preferences page of your IDE (*Visual Studio -> Preferences -> NuGet -> Sources*)
    * Add new local source to reference your local .nupkg files
    
