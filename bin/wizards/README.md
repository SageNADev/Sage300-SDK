# Wizards

## Powershell Version 4.0

With the 2017.1 release of the Sage 300 Wizards, the Wizards introduced a dependency on Microsoft's
Management Framework 4.0 when compiling the Wizards in Visual Studio. The dependent component is 
Windows PowerShell 4.0.

The component is required to zip the template files used by the wizard at compile time.

Windows Powershell 4.0 can be installed by downloading Windows Management Framework 4.0 from Microsoft.

> Windows Powershell 4.0 is only required by the Wizards for compiling the solution. This is not
a requirement if the Wizard Package is only installed via the VSIX file.

## How to Install the Sage 300 UI Customization Wizard Package

The following steps illustrate how to install the package:

* Locate the **Sage300UICustomizationSolution.vsix** file in the bin\wizards folder and 
run this file.
* Select **Yes** to install the plug-in

> **If the package is already installed, it must be uninstalled first**

## How to Uninstall the Sage 300 UI Customization Wizard Package

The following steps illustrate how to uninstall the package from Visual Studio:

* Load Visual Studio
* Select **Tools\Extensions and Updates…**
* Search for and select the **Sage 300 UI Customization Wizard**
* Select the **Uninstall** button
* Select **Yes** to confirm uninstallation of the package
* Select **Yes** to re-start Visual Studio
* The package has been uninstalled

> **This step is only required if the package is installed**
