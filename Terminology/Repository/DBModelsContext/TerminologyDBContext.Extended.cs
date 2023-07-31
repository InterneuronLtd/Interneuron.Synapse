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
﻿//using Interneuron.Terminology.Model.DomainModels;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Interneuron.Terminology.Repository.DBModelsContext
//{
//    public partial class TerminologyDBContext : DbContext
//    {
//        partial void OnModelCreatingPartial(ModelBuilder builder)
//        {
//            builder.Entity<FormularyHeader>()
//            .HasMany(p => p.FormularyIndications)
//            .WithOne(b => b.FormularyVersion)
//            .HasForeignKey(p => p.FormularyVersionId);

//            builder.Entity<FormularyHeader>()
//            .HasMany(p => p.FormularyIngredients)
//            .WithOne(b => b.FormularyVersion)
//            .HasForeignKey(p => p.FormularyVersionId);

//            builder.Entity<FormularyHeader>()
//            .HasMany(p => p.FormularyRouteDetails)
//            .WithOne(b => b.FormularyVersion)
//            .HasForeignKey(p => p.FormularyVersionId);

//            builder.Entity<FormularyHeader>()
//            .HasMany(p => p.FormularySuppliers)
//            .WithOne(b => b.FormularyVersion)
//            .HasForeignKey(p => p.FormularyVersionId);

//            builder.Entity<FormularyHeader>()
//            .HasOne(p => p.FormularyDetail)
//            .WithOne(b => b.FormularyVersion)
//            .HasForeignKey<FormularyDetail>(p => p.FormularyVersionId);
//        }
    
//    }
//}
