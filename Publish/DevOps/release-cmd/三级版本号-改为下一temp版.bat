@echo off

::(x.1)get csproj
for /f "delims=" %%a in ('findstr /M /s /i /r "<pack> <publish>" "..\..\..\*.csproj"') do set "csproj=%%~a"
::echo %csproj%

::(x.2)get version
for /f "tokens=3 delims=><" %%a in ('type %csproj%^|findstr "<Version>.*Version"') do set version=%%a

:: set version=2.1.3
:: echo  %version%


::(x.3)get v1 v2 v3
for /f "tokens=1 delims=-" %%i in ("%version%") do set numVersion=%%i

:: v1 v2 v3
for /f "tokens=1 delims=." %%i in ("%numVersion%") do set v1=%%i
for /f "tokens=2 delims=." %%i in ("%numVersion%") do set v2=%%i
for /f "tokens=3 delims=." %%i in ("%numVersion%") do set v3=%%i


::(x.4)newVersion
set /a v3=1+%v3%
set newVersion=%v1%.%v2%.%v3%-temp
:: echo %newVersion%


 
::(x.5)���ù��� �滻csproj�ļ��еİ汾��
echo �Զ��޸İ汾�� [%version%]-^>[%newVersion%]
echo.

VsTool.exe replace -r --path "..\..\.." --file "*.csproj" --old "<Version>%version%</Version>" --new "<Version>%newVersion%</Version>"
VsTool.exe replace -r --path "..\..\.." --file "packages.config" --old "%version%" --new "%newVersion%"


::(x.6)���ù��� �滻docker���������еİ汾��
VsTool.exe replace -r --path "..\..\..\Publish\ReleaseFile\docker-image" --file "*.md" --old "%version%" --new "%newVersion%"


echo.
echo.
echo.
echo �Ѿ��ɹ��޸İ汾�� [%version%]-^>[%newVersion%]
pause