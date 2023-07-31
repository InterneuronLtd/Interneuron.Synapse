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
﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Middleware
{
	public class MaximumRequestTimeoutMiddleware
	{
		private readonly RequestDelegate _next;

		public MaximumRequestTimeoutMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context, IOptions<MaximumRequestTimeoutSettings> requestTimeoutSettings)
		{
			using (var timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(context.RequestAborted))
			{
				timeoutSource.CancelAfter(TimeSpan.FromMinutes(requestTimeoutSettings.Value.TimeoutInMins));
				context.RequestAborted = timeoutSource.Token;
				await _next(context);
			}
		}
	}

	public class MaximumRequestTimeoutSettings
	{
		public int TimeoutInMins { get; set; } = 300;
	}
}
