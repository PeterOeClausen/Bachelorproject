using System;
using System.Collections.Generic;
using System.Linq;

namespace DROM_Client.Models.BusinessObjects
{
    /// <summary>
    /// Container for Role data. Also used as a transferobject between client and Web API.
    /// </summary>
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}