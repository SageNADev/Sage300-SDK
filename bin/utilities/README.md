# Utilities

## Generating JavaScript for Grids

The Grid Enhancement logic that has been included in the samples projects has dramatically changed
the JavaScript requirements of a grid implementation. The documentation for the Generating JavaScript 
for Grids utility does not reflect these changes nor does the Generating JavaScript for Grids utility
itself. However, it is still valid for generating JavaScript which does not use the enhancemnet logic.

## Merge ISV Project utility

An optional parameter (6th parameter) **/nodeploy** was added to this utility. This feature will allow
for the **Deploy** folder to be created with the necessary contents, but will not deploy to the local
Sage 300 installation when the solution is built in Release mode.