# NV.Templates.Backend

{Project description}

## Pre-Requisites

- [Visual Studio 2019](https://visualstudio.microsoft.com/)
- [.NET Core SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)

## Build/release pipeline

The build pipeline is a simple Azure DevOps multistages pipeline:

`Build -> Release Dev -> Release QA -> Release Prod`

### Adjust the pipeline

Before adding the pipeline to Azure DevOps, there is some adjustments/setup to do:

1. Create the Azure DevOps library for the 3 release stages with the following entries

    | Key | Example | Description  |
    |-|-|-|
    | AppConfigurationSku | free | The [AppConfiguration SKU](https://azure.microsoft.com/en-us/pricing/details/app-configuration/) |
    | AppServicePlanAutoScaling | { "enabled": false } | The AppService autoscaling configuration, in a JSON format |
    | AppServicePlanDefaultInstanceCount | 2 | The default number of instance to create in the AppService Plan |
    | AppServicePlanSku | P1v2 | The [AppService plan SKU](https://azure.microsoft.com/en-us/pricing/details/app-service/linux/). The project is setup to deploy on Linux AppService by default.|
    | AzureResourceManagerConnection | MyARMConnectionName | The Azure Resource Manager connection name created in Azure DevOps |
    | BillTo | MyName/MyProjectName | A string set in the `BillTo` resource tag. Usually used to understand what is billed for what project. |
    | DiagnosticsLogsLevel | Verbose | The AppService diagnostic log level |
    | DiagnosticsLogsRetentionInDays | 90 | The number of days logs are retained |
    | Environment | prd | A 3-letters string describing the environment. Used to generate resource names. |
    | Location | canadaeast | The default location for resources. Please note that some resources (Application Insights, AppConfiguration,..) have an hardcoded location to canadacentral, as those resources are not available in all Azure regions |
    | ManagedBy | MyName | A string set in the `ManagedBy` resource tag. Usually used to indicate a primary contact for the resources. |
    | ProjectName | MyProjectName | A 8-letters string describing the project. Used to generate resource names. |
    | ResourceGroup | MyResourceGroupName | The name of the resource group to use. The resource group will be created in the given Azure Subscription if not exist |
    | WebTestsLocations | [{"Id":"us-ca-sjc-azr"},{"Id":"us-il-ch1-azr"},{"Id":"us-va-ash-azr"}] | The list of WebTests locations to use in JSON format |

1. Create the Azure DevOps pipeline environments for the 3 release stages
   - Don't forget to setup environment's Approvals & Checks before the 1st pipeline run, otherwise all stages will be executed and all environments will be instanciated. 

1. Adjust the file `azure-pipelines.yml`
   - For each release stage, update:
     - `deploymentEnvironment` parameter
     - `variableGroup` parameter
     
1. Adjust the file `azure-pipelines.build.yml`
   - In the final `publish` task, update the artifact name, based on your project
     
1. Adjust the file `azure-pipelines.deploy.yml`
   - In the `DownloadBuildArtifacts@0` task, update the artifact name, based on your project
   - In the `AzureResourceManagerTemplateDeployment@3` task
     - Update the `azureResourceManagerConnection` parameter with the Azure Resource Manager connection name
     - Update the `csmFile` parameter with the correct ARM template file path, based on your project
   - In the `AzureRmWebAppDeployment@4` task
     - Update the `azureSubscription` parameter with the Azure Resource Manager connection name
     - Update the `packageForLinux` parameter with the correct Web directory path, based on your project
     - Update the `StartupCommand` parameter based on your project
   - In the `AzureAppServiceManage@0` task
     - Update the `azureSubscription` parameter with the Azure Resource Manager connection name
    
1. Finally, add the pipeline to Azure DevOps.