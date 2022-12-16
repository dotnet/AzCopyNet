## azcopy remove

Delete blobs or files from an Azure storage account

### Synopsis

"Delete blobs or files from an Azure storage account"

```
azcopy remove [resourceURL] [flags]
```

### Examples

```

Remove a single blob by using a SAS token:

   - azcopy rm "https://[account].blob.core.windows.net/[container]/[path/to/blob]?[SAS]"

Remove an entire virtual directory by using a SAS token:
 
   - azcopy rm "https://[account].blob.core.windows.net/[container]/[path/to/directory]?[SAS]" --recursive=true

Remove only the blobs inside of a virtual directory, but don't remove any subdirectories or blobs within those subdirectories:

   - azcopy rm "https://[account].blob.core.windows.net/[container]/[path/to/virtual/dir]" --recursive=false

Remove a subset of blobs in a virtual directory (For example: remove only jpg and pdf files, or if the blob name is "exactName"):

   - azcopy rm "https://[account].blob.core.windows.net/[container]/[path/to/directory]?[SAS]" --recursive=true --include-pattern="*.jpg;*.pdf;exactName"

Remove an entire virtual directory but exclude certain blobs from the scope (For example: every blob that starts with foo or ends with bar):

   - azcopy rm "https://[account].blob.core.windows.net/[container]/[path/to/directory]?[SAS]" --recursive=true --exclude-pattern="foo*;*bar"

Remove specified version ids of a blob from Azure Storage. Ensure that source is a valid blob and versionidsfile which takes in a path to the file where each version is written on a separate line. All the specified versions will be removed from Azure Storage.

  - azcopy rm "https://[srcaccount].blob.core.windows.net/[containername]/[blobname]" "/path/to/dir" --list-of-versions="/path/to/dir/[versionidsfile]"

Remove specific blobs and virtual directories by putting their relative paths (NOT URL-encoded) in a file:

   - azcopy rm "https://[account].blob.core.windows.net/[container]/[path/to/parent/dir]" --recursive=true --list-of-files=/usr/bar/list.txt
   - file content:
	   dir1/dir2
	   blob1
	   blob2

Remove a single file from a Blob Storage account that has a hierarchical namespace (include/exclude not supported):

   - azcopy rm "https://[account].dfs.core.windows.net/[container]/[path/to/file]?[SAS]"

Remove a single directory from a Blob Storage account that has a hierarchical namespace (include/exclude not supported):

   - azcopy rm "https://[account].dfs.core.windows.net/[container]/[path/to/directory]?[SAS]"

```

### Options

```
      --delete-snapshots string   By default, the delete operation fails if a blob has snapshots. Specify 'include' to remove the root blob and all its snapshots; alternatively specify 'only' to remove only the snapshots but keep the root blob.
      --dry-run                   Prints the path files that would be removed by the command. This flag does not trigger the removal of the files.
      --exclude-path string       Exclude these paths when removing. This option does not support wildcard characters (*). Checks relative path prefix. For example: myFolder;myFolder/subDirName/file.pdf
      --exclude-pattern string    Exclude files where the name matches the pattern list. For example: *.jpg;*.pdf;exactName
      --force-if-read-only        When deleting an Azure Files file or folder, force the deletion to work even if the existing object is has its read-only attribute set
      --from-to string            Optionally specifies the source destination combination. For Example: BlobTrash, FileTrash, BlobFSTrash
  -h, --help                      help for remove
      --include-after string      Include only those files modified on or after the given date/time. The value should be in ISO8601 format. If no timezone is specified, the value is assumed to be in the local timezone of the machine running AzCopy. E.g. '2020-08-19T15:04:00Z' for a UTC time, or '2020-08-19' for midnight (00:00) in the local timezone. As of AzCopy 10.5, this flag applies only to files, not folders, so folder properties won't be copied when using this flag with --preserve-smb-info or --preserve-smb-permissions.
      --include-before string     Include only those files modified before or on the given date/time. The value should be in ISO8601 format. If no timezone is specified, the value is assumed to be in the local timezone of the machine running AzCopy. E.g. '2020-08-19T15:04:00Z' for a UTC time, or '2020-08-19' for midnight (00:00) in the local timezone. As of AzCopy 10.7, this flag applies only to files, not folders, so folder properties won't be copied when using this flag with --preserve-smb-info or --preserve-smb-permissions.
      --include-path string       Include only these paths when removing. This option does not support wildcard characters (*). Checks relative path prefix. For example: myFolder;myFolder/subDirName/file.pdf
      --include-pattern string    Include only files where the name matches the pattern list. For example: *.jpg;*.pdf;exactName
      --list-of-files string      Defines the location of a file which contains the list of files and directories to be deleted. The relative paths should be delimited by line breaks, and the paths should NOT be URL-encoded.
      --list-of-versions string   Specifies a file where each version id is listed on a separate line. Ensure that the source must point to a single blob and all the version ids specified in the file using this flag must belong to the source blob only. Specified version ids of the given blob will get deleted from Azure Storage.
      --permanent-delete string   This is a preview feature that PERMANENTLY deletes soft-deleted snapshots/versions. Possible values include 'snapshots', 'versions', 'snapshotsandversions', 'none'. (default "none")
      --recursive                 Look into sub-directories recursively when syncing between directories.
```

### Options inherited from parent commands

```
      --cap-mbps float                      Caps the transfer rate, in megabits per second. Moment-by-moment throughput might vary slightly from the cap. If this option is set to zero, or it is omitted, the throughput isn't capped.
      --log-level string                    Define the log verbosity for the log file, available levels: INFO(all requests/responses), WARNING(slow responses), ERROR(only failed requests), and NONE(no output logs). (default 'INFO'). (default "INFO")
      --output-level string                 Define the output verbosity. Available levels: essential, quiet. (default "default")
      --output-type string                  Format of the command's output. The choices include: text, json. The default value is 'text'. (default "text")
      --trusted-microsoft-suffixes string   Specifies additional domain suffixes where Azure Active Directory login tokens may be sent.  The default is '*.core.windows.net;*.core.chinacloudapi.cn;*.core.cloudapi.de;*.core.usgovcloudapi.net;*.storage.azure.net'. Any listed here are added to the default. For security, you should only put Microsoft Azure domains here. Separate multiple entries with semi-colons.
```

### SEE ALSO

* [azcopy](azcopy.md)	 - AzCopy is a command line tool that moves data into and out of Azure Storage.

###### Auto generated by spf13/cobra on 15-Dec-2022
