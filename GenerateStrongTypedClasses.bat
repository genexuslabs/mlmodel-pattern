@echo off

setlocal
if not (%1. == .) set GX_SDK_DIR=%1
if not (%2. == .) set GX_PROGRAM_DIR=%2

set CodeGenPath=%GX_SDK_DIR%\Patterns\CodeGen
set TargetPath=GeneXus.Patterns.MLModel\GeneXus.Patterns.MLModel\Model

echo --- Generating Strong Typed Classes
if not exist %TargetPath% mkdir %TargetPath% 
"%CodeGenPath%\PatternCodeGen.exe" MLModel.Pattern ^
	%TargetPath%\MLModelInstance.cs ^
	%TargetPath%\MLModelSettings.cs ^
	%TargetPath%\MLModelObjects.cs

if not (%1. == .) set GX_SDK_DIR=
if not (%2. == .) set GX_PROGRAM_DIR=
