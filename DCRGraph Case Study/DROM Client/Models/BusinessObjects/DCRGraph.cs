﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DROM_Client.Models.BusinessObjects
{
    //HEY! Please talk to Peter/Johan before changing!
    public class DCRGraph
    {
        public int Id { get; set; }
        public List<Event> Events { get; set; }
    }
}