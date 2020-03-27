msbuild -restore /t:build /p:Configuration=debug /p:Platform=x86 .\src\KayMcCormick.Dev\Analysis.sln -v:q -clp:ErrorsOnly -warnAsError:MSB3276 -nologo %1 %2 %3 %4
