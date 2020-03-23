logman.exe query providers
# as Markdown
<#
$result = . "c:\Windows\System32\logman.exe" query providers
$list = New-Object "System.Collections.Generic.List[string]";
$list.Add("|Provider | GUID |")
$list.Add("|---- | ---- |")
$result `
| where {$_} `
| select -Skip 2 `
| %{
$data = $_ -split "{";
$list.Add("|" + $data[0].TrimEnd() + "|" + "{" + $data[1] + "|");
}
$list.RemoveAt($list.Count -1)
$list
#>
