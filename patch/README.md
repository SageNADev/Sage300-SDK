# Patch

## Partner Menu Items

Partner Menus were enhanced in the 2017.1 release to display at the top of the home page
on the same level as the **Home** and **Intelligence** menu items.

However, after the release of the Web SDK for 2017.1, it was discovered that the menus for 
partners were not behaving as expected. This patch corrects the discovered items.

> This patch is only necessary for developers who deploy to a local installation wishing
to see the correct behavior prior to the 2017.2 release. Also, if a partner will be 
deploying their module to a customer prior to the 2017.2 release, this patch will need 
to be applied to the customer as well. This patch will be included in the standard 
release of 2017.2.

The following sections will describe the issues discovered and the installation instructions
for the patch files.

### Issues Discovered

* Partner Menus do not show the image on the second level menus
* Partner Menus show the resource key as opposed to the resource values
* Partner Menus are only accessible via a click and do not behave like standard menu items (hover-
  over, hover-out, etc.)
* It was discovered that the Code Generation Wizard was generating an incorrect project reference 
  in the service project. This has been resolved in the Code Generation Wizard source and an updated 
  Wizard Package has been added to the **bin** folder.
* It was discovered that the Customization Document and Sample #1 for Customizations were not
  in-sync. The document has been updated in the **docs/customization** folder and the sample has been
  updated in the **samples\customization** folder.
  
### Installation Instructions

* Since the patch files will be copied to the Sage 300 Online folders, the **Sage300** web site must be stopped 
  via the IIS Manager and the **Sage.CNA.WindowsService"" must be stopped via the Services Manager. 
  You will re-start these after the files have been copied.

The following files will need to be copied to the Sage 300 Online folders:

* Copy **patch/TaskDock-Menu-BreadCrumb.js** to your Sage 300 Online Web's **Scripts/Portal** folder
  and overwrite the existing file
  
* Copy **patch/default.css** to your Sage 300 Online Web's **Content/Styles** folder
  and overwrite the existing file

* Copy **patch/Sage.CA.SBS.ERP.Sage300.Core.Portal.css** to your Sage 300 Online Web's **Content/Styles** 
  folder and overwrite the existing file

* Copy **patch/Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.dll** to your Sage 300 Online Web's 
  **bin** folder and overwrite the existing file

* Copy **patch/Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.dll** to your Sage 300 Online Worker 
  folder and overwrite the existing file

* Re-start your web site through the IIS Manager and the service through the Service Manager.