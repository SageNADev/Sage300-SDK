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

## Kendo Files

The sample solutions require certain Kendo files that are not able to be distributed via the
samples and require the ISV/Partner to have the appropriate Kendo License. Therefore, prior to 
compiling and running the solution, the required Kendo files will need to be located, copied to 
the solution's appropriate folder and added to the Web project within the solution.

### Steps

The following steps are required to add the required Kendo files to the Web project of the solution:

* Locate the **kendo.all.min.js** and **kendo.custom.min.js** files
> These files are also located in the Sage 300 Online Web's **Scripts/Kendo** folder

* Copy the **kendo.all.min.js** and **kendo.custom.min.js** files to the solution's Web project's 
**Scripts/Kendo** folder
* Load solution in Visual Studio and include these two files in the Web project
* The solution is now compilable and runnable 

## Grid Enhancement

The **Segment Codes** and **Source Journal Profiles** samples have been refactored to utilize the 
grid enhancement logic. The **Receipt** sample, while having a grid, was not a receipient of the grid 
enhancement logic. This will be addressed in the next release.

### Patch Files

The Grid Enhancement feature was completed after the Sage 300 2017 Application was code complete.
Therefore, there are a few steps that must be completed in order to patch the application. See
the README file in the **patch** folder.