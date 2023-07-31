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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Interneuron.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static TSettings AddConfig<TSettings>(this IServiceCollection services, IConfiguration configuration)
            where TSettings : class, new()
        {
            return services.AddConfig<TSettings>(configuration, options => { });
        }

        public static TSettings AddConfig<TSettings>(this IServiceCollection services, IConfiguration configuration, Action<BinderOptions> configureOptions)
            where TSettings : class, new()
        {
            if (services == null) { throw new ArgumentNullException(nameof(services)); }
            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }

            TSettings setting = configuration.Get<TSettings>(configureOptions);
            services.TryAddSingleton(setting);
            return setting;
        }
    }
}
