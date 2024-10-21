How It Works:
When the application runs, it automatically looks for a "Screenshots" folder inside the user's Pictures directory.
If the folder exists, the application proceeds to move files older than one month to the backup folder.
If the folder does not exist, the application logs an error and prompts the user to create the required folder.

Publish the Application

<code>dotnet publish -c Release -r win-x64 --self-contained</code>

After modifying the code, publish your application as mentioned earlier:
