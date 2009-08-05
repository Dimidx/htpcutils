rem call "$(SolutionDir)compile" $(OutDir) "$(ProjectDir)" "$(SolutionDir)" $(PlatformName)
rem @echo off
%1ROBOCOPY.EXE "%1Release Files" "%2\" /MIR /R:10 /A-:RASH /XD .svn /XD Cache /XF Settings.xml >nul