using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace DependencyListGenerator
{
    class Program
    {
        static List<string> lob= new List<string>();

        static void Main(string[] args)
        {
            
            var path = @"C:\Temp\cch-master\SDS";


            var deploymentObjects = new DeploymentObjects(path);
            var graph = deploymentObjects.GenerateDependencyGraph();

            var listOfObjects = deploymentObjects.GenerateListObjects();
            listOfObjects.ForEach(Console.WriteLine);
            deploymentObjects.GraphIt();


        }



    }
}
