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

    public partial class Kargo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kargo()
        {
            this.KargoHareketleri = new HashSet<KargoHareketleri>();
        }

        public int Id { get; set; }
        public Nullable<int> CikisBolgesi { get; set; }
        public Nullable<int> CikisDepartman { get; set; }
        public Nullable<int> GonderenPersonelId { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public string Aciklama { get; set; }
        public Nullable<byte> Adet { get; set; }
        public Nullable<int> AliciPersonelId { get; set; }
        public Nullable<int> VarisDepartman { get; set; }
        public Nullable<int> VarisBolgesi { get; set; }
        public string Durum { get; set; }
        public string Donus { get; set; }
        public string AliciAdSoyad { get; set; }
        public string GonderenAdSoyad { get; set; }
        public bool DurumCheck { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KargoHareketleri> KargoHareketleri { get; set; }
        public virtual BölgelerTablosu BölgelerTablosu { get; set; }
        public virtual BölgelerTablosu BölgelerTablosu1 { get; set; }
        public virtual DepartmanlarTablosu DepartmanlarTablosu { get; set; }
        public virtual DepartmanlarTablosu DepartmanlarTablosu1 { get; set; }
        public virtual KullaniciTablosu KullaniciTablosu { get; set; }
        public virtual KullaniciTablosu KullaniciTablosu1 { get; set; }
    }
    public class KargoModel
    {
        public List<Kargo> Kargos { get; set; }
    }
}

