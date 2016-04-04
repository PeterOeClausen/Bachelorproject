using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models.DBObjects
{
    public class Include
    {
        [Key]
        public int Id { get; set; }

        public int FromEventId { get; set; }
        [Required]
        [ForeignKey("FromEventId")]
        public DCREvent FromEvent { get; set; }

        public int ToEventId { get; set; }

        [Required]
        [ForeignKey("ToEventId")]
        public DCREvent ToEvent { get; set; }


        
    }
}
