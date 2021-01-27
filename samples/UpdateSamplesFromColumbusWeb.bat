@echo off
REM
REM Copy Web Screen core files to Web SDK Sample projects
REM 

set Drive=D:
set SourceWebRoot=%Drive%\Projects\SageAzureDev\Columbus-Web\Sage.CA.SBS.ERP.Sage300.Web
set DestinationSamplesRoot=%Drive%\Projects\Sage300-SDK\Develop\Samples

REM
REM List of Samples to update
REM
REM set WebSDKSampleList=ClearStatistics Receipt SegmentCodes SourceCodes SourceJournalProfiles SourceJournalProfilesReports TaxAuthorities
set WebSDKSampleList=ClearStatistics

(for %%a in (%WebSDKSampleList%) do ( 
   Call :ProcessSample %%a
))
GOTO:EOF

:ProcessSample
	@echo.    
	@echo.    
	@echo -------------------------------------------------------------------------------------------------------------------------------
	@echo Processing Web SDK Sample Project '%1'
	@echo -------------------------------------------------------------------------------------------------------------------------------
	@echo.    
	@echo.    

	set SampleName=%1
	set DestinationWebRoot=%DestinationSamplesRoot%\%SampleName%\ValuedPartner.TU.Web
	
	REM Parameters to :ProcessFolder are as follows:
	REM                  
	REM   FolderName           (based on DestinationWebRoot defined above)
	REM   DirectoriesToExclude (includes the /XD parameter to robocopy)
	REM 
    Call :ProcessFolder Views                      ""
    Call :ProcessFolder Scripts                    "/XD TestUtil Test"
    Call :ProcessFolder Customization              ""
    Call :ProcessFolder Content                    "/XD Images\nav\PM"
    Call :ProcessFolder Assets                     ""
    Call :ProcessFolder Areas\Core\Scripts         ""
    Call :ProcessFolder Areas\Core\Views           "/XD Wizard FakeAuthentication"
    Call :ProcessFolder Areas\Shared\Scripts       "/XD Revaluation IC"
	Call :ProcessFolder Areas\Shared\Views         ""
	
	@echo -------------------------------------------------------------------------------------------------------------------------------
GOTO:EOF

:ProcessFolder
	set Source=%SourceWebRoot%\%1
	set Dest=%DestinationWebRoot%\%1
	set DirectoriesToIgnore=%~2
	@echo    Folder:      %1
	@echo    Source:      %Source% 
	@echo    Destination: %Dest%
	@echo.
	@echo    Removing Folder : %Dest%
	RD /S /Q %Dest% 
	@echo    Copying...
	robocopy %Source% %Dest% *.* /E %DirectoriesToIgnore% 
GOTO:EOF
