# NV.Templates.Backend

{Project description}

## Pre-Requisites

- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

## Recommended Extensions

- [SwitchStartupProject for VS2022](https://heptapod.host/thirteen/switchstartupproject/) - Install from Visual Studio Extensions directly - This extension allows to start mutliple projects at once based on a configuration file.
- [VSColorOutput](https://mike-ward.net/vscoloroutput/) - Install from Visual Studio Extensions - This extension colors the output windows to help identify build warnings and errors.

## Build/release pipeline

The build pipeline is a simple Azure DevOps multistages pipeline:

`Build -> Release Dev -> Release QA -> Release Prod`

### Adjust the pipeline

Before adding the pipeline to Azure DevOps, there is some adjustments/setup to do:

1. Create the Azure DevOps library for the 3 release stages with the following entries

    | Key | Example | Description  |
    |-|-|-|
    | AppConfigurationSku | free | The [AppConfiguration SKU](https://azure.microsoft.com/en-us/pricing/details/app-configuration/) |
    | AppServicePlanAutoScaling | { "enabled": false } | The AppService autoscaling configuration, in JSON |
    | AppServicePlanDefaultInstanceCount | 2 | The default number of instance to create in the AppService Plan |
    | AppServicePlanSku | P1v2 | The [AppService plan SKU](https://azure.microsoft.com/en-us/pricing/details/app-service/linux/). The project is setup to deploy on Linux AppService by default.|
    | BillTo | MyName/MyProjectName | A string set in the `BillTo` resource tag. Usually used to understand what is billed for what project. |
    | DiagnosticsLogsLevel | Verbose | The AppService diagnostic log level |
    | DiagnosticsLogsRetentionInDays | 90 | The number of days logs are retained |
    | Environment | prd | A 3-letters string describing the environment. Used to generate resource names. |
    | Location | canadaeast | The default location for resources. Please note that some resources (Application Insights, AppConfiguration,..) have an hardcoded location to canadacentral, as those resources are not available in all Azure regions |
    | ManagedBy | MyName | A string set in the `ManagedBy` resource tag. Usually used to indicate a primary contact for the resources. |
    | ProjectName | MyProjectName | A 8-letters string describing the project. Used to generate resource names. |
    | ResourceGroup | MyResourceGroupName | The name of the resource group to use. The resource group will be created in the given Azure Subscription if not exist |
    | UseStagingSlots | true | A boolean to indicate whether the deployment process will use deloyment slot or not. Note that not all App Service Plan support deployment slots |
    | WebTestsLocations | [{"Id":"us-ca-sjc-azr"},{"Id":"us-il-ch1-azr"},{"Id":"us-va-ash-azr"}] | The list of WebTests locations to use, in JSON |

1. Create the Azure DevOps pipeline environments for the 3 release stages
   - Don't forget to setup environment's Approvals & Checks before the 1st pipeline run, otherwise all stages will be executed and all environments will be instanciated. 

1. Adjust the file `azure-pipelines.yml`
   - For each release stage, update:
     - `deploymentEnvironment` parameter, with the corresponding deployment environment created above
     - `variableGroup` parameter, with the corresponding variable group created above
     - `azureResourceManagerConnection` parameter, with the Azure Resource Manager connection name
          
1. Adjust the file `azure-pipelines.deploy.yml`
   - In the `AzureRmWebAppDeployment@4` task
     - Update the `StartupCommand` parameter based on your project
    
1. Finally, add the pipeline to Azure DevOps.

## Client App / SPA

If you generated this project with the SPA option, make sure you generate the `package.json` file at the root of the ClientApp folder, or it might result in compilation issues.
