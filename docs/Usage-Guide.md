# Usage Guide

---Organize the Usage Guide into sections using Markdown Headings. This guide should mirror the steps in the Quick Start but with far more details and include any optional steps.---

1. Download the package from the repository [Releases](https://github.com/Kentico/xperience-by-kentico-sitefinity-migration-tool/releases)
    - Optionally you can clone the git repository and build and run the source files
``` git clone -v "https://github.com/Kentico/xperience-by-kentico-sitecore-migration-tool.git" "{YOUR PATH}"```


1. Extract and navigate to the configuration file `/examples/Migration.Toolkit.Sitefinity.Console/appsettings.json`, make a copy of this file and update settings as needed. The details for configuration options can be found below. Once you have the file in place run the mmigration from the command line

1. Execute the command from an elevated command prompt: 
```
>./Migration.Toolkit.CLI.exe  migrate -config "/path/to/your/config/appsettings.json" 
```
1. View migrated sites, multi-lingual content, users, pages, and media in your XbyK instance.

## Details Running a Migration ##

1. Get the code from GitHub
1. Set the config options in the application.json file 
1. Backup XbyK database
1. Run the command line
1. Check the destinaion XbyK instance for correctness
    - Channels/Web Sites
    - Users
    - Page Types
    - Pages
    - Content Hub Items
    - Media
1. Adjust the configuration options as needed to correct any import issues
1. Reimport as need - migrated content will be overwritten with changes from Sitefinity - the tool is smart enough to only migrate updated and new content. Subsequent runs of the migration are generally much faster
1. As a good practice backup XbyK database before each reimport in case you need to go back to an earlier version 
1. If necessary creating a new, empty XbyK database is very simple using `dotnet kentico-xperience-dbmanager`

## Details on the configuration file ##
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  },
  "CMSHashStringSalt": "HASH#HASH#HASH",
  "ConnectionStrings": {
    "CMSConnectionString": "[#YOUR CONNECTION STRING TO XbyK HERE]",
    "SitefinityConnectionString": "[#YOUR CONNECTION STRING TO Sitefinity HERE]"
  },
  "Sitefinity": {
    "Url": "[#YOUR SITE ORIGIN HERE]",
    "WebServicePath": "api/default",
    "WebServiceApiKey": "",
    "ModuleDeploymentFolderPath": "[#YOUR PATH TO DEPLOYMENT FOLDER HERE]",
    "CodeNamePrefix": "[#YOUR SITE'S CODENAME PREFIX]",
    "PageContentTypes": [
      {
        "Name": "[#NAME OF DYNAMIC MODULE TYPE]",
        "PageRootPath": "[#ROOT PATH FOR DYNAMIC MODULE TYPE]"
      }
    ]
  },
  "WebApplicationPhysicalPath": "[#YOUR PATH TO XbyK INSTANCE#]"
}

```
### Logging ###
: Common json configuration options for [Microsoft.Extensions.Logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line#configure-logging-without-code) are allowed here.
### CMSHashStringSalt ### 
: Salt copied from XbyK appsettings.json file.
### ConnectionStrings/CMSConnectionString ### 
: Database connection to your destination XbyK instance, commonly copied from \appsettings.json
### ConnectionStrings/SitefinityConnectionString ###
: This tool pulls some date from the Sitefinity database directly. Add the database connection to your source Sitefinity instance, commonly copied from \App_Data\Sitefinity\Configuration\Data.config
### Sitefinity/Url ###
: The URL of your running source Sitefinity instance. This tool uses the Sitefinity content API, it is necessary to have it accessible. Include the full Url with protocol and trailing slash. e.g. https://local.examplesite.com/
### Sitefinity/WebServicePath ###
: Sitefinity path to the 'Default' API endpoint. It is required to have this 'Default' web service enabled. Check if it is enabled and the path inside the Sitefinity Admin under Administration/Web Services/Default. It is also necessary to enable the web service to expose all content to be migrated.
![alt text](image-1.png)
### (Optional) Sitefinity/WebServiceApiKey ###
: (optional) if the web service endpoint is configured to have a key, enter it in the here.
### Sitefinity/ModuleDeploymentFolderPath ###
: This tool will store temporary output files, they will be stored in this local directly
### Sitefinity/CodeNamePrefix ###
: The destination XbyK site to target for this migration 
### Sitefinity/PageContentTypes ###
: a list of type and their root page path. Sitefinity allows for content 'details' to be displayed from a single 'listing' page. e.g. A news 'listing' page will link to a large number of news 'details' pages. This pattern is different in XbyK. To support the strong page types in XbyK you must list these 'listing' pages and their types so they can be migrated and the child pages created. List out the Sitefinity content type that each 'listing' page displays. This tool will migrate the root page and all the child pages.

example:
```
    "PageContentTypes": [
      {
        "Name": "Region",
        "PageRootPath": "/Regions"
      }
      {
        "Name": "Blog",
        "PageRootPath": "/Sports-Blog"
      }
      {
        "Name": "Blog",
        "PageRootPath": "/Events-Blog"
      }
      {
        "Name": "Author",
        "PageRootPath": "/News/Authors"
      }
    ]

```
### WebApplicationPhysicalPath ###
: Path to the destination XbyK instance. Exclude the trailing slash e.g. c:\develop\my_xby_site