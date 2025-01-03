# Specify the path to your Visual Studio project
$projectPath = $PWD.Path

# Check if the project path exists
if (Test-Path $projectPath) {
    Write-Host "Cleaning bin and obj folders in $($projectPath)..."
    # Call the function to remove bin and obj folders
    Get-ChildItem -Path $Path -include bin,obj -Recurse | ForEach-Object ($_) { Remove-Item $_.FullName -Force -Recurse }
    Write-Host "Cleanup complete."
} else {
    Write-Host "Error: Project path $($projectPath) not found."
}