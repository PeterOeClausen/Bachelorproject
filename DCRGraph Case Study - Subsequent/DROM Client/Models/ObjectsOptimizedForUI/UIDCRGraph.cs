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
    /// <summary>
    /// UI optimized class for the DCRGraph object. Observable collection notifies view when items are added or removed.
    /// </summary>
    public class UIDCRGraph
    {
        public int DCRGraphId { get; set; }
        public ObservableCollection<Event> Events { get; set; }
    }
}
