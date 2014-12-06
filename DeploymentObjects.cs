using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using QuickGraph;
using QuickGraph.Algorithms.Search;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace DependencyListGenerator
{
    public class DeploymentObjects
    {

        IEnumerable<string> sqlObjects;
        string _path;
        AdjacencyGraph<string, Edge<string>> _graph = new AdjacencyGraph<string, Edge<string>>();

        public DeploymentObjects(string path)
        {
            _path = path;
            sqlObjects = Directory.EnumerateFiles(path)
                            .Select(x => Path.GetFileName(x))
                            .Select(x => x.Replace(".sql", ""));
                
        }

        public AdjacencyGraph<string,Edge<string>> GenerateDependencyGraph()
        {
            // Enumerate through files in path
            Directory.EnumerateFiles(_path).ToList().ForEach(currentFile =>
            {
                // Generate SQL object names from file names
                var sourceObject = Path.GetFileName(currentFile).Replace(".sql", "");

                // Read content of SQL file                
                var currentFileContents = File.ReadAllText(currentFile);

                // Add current SQL file as a vertex in graph
                _graph.AddVertex(sourceObject);

                // Find SQLObjects in Currentfile and add to graph
                sqlObjects.Where(sqlObject => sqlObject != sourceObject).ToList()
                    .ForEach(sqlObject => {
                        if (currentFileContents.Contains(sqlObject))
                            _graph.AddEdge(new Edge<string>(sourceObject, sqlObject));
                    });
            });

            return _graph;

        }

        public List<string> GenerateListObjects()
        {
            var listOfObjects = new List<string>();
            var dfs = new DepthFirstSearchAlgorithm<string, Edge<string>>(_graph);
            dfs.DiscoverVertex += vertex =>
            {
                listOfObjects.Add(vertex);
            };

            dfs.Compute();

            listOfObjects.Reverse();

            if (listOfObjects.Count < _graph.VertexCount)
            {
                Console.WriteLine("Error! List Of Objects is less than vertex count in graph, exitting!");
                Environment.Exit(1);
            }


            return listOfObjects;
        }

        public void GraphIt()
        {
            var graphviz = new GraphvizAlgorithm<string, Edge<String>>(_graph);
            graphviz.FormatVertex += new FormatVertexEventHandler<string>(FormatVertex);
            string output = graphviz.Generate(new FileDotEngine(), @"c:\temp\graph");
        }

        private static void FormatVertex(object sender, FormatVertexEventArgs<string> e)
        {
            e.VertexFormatter.Label = e.Vertex;
        }


    }
}
