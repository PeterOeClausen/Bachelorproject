using System;
using System.Collections.Generic;
using System.Linq;

namespace DROM_Client.Models.BusinessObjects
{
    /// <summary>
    /// Class to hold DCRGraph data. Also used as transferobject between client and Web API.
    /// </summary>
    public class DCRGraph
    {
        public int Id { get; set; }
        public List<Event> Events { get; set; }
    }
}