<#
.Synopsis
   Kurzbeschreibung
.DESCRIPTION
   Lange Beschreibung
.EXAMPLE
   Beispiel für die Verwendung dieses Cmdlets
.EXAMPLE
   Ein weiteres Beispiel für die Verwendung dieses Cmdlets
#>
function Add-Folder
{
    [CmdletBinding()]
    Param
    (
        # Folder to index
        [Parameter(Mandatory=$true,
                   ValueFromPipelineByPropertyName=$true,Position=0)]
		#[ValidateScript(ScriptBlock={ test-path $_ })]
        [string]$Folder,

        # Define which index the objects shall be assigned to
        [Parameter(Mandatory=$true)]
		[ValidateSet('Documents', 'Books', 'Music', 'Films')]
		[string]$IndexAs
    )

    Begin
    {
		Add-Type -Path 'D:\Projects\GitHub\DesktopSearch\src\DesktopSearch.Core\bin\Debug\netstandard1.6\DesktopSearch.Core.dll'
    }
    Process
    {
		$cfg = New-Object DesktopSearch.Core.Configuration.ElasticSearchConfig
		New-Object DesktopSearch.Core.ElasticSearch.SearchService -ArgumentList $cfg
    }
    End
    {
    }
}

#. D:\Projects\GitHub\DesktopSearch\src\DesktopSearch.PS\AddFolderCmdlet.ps1
#Add-Folder -Folder c:\temp -IndexAs Documents