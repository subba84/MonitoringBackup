﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringWebService.Models
{
    public partial class tblCctvCameraGroups
    {
        [Key]
        public int Id { get; set; }
        public int GroupId { get; set; }
        [StringLength(50)]
        public string Devices { get; set; }
        public int? Interval { get; set; }
        public int? Retries { get; set; }
        public bool? IsActive { get; set; }
        [StringLength(50)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
    }
}