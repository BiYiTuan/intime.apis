rem --rem --自动安装服务脚本
@echo 正在安装服务

sc create "com.intime.jobscheduler" binpath= "%~dp0..\com.intime.jobscheduler" displayname= "com.intime.jobscheduler"
net start "com.intime.jobscheduler"

pause


