## KubeSvcMonitor
Kubelet and Kube-Proxy on Windows don't have an action if the Windows service stops, resulting in the worker node being "Not Ready."

KubeSvcMonitor is a simple Windows Service that monitors Kubelete and Kube-Proxy service status. In case one or both services stops, KubeSvcMonitor will restart the services within 30 seconds.

## Install the service
`New-Service -Name "KubeSvcMonitor" -Description "Monitors Kubernetes services and restarts them if they are stopped" -BinaryPathName <full-path-of-exe>`  
**or**  
`sc create "KubeSvcMonitor"  binpath=<full-path-of-exe>`

The binpath must contain the following files:

 - KubeSvcMonitor.exe 
 - KubeSvcMonitor.exe.config

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
