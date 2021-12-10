//Interneuron Synapse

//Copyright(C) 2021  Interneuron CIC

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


﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.CareRecord.API.AppCode.Extensions
{
    public static class AutoMapperExtensions
    {
        //IConfigurationProvider
        //public static async Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable)
        //{
        //    return await queryable.ProjectTo<TDestination>().DecompileAsync().ToListAsync();
        //}

        //public static IQueryable<TDestination> ProjectToQueryable<TDestination>(this IQueryable queryable)
        //{
        //    return queryable.ProjectTo<TDestination>().Decompile();
        //}

        //public static IPagedList<TDestination> ProjectToPagedList<TDestination>(this IQueryable queryable, int pageNumber, int pageSize)
        //{
        //    return queryable.ProjectTo<TDestination>().Decompile().ToPagedList(pageNumber, pageSize);
        //}

        //public static async Task<TDestination> ProjectToSingleOrDefaultAsync<TDestination>(this IQueryable queryable)
        //{
        //    return await queryable.ProjectTo<TDestination>().DecompileAsync().SingleOrDefaultAsync();
        //}
    }
}

