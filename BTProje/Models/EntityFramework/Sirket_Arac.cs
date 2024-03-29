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
    
    public partial class Sirket_Arac
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sirket_Arac()
        {
            this.Yolcu = new HashSet<Yolcu>();
        }
    
        public int sirketaracid { get; set; }
        public Nullable<int> aracid { get; set; }
        public string gorev { get; set; }
        public string soforadsoyad { get; set; }
        public string yolcuad { get; set; }
        public string yolcubirim { get; set; }
        public Nullable<int> yolcusayisi { get; set; }
        public Nullable<int> gidiskm { get; set; }
        public Nullable<int> donuskm { get; set; }
        public Nullable<System.DateTime> gorevgidistarih { get; set; }
        public Nullable<System.DateTime> gorevdonustarih { get; set; }
        public string donussoforad { get; set; }
        public Nullable<int> kullaniciid { get; set; }
        public Nullable<int> bolgeid { get; set; }
        public string ikincikullanici { get; set; }
        public Nullable<int> durum { get; set; }
        public Nullable<int> yolcu1 { get; set; }
        public Nullable<int> yolcu2 { get; set; }
        public Nullable<int> yolcu3 { get; set; }
        public Nullable<int> yolcu4 { get; set; }
    
        public virtual AraçTablosu AraçTablosu { get; set; }
        public virtual BölgelerTablosu BölgelerTablosu { get; set; }
        public virtual KullaniciTablosu KullaniciTablosu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Yolcu> Yolcu { get; set; }
    }
}
