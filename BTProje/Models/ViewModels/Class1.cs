using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace BTProje.Models
{
    public class Class1
    {
        public Class1()
        {
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Çıkış Bölgesi Boş Geçilemez")]
        public Nullable<int> CikisBolgesi { get; set; }
        [Required(ErrorMessage = "Çıkış Departmanı Boş Geçilemez")]
        public Nullable<int> CikisDepartman { get; set; }
        [Required(ErrorMessage = "Gönderen Personel Boş Geçilemez")]
        public Nullable<int> GonderenPersonelId { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        [StringLength(150, ErrorMessage = "Açıklama En Fazla 150 Karakter Olabilir.")]
        public string Aciklama { get; set; }
        [Required(ErrorMessage = "Adet Boş Geçilemez")]
        [StringLength(2, ErrorMessage = "Adet En Fazla 99 Olabilir")]
        public Nullable<byte> Adet { get; set; }
        [Required(ErrorMessage = "Alıcı Personel Boş Geçilemez")]
        public Nullable<int> AliciPersonelId { get; set; }

        [Required(ErrorMessage = "Varış Departmanı Boş Geçilemez")]
        public Nullable<int> VarisDepartman { get; set; }

        [Required(ErrorMessage = "Varış Bölgesi Boş Geçilemez")]
        public Nullable<int> VarisBolgesi { get; set; }

        public string Durum { get; set; }
        public string Donus { get; set; }
    }
}
