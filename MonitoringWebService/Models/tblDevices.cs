﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringWebService.Models
{
    public partial class tblDevices
    {
        [Key]
        public int DeviceId { get; set; }
        [Required]
        [StringLength(50)]
        public string DeviceName { get; set; }
        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; }
        public int DeviceTypeId { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        [StringLength(50)]
        public string Location { get; set; }
        [StringLength(50)]
        public string IpAddress { get; set; }

        [StringLength(50)]
        public string UserId { get; set; }
        [StringLength(10)]
        public string Password { get; set; }
        public int? VendorId { get; set; }

        public int? ManagedTypeId { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }
    }
}