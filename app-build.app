name: fledit
branch: Release
project-name: OpenFL.Editor
flags: NO_INFO_TO_ZIP;NO_STRUCTURE

#Additional Build Info
include: %buildout%\%name%.exe.config

#Build Info
target: %buildout%\%name%.exe
output: .\docs\latest\%name%.zip
solution: .\src\%project-name%.sln
buildout: .\src\%project-name%\bin\%branch%
buildcmd: msbuild {0} /t:Build /p:Configuration=%branch%