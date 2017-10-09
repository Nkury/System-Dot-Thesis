using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BayesianNetwork
{
    public class BNode
    {

        public string nodeName;
        public int node_id;
        public List<BNode> parents;

        public CPT table; // conditional probability table

        public int Evidence;
        
        // probs are a list of the true probabilities
        public BNode(List<BNode> parent, string name, int id, List<double> probs)
        {
            parents = new List<BNode>();
            parents = parent;
            nodeName = name;
            node_id = id;

            table = new CPT(parent == null ? 2 : (int)Mathf.Pow(2, parents.Count), 2, probs);
        }

        // evidence = 
        // 0 for false; 1 for true
        public double CalculateProbability(int evidence)
        {
            double sum = 0.0;
            double product = 1;

            for(int i = 0; i < table.GetRows(); i++)
            {
                product = 1; // reset product
                product *= table.GetValue(i, evidence);
                if (parents != null) {
                    for (int j = parents.Count - 1; j >= 0; j--)
                    {
                        product *= parents[j].CalculateProbability((i / (int)Mathf.Pow(2, j)) % 2);
                    }
                }
                sum += product;
            }

            // because the values taken from the table are reversed
            return sum;
        }        
    }
}
