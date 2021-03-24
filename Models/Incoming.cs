using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WarehouseManager.Models
{
    [Table("incoming")]
    public class Incoming
    {
        [Key]
        [BindNever]
        [Column("id")]
        public int ID { get; set; }
        // [Required]
        // public User User { get; set; }
        [Required]
        [ForeignKey("client_id")]
        public virtual Client Origin { get; set; }
        [Required]
        [ForeignKey("stock_id")]
        public virtual Stock Destination { get; set; }
        [Required]
        [ForeignKey("vehicle_id")]
        public virtual Vehicle Vehicle { get; set; }
        [Required]
        [ForeignKey("driver_id")]
        public virtual Driver Driver { get; set; }
        [Required]
        [Column("gross_wheight")]
        public int GrossWeight { get; set; }
        [Required]
        [Column("vehicle_tare")]
        public int VehicleTare { get; set; }
        [Required]
        [Column("net_weight")]
        public int NetWeight { get; set; }
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [Column("status")]
        public OpStatus Status { get; set; }
    }
}