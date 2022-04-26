@echo off
REM
REM This batch file will remove the following folders and files 
REM from each Web SDK sample.
REM
REM -----------------------------------------------------------------
REM  Folders
REM -----------------------------------------------------------------
REM
REM 	.vs
REM 	packages
REM     ValuedPartner.TU.BusinessRepository\bin
REM     ValuedPartner.TU.BusinessRepository\obj
REM     ValuedPartner.TU.Interfaces\bin
REM     ValuedPartner.TU.Interfaces\obj
REM     ValuedPartner.TU.Models\bin
REM     ValuedPartner.TU.Models\obj
REM     ValuedPartner.TU.Resources\bin
REM     ValuedPartner.TU.Resources\obj
REM     ValuedPartner.TU.Services\bin
REM     ValuedPartner.TU.Services\obj
REM     ValuedPartner.TU.Web\___DEPLOY___
REM     ValuedPartner.TU.Web\bin
REM     ValuedPartner.TU.Web\logs
REM     ValuedPartner.TU.Web\obj
REM		ValuedPartner.TU.Web\Areas\Shared\Scripts\out
REM
REM -----------------------------------------------------------------
REM  Individual Files
REM -----------------------------------------------------------------
REM
REM		UpgradeLog.txt
REM		ValuedPartner.TU.Web\Antlr3.Runtime.dll
REM		ValuedPartner.TU.Web\MergeISVProject.exe
REM		ValuedPartner.TU.Web\Newtonsoft.Json.dll
REM		ValuedPartner.TU.Web\WebGrease.dll
REM		ValuedPartner.TU.Web\WG.EXE
REM

set Drive=D:
set Branch=Develop
set SamplesRoot=%Drive%\Projects\Sage300-SDK\%Branch%\Samples

@echo Beginning WebSDK Sample Cleanup...
call:CleanSample "ClearStatistics"
call:CleanSample "Receipt"
call:CleanSample "SegmentCodes"
call:CleanSample "SourceCodes"
call:CleanSample "SourceJournalProfiles"
call:CleanSample "SourceJournalProfilesReports"
call:CleanSample "TaxAuthorities"
@echo End of WebSDK Sample Cleanup.
goto:eof

:CleanSample
REM -----------------------------------------------------------------
REM  Folders
REM -----------------------------------------------------------------
RD /S /Q %SamplesRoot%\%~1\.vs
RD /S /Q %SamplesRoot%\%~1\packages
RD /S /Q %SamplesRoot%\%~1\ValuedPartner.TU.BusinessRepository\bin
RD /S /Q %SamplesRoot%\%~1\ValuedPartner.TU.BusinessRepository\obj
RD /S /Q %SamplesRoot%\%~1\ValuedPartner.TU.Interfaces\bin
RD /S /Q %SamplesRoot%\%~1\ValuedPartner.TU.Interfaces\obj
RD /S /Q %SamplesRoot%\%~1\ValuedPartner.TU.Models\bin
RD /S /Q %SamplesRoot%\%~1\ValuedPartner.TU.Models\obj
RD /S /Q %SamplesRoot%\%~1\ValuedPartner.TU.Resources\bin
RD /S /Q %SamplesRoot%\%~1\ValuedPartner.TU.Resources\obj
RD /S /Q %SamplesRoot%\%~1\ValuedPartner.TU.Services\bin
RD /S /Q %SamplesRoot%\%~1\ValuedPartner.TU.Services\obj
RD /S /Q %SamplesRoot%\%~1\ValuedPartner.TU.Web\___DEPLOY___
RD /S /Q %SamplesRoot%\%~1\ValuedPartner.TU.Web\bin
RD /S /Q %SamplesRoot%\%~1\ValuedPartner.TU.Web\logs
RD /S /Q %SamplesRoot%\%~1\ValuedPartner.TU.Web\obj
RD /S /Q %SamplesRoot%\%~1\ValuedPartner.TU.Web\Areas\Shared\Scripts\out

REM -----------------------------------------------------------------
REM  Individual Files
REM -----------------------------------------------------------------
DEL /F %SamplesRoot%\%~1\UpgradeLog.txt
DEL /F %SamplesRoot%\%~1\ValuedPartner.TU.Web\Antlr3.Runtime.dll
DEL /F %SamplesRoot%\%~1\ValuedPartner.TU.Web\MergeISVProject.exe
DEL /F %SamplesRoot%\%~1\ValuedPartner.TU.Web\Newtonsoft.Json.dll
DEL /F %SamplesRoot%\%~1\ValuedPartner.TU.Web\WebGrease.dll
DEL /F %SamplesRoot%\%~1\ValuedPartner.TU.Web\WG.EXE
goto:eof
