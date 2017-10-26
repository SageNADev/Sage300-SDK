# Samples

## Environment setup for compilation

For successful compilation, the environment variable **Sage300WebDir** must be set to the 
install location of the Sage 300 Online Web folder.

By default, the SDK **Sage300WebDir** variable is set to the Sage 300 Online Web install path 
of the workstation that ran the Sage 300 code generation wizard. This variable must be 
overridden before building locally on other workstations since the project files need to provide
a hint mechanism to the Visuial Studio files in order to properly located the referenced files.

This can be accomplished in the following ways:

* Set the system variable **Sage300WebDir** on the machine to the Sage 300 Online Web folder

  * Using the Powershell Command Window:
  > ps> [Environment]::SetEnvironmentVariable("Sage300WebDir", "{Online\Web path}", "Machine")

  * Using the Windows System Environment Variables window:
    * Control Panel -> System -> Advanced system settings: System Properties window
    * System Properties --> Advanced tab --> Environment Variables...: Environment Variables window
    * Environment Variables --> System Variables --> New...: New System Variables
    * New System Variables:
      * Variable name: Sage300WebDir
      * Variable value: {Online\Web path}

## Database and Credentials

By default, **SAMLTD** is the database and **ADMIN/ADMIN** are the credentials utilized by the samples. 

These references are located in the Web project's **web.config** and **Global.asax** files. If a different 
database or other credentials are required, these files will require modification.

## Kendo File

The sample solutions requires a certain Kendo file that is not able to be distributed via the
samples and requires the ISV/Partner to have the appropriate Kendo License. Therefore, prior to 
compiling and running the solution, the required Kendo file will need to be located, copied to 
the solution's appropriate folder and added to the Web project within the solution.

### Steps

The following steps are required to add the required Kendo file to the Web project of the solution:

* Locate the **kendo.all.min.js** file
> This file is also located in the Sage 300 Online Web's **Scripts/Kendo** folder

* Copy the **kendo.all.min.js** file to the solution's Web project's **Scripts/Kendo** folder
* Load solution in Visual Studio and include this file in the Web project
* The solution is now compilable and runnable 

## Grid Enhancement

The **Segment Codes** and **Source Journal Profiles** samples have been refactored to utilize the 
grid enhancement logic. The **Receipt** sample, while having a grid, was not a receipient of the grid 
enhancement logic. This will be addressed in a future release.

## Web API WCF Data Services

This sample is no longer applicable as the newer version of the OData specification does not allow
the behavior being demonstated. However, the sample is being left in the SDK merely for reference
and potential future enhancement.

## Resx Generation

Sample source files are provided as examples of a Resource Information Text File and Settings Text
File, which can be supplied to the Resx Generation Utility. Refer to the Resx Generation Utility document
located in the **docs\utilities** folder for details.

## Web API Samples

The Admin user does not have Sage 300 Web API privileges. In order to make various Sage 300 Web API 
requests, you will need to configure a new user and give it the proper user authorizations. Documentation
for the samples will assume you use **SAMLTD** as the database and **WEBAPI/WEBAPI** as the credentials.

Configuring a new user can be accomplished as follows:

* In Sage 300, navigate to Administrative Services -> Users
* Enter **WEBAPI** as the User ID, **WEBAPI** as the User Name, **WEBAPI** as the Password and **WEBAPI** in Verify
* Click Save when finished
* Go to Administrative Services -> Security Groups
* Ensure that for each application that has Sage 300 Web API rights that there is a group ID associated with it
* Go to Administrative Services -> User Authorizations
* For the WEBAPI user, assign the group ID that has Sage 300 Web API rights to each corresponding application