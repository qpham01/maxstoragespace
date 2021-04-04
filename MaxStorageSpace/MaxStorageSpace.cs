using System;
using System.Collections.Generic;

namespace MaxStorageSpace
{
    public class Node
    {
        public readonly (long, long) Location;
        public HashSet<Node> Neighbors = new HashSet<Node>();

        public Node(long x, long y)
        {
            Location = (x, y);
        }

        public override int GetHashCode()
        {
            return Location.GetHashCode();
        }
    }

    public class Graph
    {
        public long Id;
        public Node StartNode;
        public Dictionary<(long, long), Node> Nodes = new Dictionary<(long, long), Node>();
    }

    public class MaxStorageSpace
    {
        public static long Calculate(long n, long m, long[] h, long[] v)
        {
            if (h.Length == 0 && v.Length == 0) return 1L;
            Array.Sort(h);
            Array.Sort(v);
            var maxHorizontalCount = 0L;
            if (h.Length > 0)
            {
                var previousSeparator = -1L;
                var horizontalCount = 1L;
                foreach (var separator in h)
                {
                    if ((separator - previousSeparator) == 1) horizontalCount++;
                    else horizontalCount = 1L;
                    if (horizontalCount > maxHorizontalCount)
                    {
                        maxHorizontalCount = horizontalCount;
                    }

                    previousSeparator = separator;
                }
            }
            
            var maxVerticalCount = 0L;
            if (v.Length > 0)
            {
                var previousSeparator = -1L;
                var verticalCount = 1L;
                foreach (var separator in v)
                {
                    if ((separator - previousSeparator) == 1) verticalCount++;
                    else verticalCount = 1L;
                    if (verticalCount > maxVerticalCount)
                    {
                        maxVerticalCount = verticalCount;
                    }

                    previousSeparator = separator;
                }
            }
            if (maxHorizontalCount == 0) return maxVerticalCount + 1;
            if (maxVerticalCount == 0) return maxHorizontalCount + 1;
            return (maxHorizontalCount + 1) * (maxVerticalCount + 1);
        }

        public static long CalculateGraph(long n, long m, long[] h, long[] v)
        {
            if (h.Length == 0 && v.Length == 0) return 1L;
            Array.Sort(h);
            Array.Sort(v);
            var nextGraphId = 0;
            var nodeMap = new Dictionary<(long, long), Graph>();
            var graphList = new List<Graph>();
            foreach (var separator in h)
            {
                if (separator < 1 || separator > n - 1) continue;
                var row1 = separator - 1;
                var row2 = separator;
                for (var i = 0; i < m; ++i)
                {
                    Node node1 = null;
                    if (!nodeMap.TryGetValue((row1, i), out var graph1))
                    {
                        node1 = new Node(row1, i);
                        graph1 = new Graph() { Id = nextGraphId, StartNode = node1 };
                        graphList.Add(graph1);
                        nextGraphId++;
                        nodeMap.Add((row1, i), graph1);
                        graph1.Nodes.Add((row1, i), node1);
                        graph1.StartNode = node1;
                    }
                    else
                    {
                        node1 = graph1.Nodes[(row1, i)];
                    }

                    if (nodeMap.TryGetValue((row2, i), out var graph2))
                    {
                        var node = graph2.Nodes[(row2, i)];
                        node1.Neighbors.Add(node);
                        nodeMap[(row2, i)] = graph1;
                        graph1.Nodes.Add((row2, i), node);
                    }
                    else
                    {
                        var node = new Node(row2, i);
                        node1.Neighbors.Add(node);
                        graph1.Nodes.Add((row2, i), node);
                        nodeMap.Add((row2, i), graph1);
                    }
                }
            }
            foreach (var separator in v)
            {
                if (separator < 1 || separator > m - 1) continue;
                var col1 = separator - 1;
                var col2 = separator;
                for (var i = 0; i < n; ++i)
                {
                    Node node1 = null;
                    if (!nodeMap.TryGetValue((i, col1), out var graph1))
                    {
                        node1 = new Node(i, col1);
                        graph1 = new Graph() { Id = nextGraphId, StartNode = node1 };
                        nextGraphId++;
                        nodeMap.Add((i, col1), graph1);
                        graph1.Nodes.Add((i, col1), node1);
                    }
                    else
                    {
                        node1 = graph1.Nodes[(i, col1)];
                    }

                    if (nodeMap.TryGetValue((i, col2), out var graph2))
                    {
                        var node = graph2.Nodes[(i, col2)];
                        node1.Neighbors.Add(node);
                        nodeMap[(i, col2)] = graph1;
                        graph1.Nodes.Add((i, col2), node);
                    }
                    else
                    {
                        var node = new Node(i, col2);
                        graph1.Nodes.Add((i, col2), node);
                        node1.Neighbors.Add(node);
                        nodeMap.Add((i, col2), graph1);
                    }
                }
            }

            var maxSpace = 0;
            foreach (var graph in nodeMap.Values)
            {
                var space = DFSSize(graph);
                if (maxSpace < space)
                {
                    maxSpace = space;
                }
            }

            return maxSpace;
        }

        private static int DFSSize(Graph graph)
        {
            var path = new HashSet<Node>();
            DFS(graph.StartNode, path);
            return path.Count;
        }

        private static void DFS(Node node, HashSet<Node> path)
        {
            path.Add(node);
            foreach (var neighbor in node.Neighbors)
            {
                DFS(neighbor, path);
            }
        }
    }
}
