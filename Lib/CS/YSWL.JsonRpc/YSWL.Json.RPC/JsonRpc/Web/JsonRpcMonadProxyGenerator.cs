#region License, Terms and Conditions
//
// YSWL - JSON and JSON-RPC for Microsoft .NET Framework and Mono
//
// Copyright (c) 2006-2012 YS56. All Rights Reserved.
//
// This library is free software; you can redistribute it and/or modify it under
// the terms of the GNU Lesser General Public License as published by the Free
// Software Foundation; either version 3 of the License, or (at your option)
// any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
// details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this library; if not, write to the Free Software Foundation, Inc.,
// 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//
#endregion

namespace YSWL.Json.RPC.Web
{
    #region Imports

    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using YSWL.Json.RPC.Services;

    #endregion

    public sealed class JsonRpcMonadProxyGenerator : JsonRpcProxyGeneratorBase
    {
        public JsonRpcMonadProxyGenerator(IService service) : 
            base(service) {}

        protected override string ClientFileName
        {
            get { return Service.GetClass().Name + "Proxy.ps1"; }
        }
        
        protected override void WriteProxy(IndentedTextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteLine("# This PowerShell script was automatically generated by");
            writer.Write("# ");
            writer.WriteLine(GetType().AssemblyQualifiedName);
            writer.Write("# on ");
            DateTime now = DateTime.Now;
            TimeZone timeZone = TimeZone.CurrentTimeZone;
            writer.Write(now.ToLongDateString());
            writer.Write(" at ");
            writer.Write(now.ToLongTimeString());
            writer.Write(" (");
            writer.Write(timeZone.IsDaylightSavingTime(now) ? 
                timeZone.DaylightName : timeZone.StandardName);
            writer.WriteLine(")");
            writer.WriteLine();

            Uri url = new Uri(Request.Url.GetLeftPart(UriPartial.Path));
            ServiceClass service = Service.GetClass();

            writer.Write("function ");
            writer.Write(service.Name);
            writer.WriteLine(" {");
            writer.Indent++;
            writer.Write("param($url = '");
            writer.Write(url.ToString());
            writer.WriteLine("')");
            writer.WriteLine();
            writer.WriteLine("$self = New-Object PSObject");
            writer.WriteLine("$self | Add-Member NoteProperty url $url");
            writer.WriteLine("$self | Add-Member NoteProperty id 0");
            writer.WriteLine();
    
            Method[] methods = service.GetMethods();
            string[] methodNames = new string[methods.Length];
    
            for (int i = 0; i < methods.Length; i++)
            {
                Method method = methods[i];
                methodNames[i] = method.Name;

                if (method.Description.Length > 0)
                {
                    writer.Write("# ");
                    writer.WriteLine(Regex.Replace(method.Description, "(\r\n)|\r|\n", "$0 #", RegexOptions.None));
                    writer.WriteLine();
                }

                writer.Write("$self | Add-Member ScriptMethod ");
                writer.Write(method.Name.Replace(".", "_"));
                writer.WriteLine(" {");
                writer.Indent++;
                writer.Write("$this.maticsoft_rpc('");
                writer.Write(method.Name);
                writer.Write("', @{");

                Parameter[] parameters = method.GetParameters();
                
                foreach (Parameter parameter in parameters)
                {
                    if (parameter.Position > 0)
                        writer.Write(';');
                    writer.Write(' ');
                    writer.Write(parameter.Name);
                    writer.Write(" = $args[");
                    writer.Write(parameter.Position.ToString(CultureInfo.InvariantCulture));
                    writer.Write(']');
                }

                writer.WriteLine(" })");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
            }
    
            writer.WriteLine();
            writer.WriteLine(@"$self | Add-Member ScriptMethod maticsoft_rpc {
        $this.id++
        $req = [YSWL.Json.Conversion.JsonConvert]::ExportToString(@{ 
            'id' = $this.id; 'method' = $args[0]; 'params' = $args[1]})
        $form = New-Object Collections.Specialized.NameValueCollection
        $form.Add('JSON-RPC', $req)
        $wc = New-Object Net.WebClient
        $bytes = $wc.UploadValues($this.url, $form)
        if ($bytes) {
            # TODO: Get the encoding from the charset in the Content-Type response header
            $rsp = [YSWL.Json.Conversion.JsonConvert]::Import([Text.Encoding]::UTF8.GetString($bytes))
            if ($rsp.error) {
                throw $rsp.error.message
            }
            else {
                $rsp.result
            }
        }
    }

    $self");
    
            writer.Indent--;
            writer.WriteLine("}");
        }
    }
}
