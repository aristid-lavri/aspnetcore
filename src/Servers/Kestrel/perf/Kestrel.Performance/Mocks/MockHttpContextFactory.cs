// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Microsoft.AspNetCore.Server.Kestrel.Performance
{
    public class MockHttpContextFactory : IHttpContextFactory
    {
        private readonly object _lock = new object();
        private readonly List<DefaultHttpContext> _cache = new List<DefaultHttpContext>();

        public HttpContext Create(IFeatureCollection featureCollection)
        {
            DefaultHttpContext httpContext;

            lock (_lock)
            {
                if (_cache.Count > 0)
                {
                    httpContext = _cache[_cache.Count - 1];
                    _cache.RemoveAt(_cache.Count - 1);
                }
                else
                {
                    httpContext = new DefaultHttpContext();
                }
            }

            httpContext.Initialize(featureCollection);
            return httpContext;
        }

        public void Dispose(HttpContext httpContext)
        {
            lock (_lock)
            {
                _cache.Add((DefaultHttpContext)httpContext);
            }
        }
    }
}
