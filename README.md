- [Intro](#intro)
- [Workshop](#workshop)
    * [Intro](#intro-1)
    * [Part 1: Creating building and deploying a web app](#part-1--creating-building-and-deploying-a-web-app)
        + [Fork the template repo to your personal GitHub](#fork-the-template-repo-to-your-personal-github)
    * [Create a web app](#create-a-web-app)
        + [Verify app has been deployed](#verify-app-has-been-deployed)
        + [Part 1 finished](#part-1-finished)
    * [Part 2: Add application configuration and secrets](#part-2--add-application-configuration-and-secrets)
        + [Clone the repo](#clone-the-repo)
        + [Add configuration provider in code](#add-configuration-provider-in-code)
        + [Add a managed identity in Azure](#add-a-managed-identity-in-azure)
        + [Verify app has access to the memes](#verify-app-has-access-to-the-memes)
        + [Part 2 finished](#part-2-finished)
    * [Part 3: Automate testing](#part-3--automate-testing)
        + [Verify test run](#verify-test-run)
        + [Part 3 finished](#part-3-finished)
- [For the course instructor](#for-the-course-instructor)
    * [Workshop setup](#workshop-setup)
        + [Web App: APP-BD-CatMemeAPI and APP-BD-CatMemeWeb](#web-app--app-bd-catmemeapi-and-app-bd-catmemeweb)
        + [Key vault: APP-BD-KeyVault](#key-vault--app-bd-keyvault)
        + [App config: APP-BD-AppConfig](#app-config--app-bd-appconfig)
        + [Access for workshop participants](#access-for-workshop-participants)

# Intro
Course material for build and deploy. This includes workshop guide and instructions for the instructor.

Related repo: https://github.com/Omegapoint-Norge-Academy/build-and-deploy-workshop-template

# Workshop
## Intro
The workshop is split into three parts.
1. Creating building and deploying a web app
2. Add application configuration and secrets
3. Automate testing

## Part 1: Creating building and deploying a web app
### Fork the template repo to your personal GitHub
1. Go to https://github.com/Omegapoint-Norge-Academy/build-and-deploy-workshop-template
2. Fork the repo

![alt text](Resources/fork.PNG?raw=true)

3. Select your personal GitHub account, name the repo `cat-meme-web` and click `Create fork`

![alt text](Resources/fork-2.PNG?raw=true)

## Create a web app
1. Go to https://portal.azure.com and login with your Omegapoint account
2. Go to the resource group `APP-Build-and-Deploy`
https://portal.azure.com/#@itverket.no/resource/subscriptions/d6d57ee0-64af-4adc-b5cc-44e24edcdb53/resourceGroups/APP-Build-and-Deploy/overview
3. Click the `Create` button 

![alt text](Resources/new_resource.PNG?raw=true)

4. Search for and select "Web App"
5. Click create

![alt text](Resources/new_app.PNG?raw=true)

6. In the basics section fill in these values:
- **Name:** The website name (must be unique). Please include your personal name in the name to avoid confusion
(e.g. "eivind-cat-meme-web")
- **Runtime stack:** .NET 7 (STS)
- **Operating system:** Windows
- **Region:** West Europe
- **Windows plan:** APP-BD-Plan (S1)

7. Click on `Next: Deployment` at the bottom of the page
8. Click `Enable` on `Continuous deployment`
9. Select your personal github account
10. Select yourself as Organization, and select `cat-meme-web` for Repository and `main` for branch
11. Click on `Next: Networking` at the bottom of the page
12. Click on `Next: Monitoringv at the bottom of the page
13. Select no on `Enable Application Insights`
13. Click `Review and Create`
14. Check the details and click `Create`

Azure will create a GitHub Action for you as wall as a webapp. Go to your personal github and select the `cat-meme-web`
repo an go to Actions to view the deploy

### Verify app has been deployed
Open the app by clicking "Browse"

![alt text](Resources/created-web-app.PNG?raw=true)

### Part 1 finished
Please help your fellow workshop participants to get to this point. Then we will go through what has happen together

## Part 2: Add application configuration and secrets
### Clone the repo
For the app to work, we need to access configuration and secrets.

First, clone the repository and open the solution in an editor of choice (Visual studio/Visual studio code/Rider)

![alt text](Resources/fork-clone.PNG?raw=true)

![alt text](Resources/fork-clone-2.PNG?raw=true)

### Add configuration provider in code
When you have opened the project, install the following nuget packages to `Cat.Meme.Server`
```
Microsoft.Azure.AppConfiguration.AspNetCore
Azure.Identity
```
Add this code to the `Program.cs` file in the Cat.Memes.Server project. Add it inside the if sentence
`if (builder.Environment.IsProduction())` 

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

1. Go to you web app and turn on `Identity`. Click `Yes` in the popup window. This will assign a managed identity to your application 

![alt text](Resources/managed-identity-web-app.PNG?raw=true)

2. Go to the `APP-BD-AppConfig` resource in Azure and select `Access control (IAM)` and click `Add` -> `Add role assignment`

![alt text](Resources/app-config-access.PNG?raw=true)

3. Select the `App Configuration Data Reader` and click `Next`

![alt text](Resources/app-config-access-2.PNG?raw=true)

4. Select `Managed identity` abd select your web app from the list.

![alt text](Resources/app-config-access-3.PNG?raw=true)

5. Click `Next` and `Review + Assign`
6. Do the exact same thing for `APP-BD-KeyVault`, but this time, assign the `Key Vault Secrets User` role to your web app

### Verify app has access to the memes
Open the app by clicking "Browse"

![alt text](Resources/created-web-app.PNG?raw=true)

### Part 2 finished
Please help your fellow workshop participants to get to this point. Then we will go through what has happen together

## Part 3: Automate testing
Now we want to automate the running of our tests in our workflow. We don't want to deploy anything that fails our tests.

1. Go to the `.github\workflows` folder in your repo.
2. Open the workflow file and add the following step between build and publish
``` yaml
      - name: Test with the dotnet CLI
        run: dotnet test Cat.Memes.sln
```
3. Commit and push your changes
4. You should now be able to observe the tests running in the workflow

### Verify test run
Verify that the tests run ok

### Part 3 finished
**Congratz, you have now finished the workshop!** :tada:

# For the course instructor
## Workshop setup
To prepare the workshop create the following in Azure
- Resource group named `APP-Build-and-Deploy`
- App Service Plan named `APP-BD-Plan` with pricing tier S1
- Key vault named `APP-BD-KeyVault`
- App Configuration named `APP-BD-AppConfig`
- Web App named `APP-BD-CatMemeAPI`
- Web App named `APP-BD-CatMemeWeb`

### Web App: APP-BD-CatMemeAPI and APP-BD-CatMemeWeb
Assign managed identities and deploy the applications from GitHub actions

### Key vault: APP-BD-KeyVault
Add these secrets to Key Vault
- `ApiPassword` with a random value

Assign the role `Key Vault Secrets User` to `APP-BD-CatMemeAPI` and `APP-BD-CatMemeWeb`

### App config: APP-BD-AppConfig
Add the following to the configuration
- `ApiUrl` that points at the url for `APP-BD-CatMemeAPI`
- `ApiUser` with value `admin`
- A key vault reference named `ApiPassword` that references `ApiPassword` in `APP-BD-KeyVault`

Assign the role `App Configuration Data Reader` to `APP-BD-CatMemeAPI` and `APP-BD-CatMemeWeb`

### Access for workshop participants
- Assign the role `Website Contributor` to the resource group `APP-Build-and-Deploy`
- Assign the role `User Access Administrator` to the key vault `APP-BD-KeyVault`. Add a condition that only allows
the users to edit the role `Key Vault Secrets User`. Go to advanced and remove the condition that allows the users
to delete the role. This way they can only add and update `Key Vault Secrets User`
- Assign the role `User Access Administrator` to the key vault `APP-BD-AppConfig`. Add a condition that only allows
  the users to edit the role `App Configuration Data Reader`. Go to advanced and remove the condition that allows the users
  to delete the role. This way they can only add and update `App Configuration Data Reader`
