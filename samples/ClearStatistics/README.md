# Clear Statstics Sample

The Clear Statistics Sample provides a working solution that illustrates the Process code
type. Like the other samples, the Kendo files must be added to the project before compiling
(these instructions are found in the README file in the Samples folder). This sample has additional
steps that must be performed prior to running the sample.

## Clear Statistics Script

The **ClearStatistics_WorkerRole_Data.sql** script is located in the root of the
**samples\ClearStatistics** folder. This script includes the INSERT SQL commands for the
work flow tables of your landlord database. The workflow id of 9128 has been assigned to this sample.

### Copy script to Sage 300 runtime folder

Copy **ClearStatistics_WorkerRole_Data.sql** to your Sage 300 runtime folder in order for it to 
be consumed by the **LandlordDatabaseSetup.config** file. This is the file that will be 
invoked from the Portal button of the Database Setup Screen.

### Modify the config file to include the new script

Edit the **LandlordDatabaseSetup.config** file which is located in your Sage 300 runtime
folder to include the script **ClearStatistics_WorkerRole_Data.sql**. Thus, when the Portal
button of the Database Setup Screen is selected, it will insert these records to the workflow tables.

### Stop the Sage CNA Windows Service

The Sage CNA Windows Service is a service that faciliates the work flow implementation. Therefore,
this service must be stopped in order to proceed with not only applying the patch files, but for 
also compiling the sample solution in Release mode. In the final release of 2017.1, this step will
not be required as these patch files will be included in the release and the utility that deploys
the binaries to the Web and Worker folders will be modified to stop the service and re-start the 
service once the binaries are copied.

* Access the Services MMC from your desktop (simply type in **Services** in the search feature
  of the desktop) and select the Service MMC.
* Locate the **Sage.CNA.WindowsService** and select the Stop button

### Apply Patch files

This step will be removed in the 2017.1 release of the SDK. But, in the meantime, certain files
must be patched in both the Web and Worker folders of the Sage 300 Web Screens. See the README
file located in the **patch** folder for instructions.

### Build the Clear Statistics sample

* Load the **ClearStatistics.sln**
* Be sure to follow the instructions for adding the Kendo min files first
* Build the solution in **Debug** mode first to ensure that all builds successfully
* Change the solution's mode to **Release**
* Select **Clean Build** to perform a clean first
* Select **Build Solution** to have the binaries deployed to the local installation

> **The deployment of the binaries to both the Web and Worker folders is required because while the
  solution will be run in debug mode, the workflow engine and queue are part of the installation
  and therefore the binaries for the worker must reside in the Sage 300 Online's Worker folder.** 

* Change the solution's mode back to **Debug** since once it is deployed, unless other changes are made, 
  there is no reason to redeploy again.

### Start the Sage CNA Windows Service and apply the new script

The Database Setup Screen will peform both of these features!

* Access the Database Setup Screen from the desktop
* Select the **Portal** button
* Enter the proper password for the Landlord Database
* The new script will be applied and the windows service will be re-started

### Run the solution

Now that the patch has been applied, the script applied to the Landlord database and the sample's
binaries have been deployed to the Sage 300 Online Web and Worker folders, the solution can finally be
run to see it in action.

> **The debug of the sample will not step through the worker implementation or through the repository
  code as the worker calls back into the repository itself via the worker instance. Therefore, logging
  statements become quite useful and these will be available in the trace.log file of the worker in the 
  Sage 300 Online Worker's Logs folder.** 
