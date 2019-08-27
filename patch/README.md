
# Patch

## New Grid Framework Patch for the Flat Code Type 

It does not happen every release, but every once in a while when the Sage 300 application is
complete and the Web SDK is being finalized, an issue is discovered that must be patched
for developers on the current release not to be impacted.

An issue was recently discovered in the grid framework where overriding the Sage Grid
Controller did not result in the custom controller being invoked. As a result, the 
developer could not override the CRUD operations for a grid on the server side.

> This patch will be part of the Sage 300 2020.1 release as part of the codebase.

Please follow the instructions if developing a Flat Code Type screen that contains a
grid.

### Server Side

The **Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.dll** must be copied to the 
developers local Sage 300 installation **Web/bin** folder:

1. Copy the existing **Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.dll** from the 
   **../Online/Web/bin** folder to a safe location.
2. Copy the **patch/Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.dll** from the Web
   SDK to the **../Online/Web/bin** folder and override the existing file.

> This modified assembly file is required to be placed in the local 
  installation since the project references for this file point to the 
  local installation.

### Client Side

The **Sage.CA.SBS.ERP.Sage300.Common.AccpacGrid.js** must be copied to the 
developers local Sage 300 installation **Web/Areas/Core/Scripts** folder:

1. Copy the existing **Sage.CA.SBS.ERP.Sage300.Common.AccpacGrid.js** from the 
   **../Online/Web/Areas/Core/Scripts** folder to a safe location.
2. Copy the **patch/Sage.CA.SBS.ERP.Sage300.Common.AccpacGrid.js** from the Web
   SDK to the **../Online/Web/Areas/Core/Scripts** folder and override the 
   existing file.
3. To ensure the correct JavaScript file is invoked, re-start IIS and clear
   the browser cache.

> This modified JavaScript file has been bundled into the Code Generation and
  Upgrade Wizards. Therefore, this modified file is already present in the 
  Visual Studio environment. However, this patch is required only to update 
  the local installation when the Visual Studio compile mode is set to 
  **Release** for copying to the local installation.


