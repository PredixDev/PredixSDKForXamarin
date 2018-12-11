# PredixSDKforXamarin Example projects

### Using the Examples

This folder contains many sample projects. You can view all the examples using the solution file, [PredixSDKXamarinDemos](PredixSDKXamarinDemos.sln)

The examples are grouped into into folders for the functionality being demonstrated, each functionality folder may have multple example projects within it. 

Additional README.md files within the shared projects (folders without .iOS/.Android appended) can provide additional information where appropriate

### Steps To Run

1. Install Visual Studio (Community Edition works fine)
2. Clone or download this repository
3. After opening Visual Studio, press `Open…` and find the `PredixSDKXamarinDemos.sln` file
4. Move on to the **Resolving Dependencies** section below.
5. (Optional) If working behind a proxy, check out the **Proxy Configuration** section below.

#### Resolving Dependencies

At this point Visual Studio will automatically restore NuGet packages the demo projects reference. In this process you'll be prompted (bombarded) multiple times over to accept package license agreements. Ensure you accept them all before proceeding with running any demo project.

(If only there were an "Accept All" button for a package referenced by multiple projects :( ...)

Follow the steps below if you would like to manually restore your packages (as a sanity check or in case of failure):

1. Right-click `PredixSDKXamarinProjects` on the solution view to the left.
2. Click `Restore NuGet Packages`. The status bar on the top should indicate restoration is in progress.
3. Accept all licenses that you may be prompted to address

#### Proxy Configuration

To download NuGet dependencies, your machine should be able to access the public source URL for nuget.org (https://api.nuget.org/v3/index.json). If you're operating behind a corporate proxy, you could either whitelist the above URL, or modify your local NuGet Configuration file with your proxy configuration. Below are the steps to modify your local configuration:

1. Find where your `nuget.config` file lives by [checking this table.](https://docs.microsoft.com/en-us/nuget/consume-packages/configuring-nuget-behavior#config-file-locations-and-uses)
2. Modify the config file as follows (user and password fields may not be necessary)
```xml
<configuration>
    <!-- stuff -->
    <config>
        <!-- Proxy settings -->
        <add key="http_proxy" value="host" />
        <add key="http_proxy.user" value="username" />
        <add key="http_proxy.password" value="encrypted_password" />
    </config>
    <!-- stuff -->
</configuration>
```
3. Save your config file and restart Visual Studio. 
4. (If necessary) follow the steps under **Resolving Dependencies** to manually restore your packages.

Additional NuGet configuration resources:
- [Configuring NuGet behavior](https://docs.microsoft.com/en-us/nuget/consume-packages/configuring-nuget-behavior)
- [NuGet.config Reference](https://docs.microsoft.com/en-us/nuget/reference/nuget-config-file)
