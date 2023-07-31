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
using System;

namespace Interneuron.Infrastructure.Exceptions.Handlers
{
    public class InterneuronGenericExceptionHandler : IExceptionHandler
    {
        private Exception ex;

        public InterneuronGenericExceptionHandler(Exception ex)
        {
            this.ex = ex;
        }
        public void HandleExceptionAsync(IntrneuronExceptionHandlerOptions options)
        {
            var errorId = Guid.NewGuid().ToString();
            var errorResponseMessage = $"Error fetching data from database. Please check the error log with ID: {errorId} for more details";

          
            if (options != null)
                options.OnException?.Invoke(this.ex, errorId, errorResponseMessage);

            if (options != null)
                options.OnExceptionHandlingComplete?.Invoke(this.ex, errorId);
        }
    }
}
