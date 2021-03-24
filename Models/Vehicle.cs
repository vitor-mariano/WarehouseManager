using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WarehouseManager.Models
{
    [Table("vehicle")]
    public class Vehicle
    {
        [Key]
        [BindNever]
        [Column("id")]
        public int ID { get; set; }
        [Required]
        [Column("plate1", TypeName = "varchar(7)")]
        public string Plate1 { get; set; }
        [Column("plate2", TypeName = "varchar(7)")]
        public string Plate2 { get; set; }
        [Column("plate3", TypeName = "varchar(7)")]
        public string Plate3 { get; set; }
        [Column("rntrc", TypeName = "varchar(8)")]
        public string RNTRC { get; set; }
        [BindNever]
        public virtual ICollection<Incoming> Incomings { get; set; }
        [BindNever]
        public virtual ICollection<Shipping> Shippings { get; set; }
        [BindNever]
        public virtual ICollection<Enhancement> Enhancements { get; set; }
    }
}