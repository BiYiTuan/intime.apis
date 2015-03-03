rem --rem --自动安装服务脚本
@echo 正在安装服务

sc create "Yintai.Architecture.FileUploadServer" binpath= "%~dp0..\Yintai.Architecture.FileUploadServer" displayname= "Yintai.Architecture.FileUploadServer"
net start "Yintai.Architecture.FileUploadServer"

pause


