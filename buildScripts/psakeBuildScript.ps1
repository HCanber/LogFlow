Properties {
  $build_dir = Split-Path $psake.build_script_file
  $root_dir = Normalize-Path "$build_dir\.."
  $src_dir =  "$root_dir\source"
  $packages_dir =  "$root_dir\packages"
  $release_dir = "$root_dir\_release"
  $sln_path = "$root_dir\LogFlow.sln"
  $Configuration = "Release"
  $nuget_spec = "$src_dir\LogFlow\LogFlow.nuspec"
}

FormatTaskName {
   param($taskName)
   $s="$taskName "
   write-host ($s + ("-"* (70-$s.Length))) -foregroundcolor Cyan
}

Task default -Depends nuget

Task nuget -Depends CompileNuget, GitLink, CreateNugetPackage

Task CompileNuget {
  Exec { msbuild $sln_path /p:Configuration=$Configuration /v:quiet /p:OutDir=$release_dir } 
}

Task GitLink {   #See https://github.com/GitTools/GitLink
  $gitlink_exe = FindFile "$packages_dir\gitlink*" gitlink.exe
  Exec { &"$gitlink_exe" "$root_dir" -u "https://github.com/LogFlow/LogFlow" -ignore "LogFlow.Specifications,LogFlow.Examples"}
}

Task CreateNugetPackage {
  $nuget_exe = FindFile "$root_dir\.nuget\" nuget.exe
  Exec { &$nuget_exe "pack" "$nuget_spec" }
}



function Normalize-Path([string]$Path){
  [System.IO.Path]::GetFullPath($Path)
}

function FindFile([string]$Path,[string]$FileName){
  (Get-ChildItem "$Path" -Filter "$FileName" -Recurse | Sort-Object -Property FullName -Descending | Select-Object -First 1).FullName
}