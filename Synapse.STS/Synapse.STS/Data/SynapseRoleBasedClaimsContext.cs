 //Interneuron synapse

//Copyright(C) 2023  Interneuron Holdings Ltd

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

//See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.
﻿
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Synapse.STS.Data
{
    public partial class SynapseRoleBasedClaimsContext : DbContext
    {

        public SynapseRoleBasedClaimsContext()
        {

        }

        public SynapseRoleBasedClaimsContext(DbContextOptions<SynapseRoleBasedClaimsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApiScopePermissions> ApiScopePermissions { get; set; }
        public virtual DbSet<ApiScopes> ApiScopes { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<UserRolesExternalProviders> UserRolesExternalProviders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //  optionsBuilder.UseNpgsql(_configuration["Settings:ConnectionString"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiScopePermissions>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.RoleId).IsRequired();
            });

            modelBuilder.Entity<ApiScopes>(entity =>
            {
                entity.HasIndex(e => e.ApiResourceId);

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<UserRolesExternalProviders>(entity =>
            {
                entity.ToTable("UserRoles_ExternalProviders");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ExternalSubjectId).IsRequired();
            });
        }
    }
}
