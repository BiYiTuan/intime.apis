rem --rem --自动安装服务脚本
@echo 正在安装服务

sc create "com.intime.messagelistener" binpath= "%~dp0..\com.intime.fashion.messagelistener" displayname= "com.intime.messagelistener"
net start "com.intime.messagelistener"

pause


