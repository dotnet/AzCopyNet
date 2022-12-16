## azcopy login

Log in to Azure Active Directory (AD) to access Azure Storage resources.

### Synopsis

To be authorized to your Azure Storage account, you must assign the **Storage Blob Data Contributor** role to your user account in the context of either the Storage account, parent resource group or parent subscription.
This command will cache encrypted login information for current user using the OS built-in mechanisms.
Please refer to the examples for more information.

If you set an environment variable by using the command line, that variable will be readable in your command line history. Consider clearing variables that contain credentials from your command line history.  To keep variables from appearing in your history, you can use a script to prompt the user for their credentials, and to set the environment variable.

```
azcopy login [flags]
```

### Examples

```
Log in interactively with default AAD tenant ID set to common:
- azcopy login

Log in interactively with a specified tenant ID:

   - azcopy login --tenant-id "[TenantID]"

Log in by using the system-assigned identity of a Virtual Machine (VM):

   - azcopy login --identity

Log in by using the user-assigned identity of a VM and a Client ID of the service identity:
   
   - azcopy login --identity --identity-client-id "[ServiceIdentityClientID]"

Log in by using the user-assigned identity of a VM and an Object ID of the service identity:

   - azcopy login --identity --identity-object-id "[ServiceIdentityObjectID]"

Log in by using the user-assigned identity of a VM and a Resource ID of the service identity:
 
   - azcopy login --identity --identity-resource-id "/subscriptions/<subscriptionId>/resourcegroups/myRG/providers/Microsoft.ManagedIdentity/userAssignedIdentities/myID"

Log in as a service principal by using a client secret:
Set the environment variable AZCOPY_SPA_CLIENT_SECRET to the client secret for secret based service principal auth.

   - azcopy login --service-principal --application-id <your service principal's application ID>

Log in as a service principal by using a certificate and it's password:
Set the environment variable AZCOPY_SPA_CERT_PASSWORD to the certificate's password for cert based service principal auth

   - azcopy login --service-principal --certificate-path /path/to/my/cert --application-id <your service principal's application ID>

   Please treat /path/to/my/cert as a path to a PEM or PKCS12 file-- AzCopy does not reach into the system cert store to obtain your certificate. --certificate-path is mandatory when doing cert-based service principal auth.

Subcommand for login to check the login status of your current session.
	- azcopy login status 

```

### Options

```
      --aad-endpoint string           The Azure Active Directory endpoint to use. The default (https://login.microsoftonline.com) is correct for the public Azure cloud. Set this parameter when authenticating in a national cloud. Not needed for Managed Service Identity
      --application-id string         Application ID of user-assigned identity. Required for service principal auth.
      --certificate-path string       Path to certificate for SPN authentication. Required for certificate-based service principal auth.
  -h, --help                          help for login
      --identity                      Log in using virtual machine's identity, also known as managed service identity (MSI).
      --identity-client-id string     Client ID of user-assigned identity.
      --identity-object-id string     Object ID of user-assigned identity.
      --identity-resource-id string   Resource ID of user-assigned identity.
      --service-principal             Log in via Service Principal Name (SPN) by using a certificate or a secret. The client secret or certificate password must be placed in the appropriate environment variable. Type AzCopy env to see names and descriptions of environment variables.
      --tenant-id string              The Azure Active Directory tenant ID to use for OAuth device interactive login.
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
* [azcopy login status](azcopy_login_status.md)	 - Prints if you are currently logged in to your Azure Storage account.

###### Auto generated by spf13/cobra on 15-Dec-2022
