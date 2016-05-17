using System;
using System.Collections.Generic;
using System.Linq;

namespace DROM_Client.Models.BusinessObjects
{
    /// <summary>
    /// Container for Group data. Also used as a transferobject between client and Web API.
    /// </summary>
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}