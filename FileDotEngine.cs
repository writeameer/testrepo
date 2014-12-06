using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;
using System.IO;

public sealed class FileDotEngine : IDotEngine
{
    public string Run(GraphvizImageType imageType, string dot, string outputFileName)
    {
        string output = outputFileName;
        File.WriteAllText(output, dot);

        // assumes dot.exe is on the path:
        var execFile = @"C:\Program Files (x86)\Graphviz2.38\bin\dot.exe";
        var args = string.Format(@"{0} -Tpng -O", output);
        System.Diagnostics.Process.Start(execFile, args).WaitForExit();
        return output;

    }
}