﻿# Design DesktopSearch.Core

## Domain Entities

### FolderProcessor

Responsible for walking a folder (recursively) and indexing all its files using the configured strategy (code, documents)

### CodeFolderProcessor

Takes the contents of e.g. a files and extracts relevant index information using Roslyn

### DocumentFolderProcessor
Takes the contents of e.g. a files and sends it to ElasticSearch directly so that it is processed by Tika

