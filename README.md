# Zendesk Lead
> This is kind of a "hacky" method for creating a new Zendesk lead based on an email received.

This was created more as a test for using the  <a href ="https://developers.getbase.com/docs/rest/reference"> Zendesk API</a>. Using <a href="https://developers.google.com/gmail/api/quickstart/dotnet"> Googles Gmail API </a> we look at a specific email address every minute to determine if a specific email has been received, either by sender, date, or Subject line. From this we can determine if a new Zendesk Deal or lead needs to be created. Once the type of email is determined, then we start a process of making a local bash script from parsed information from the Email, From here we connect to an Azure Database and query the information we need. We then run the bash script locally and return a .json string, determining if the script ran successfully or returning information about line items or users. 

## Development setup

To properly run this application you will need to create an access token for Zendesk and enable the Gmail API and download credentials.json to be put into the project directory. For now I just have filler variable names like `{varName}` throughout the bash files and in the `EmailMain.cs`, then the project SHOULD work :smile:!

nuget: 
```sh
PM> Install-Package Google.Apis.Gmail.v1
```

## Release History

* 0.0.1
    * "Hacky" Work in progress
