
# DesktopSearch

Supports searching of documents and code using ElasticSearch.

## Setup

**Starting ElasticSearch from docker:**\
``` docker run -p 9200:9200 -d elasticsearch ```

## ElasticSearch
**Dumping all stored documents:**\
<http://localhost:9200/codesearch/_search?pretty=true&q=*:*>

**Simple Filtering**\
<http://localhost:9200/codesearch/_search?pretty=true&q=firstname:mar*>
- fieldname must be lowercase!
- fieldname can be omitted, then all fields are searched

## Using Repl over ElasticSearch
```c# 

#r "C:\Users\Mario\.nuget\packages\Newtonsoft.Json\9.0.1\lib\netstandard1.0\Newtonsoft.Json.dll"
#r "C:\Users\Mario\.nuget\packages\Elasticsearch.Net\2.4.2\lib\net46\Elasticsearch.Net.dll"
#r "C:\Users\Mario\.nuget\packages\NEST\2.4.2\lib\net46\Nest.dll" 

using Nest;
var settings = new ConnectionSettings(new Uri("http://localhost:9200"));
settings.DefaultIndex("codesearch");

var elastic = new ElasticClient(settings);

```

## Assemblies
![Assemblies](./Documents/Images/Assemblies.png)

## DataModel
![Model Elemenets](./Documents/Images/DataModel.png)