
echo "smth"
echo "calculation"
$MyVariable ="- some data here -"
"MY_VAR=$MyVariable" | Out-File -FilePath $env:GITHUB_ENV -Append
 
