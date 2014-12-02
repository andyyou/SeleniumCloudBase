@echo off
echo %U3_APP_DATA_PATH%
set letter=%U3_APP_DATA_PATH:~0,2%
%letter%
cd %U3_APP_DATA_PATH%
firefox.exe -Profile "profile/"