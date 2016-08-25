# Design DesktopSearch.Core

## Domain Entities

| Entity          | Description                                                                                    |
|-----------------|------------------------------------------------------------------------------------------------|
| FolderProcessor | Responsible for walking a folder (recursively) and indexing all its files using the configured strategy (code, documents)|
| CodeIndexer     | Takes the contents of e.g. a files and extracts relevant index information using Roslyn        |
| DocumentIndexer | Takes the contents of e.g. a files and sends it to ElasticSearch directly so that it is processed by Tika |

