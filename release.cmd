@echo off
copy .\bin\Release\MCEControl.exe ..\build
copy .\MCEControl.commands ..\build
copy .\Installer\license.txt ..\build
copy .\Installer\MCEController.nsi ..\build
start ..\build