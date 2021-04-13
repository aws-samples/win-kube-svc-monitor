## Install the service
`New-Service -Name "KubeSvcMonitor" -Description "Monitors Kubernetes services and restarts them if they are stopped" -BinaryPathName <full-path-of-exe>`  
**or**  
`sc create "KubeSvcMonitor"  binpath=<full-path-of-exe>`

## Start the service
`Start-Service -Name "KubeSvcMonitor"`  
**or**  
`sc start "KubeSvcMonitor"`


## Stop the service
`Stop-Service -Name "KubeSvcMonitor"`  
**or**  
`sc stop "KubeSvcMonitor"`

## Remove the service
`Remove-Service -Name "KubeSvcMonitor"`  
**or**  
`sc delete "KubeSvcMonitor"`

# Logging
KubeSvcMonitor logs to the Event Viewer. Check `Applications and Services Logs -> KubeSvcMonitor'
