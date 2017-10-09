using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BayesianNetwork
{
    public class BNetwork
    {
        string networkName;
        List<BNode> nodes = new List<BNode>();

        public BNetwork(string name, List<BNode> nodes)
        {
            networkName = name;
            this.nodes = nodes;
        }

    }
}
