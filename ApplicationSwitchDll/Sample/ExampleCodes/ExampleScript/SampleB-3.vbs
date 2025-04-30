dim shell, fso
dim text, outputFile
set shell = CreateObject("WScript.Shell")
set fso = CreateObject("Scripting.FileSystemObject")

outputFile = shell.ExpandEnvironmentStrings("%USERPROFILE%\Desktop\test.txt")

set text = fso.OpenTextFile(outputFile, 8, True)
text.WriteLine "VBS BBBBBBBBBBBBBB"
text.Close