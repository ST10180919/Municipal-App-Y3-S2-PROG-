using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Models
{
    public class GraphNode
    {
        public string Identifier { get; set; } 
        public string Type { get; set; } 
        public Dictionary<string, GraphEdge> Edges { get; set; } 

        public GraphNode(string identifier, string type)
        {
            Identifier = identifier;
            Type = type;
            Edges = new Dictionary<string, GraphEdge>();
        }
    }

    public class GraphEdge
    {
        public GraphNode TargetNode { get; set; }
        public string EdgeType { get; set; } 

        public GraphEdge(GraphNode targetNode, string edgeType)
        {
            TargetNode = targetNode;
            EdgeType = edgeType;
        }
    }
}
