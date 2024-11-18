using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Models
{
    // Declaration of AI use: ChatGPT helped me make the code in this file 

    //---------------------------------------------------------------------------------
    /// <summary>
    /// Represents a node in a graph with unique identification, type, and edges connecting to other nodes.
    /// </summary>
    public class GraphNode
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Unique identifier for the graph node.
        /// </summary>
        public string Identifier { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Type of the graph node.
        /// </summary>
        public string Type { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Dictionary containing edges connecting this node to other nodes.
        /// </summary>
        public Dictionary<string, GraphEdge> Edges { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphNode"/> class.
        /// </summary>
        /// <param name="identifier">Unique identifier for the node.</param>
        /// <param name="type">Type of the node.</param>
        public GraphNode(string identifier, string type)
        {
            Identifier = identifier;
            Type = type;
            Edges = new Dictionary<string, GraphEdge>();
        }
    }

    //---------------------------------------------------------------------------------
    /// <summary>
    /// Represents an edge in a graph connecting two nodes with an edge type.
    /// </summary>
    public class GraphEdge
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Target node that this edge points to.
        /// </summary>
        public GraphNode TargetNode { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Type of the edge.
        /// </summary>
        public string EdgeType { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphEdge"/> class.
        /// </summary>
        /// <param name="targetNode">The node this edge connects to.</param>
        /// <param name="edgeType">The type of the edge.</param>
        public GraphEdge(GraphNode targetNode, string edgeType)
        {
            TargetNode = targetNode;
            EdgeType = edgeType;
        }
    }
}
//---------------------------------------EOF-------------------------------------------