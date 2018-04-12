@echo off
:: ---------------------------------------------------------------------------------------
:: File: RebuildPayload.bat
::
:: Purpose: This batch file will gather up all the necessary assets 
::          and create a file called UpgradeWebItems.zip located
::          in a folder called ItemTemplates.
::
::          Step 1: Do setup tasks
::                  * Setup constants and variables
::                  * Create a temporary folder
::
::          Step 2: Copy assets to temporary folder and build 
::                  new Items.zip from CNA2\Columbus-Web sources
::                  and other files as listed below:
::                     * \SDK\Settings\AccpacDotNetVersion.props
::                     * \assets\__TemplateIcon.ico
::                     * \assets\Items.vstemplate
::
::          Step 3: Clear out the final destination folder
::
::          Step 4: Zip everything up into a file called Items.zip
::
::          Step 5: Remove the temporary working folder
::
::          Step 6: Done!
:: ---------------------------------------------------------------------------------------

setlocal

:: ---------------------------------------------------------------------------------------
:: Set the final zip filename(s)
:: ---------------------------------------------------------------------------------------
::set ContainerZipFileName=Items
set ItemsZipFileName=Items

:: ---------------------------------------------------------------------------------------
:: Set the destination folder for the zip file
:: ---------------------------------------------------------------------------------------
:: set zipDestinationPath=.
set zipDestinationPath=ItemTemplates
if not [%1]==[] (
  set zipDestinationPath=%1
)
echo zipDestinationPath=%zipDestinationPath%

:: ---------------------------------------------------------------------------------------
::
:: Create a datetimestamped temporary folder to serve
:: as our working folder
:: Also, within this folder, create two folders as follows:
::   \Web
::   \Web-All
::
:: ---------------------------------------------------------------------------------------
for /f "delims=" %%a in ('wmic OS get localdatetime  ^| find "."') do set datetime=%%a
:: Example timestamp = 20180321104611.887000-420
set "YYYY=%datetime:~0,4%"
set "MM=%datetime:~4,2%"
set "DD=%datetime:~6,2%"
set "HH=%datetime:~8,2%"
set "MI=%datetime:~10,2%"
set "SS=%datetime:~12,2%"
set "MS=%datetime:~15,3%"
set workingFolder=%YYYY%-%MM%-%DD%-%HH%-%MI%-%SS%-%MS%
mkdir %workingFolder%
set workingFolderWebAll=%workingFolder%\Web-All
mkdir %workingFolderWebAll%
set workingFolderWeb=%workingFolder%\Web
mkdir %workingFolderWeb%

:: ---------------------------------------------------------------------------------------
:: Copy assets to the working folder
:: ---------------------------------------------------------------------------------------
copy ..\..\..\..\Settings\AccpacDotNetVersion.props %workingFolder%
copy Assets\*.* %workingFolder%
:: Copy all folders and files from the SDK\src\Wizards\Templates\Web folder
:: to the local 'Web-All' folder
robocopy /E ..\..\Templates\Web %workingFolderWebAll%

:: ---------------------------------------------------------------------------------------
:: Now, using the manifest file, copy only the necessary folders and files
:: from the 'Web-All' folder to the 'Web' folder and then delete the 'Web-All' folder
:: ---------------------------------------------------------------------------------------
call :CleanWebFolderBasedOnManifestFile %workingFolderWebAll% %workingFolderWeb%
rd /S /Q %workingFolderWebAll%


:: ---------------------------------------------------------------------------------------
:: Move whatever files are in the root of the temporary date/timestamped folder
:: into the 'Web' folder
:: ---------------------------------------------------------------------------------------
move /Y %workingFolder%\*.* %workingFolderWeb%

:: ---------------------------------------------------------------------------------------
:: Create the Items.zip file
:: ---------------------------------------------------------------------------------------
call :Zip2 %workingFolderWeb% %workingFolder%\%ItemsZipFileName%.zip

:: ---------------------------------------------------------------------------------------
:: Remove the 'Web' folder once a Items.zip file has been created
:: ---------------------------------------------------------------------------------------
if exist %workingFolder%\%ItemsZipFileName%.zip (
  echo The 'Items.zip' file has been created successfully!
  rd /S /Q %workingFolderWeb%
)

:: ---------------------------------------------------------------------------------------
:: Clear out the final destination folder
:: ---------------------------------------------------------------------------------------
del %zipDestinationPath%\*.zip
echo '%zipDestinationPath%' folder has been cleared.

:: ---------------------------------------------------------------------------------------
:: Copy the final Items.zip to the final destination
:: This file contains all of the components outlined in step 2
:: ---------------------------------------------------------------------------------------
copy %workingFolder%\%ItemsZipFileName%.zip %zipDestinationPath%
echo %ItemsZipFileName%.zip copied to '%zipDestinationPath%' 

:: ---------------------------------------------------------------------------------------
:: Remove the working folder
:: ---------------------------------------------------------------------------------------
rd /S /Q %workingFolder%
echo Temporary folder '%workingFolder%' has been removed.

:: ---------------------------------------------------------------------------------------
:: That's it :)
:: ---------------------------------------------------------------------------------------
goto :EOF












:: ---------------------------------------------------------------------------------------
:: Function Definitions
:: ---------------------------------------------------------------------------------------

:Zip
  setlocal
  @echo on
  set zipFolderName=%~1
  set zipDir="%~dp0\%zipFolderName%"
  powershell.exe -ExecutionPolicy Bypass -file "%~dp0\pszip.ps1" ^
     "%zipDir%" "%zipDestinationPath%\%ContainerZipFileName%.zip"
  @echo off
  goto :EOF

:: ---------------------------------------------------------------------------------------
:: A more generic version of the function :Zip 
:: ---------------------------------------------------------------------------------------
:Zip2
  setlocal
  @echo on
  set sourceFolder=%~1
  set destinationFile=%~2
  set zipDir="%~dp0%sourceFolder%"
  powershell.exe -ExecutionPolicy Bypass -file "%~dp0\pszip.ps1" ^
     "%zipDir%" "%destinationFile%"
  @echo off
  goto :EOF

:: ---------------------------------------------------------------------------------------
:: Populate a folder called 'Web' that will contain only the necessary components
:: sourced from the Web-All folder. The list of valid folders and files is contained
:: within the manifest file (defined in psCleanWebFolder.ps1)
:: ---------------------------------------------------------------------------------------
:CleanWebFolderBasedOnManifestFile
  setlocal
  @echo on
  set source=%~1
  set dest=%~2
  powershell.exe -ExecutionPolicy Bypass -file "%~dp0psCleanWebFolder.ps1" "%source%" "%dest%"
  @echo off
  goto :EOF

:: End of File