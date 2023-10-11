using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NetFx40ConsoleTest
{
    class HttpClientTests
    {

        private static IDictionary<string, string> _headers = new Dictionary<string, string>()
        {
            { "User-Agent", "C# Http Client" },
            { "Accept", "application/json" },
            { "Authorization", "eZ_IOizkY812rTO9b_9nyIgrvVDE7H6yA3__" }
        };

        public static async Task<string> PostFormForStringAsync(string requestUri, IDictionary<string, string> forms,
            IDictionary<string, string> files)
        {
            Console.WriteLine("HttpClientHelper$PostFileForStringAsync -> requestUri={0}", requestUri);
            Console.WriteLine("HttpClientHelper$PostFileForStringAsync -> parameters={0}, files={1}", forms, files);
            using (HttpClient client = new HttpClient())
            {
                // 常规设置
                client.Timeout = TimeSpan.FromSeconds(300);
                foreach (KeyValuePair<string, string> header in _headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                // 发起请求
                using (MultipartFormDataContent formDataContent = new MultipartFormDataContent())
                {
                    // 添加参数列表
                    if (null != forms && 0 < forms.Count)
                    {
                        foreach (KeyValuePair<string, string> param in forms)
                        {
                            formDataContent.Add(new StringContent(param.Value), param.Key);
                        }
                    }

                    // 添加文件列表
                    if (null != files && 0 < files.Count)
                    {
                        string filePath;
                        foreach (KeyValuePair<string, string> file in files)
                        {
                            // 转换文件字节码
                            filePath = file.Value;
                            byte[] fileBytes = File.ReadAllBytes(filePath);
                            ByteArrayContent fileContent = new ByteArrayContent(fileBytes);

                            formDataContent.Add(fileContent, file.Key,
                                filePath.Substring(filePath.LastIndexOf(@"\", StringComparison.Ordinal) + 1));
                        }
                    }

                    // 上传文件
                    HttpResponseMessage response = await client.PostAsync(requestUri, formDataContent);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("HttpClientHelper$PostForStringAsync -> response={0}", content);
                        return content;
                    }
                }
            }

            return null;
        }

        public static async Task Main1(string[] args)
        {
            try
            {
                string requestUri = "http://127.0.0.1:8080/api/document/upload";
                Dictionary<string, string> files = new Dictionary<string, string>()
                {
                    { "multipartFile", @"E:\cache_iso\11x64_Lightn PE_22000.376_Network_2022.09.26_Stable.iso" } // 276MB
                    // { "multipartFile", @"E:\cache_iso\proxmox-ve_7.4-1.iso" } // 1.04GB
                };
                string content = await PostFormForStringAsync(requestUri, null, files);
                Console.WriteLine(content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            // Wait for 15 seconds
            // Thread.Sleep(TimeSpan.FromSeconds(15));
        }
    }
}