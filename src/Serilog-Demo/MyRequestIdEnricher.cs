using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Demo
{
    /// <summary>
    /// 自定义Enricher
    /// </summary>
    public class MyRequestIdEnricher : ILogEventEnricher
    {
        private const string MyRequestIdPropertyName = "MyRequestId";
        private readonly string MyRequestIdItemKey = typeof(MyRequestIdEnricher).Name + "+RequestId";
        private readonly IHttpContextAccessor _contextAccessor;

        public MyRequestIdEnricher() : this(new HttpContextAccessor())
        {
        }

        internal MyRequestIdEnricher(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var httpContext = _contextAccessor.HttpContext;
            if (httpContext == null)
                return;

            if (httpContext.Items[MyRequestIdItemKey] is LogEventProperty logEventProperty)
            {
                logEvent.AddPropertyIfAbsent(logEventProperty);
                return;
            }

            var header = "my-request-id";
            var requestId = httpContext.Request.GetHeader(header);
            if (string.IsNullOrWhiteSpace(requestId))
            {
                requestId = Guid.NewGuid().ToString("N");
            }

            var property = new LogEventProperty(MyRequestIdPropertyName, new ScalarValue(requestId));
            httpContext.Items.Add(MyRequestIdItemKey, property);
            logEvent.AddOrUpdateProperty(property);
        }
    }
}
