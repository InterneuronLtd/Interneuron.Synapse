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
using Interneuron.Common.Extensions;
using System.Text;

namespace Interneuron.CareRecord.Infrastructure.Search
{
    public class SynapseSearchTerm
    {
        public SynapseSearchTerm(string name, string op, string value, ISearchExpressionProvider searchExpressionProvider)
        {
            this.Name = name;
            this.Op = op;
            this.Value = value;
            this.ExpressionProvider = searchExpressionProvider;
        }

        public string Name { get; set; }
        public string Op { get; set; }
        public string Value { get; set; }
        public ISearchExpressionProvider ExpressionProvider { get; set; }
    }
}
