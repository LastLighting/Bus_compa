//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bus_company.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Timetable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Timetable()
        {
            this.Orders = new HashSet<Order>();
        }
    
        public int Id { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public Nullable<int> Trip { get; set; }
        public Nullable<int> Bus { get; set; }
        public string Driver { get; set; }
        public Nullable<int> Free_paces { get; set; }
        public Nullable<int> Price_place { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Bus Bus1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        public virtual Trip Trip1 { get; set; }
    }
}
