//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace e5movies.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Review
    {
        public int ReviewID { get; set; }
        public string Comment { get; set; }
        public int ImageID { get; set; }
        public Nullable<System.DateTime> DateReviewed { get; set; }
        public Nullable<int> rating { get; set; }
    
        public virtual Image Image { get; set; }
    }
}
