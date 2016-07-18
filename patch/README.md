# Grid Enhancement Patch

This patch is required due to the late completion of the grid enhancement work to several 
solutions in the **samples** folder. This work consists of a simpler approach to configuring
a grid for a screen.

Classes were added to the Common and Web projects, while a new method was introduced into the 
Global.js file, and finally a new partial view was created to support this enhancement.

This code files included in this patch will be included in the Sage 300 2017.1 code base and 
therefore the patch will not be required for release 2017.1

## Requirements

Since the Sage 300 Application has already been released for 2017, this patch is required to:

* Support the samples which have grids implementing this new behavior
* Create a grid which utilizes this grid enhancement logic
* Patch the Sage 300 installation to support a screen which is deployed and invoked fron the portal

> **This patch is NOT required to be applied IF the samples are not to be run or if ISV/Partner
code does not use this grid enhancement logic in their code**

## Contents

The Grid Enhancement Patch contains the following files:

* OnlineAreas\Gridpage.cshtml

* OnlineWeb\App_Web_igqienx4.dll
* OnlineWeb\gridpage.cshtml.341fc412.compiled
* OnlineWeb\Sage.CA.SBS.ERP.Sage300.Common.Models.dll
* OnlineWeb\Sage.CA.SBS.ERP.Sage300.Core.Web.dll

* GridPage.cshtml
* Sage.CA.SBS.ERP.Sage300.Common.Global.js

## Steps

The following steps are required to apply this patch:

* Backup files to a safe location first
  * The two dll files (..Common.Models.dll and ..Core.Web.dll) in the 
     Sage 300 Online Web's **bin** folder
  * The Sage.CA.SBS.ERP.Sage300.Common.Global.js file in the Sage 300 Online Web's 
     **Areas/Shared/Scripts** folder

* Copy 
  * The file found in the **patch\OnlineAreas** folder to the Sage 300 Online Web's 
     **Areas\Core\Views\Shared** folder and overwrite the existing file
  * The files found in the **patch\OnlineWeb** folder to the Sage 300 Online Web's 
     **bin** folder and overwrite the existing files
  * The **Sage.CA.SBS.ERP.Sage300.Common.Global.js** file to the Sage 300 Online Web's 
     **Areas/Shared/Scripts** folder and overwrite the existing file
  
  * If any ISV/Partner solutions have been created
    * The **Sage.CA.SBS.ERP.Sage300.Common.Global.js** file to the solution's **Areas/Shared/Scripts**  
      folder and overwrite the existing file
    * The **GridPage.cshtml** file to the solution's **Areas/Core/Views/Shared** folder
      * Load the ISV/Partner solution and include the **GridPage.cshtml** file to the solution's Web project
      
> **The steps to update the Sage 300 Online Web folders are not only for a local developer installation, but 
will also need to be applied to a customer installation if ISV/Partner screens are deployed that use
the grid enhancement logic.**
   