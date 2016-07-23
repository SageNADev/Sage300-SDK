@echo off
setlocal
set copyDestParam=%1
set myDir=%~dp0

if [%copyDestParam%]==[] (
  echo Usage: %~n0.bat [Web project path]
  goto :EOF
)

set SRC_FILES_COPY_LIST=Assets\fonts\sagearmonyeicons.eot,Content\fonts\sagearmonyeicons.eot,Content\Images\loading_new.gif,Content\Images\ItemWizardWelcome.jpg,Content\Images\nav\bg_menu_1.jpg,Content\Images\nav\bg_menu_10.jpg,Content\Images\nav\bg_menu_11.jpg,Content\Images\nav\bg_menu_2.jpg,Content\Images\nav\bg_menu_3.jpg,Content\Images\nav\bg_menu_4.jpg,Content\Images\nav\bg_menu_5.jpg,Content\Images\nav\bg_menu_6.jpg,Content\Images\nav\bg_menu_7.jpg,Content\Images\nav\bg_menu_8.jpg,Content\Images\nav\bg_menu_9.jpg,Assets\images\ui\sprite_icons.png,Content\Images\arrow.png,Content\Images\bar_left.png,Content\Images\bar_middle.png,Content\Images\bar_right.png,Content\Images\editor.png,Content\Images\logo-img-holder.png,Content\Images\sage_300_logo.png,Content\Images\sprite.png,Content\Images\tick.png,Content\Images\toolBar_icons.png,Content\Images\toolBar_icons_disabled.png,Content\Images\tour_icon_sprite.png,Content\Images\vpf-logo.png,Content\Images\white-left-arrow-t.png,Content\Images\white-right-arrow-t.png,Content\jasmine\jasmine_favicon.png,Assets\fonts\sagearmonyeicons.ttf,Content\fonts\sagearmonyeicons.ttf,Assets\fonts\sagearmonyeicons.woff,Content\fonts\sagearmonyeicons.woff

goto :upgradeSDK %copyDestParam%

goto :EOF


:upgradeSDK
  setlocal
  set upgradeDest=%copyDestParam%
  echo Upgrade SDK for %upgradeDest%

  set /A XCOUNT=0
  for %%f in (%SRC_FILES_COPY_LIST%) do (
    call :copyToDest %%f
    set /A XCOUNT+=1
  )
  echo Files upgraded: %XCOUNT% 
  
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

  del %dest%
goto :EOF
