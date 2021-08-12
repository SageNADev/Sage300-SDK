# Wizards

## Powershell Version 4.0

With the 2017.1 release of the Sage 300 Wizards, the Wizards introduced a dependency on Microsoft's
Management Framework 4.0 when compiling the Wizards in Visual Studio. The dependent component is 
Windows PowerShell 4.0.

The component is required to zip the template files used by the wizard at compile time.

Windows Powershell 4.0 can be installed by downloading Windows Management Framework 4.0 from Microsoft.

> Windows Powershell 4.0 is only required by the Wizards for compiling the solution. This is not
a requirement if the Wizard Package is only installed via the VSIX file.

### How to Build the Sage 300 UI Wizard Package

The following steps illustrate how to build the package:

* Load the **Sage300UIWizardPackage** solution
* Select **Build\Build Solution**
* The package will be successfully built to the output folder specified by the 
solution configuration (Debug or Release). The **Sage300UIWizardPackage.vsix** is 
the artifact that will be used to the install the package.

> **Building the package does not install the package**

### How to Run the Sage 300 UI Wizard Package in Debug Mode

The following steps illustrate how to run the package in debug mode from within Visual Studio:

* Load the **Sage300WizardUIPackage** solution
  *	This a solution which contains both wizard projects and other information to allow 
the Visual Studio plug-in to be debugged
* Right-Click on the **Sage300MenuExtension** project in order to display the 
properties page for this project and select **Set as StartUp Project**
* Right-Click on the **Sage300MenuExtension** project in order to display the 
properties page for this project and select **Properties**
* Select the **Debug** Tab to display the properties for debugging
* In the **Start Action** section, select the **Start external program** option and enter 
the location of **devenv.exe** cooresponding to the version of Visual Studio into the textbox: 
  *	i.e. C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\devenv.exe

> Location and version of **devenv.exe** may be different than what is specified above based upon Version of Visual Studio and installation location

* In the **Start Options** section, enter the following information into the **Command line arguments** textbox:
  *	/rootsuffix Exp
* Run the solution to start debugging!

### How to Build the Sage 300 UI Customization Wizard Executable (Standalone)

The following steps illustrate how to build the executable:

* Load the **Sage300CustomizationWizard** solution
* Select **Build\Build Solution**
* The executable will be successfully built to the output folder specified by the 
solution configuration (Debug or Release). The **Sage.CA.SBS.ERP.Sage300.CustomizationWizard.exe** is 
the artifact produced.

> **The Newtonsoft.Json.dll must accompany the executable if it is copied elsewhere**

### How to Run the Sage 300 UI Customization Wizard Executable in Debug Mode (Standalone)

The following steps illustrate how to run the executable in debug mode from within Visual Studio:

* Load the **Sage300CustomizationWizard** solution
* Run the solution to start debugging!

### How to Build the Sage 300 UI Customization Wizard Package (Plug-in)

The following steps illustrate how to build the package:

* Load the **Sage300UICustomizationWizard** solution
* Select **Build\Build Solution**
* The package will be successfully built to the output folder specified by the 
solution configuration (Debug or Release). The **Sage300UICustomizationSolution.vsix** is 
the artifact that will be used to the install the package.

> **Building the package does not install the package**

### How to Build the Sage 300 Upgrade Wizard Package

The following steps illustrate how to build the package:

* Load the **Sage300UpgradeWizardPackage** solution
* Select **Build\Build Solution**
* The package will be successfully built to the output folder specified by the 
solution configuration (Debug or Release). The **Sage300UpgradeWizardPackage.vsix** is 
the artifact that will be used to the install the package.

> **Building the package does not install the package**
