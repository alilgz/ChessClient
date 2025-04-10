
echo "smth"
echo "calculation"
"MY_VAR=$MyVariable" | Out-File -FilePath $env:GITHUB_ENV -Append
 
