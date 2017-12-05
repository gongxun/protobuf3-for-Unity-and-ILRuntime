@echo off
for %%i in (*.proto) do (
   echo gen %%~nxi...
   tool\protoc.exe --csharp_out=OutputC#  %%~nxi)

echo finish... 
pause