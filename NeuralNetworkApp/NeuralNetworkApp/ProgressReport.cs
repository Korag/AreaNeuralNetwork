using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkApp
{
    public class ProgressReport
    {
        public int Iteration { get; set; }
        public List<int[]> Weight { get; set; }

        public List<int[]> Point { get; set; }

        public List<int[]> Delta { get; set; }


    }
}
