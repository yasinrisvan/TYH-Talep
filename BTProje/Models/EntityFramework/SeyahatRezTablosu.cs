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
    
    public partial class SeyahatRezTablosu
    {
        public int SeyahatRezid { get; set; }
        public Nullable<int> SeyahatTid { get; set; }
        public Nullable<System.DateTime> SeyahatBasT { get; set; }
        public Nullable<System.DateTime> SeyahatBitisT { get; set; }
        public Nullable<int> BaslamaYeri { get; set; }
        public Nullable<int> VarısYeri { get; set; }
        public Nullable<int> Yolcuid { get; set; }
        public string Konaklama { get; set; }
        public string MasrafMKodu { get; set; }
    
        public virtual SeyahatTipleriTablosu SeyahatTipleriTablosu { get; set; }
    }
}
