function Invoke-UpdateAppveyorVersion
{
	$build_ver = $env:APPVEYOR_BUILD_VERSION
	$build_number = $env:APPVEYOR_BUILD_NUMBER 

	$fulldate = Get-Date -Format g
	$year = Get-Date -format yy
	$julianYear = $year.Substring(0)
	$dayOfYear = (Get-Date).DayofYear
	$julianDate = $julianYear + "{0:D3}" -f $dayOfYear

	$jul_index = $build_ver.IndexOf("julian")
	$build_ver = $build_ver.Replace("julian","$julianDate")

	Update-AppveyorBuild -Version "$build_ver"
}

function Invoke-UpdateAssemblyVersion
{
	$fulldate = Get-Date -Format g
    $productVersion = $env:APPVEYOR_BUILD_VERSION
	# it is possible you get 1.2.3.4 or even 1.2.3.4.5 and now 1.2.3.4-5
	
    if ($productVersion -eq $null)
    {
        $productVersion = "1.1.1.1"
    }
	
    if ($env:APPVEYOR)
    {
        $buildMachineName = "APPVEYOR"
    }
    else
    {
        $buildMachineName = $env:computername
    }	
      
	$SrcPath = "."
	Write-Verbose "SrcPath is $SrcPath" -Verbose
    Write-Verbose "Executing Update-AssemblyInfoVersionFiles in path $SrcPath for product version Version $productVersion"  -Verbose

	#split product version in SemVer language
	$versions = $productVersion.Split('.')
	Try{
		$major = "0"
		$minor = "0"
		$patch = "0"
		$patch_build = "0"
		$major = $versions[0]
		$minor = $versions[1]
		$patch = $versions[2]
		# yes we are going to loose the build of day!
		$patch_build = $versions[3] -replace "[\-].*",""
	}
	Catch{
		Write-Verbose "Your version ($productVersion) should at least have 4 parts seperated by dot. Assuming the rest as zero!" -Verbose
	}
	
	$ms_version = "MS$major.$minor.$patch.$patch_build"
	$internal_version = "$major.$minor.$patch.$patch_build"
	
	if ($env:APPVEYOR_REPO_COMMIT  -eq $null)
	{
		$commit_id = "localcommit -"
	}
	else
	{
		$commit_id = $env:APPVEYOR_REPO_COMMIT
		$commit_id = $commit_id.Substring(0,7)
	}

	if ($env:APPVEYOR_REPO_BRANCH -eq $null){
		$branch_name = "some-local-branch".ToUpper()
	}
	else{
		$branch_name = $env:APPVEYOR_REPO_BRANCH.ToUpper()
	}
	
	$assemblyFileVersion = $internal_version
     
    Write-Verbose "Transformed Assembly File Version is $assemblyFileVersion" -Verbose
    Write-Verbose "Transformed Assembly Title is : $ms_version ($commit_id) built from $branch_name by $buildMachineName on $fulldate. Full version is $productVersion." -Verbose
 
    $AllVersionFiles = Get-ChildItem $SrcPath inContactVersion.* -recurse
	
    foreach ($file in $AllVersionFiles) 
    { 
		#version replacements
        (Get-Content $file.FullName) |
        %{$_ -replace 'AssemblyTitle\(".*"\)', "AssemblyTitle(""$ms_version ($commit_id) built from $branch_name by $buildMachineName on $fulldate. Full version is $productVersion."")" } |
        %{$_ -replace '(#define\sVER_FILE_DESCRIPTION_STR\s+)"(.*)"', "`$1 ""$ms_version ($commit_id) built from $branch_name by $buildMachineName on $fulldate. Full version is $productVersion.""" } |
        %{$_ -replace 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)', "AssemblyFileVersion(""$assemblyFileVersion"")" } |
        %{$_ -replace '(#define\sVER_FILE_VERSION_STR\s+)"(.*)"', "`$1 ""$assemblyFileVersion""" } |
		Set-Content $file.FullName -Force
    }
  
    $AllVersionFiles = Get-ChildItem $SrcPath *.wxs -recurse
	
    foreach ($file in $AllVersionFiles) 
    { 
		#version replacements
        (Get-Content $file.FullName) |
        %{$_ -replace '\*INCONTACT_BUILD_LABEL\*', "$ms_version ($commit_id) built from $branch_name by $buildMachineName on $fulldate. Full version is $productVersion." } |
        %{$_ -replace '111.111.111.111', "$assemblyFileVersion" } |
		Set-Content $file.FullName -Force
    }  
}

     