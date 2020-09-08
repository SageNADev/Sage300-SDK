------------------------------------------------------------------------------------------
-                     How to update the 'Web' and 'Solution' folders                     -
------------------------------------------------------------------------------------------

1. Update files in the 'Web' folder

   Run the batch file 'CopyWebRepoFiles.bat'

2. Update the AccpacDotNetVersion.props file

   Run the batch file UpdateAccpacDotNetVersionProps.bat

3. Rebuild the Web.vstemplate file located in the 'Web' folder

   Run the batch file 'RebuildWebDotVstemplateFile.bat'

4. Create/Update the two zip files located in this folder

   Run the batch file 'CreateTemplateZipFiles.bat'

5. Move the resulting zip files to their final destination

   Run the batch file 'MoveZipArchivesToFinalDestination.bat' 

6. The Customization Wizard can now be rebuilt with the
   latest Sage 300 Web Screen Framework source code!
 

Alternatively, you can call RebuildAndDeployAll.bat to run all the
above steps in order.