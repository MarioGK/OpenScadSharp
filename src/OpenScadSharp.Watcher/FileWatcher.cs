using Timer = System.Timers.Timer;

namespace OpenScadSharp.Watcher;

public class FileWatcher
{
    public string FilePath { get; set; }

    public double PollRate
    {
        get => _timer.Interval;
        set => _timer.Interval = value;
    }

    public delegate void FileChangedEventHandler(string fileContents);
    
    public event FileChangedEventHandler? FileChanged;

    private readonly Timer _timer;
    private DateTime _lastWriteTime;
    private readonly StreamReader _streamReader;
    
    public FileWatcher(string filePath)
    {
        FilePath = filePath;

        _streamReader = new StreamReader(FilePath, new FileStreamOptions
        {
            Access = FileAccess.Read,
            Mode = FileMode.Open,
            Share = FileShare.ReadWrite
        });

        PollRate = 500;
        _timer = new Timer(PollRate);
        _timer.Elapsed += async (_, _) => await PollFile();
    }
    
    private async Task PollFile()
    {
        var writeTime = File.GetLastWriteTime(FilePath);
        
        if(_lastWriteTime == writeTime)
        {
            return;
        }
        
        _lastWriteTime = writeTime;
        _streamReader!.BaseStream.Position = 0;

        var scriptContent = await _streamReader.ReadToEndAsync();
        FileChanged?.Invoke(scriptContent);
    }

    public void Watch()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }
}