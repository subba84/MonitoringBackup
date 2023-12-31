﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MonitoringWebService.Models
{
    public partial class tblSubCategories
    {
        [Key]
        public int SubCategoryId { get; set; }
        [Required]
        [StringLength(50)]
        public string SubCategoryName { get; set; }
        public int CategoryId { get; set; }
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

        [ForeignKey("CategoryId")]
        [InverseProperty("tblSubCategories")]
        public virtual tblCategories Category { get; set; }
    }
}