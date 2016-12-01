setlocal

call "C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\vcvarsall.bat" x86_amd64
@setlocal enableextensions
@cd /d "%~dp0"
devenv /rebuild Release .\Sage300UIWizardPackage.sln /out build.log