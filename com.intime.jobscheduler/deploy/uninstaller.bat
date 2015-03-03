@echo Ð¶ÔØWindowService

net stop "com.intime.jobscheduler"
sc delete "com.intime.jobscheduler"

@echo ³É¹¦£¡
pause
