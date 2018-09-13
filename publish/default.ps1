properties {
    $nuget_api_key = $nuget_api_key
    $version = $version
    $root = Resolve-Path "..\"
    $nuget = "..\packages\NuGet.CommandLine.4.7.1\tools\NuGet.exe"
    $slnPath = Join-Path $root "EasyTests.sln"
    $easyTestsProj = "..\EasyTests\EasyTests.csproj"
    $easyTestsPackDir = ".\EasyTestsPack"
    $msbuild = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\\MSBuild\15.0\bin\MSBuild.exe"
}

task default -depends Clean, Build, Pack, Publish

task Clean {
    if(Test-Path $easyTestsPackDir){
        rm $easyTestsPackDir -Recurse
    }
}

task Build {
    exec { & $msbuild $slnPath "/p:Configuration=Release"}
}

task Pack {
    mkdir $easyTestsPackDir
    exec {
        & $nuget pack $easyTestsProj -version $version -outputdirectory $easyTestsPackDir -properties Configuration=Release
    }
}

task Publish {
    $easyTestPackName = (gci -Path $easyTestsPackDir *.nupkg).FullName
    
    exec {
        & $nuget push $easyTestPackName $nuget_api_key -Source https://api.nuget.org/v3/index.json
    }
}