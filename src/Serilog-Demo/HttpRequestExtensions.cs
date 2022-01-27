using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Demo
{
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// 获取 <see cref="HttpRequest"/> 对象的请求报文头
        /// </summary>
        /// <param name="request"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetHeader(this HttpRequest request, string name)
        {
            if (request.Headers.TryGetValue(name, out var values))
            {
                return values.FirstOrDefault();
            }
            return null;
        }
    }
}
