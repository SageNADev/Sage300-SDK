# Process Sample Patch

This patch is required due to a defect discovered while creating the Clear Statistics sample.

The binary files included in this patch will be included in the Sage 300 2017.1 release and 
therefore the patch will not be required for release 2017.1 (it is only required for accessing
the Clear Statistics sample PRIOR to the 2017.1 release).

## Requirements

Since the Sage 300 Application has not been released for 2017.1, this patch is required to:

* Patch the Sage 300 installation to support not only the Clear Statistics Sample but
  also any ISV/Partner Process type projects.

> **This patch is NOT required to be applied IF the Clear Statistic sample is not run or if 
  ISV/Partner does not have Process type projects**

## Contents

The Process Sample Patch contains the following files:

* OnlineWeb\Sage.CA.SBS.ERP.Sage300.Common.Utilities.dll
* OnlineWeb\Sage.CA.SBS.ERP.Sage300.Core.Configuration.dll
* OnlineWeb\Sage.CA.SBS.ERP.Sage300.Worker.dll

* OnlineWorker\Sage.CA.SBS.ERP.Sage300.Common.Utilities.dll
* OnlineWorker\Sage.CA.SBS.ERP.Sage300.Core.Configuration.dll
* OnlineWorker\Sage.CA.SBS.ERP.Sage300.Worker.dll

## Steps

The following steps are required to apply this patch:

* Backup files to a safe location first
  * The three dll files mentioned above in the Sage 300 Online Web's **bin** folder
  * The three dll files mentioned above in the Sage 300 Online Worker's folder

* Copy 
  * The files found in the **patch\OnlineWeb** folder to the Sage 300 Online Web's 
     **bin** folder and overwrite the existing files
  * The files found in the **patch\OnlineWorker** folder to the Sage 300 Online Workers's 
     folder and overwrite the existing files
  
> **The steps to update the Sage 300 Online Web/Worker folders are not only for a local developer 
installation, but will also need to be applied to a customer installation if ISV/Partner screens 
are deployed that use a Process type by an ISV/Partner.**
   