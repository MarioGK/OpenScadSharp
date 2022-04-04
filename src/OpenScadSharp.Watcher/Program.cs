using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

var stopWatch = new Stopwatch();
var scriptOptions = ScriptOptions.Default
    .WithReferences(typeof(OpenScadSharp.Test).Assembly)
    .WithImports("OpenScadSharp");

var scriptPath = @"F:\Projects\OpenScadSharp\src\OpenScadSharp.Example\Program.cs";

var watcher = new FileSystemWatcher(Path.GetDirectoryName(scriptPath)!)
{
    Filter = "Program.cs",
    NotifyFilter = NotifyFilters.LastWrite
};

var scriptReader = new StreamReader(scriptPath, new FileStreamOptions
{
    Access = FileAccess.Read,
    Mode = FileMode.Open,
    Share = FileShare.ReadWrite
});

watcher.Changed += async (_, eventArgs) =>
{
    watcher.EnableRaisingEvents = false;
    stopWatch.Start();
    
    scriptReader.BaseStream.Position = 0;

    var scriptContent = scriptReader.ReadToEnd();
    
    Console.WriteLine(scriptContent);
    var script = CSharpScript.Create(scriptContent, scriptOptions);
    var state = await script.RunAsync();
    var result = await state.ContinueWithAsync<string>("Test.TestString");
    
    stopWatch.Stop();
    
    Console.WriteLine(result.ReturnValue);
    Console.WriteLine("Elapsed time: {0}MS", stopWatch.ElapsedMilliseconds);
    watcher.EnableRaisingEvents = true;
};

watcher.EnableRaisingEvents = true;

Console.ReadLine();

//scriptReader.Dispose();