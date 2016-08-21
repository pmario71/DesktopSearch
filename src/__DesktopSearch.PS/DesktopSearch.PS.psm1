[string]$elastic = 'http://localhost:9200/'
[string]$desktopsearch = 'http://localhost:5000/'

class Ctx
{
    [System.Diagnostics.Process]$Docker
    [System.Diagnostics.Process]$DesktopServiceAPI
}

<#
	My Function
#>
#function Get-Function {
#	$res = wget -Uri 'http://localhost:9200/docsearch/_stats' -Method Get
#}

<#
.Synopsis
   Starts all services required for running DesktopSearch.
.DESCRIPTION
   Lange Beschreibung
.EXAMPLE
   Beispiel für die Verwendung dieses Cmdlets
.EXAMPLE
   Ein weiteres Beispiel für die Verwendung dieses Cmdlets
#>
function Start-DesktopSearchService
{
    [CmdletBinding()]
    [Alias()]
    Param
    (
    )

    Begin
    {
    }
    Process
    {
        $ctx = New-Object Ctx

        $docker_error = (Join-Path $env:temp elastic_docker.txt)
        $desktopsearch_error = (Join-Path $env:temp desktopsearch.api.txt)

        # start elastic search using docker
        $ctx.Docker = Start-Process 'docker' -ArgumentList 'run -p 9200:9200 -d elasticsearch' -RedirectStandardError $docker_error
        
        # start desktopsearch.api
        $ctx.DesktopServiceAPI = Start-Process 'dotnet' -ArgumentList 'run -p \'D:\Projects\GitHub\DesktopSearch\src\DesktopSearch.API\'' -RedirectStandardError $desktopsearch_error

        return $ctx
    }
    End
    {
    }
}

<#
.Synopsis
   Starts all services required for running DesktopSearch.
.DESCRIPTION
   Lange Beschreibung
.EXAMPLE
   Beispiel für die Verwendung dieses Cmdlets
.EXAMPLE
   Ein weiteres Beispiel für die Verwendung dieses Cmdlets
#>
function Stop-DesktopSearchService
{
    [CmdletBinding()]
    [Alias()]
    Param
    (
      [ValidateNotNull]
      [Ctx]$Context
    )

    Begin
    {
    }
    Process
    {
        $Context.DesktopServiceAPI.Kill()

        # start elastic search using docker
        $ docker run -p 9200:9200 -d elasticsearch

        # start desktopsearch.api
        $ dotnet run -p 'D:\Projects\GitHub\DesktopSearch\src\DesktopSearch.API'
    }
    End
    {
    }
}