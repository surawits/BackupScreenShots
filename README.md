# BackupScreenShots

## Description
This is a .NET 8 console application that automatically moves image files older than one month from a specified "Screenshots" folder in the user's Pictures directory to a backup folder organized by month. The application logs its activities and any errors encountered during execution.

## Features
- Automatically detects the user's "Screenshots" folder.
- Moves files older than one month to a backup folder.
- Organizes backup files by the month they were last modified.
- Logs actions and errors to both a log file and the console.

## Prerequisites
- .NET 8 SDK installed on your machine.
- Access to a Windows environment (or appropriate target runtime if deploying to other OS).

## Installation
1. Clone the repository:
   git clone https://github.com/surawits/BackupScreenShots.git
   cd BackupScreenShots

2. Restore the project dependencies:
   dotnet restore

3. Build the project:
   dotnet build

4. Publish the project to create a single executable file:
   dotnet publish -c Release -r win-x64 --self-contained

## Usage
1. Navigate to the publish directory:
   cd bin\Release\net8.0\win-x64\publish\

2. Run the application:
   BackupScreenShots.exe

3. The application will automatically check the "Screenshots" folder in your Pictures directory and move files older than one month to the backup folder.

## Logging
- Logs are written to the logs folder in the application directory.
- Each day's log is stored in a separate file, and the application maintains up to 30 log files.

## Setting Up Task Scheduler
To run the application automatically at scheduled intervals, you can set it up using Windows Task Scheduler:

1. **Open Task Scheduler**:
   - Press `Win + R`, type `taskschd.msc`, and hit `Enter`.

2. **Create a New Task**:
   - In the Task Scheduler, click on "Create Basic Task" in the right-hand Actions pane.

3. **Name Your Task**:
   - Give your task a name (e.g., "Backup Screenshots") and a description, then click "Next".

4. **Trigger the Task**:
   - Choose how frequently you want the task to run (e.g., Daily, Weekly). Click "Next" and set the specifics for the trigger (time, recurrence).

5. **Action**:
   - Select "Start a program" and click "Next".
   - Click "Browse" and navigate to the location of your executable (e.g., `C:\Path\To\Your\Project\bin\Release\net8.0\win-x64\publish\BackupScreenShots.exe`).
   - Click "Next".

6. **Finish**:
   - Review your settings and click "Finish" to create the task.

The application will now run automatically at the scheduled times you specified.

## Contributing
Contributions are welcome! Please open an issue or submit a pull request for any suggestions or improvements.

## License
This project is licensed under the MIT License.

## Acknowledgments
- Thank you to the contributors and the .NET community for their support and resources.
