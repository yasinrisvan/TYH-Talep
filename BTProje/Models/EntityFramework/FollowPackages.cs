//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BTProje.Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class FollowPackages
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string CompanyInformation { get; set; }
        public string ShippingCompany { get; set; }
        public string BuyerCompany { get; set; }
        public string PickUperPersonNameSurname { get; set; }
        public Nullable<bool> Situation { get; set; }
    }
}
