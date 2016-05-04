using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models.DBObjects
{
    public class DeliveryType
    {
        [Key]
        public string Type { get; set; }

        [Required]
        public int OrderType { get; set; }

    }
}
