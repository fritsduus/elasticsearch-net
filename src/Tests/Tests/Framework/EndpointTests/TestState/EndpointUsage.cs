using System;
using System.Collections.Concurrent;

namespace Tests.Framework.Integration
{
	public class EndpointUsage
	{
		private readonly object _lock = new object();
		private readonly ConcurrentDictionary<int, LazyResponses> _usages = new ConcurrentDictionary<int, LazyResponses>();

		public CallUniqueValues CallUniqueValues { get; }

		public bool CalledSetup { get; internal set; }
		public bool CalledTeardown { get; internal set; }

		public EndpointUsage()
		{
			this.CallUniqueValues = new CallUniqueValues();
		}

		public LazyResponses CallOnce(Func<LazyResponses> clientUsage, int? k = null)
		{
			var key = k ?? clientUsage.GetHashCode();
			if (_usages.TryGetValue(key, out var lazyResponses)) return lazyResponses;
			lock (_lock)
			{
				if (_usages.TryGetValue(key, out lazyResponses)) return lazyResponses;
				var response = clientUsage();
				_usages.TryAdd(key, response);
				return response;
			}
		}
	}
}
