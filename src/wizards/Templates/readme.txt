-------------------------------------------------------------------------
-                  How to update the 'Web' folder                       -
-------------------------------------------------------------------------


1. Update files in the 'Web' folder

   Run the batch file 'CopyWebRepoFiles.bat'

2. Rebuild the Web.vstemplate file located in the 'Web' folder

   Run the batch file 'RebuildWebDotVstemplateFile.bat'

3. Create the SEVEN zip files located in this folder

   Run the batch file 'CreateTemplateZipFiles.bat'

4. Move the resulting zip files to their final destination

   Run the batch file 'MoveTemplateZipFilesToFinalDestination.bat' 

5. The Solution/Code Generation wizards can now be rebuilt with the
   latest Sage 300 Web Screen Framework source code!

Note: The other folders (excluding 'Web') need to be updated manually. 
      These folders (and files) generally don't change so they can 
      be left as is.