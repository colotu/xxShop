#region License

/*
 * Copyright 2002-2012 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System.Text;

namespace YSWL.OAuth.Tencent.QQ.Converters
{
    /// <summary>
    /// Implementation of <see cref="Http.Converters.IHttpMessageConverter"/> that can read and write strings.
    /// </summary>
    /// <remarks>
    /// By default, this converter supports all media types '*/*', and writes with a 'Content-Type' 
    /// of 'text/plain'. 
    /// This can be overridden by setting the <see cref="P:SupportedMediaTypes"/> property.
    /// </remarks>
    /// <author>Ben</author>
    public class OpenIdJsonHttpMessageConverter : Http.Converters.Json.TextJsonHttpMessageConverter
    {
        protected override string ConvertToJson(string result)
        {
            return result.Replace("callback(", "").Replace(");", "").Replace("\n", "").Replace("\r\n", "");
        }
    }
}
