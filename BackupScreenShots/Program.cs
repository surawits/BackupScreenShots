using System;
using System.IO;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;

class Program
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    static async Task Main(string[] args)
    {
        ConfigureNLog();  // Configure NLog

        // Automatically set the screenshots folder to the user's Pictures directory
        string screenshotsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Screenshots");

        // Validate the folder path
        if (!Directory.Exists(screenshotsFolder))
        {
            logger.Error($"The specified folder does not exist: {screenshotsFolder}");
            Console.WriteLine("The specified folder does not exist. Please check the path and create the Screenshots folder in your Pictures directory.");
            return;
        }

        string backupFolder = Path.Combine(screenshotsFolder, "backup");

        logger.Info("File moving process started.");

        // Ensure backup folder exists
        Directory.CreateDirectory(backupFolder);

        // Get today's date and subtract one month
        DateTime oneMonthAgo = DateTime.Now.AddMonths(-1);

        // Get all files in the screenshots folder, excluding the backup folder
        string[] files = Directory.GetFiles(screenshotsFolder, "*.*", SearchOption.TopDirectoryOnly);

        foreach (var file in files)
        {
            FileInfo fileInfo = new(file);

            // Check if the file is older than 1 month
            if (fileInfo.LastWriteTime < oneMonthAgo)
            {
                // Create a subfolder in the backup folder based on the file's last write month
                string monthSubfolder = Path.Combine(backupFolder, fileInfo.LastWriteTime.ToString("yyyy-MM"));

                // Ensure the month subfolder exists
                Directory.CreateDirectory(monthSubfolder);

                // Move the file to the backup subfolder
                string destinationFile = Path.Combine(monthSubfolder, fileInfo.Name);

                try
                {
                    // Move file asynchronously
                    await Task.Run(() => File.Move(file, destinationFile));
                    logger.Info($"Moved: {file} to {destinationFile}");
                }
                catch (Exception ex)
                {
                    logger.Error(ex, $"Error moving file {file}: {ex.Message}");
                }
            }
        }

        logger.Info("File moving process completed.");
        LogManager.Shutdown();  // Flush and close logs
    }

    private static void ConfigureNLog()
    {
        var config = new LoggingConfiguration();

        // Define a file target
        var logfile = new FileTarget("logfile")
        {
            FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "log-${shortdate}.txt"),
            ArchiveFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "archive", "log-{#}.txt"),
            ArchiveEvery = FileArchivePeriod.Day,
            MaxArchiveFiles = 30,  // Keep a maximum of 30 archived files
            ArchiveNumbering = ArchiveNumberingMode.Rolling,
            ConcurrentWrites = true,
            KeepFileOpen = false,
            Layout = "${longdate} ${level:uppercase=true} ${message} ${exception:format=ToString}"
        };

        // Define a console target
        var consoleTarget = new ConsoleTarget("logconsole")
        {
            Layout = "${longdate} ${level:uppercase=true} ${message} ${exception:format=ToString}"
        };

        // Add targets to the config
        config.AddTarget(logfile);
        config.AddTarget(consoleTarget);

        // Define logging rules (log to both file and console)
        config.AddRule(LogLevel.Info, LogLevel.Fatal, logfile);
        config.AddRule(LogLevel.Info, LogLevel.Fatal, consoleTarget);

        // Apply config
        LogManager.Configuration = config;
    }
}
