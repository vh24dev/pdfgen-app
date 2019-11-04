# vh24-pdf-generation

#### Build
- Install .NET Core and CLI - https://docs.microsoft.com/en-us/dotnet/core/
- Clone repo
- Build via `dotnet build` or `dotnet build -r <runtime>`, e.g., `dotnet build -r linux-x64`. See https://docs.microsoft.com/en-us/dotnet/core/rid-catalog for a runtime catalog.

#### Deploy
- Create a self-contained app via `dotnet publish -c Release -r <runtime> --self-contained true`, e.g., `dotnet publish -c Release -r linux-x64 --self-contained true`.
- Find the `publish` folder and copy it on to the server. Usually found in `<project folder>/bin//Release/netcoreapp2.2/<runtime>/publish`, e.g., `dotnet_pdf/bin/Release/netcoreapp2.2/linux-x64/publish`.

Script `build_deploy.sh` has the basic steps to follow.

#### Run
- From source (project folder) `- You can run from the project folder with `dotnet run`. See https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-run for more info.`
- To run the self-contained app (publish folder), do `./<publish folder>/dotnet_pdf`.

#### Usage
`dotnet_pdf <path to docx> [path to pdf]`

Writes to `<path to docx>.pdf` if no second argument provided.
Note: You can use the `PDFGEN_CUSTOM_FONT_PATH` env var to specify a custom fonts folder. By default it uses `<publish/project folder>/assets/custom_fonts`.
`Bell-MT` are included.

