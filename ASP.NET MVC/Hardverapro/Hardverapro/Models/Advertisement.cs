using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hardverapro.Models
{
    public class Advertisement
    {
        [Key]
        public string UID { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        [Display(Name="Photo")]
        public byte[] PictureData { get; set; }

        [NotMapped]
        [Display(Name = "Picture")]
        public IFormFile PictureFormData { get; set; }

        [Range(1, 999999)]
        public int Price { get; set; }

        [StringLength(100)]
        [Required]
        [Display(Name = "Product")]
        public string Name { get; set; }

        [StringLength(100)]
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Shipping")]
        public string ShipMethod { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [Display(Name ="Creation Date")]
        public DateTime CreationDate { get; set; }

        public virtual IdentityUser Creator { get; set; }
    }
}
