@echo off

setlocal
set CodeGenPath="%GX_SDK_DIR%"\Patterns\CodeGen
set TargetPath=GeneXus.Patterns.MLModel\GeneXus.Patterns.MLModel\Model

echo --- Generating Strong Typed Classes
%CodeGenPath%\PatternCodeGen.exe MLModel.Pattern ^
	%TargetPath%\MLModelInstance.cs ^
	%TargetPath%\MLModelSettings.cs ^
	%TargetPath%\MLModelObjects.cs

