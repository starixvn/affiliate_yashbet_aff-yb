@echo off

setlocal ENABLEDELAYEDEXPANSION
set build_number=%1
if "%build_number%" == "" goto missing_build_number

..\..\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user "-target:..\..\packages\xunit.runner.console.2.4.1\tools\net452\xunit.console.exe" "-targetargs:..\Fanex.Affiliate.Admin.Core.UnitTests\bin\Debug\Fanex.Affiliate.Admin.Core.UnitTests.dll" -filter:"+[*]* -[Fanex.Affiliate.Admin.Core.UnitTests*]*" -output:Admin.UnitTestCoverage_%build_number%.xml -showunvisited -searchdirs:"..\Fanex.Affiliate.Admin.Core\bin\Debug"

..\..\packages\ReportGenerator.3.1.2\tools\ReportGenerator.exe "-reports:.\Admin.UnitTestCoverage_%build_number%.xml" "-targetdir:.\CoverageReport"

echo Success.
goto end

:missing_build_number
echo Please provide name of your test build number.
goto end

:error
echo Failure.

:end
echo Run Unit Test Coverage Done.