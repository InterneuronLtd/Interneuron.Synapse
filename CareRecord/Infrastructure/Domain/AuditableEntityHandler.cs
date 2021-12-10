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


using System;

namespace Interneuron.CareRecord.Infrastructure.Domain
{
    public class AuditableEntityHandler<T> : IEntityEventHandler<T> where T: class
	{
		public void Execute(T entity, EventType eventType)
		{
			var auditedEntity = entity as IAuditable;

			if (auditedEntity != null)
			{
				if (eventType == EventType.Add)
				{
					auditedEntity.CreatedDate = DateTime.UtcNow;
				}
				auditedEntity.UpdatedDate = DateTime.UtcNow;
			}
		}
	}

}
