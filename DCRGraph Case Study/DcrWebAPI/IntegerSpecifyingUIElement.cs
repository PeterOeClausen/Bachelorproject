//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DcrWebAPI
{
    using System;
    using System.Collections.Generic;
    
    public partial class IntegerSpecifyingUIElement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IntegerSpecifyingUIElement()
        {
            this.EventUIElemements = new HashSet<EventUIElemement>();
        }
    
        public int Id { get; set; }
        public string Integer { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EventUIElemement> EventUIElemements { get; set; }
    }
}