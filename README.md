A simple command line tool that lets you set the volume of running applications.

### Sample Phantom Dust batch script:
```shell
start "C:\Windows\explorer.exe" shell:AppsFolder\Microsoft.MSEsper_8wekyb3d8bbwe!App
start <PATH_TO_PhantomDustVolumeSetter.exe> --volume <YOUR_VOLUME_LEVEL_HERE>
```

### Example command line usages:
|command|description|
|---|---|
|`PhantomDustVolumeSetter.exe --help`|prints help|
|`PhantomDustVolumeSetter.exe --volume 75`|sets the volume of PDUWP.exe to 75% of your speaker volume (e.g. if your speaker volume is 24, then PDUPW will be set to 18)|
|`PhantomDustVolumeSetter.exe --volume 25 --processName "myapp.exe"`|sets the volume of myapp.exe to 25% of your speaker volume|