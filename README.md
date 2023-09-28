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

## Configure workflow
1. Go to your personal github and select the "cat-meme-web" repo
2. Go to the "Actions" tab
3. Clone the repo to your computer

![alt text](Resources/fork-clone.PNG?raw=true)

![alt text](Resources/fork-clone-2.PNG?raw=true)

4. Open the project in an editor of choise (Visual studio/Visual studio code/Rider)
5. 
## Verify app has been deployed
TODO

## Add key vault
TODO

## Add tests to workflow
TODO