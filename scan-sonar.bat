SET /P _keystring= Please enter Sonar Key:
cd src
dotnet "../sonar-scanner/SonarScanner.MSBuild.dll" begin /k:"Swastika.Core" /o:"swastika-io" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="%_keystring%" /d:sonar.exclusions="**/wwwroot/lib/**,**/*.jpg,**/*.png,**/*.svg,**/*.xml,**/wwwroot/Content/**, **/wwwroot/js/plugins/**, **/*.txt, **/wwwroot/js/smoothscroll.js, **/wwwroot/app-portal/**, **/wwwroot/app/**, **/wwwroot/js/**, **/ViewModels/**, **/Migrations/**, **/Models/**" /d:sonar.coverage.exclusions="/**"

dotnet build

dotnet "../sonar-scanner/SonarScanner.MSBuild.dll" end /d:sonar.login="%_keystring%"
