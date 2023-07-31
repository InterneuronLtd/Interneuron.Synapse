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
using System;

namespace Interneuron.Caching
{
    public class CacheSettings
    {
        private IConfiguration _configuration;

        static class Constants
        {
            public const string CacheDurationInMinutes = "cacheDurationInMinutes";
            public const string Enabled = "enabled";
            public const string Provider = "provider";
        }

        //private static readonly CacheSettings Settings = CacheServiceExtension.Configuration == null ? null : CacheServiceExtension.Configuration.GetSection("cache").Get<CacheSettings>();

        //public static CacheSettings Instance
        //{
        //    get
        //    {
        //        return Settings ?? new CacheSettings();
        //    }
        //}


        

        public CacheSettings(IConfiguration configuration)
        {
            this._configuration = configuration;

            var cacheSection = this._configuration.GetSection("cache");

            if (cacheSection.Exists())
            {
                var duration = cacheSection.GetValue<int>(Constants.CacheDurationInMinutes);
                this.CacheDurationInMinutes = duration;

                var enabled = cacheSection.GetValue<bool>(Constants.Enabled);
                this.Enabled = enabled;

                var provider = cacheSection.GetValue<string>(Constants.Provider);
                this.Provider = provider;
            }
        }


        public int CacheDurationInMinutes { get; } = 5;

        public string Provider { get; set; } = null;

        public bool Enabled { get; set; } = false;
    }
}
