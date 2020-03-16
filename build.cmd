msbuild -restore /t:build /p:Configuration=debug /p:Platform=x86 .\src\KayMcCormick.Dev\Deployment.sln -v:q -clp:ErrorsOnly -warnAsError:MSB3276 -nologo
