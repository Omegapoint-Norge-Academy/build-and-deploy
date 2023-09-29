# Build and Deploy
Course material for build and deploy

# Workshop
## Fork the template repo to your personal GitHub
1. Go to https://github.com/Omegapoint-Norge-Academy/build-and-deploy-workshop-template
2. Fork the repo

![alt text](Resources/fork.PNG?raw=true)

3. Select your personal GitHub account, name the repo "cap-meme-web" and click create

![alt text](Resources/fork-2.PNG?raw=true)


## Create a web app
1. Goto https://portal.azure.com and login with your Omegapoint account
2. Go to the resource group "APP-Build-and-Deploy" https://portal.azure.com/#@itverket.no/resource/subscriptions/d6d57ee0-64af-4adc-b5cc-44e24edcdb53/resourceGroups/APP-Build-and-Deploy/overview
3. Click the create button 

![alt text](Resources/new_resource.PNG?raw=true)

4. Search for and select "Web App"
5. Click create

![alt text](Resources/new_app.PNG?raw=true)

6. In the basics section fill in these values:
- **Name:** The website name (must be unique). Please include your personal name in the name to avoid confusion (ex. "eivind-cat-meme-web")
- **Runtime stack:** .NET 7 (STS)
- **Operating system:** Windows
- **Region:** West Europe
- **Windows plan:** APP-BD-Plan (S1)

7. Click on "Next: Deployment" at the bottom of the page
8. Click "Enable" on "Continuous deployment"
9. Select your personal github account
10. Select yourself as Organization, and select "cat-meme-web" for Repository and "main" for branch
11. Click on "Next: Networking" at the bottom of the page
12. Click on "Next: Monitoring" at the bottom of the page
13. Select no on "Enable Application Insights"
13. Click "review and create"
14. Check the details and click create

Azure will create a GitHub Action for you as wall as a webapp. Go to your personal github and select the "cat-meme-web" repo an go to Actions to view the deploy

## Verify app has been deployed
Open the app by clicking "Browse"

![alt text](Resources/created-web-app.PNG?raw=true)

## Add app configuration and secrets
### Clone the repo
For the app to work, we need to access configuration and secrets.

First, clone the repository and open the solution in an editor of choice (Visual studio/Visual studio code/Rider)

![alt text](Resources/fork-clone.PNG?raw=true)

![alt text](Resources/fork-clone-2.PNG?raw=true)

### Add configuration provider in code
When you have opened the project, install the following nuget packages to Cat.Meme.Server
```
Microsoft.Azure.AppConfiguration.AspNetCore
Azure.Identity
```
Add this code to the `Program.cs` file in the Cat.Memes.Server project. Add it inside the if sentence `if (builder.Environment.IsProduction())` 

``` csharp
builder.Configuration.AddAzureAppConfiguration(options => 
        options.Connect(
            new Uri(builder.Configuration["AppConfigEndpoint"] ?? throw new InvalidOperationException()),
            new ManagedIdentityCredential())
        .ConfigureKeyVault(kv =>
        {
            kv.SetCredential(new ManagedIdentityCredential());
        }));
```

This code adds Azure App Configuration as a configuration provider for this application. The uri is the address the
Azure App Configuration is hosted at. Also we provide a Managed Identity. This is an identity managed in Azure.
More about managed identities [here](https://learn.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview)

We also add a Key Vault credentials for the app configuration. This enables us to get any secrets stored in Azure Key Vault that
our App Configuration references. More about configuring this [here](https://learn.microsoft.com/en-us/samples/azure/azure-sdk-for-net/app-secrets-configuration/?tabs=visualstudio)

We haven't configured a managed identity in Azure for our web App. Lets do that now.

### Add a managed identity in Azure
In this step we want to add a managed identity to our web app and assign access for that identity to `Application Configuration` and `Key Vault`

1. Goto you web app and turn on `Identity`. Click `Yes` in the popup window. This will assign a managed identity to your application 

![alt text](Resources/managed-identity-web-app.PNG?raw=true)

2. Goto the `APP-BD-AppConfig` resource in Azure and select `Access control (IAM)` and click `Add` -> `Add role assignment`

![alt text](Resources/app-config-access.PNG?raw=true)

3. Select the `App Configuration Data Reader` and click `Next`

![alt text](Resources/app-config-access-2.PNG?raw=true)

4. Select `Managed identity` abd select your web app from the list.

![alt text](Resources/app-config-access-3.PNG?raw=true)

5. Click `Next` and `Review + Assign`
6. Do the exact same thing for `APP-BD-KeyVault`, but this time, assign the `Key Vault Secrets User` role to your web app
## Add tests to workflow
TODO