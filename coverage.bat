OpenCover.Console.exe -register:user -target:"nunit3-console.exe" -targetargs:".\tests\IsThereAnyNews.Services.Tests\bin\Debug\IsThereAnyNews.Services.Tests.dll" -filter:+[*]* -output:.\coverage\coverage.xml
ReportGenerator.exe -reports:.\coverage\coverage.xml -targetdir:coverage  



