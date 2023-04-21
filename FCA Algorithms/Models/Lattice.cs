using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FCA_Algorithms.Models
{
    public class Lattice
    {
        public RootData Data { get; set; }
        public List<NodeData> Nodes { get; set; }
    }
    public class RootData
    {
        public int NodesCount { get; set; }
        public int ArcsCount { get; set; }
    }
    public class NodeData
    {
        public class Data
        {
            public int Count { get; set; }
            public string[] Names { get; set; }
        }
        public Data Ext { get; set; }
        public Data Int { get; set; }
    }
}
