@echo off
setlocal
set copyDestParam=%1
set myDir=%~dp0

if [%copyDestParam%]==[] (
  echo Usage: %~n0.bat [Web project path]
  echo e.g. %~n0.bat c:\dev\ValuedPartnerApp\ValuedPartner.Web
  goto :EOF
)

set SRC_FILES_COPY_LIST=AccpacDotNetVersion.props,ValuedPartner.Web.csproj,Enums\UserInterfaceMode.cs,Areas\Core\Views\Error\ICTNotAvailable.cshtml,Areas\Core\Views\Find\Find.cshtml,Areas\Core\Views\Shared\_OptionsMenu.cshtml,Areas\Core\Views\Shared\_OptionsMenuDisableCheck.cshtml,Areas\Core\Views\Shared\LabelMenuPopup.cshtml,Areas\Shared\Views\DocumentHistory\_Localization.cshtml,Areas\Shared\Views\Shared\_Localization.cshtml,Areas\Shared\Views\Shared\_LocalizedLayout.cshtml,Views\Partials\_FeatureTour.cshtml,Views\Partials\_Footer.cshtml,Views\Partials\_MenuUrl.cshtml,Views\Partials\_QuickMenu.cshtml,Views\Portal.cshtml,Views\Shared\_GenericLayout.cshtml,Views\Shared\_Layout.cshtml,Views\UnsupportedBrowser\Error.cshtml,Assets\styles\css\default.min.css,Content\jasmine\jasmine.css,Content\Styles\base.css,Content\Styles\custom.css,Content\Styles\default.css,Content\Styles\Sage.CA.SBS.ERP.Sage300.Core.Portal.css,Content\Styles\Sage.CA.SBS.ERP.Sage300.StandAlone.css,Content\Styles\Sage.CA.SBS.ERP.Sage300.Vpf.css,Content\Styles\Sage.CA.SBS.ERP.Sage300.Widget.css,Areas\Core\Scripts\ExportImport\Sage.CA.SBS.ERP.Sage300.Common.Plugin.Export.js,Areas\Core\Scripts\Process\Sage.CA.SBS.Sage300.Common.Process.js,Areas\Core\Scripts\Sage.CA.SBS.ERP.Sage300.Common.BatchGridHelper.js,Areas\Core\Scripts\Sage.CA.SBS.ERP.Sage300.Common.FinderGrid.js,Areas\Core\Scripts\Sage.CA.SBS.ERP.Sage300.Common.OptionalFields.js,Areas\Core\Scripts\Sage.CA.SBS.ERP.Sage300.Common.Plugin.Finder.js,Areas\Shared\Scripts\Sage.CA.SBS.ERP.Sage300.Common.Customization.js,Areas\Shared\Scripts\Sage.CA.SBS.ERP.Sage300.Common.Global.js,Areas\Shared\Scripts\Sage.CA.SBS.ERP.Sage300.Common.iFrameHelper.js,Areas\Shared\Scripts\Sage.CA.SBS.ERP.Sage300.Common.InquiryGrid.js,Areas\Shared\Scripts\Sage.CA.SBS.ERP.Sage300.Common.KendoHelpers.js,Areas\Shared\Scripts\Sage.CA.SBS.ERP.Sage300.Common.LabelMenuHelper.js,Areas\Shared\Scripts\Sage.CA.SBS.ERP.Sage300.Common.SessionTimeOut.js,Scripts\Help\Help.js,Scripts\jasmine\boot.js,Scripts\jasmine\console.js,Scripts\jasmine\jasmine-html.js,Scripts\jasmine\jasmine.js,Scripts\Portal\TaskDock-Menu-BreadCrumb.js,Assets\styles\less\framework\buttons.less,Assets\styles\less\framework\form-header.less,Assets\styles\less\framework\forms.less,Assets\styles\less\framework\popovers.less,Assets\styles\less\framework\summary.less,Assets\styles\less\framework\tables.less,Assets\styles\less\framework\tabs.less,Assets\styles\less\framework\totals.less,Assets\styles\less\framework\type.less,Assets\styles\less\layouts\blank.less,Assets\styles\less\libs\glyphicons.less,Assets\fonts\sagearmonyeicons.svg,Content\fonts\sagearmonyeicons.svg

set DEST_FILE_DEL_LIST=Areas\Shared\Scripts\Sage.CA.SBS.Sage300.Common.SessionTimeOut.js

goto :upgradeSDK %copyDestParam%

goto :EOF


:upgradeSDK
  setlocal
  set upgradeDest=%copyDestParam%
  echo Upgrade SDK for %upgradeDest%

  rem Copy with norecursive set
  call :copyToDest Web.config norecursive
  
  rem Copy list of files
  set /A XCOUNT=0
  for %%f in (%SRC_FILES_COPY_LIST%) do (
    call :copyToDest %%f
    set /A XCOUNT+=1
  )
  echo Files upgraded: %XCOUNT%

  rem Delete files  
  set /A XCOUNT=0
  for %%d in (%DEST_FILE_DEL_LIST%) do (
    call :deleteFromDest %%d
    set /A XCOUNT+=1
  )
  echo Files deleted: %XCOUNT%
  
goto :EOF

:copyToDest
  setlocal
  set subPath=%1
  set src="%myDir%%subPath%"
  set recursiveCopyAll=*
  set cmd=xcopy /s/e/y
  if "%2"=="norecursive" (
    echo no recursive set
    set recursiveCopyAll=
    set cmd=copy /v/y
  )
  set dest="%upgradeDest%\%subPath%%recursiveCopyAll%"
  echo copy to %dest%
  %cmd% %src% %dest%
  
goto :EOF

:deleteFromDest
  setlocal
  set dest="%upgradeDest%\%1"
  echo %1
  del %dest%
goto :EOF
