@pushd %~dp0

cd ..
.\packages\Pickles.CommandLine.2.20.1\tools\pickles.exe --feature-directory=.\SpecFlowTraining.GeekPizza.Specs\Features --output-directory=.\documentation --documentation-format=dhtml

@REM    --link-results-file=.\Specs\bin\Debug\TestResult.xml 

@cd %~dp0
