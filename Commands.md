### Publish

```sh
dotnet publish -c Release -r win-x64

dotnet publish -c Release -r win-x64 -p:PublishSingleFile=True -p:PublishTrimmed=True
```