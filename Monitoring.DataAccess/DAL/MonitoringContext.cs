﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;



namespace MonitoringWebService.Models
{
    public partial class MonitoringContext : DbContext
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["MonitoringConnection"].ConnectionString;
       

        public MonitoringContext() : base(connectionString)
        {

        }
        public virtual DbSet<tblCategories> tblCategories { get; set; }
        public virtual DbSet<tblSubCategories> tblSubCategories { get; set; }
        public virtual DbSet<tblTimerIntervals> tblTimerIntervals { get; set; }
        public virtual DbSet<tblAppSettings> tblAppSettings { get; set; }
        public virtual DbSet<tblDevices> tblDevices { get; set; }
        public virtual DbSet<tblDeviceTypes> tblDeviceTypes { get; set; }
        public virtual DbSet<tblSlides> tblSlides { get; set; }
        public virtual DbSet<tblDashBoards> tblDashBoards { get; set; }
        public virtual DbSet<tblServerGroups> tblServerGroups { get; set; }
        public virtual DbSet<DashBoardSlideMapping> DashBoardSlideMapping { get; set; }
        public virtual DbSet<tblDeviceGroups> tblDeviceGroups { get; set; }

        public virtual DbSet<tblVendors> tblVendors { get; set; }

        public virtual DbSet<tblOtherDeviceGroups> tblOtherDeviceGroups { get; set; }

        public virtual DbSet<tblAccessPointGroups> tblAccessPointGroups { get; set; }

        public virtual DbSet<tblCctvCameraGroups> tblCctvCameraGroups { get; set; }

        public virtual DbSet<tblFirewallGroups> tblFirewallGroups { get; set; }

        public virtual DbSet<tblNetworkswitchGroups> tblNetworkswitchGroups { get; set; }

        public virtual DbSet<tblPrinterGroups> tblPrinterGroups { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           
        }
    }
}