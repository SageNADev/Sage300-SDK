# Wizards

## Powershell Version 4.0

With the 2017.1 release of the Sage 300 Wizards, the Wizards introduced a dependency on Microsoft's
Management Framework 4.0 when compiling the Wizards in Visual Studio. The dependent component is 
Windows PowerShell 4.0.

The component is required to zip the template files used by the wizard at compile time.

Windows Powershell 4.0 can be installed by downloading Windows Management Framework 4.0 from Microsoft.

> Windows Powershell 4.0 is only required by the Wizards for compiling the solution. This is not
a requirement if the Wizard Package is only installed via the VSIX file.

### How to Build the Sage 300 Solution Wizard Package

The following steps illustrate how to build the package:

* Load the **Sage.CA.SBS.ERP.Sage300.SolutionWizardPackage** solution
* Select **Build\Build Solution**
* The package will be successfully built to the output folder specified by the 
solution configuration (Debug or Release). The **Sage.CA.SBS.ERP.Sage300.SolutionWizardPackage.vsix** is 
the artifact that will be used to the install the package.

> **Building the package does not install the package**

### How to Build the Sage 300 Wizard Package

> **This package contains the Code Generation Wizard, the Finder Generator, **
> **the Upgrade Wizard, and the Language Wizard**

The following steps illustrate how to build the package:

* Load the **Sage.CA.SBS.ERP.Sage300.WizardPackage** solution
* Select **Build\Build Solution**
* The package will be successfully built to the output folder specified by the 
solution configuration (Debug or Release). The **Sage.CA.SBS.ERP.Sage300.WizardPackage.vsix** is 
the artifact that will be used to the install the package.

> **Building the package does not install the package**

### How to Build the Sage 300 Customization Wizard Executable (Standalone)

The following steps illustrate how to build the executable:

* Load the **Sage300CustomizationWizard** solution
* Select **Build\Build Solution**
* The executable will be successfully built to the output folder specified by the 
solution configuration (Debug or Release). The **Sage.CA.SBS.ERP.Sage300.CustomizationWizard.exe** is 
the artifact produced.

> **The Newtonsoft.Json.dll must accompany the executable if it is copied elsewhere**

### How to Build the Sage 300 Customization Wizard Package (Plug-in)

The following steps illustrate how to build the package:

* Load the **Sage300UICustomizationWizard** solution
* Select **Build\Build Solution**
* The package will be successfully built to the output folder specified by the 
solution configuration (Debug or Release). The **Sage300UICustomizationSolution.vsix** is 
the artifact that will be used to the install the package.

> **Building the package does not install the package**
