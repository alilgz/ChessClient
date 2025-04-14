
echo "smth"
echo "calculation"
$MyVariable ="TEST VAL1111"
"MY_VAR=$MyVariable" | Out-File -FilePath $env:GITHUB_ENV -Append
 
