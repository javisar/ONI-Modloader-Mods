@echo off

xcopy "%PROGRAMFILES(X86)%\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Unity*.dll" .\ /Y
xcopy "%PROGRAMFILES(X86)%\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp*.dll" .\ /Y
xcopy "%PROGRAMFILES(X86)%\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll" .\ /Y

