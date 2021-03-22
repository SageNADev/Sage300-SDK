-------------------------------------------------------------------------
-                  How to update the 'Web' folder                       -
-------------------------------------------------------------------------


Step 1
	Update files in the 'Web' folder

	Run the batch file 'CopyWebRepoFiles.bat'

Step 2
	Rebuild the Web.vstemplate file located in the 'Web' folder

   	Run the batch file 'RebuildWebDotVstemplateFile.bat'

Step 3
	Create the SEVEN zip files located in this folder

   	Run the batch file 'CreateTemplateZipFiles.bat'

Step 4
	Move the resulting zip files to their final destination

	Run the batch file 'MoveTemplateZipFilesToFinalDestination.bat' 

   	The Solution/Code Generation wizards can now be rebuilt with the
   	latest Sage 300 Web Screen Framework source code!

	Note: The other folders (excluding 'Web') need to be updated manually. 
        These folders (and files) generally don't change so they can be left as is.

Step 5 
	Ensure the correct version of AccpacDotNetVersion.props has been 
        copied to the Solution Wizard resources folder

	Run the batch file 'CopyAccpacDotNetVersionSettings.bat'


Alternatively, you can just run 'RebuildAndDeployAll.bat' to execute all the above steps.