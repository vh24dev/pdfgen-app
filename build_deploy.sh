#!/usr/bin/env bash

# todo: server and runtime as param

dotnet build -r linux-x64
dotnet publish -c Release -r linux-x64 --self-contained true
cp -R bin/Release/netcoreapp2.2/linux-x64/publish pdfgen
tar -cf pdfgen.tar.gz pdfgen
scp pdfgen.tar.gz root@116.203.141.54:/home/docgen #vh24 prod
rm -R pdfgen
rm pdfgen.tar.gz
