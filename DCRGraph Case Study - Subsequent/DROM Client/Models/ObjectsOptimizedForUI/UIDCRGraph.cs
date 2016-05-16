using DROM_Client.Models.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DROM_Client.Models.ObjectsOptimizedForUI
{
    public class UIDCRGraph
    {
        public int DCRGraphId { get; set; }
        public ObservableCollection<Event> Events { get; set; }
    }
}
