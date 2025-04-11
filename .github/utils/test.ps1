
echo "smth"
echo "calculation"
$MyVariable =""
"MY_VAR=$MyVariable" | Out-File -FilePath $env:GITHUB_ENV -Append
 
