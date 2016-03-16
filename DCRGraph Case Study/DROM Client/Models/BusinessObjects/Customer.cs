using System;
using System.Collections.Generic;
using System.Linq;

namespace DROM_Client.Models.BusinessObjects
{
    //HEY! Please talk to Peter/Johan before changing!
    public class Customer
    {
        public int Id { get; set; } //Id = -1 as unknown.
        public string FirstAndMiddleNames { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public string StreetAndNumber { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
    }
}