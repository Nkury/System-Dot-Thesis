using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BayesianNetwork
{
    public class CPT {

        private int rows;
        private int cols;
        private double[,] table;

        public CPT(int rows, int cols, List<double> probabilities)
        {
            this.rows = rows;
            this.cols = cols;
            table = new double[rows, cols];
            SetTable(probabilities);
        }

        // EXAMPLE: where p1 = first parent, p2 = second parent
        //   p1  p2     T    F  
        // -------------------
        //   T , T | [ .6 , .4 ]
        //   T , F | [ .2 , .8 ]
        //   F , T | [ .3 , .7 ]
        //   F , F | [ .1 , .9 ]
        public void SetTable(List<double> probabilities)
        {
            int i = 0;
            foreach(double prob in probabilities)
            {
                table[i, cols - 2] = prob;
                table[i, cols - 1] = 1 - prob;
                i++;
            }
        }

        public double GetValue(int row, int col)
        {
            return table[row, col];
        }

        public int GetRows()
        {
            return rows;
        }

        public int GetCols()
        {
            return cols;
        }
    }
}
