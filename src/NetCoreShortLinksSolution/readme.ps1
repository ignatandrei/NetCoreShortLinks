$path = "../../README.md"
$path =Resolve-Path $path 
Write-Host $path 

$fileContent = gc $path

# Write-Host $fileContent.Length

For($i=0;$i -lt $fileContent.Length ; $i++){
    $line = $fileContent[$i];
    If ($line  -match '^<!--') {
        $state='comment'
        $fileContent[$i] =$null  # because `continue` doesn't work in a foreach-object
      }
      If ($line  -match '-->$') {
        $state='content'
        $fileContent[$i] =$null  
      }
      If ($state -eq 'comment') {
        $fileContent[$i] =$null  
      } 
}

$fileContent |Set-Content $path

$md = ConvertFrom-Markdown -Path $path
# $md
$md.Html | Out-File -Encoding utf8 .\readme.txt

$fileContent = gc "readme.txt"
For($i=0;$i -lt $fileContent.Length ; $i++){
    $line = $fileContent[$i];
	Write-Host $line
	if($line -match 'alt="Build Nuget"'){
		$line="";
	}		
	if($line -match '</h1>'){
		
		$line=$line -replace '(<h1.*">)' 
		$line=$line.replace('</h1>','')
	
	}
	if($line -match '</p>'){
		$line=$line.replace('</p>','')
		$line=$line -replace '(<p.*">)'
		$line=$line.replace('<p>','')
	}
	if($line -match '<p>'){
		$line=$line.replace('<p>','')
	}
	if($line -match '<pre><code class="language-csharp">'){
		
		$line=""
		
	}
	if($line -match '</code></pre>'){
		
		$line=""
		
	}
	if($line -match '</a>'){
	
		$line=$line.replace('</a>','')
		
	}
	Write-Host $line
	$fileContent[$i]=$line;
	
}

$fileContent | Out-File -Encoding utf8 .\readme.txt