﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nest.Resolvers.Converters;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Newtonsoft.Json.Converters;
using Nest.Resolvers;

namespace Nest
{
	[JsonConverter(typeof(ReadAsTypeConverter<NestedFilterDescriptor<object>>))]
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public interface INestedFilterDescriptor : IFilterBase
	{
		[JsonProperty("score_mode"), JsonConverter(typeof (StringEnumConverter))]
		NestedScore? Score { get; set; }

		[JsonProperty("query")]
		IQueryDescriptor Query { get; set; }

		[JsonProperty("path")]
		PropertyPathMarker Path { get; set; }

		[JsonProperty("_scope")]
		string Scope { get; set; }
	}

	public class NestedFilterDescriptor<T> : FilterBase, INestedFilterDescriptor where T : class
	{
		NestedScore? INestedFilterDescriptor.Score { get; set; }

		IQueryDescriptor INestedFilterDescriptor.Query { get; set; }

		PropertyPathMarker INestedFilterDescriptor.Path { get; set; }

		string INestedFilterDescriptor.Scope { get; set; }

		bool IFilterBase.IsConditionless
		{
			get
			{
				return ((INestedFilterDescriptor)this).Query == null 
					|| ((INestedFilterDescriptor)this).Query.IsConditionless;
			}
		}

		public NestedFilterDescriptor<T> Query(Func<QueryDescriptor<T>, BaseQuery> querySelector)
		{
			var q = new QueryDescriptor<T>();
			((INestedFilterDescriptor)this).Query = querySelector(q);
			return this;
		}

		public NestedFilterDescriptor<T> Score(NestedScore score)
		{
			((INestedFilterDescriptor)this).Score = score;
			return this;
		}
		public NestedFilterDescriptor<T> Path(string path)
		{
			((INestedFilterDescriptor)this).Path = path;
			return this;
		}
		public NestedFilterDescriptor<T> Path(Expression<Func<T, object>> objectPath)
		{
			((INestedFilterDescriptor)this).Path = objectPath;
			return this;
		}
		public NestedFilterDescriptor<T> Scope(string scope)
		{
			((INestedFilterDescriptor)this).Scope = scope;
			return this;
		}
	}
}
