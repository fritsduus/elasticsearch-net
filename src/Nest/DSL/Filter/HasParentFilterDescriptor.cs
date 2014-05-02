﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nest.Resolvers.Converters;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Nest.Resolvers;

namespace Nest
{
	[JsonConverter(typeof(ReadAsTypeConverter<HasParentFilterDescriptor<object>>))]
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public interface IHasParentFilter : IFilterBase
	{
		[JsonProperty("type")]
		TypeNameMarker Type { get; set; }

		[JsonProperty("_scope")]
		string Scope { get; set; }

		[JsonProperty("query")]
		IQueryDescriptor Query { get; set; }
	}

	public class HasParentFilterDescriptor<T> : FilterBase, IHasParentFilter where T : class
	{
		TypeNameMarker IHasParentFilter.Type { get; set; }

		string IHasParentFilter.Scope { get; set;}
		
		IQueryDescriptor IHasParentFilter.Query { get; set; }

		bool IFilterBase.IsConditionless
		{
			get
			{
				var pf = ((IHasParentFilter)this);
				return pf.Query == null 
					|| pf.Query.IsConditionless 
					|| pf.Type.IsNullOrEmpty();
			}
		}

		public HasParentFilterDescriptor()
		{
			((IHasParentFilter)this).Type = TypeNameMarker.Create<T>();
		}

		public HasParentFilterDescriptor<T> Query(Func<QueryDescriptor<T>, BaseQuery> querySelector)
		{
			var q = new QueryDescriptor<T>();
			((IHasParentFilter)this).Query = querySelector(q);
			return this;
		}

		public HasParentFilterDescriptor<T> Scope(string scope)
		{
			((IHasParentFilter)this).Scope = scope;
			return this;
		}

		public HasParentFilterDescriptor<T> Type(string type)
		{
			((IHasParentFilter)this).Type = type;
			return this;
		}
	}
}
