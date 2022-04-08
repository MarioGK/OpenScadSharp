using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using OpenScadSharp.Watcher;

var stopWatch = new Stopwatch();
var scriptOptions = ScriptOptions.Default
    .WithReferences(typeof(OpenScadSharp.ScriptManager).Assembly)
    .WithImports("OpenScadSharp");

var scriptPath = @"B:\Projects\OpenScadSharp\src\OpenScadSharp.Example\Program.cs";

var fileWatcher = new FileWatcher(scriptPath);

fileWatcher.FileChanged += RunScript;

fileWatcher.Watch();

Console.ReadLine();

async void RunScript(string scriptContent)
{
    stopWatch!.Start();
    
    Console.WriteLine(scriptContent);
    var script = CSharpScript.Create(scriptContent, scriptOptions);
    var state = await script.RunAsync();
    var result = await state.ContinueWithAsync<string>("ScriptManager.ScriptToText()");
    
    stopWatch!.Stop();
    
    Console.WriteLine(result.ReturnValue);
    Console.WriteLine("Elapsed time: {0}MS", stopWatch.ElapsedMilliseconds);
}
