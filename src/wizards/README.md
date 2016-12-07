# Wizards

## Powershell Version 4.0

With the 2017.1 release of the Sage 300 Wizards, the Wizards introduced a dependency on Microsoft's
Management Framework 4.0 when compiling the Wizards in Visual Studio. The dependent component is 
Windows PowerShell 4.0.

The component is required to zip the template files used by the wizard at compile time.

Windows Powershell 4.0 can be installed by downloading Windows Management Framework 4.0 from Microsoft.

> Windows Powershell 4.0 is only required by the Wizards for compiling the solution. This is not
a requirement if the Wizard Package is only installed via the VSIX file.