using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayingWithDCRGraphs
{
    public class DCRNode
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string StatusMessageAfterExecution { get; set; }
        public bool Included { get; set; }
        public bool Pending { get; set; }
        public bool Executed { get; set; }

        public DCRNode()
        {

        }
    }
}
