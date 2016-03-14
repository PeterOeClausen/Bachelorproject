using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcrWebAPI.XMLParser
{
    /// <summary>
    /// Class repressenting a relation or constraint, can be either Condition, Response, Milestone, Exclusion, Inclusion depending on context etc.
    /// </summary>
    public class Constraint
    {
        public string fromNodeId { get; set; }
        public string toNodeId { get; set; }
    }
}
