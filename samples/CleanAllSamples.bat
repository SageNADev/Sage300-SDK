@echo off
REM
REM This batch file will remove the following folders from each Web SDK sample
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

set Drive=D:
set SamplesRoot=%Drive%\Projects\Sage300-SDK\Develop\Samples

set SampleName=ClearStatistics
@echo Cleaning %SampleName%...
RD /S /Q %SamplesRoot%\%SampleName%\.vs
RD /S /Q %SamplesRoot%\%SampleName%\packages
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.BusinessRepository\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.BusinessRepository\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Interfaces\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Interfaces\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Models\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Models\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Resources\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Resources\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Services\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Services\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\___DEPLOY___
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\logs
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\obj

set SampleName=Receipt
@echo Cleaning %SampleName%...
RD /S /Q %SamplesRoot%\%SampleName%\.vs
RD /S /Q %SamplesRoot%\%SampleName%\packages
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.BusinessRepository\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.BusinessRepository\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Interfaces\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Interfaces\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Models\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Models\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Resources\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Resources\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Services\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Services\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\___DEPLOY___
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\logs
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\obj

set SampleName=SegmentCodes
@echo Cleaning %SampleName%...
RD /S /Q %SamplesRoot%\%SampleName%\.vs
RD /S /Q %SamplesRoot%\%SampleName%\packages
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.BusinessRepository\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.BusinessRepository\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Interfaces\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Interfaces\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Models\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Models\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Resources\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Resources\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Services\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Services\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\___DEPLOY___
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\logs
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\obj

set SampleName=SourceCodes
@echo Cleaning %SampleName%...
RD /S /Q %SamplesRoot%\%SampleName%\.vs
RD /S /Q %SamplesRoot%\%SampleName%\packages
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.BusinessRepository\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.BusinessRepository\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Interfaces\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Interfaces\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Models\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Models\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Resources\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Resources\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Services\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Services\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\___DEPLOY___
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\logs
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\obj

set SampleName=SourceJournalProfiles
@echo Cleaning %SampleName%...
RD /S /Q %SamplesRoot%\%SampleName%\.vs
RD /S /Q %SamplesRoot%\%SampleName%\packages
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.BusinessRepository\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.BusinessRepository\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Interfaces\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Interfaces\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Models\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Models\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Resources\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Resources\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Services\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Services\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\___DEPLOY___
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\logs
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\obj

set SampleName=SourceJournalProfilesReports
@echo Cleaning %SampleName%...
RD /S /Q %SamplesRoot%\%SampleName%\.vs
RD /S /Q %SamplesRoot%\%SampleName%\packages
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.BusinessRepository\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.BusinessRepository\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Interfaces\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Interfaces\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Models\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Models\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Resources\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Resources\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Services\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Services\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\___DEPLOY___
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\logs
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\obj

set SampleName=TaxAuthorities
@echo Cleaning %SampleName%...
RD /S /Q %SamplesRoot%\%SampleName%\.vs
RD /S /Q %SamplesRoot%\%SampleName%\packages
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.BusinessRepository\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.BusinessRepository\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Interfaces\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Interfaces\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Models\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Models\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Resources\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Resources\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Services\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Services\obj
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\___DEPLOY___
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\bin
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\logs
RD /S /Q %SamplesRoot%\%SampleName%\ValuedPartner.TU.Web\obj



