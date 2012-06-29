using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;

using HtmlAgilityPack;

namespace LinkMetaUtility
{
    /// <summary>
    /// Model representing all the properties needed for link meta.
    /// </summary>
    public class LinkMeta
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Title: {0} \r\n", Title);
            sb.AppendFormat("Url: {0} \r\n", Url);
            sb.AppendFormat("Description: {0} \r\n", Description);
            sb.AppendFormat("ImgUrl: {0} \r\n", ImgUrl);
            return sb.ToString();
        }
    }


    /// <summary>
    /// Provides static access to GetMeta method.
    /// </summary>
    public class LinkMetaUtility
    {
        private static bool IsUrlRelative(string url)
        {
            return !(url.StartsWith("http://") || url.StartsWith("https://"));
        }

        private static string GetHtmlString(string url)
        {
            string s = "";
            try
            {
                WebClient wc = new WebClient();
                wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                Stream data = wc.OpenRead(url);
                StreamReader reader = new StreamReader(data);
                s = reader.ReadToEnd();
                data.Close();
                reader.Close();
            }
            catch (WebException) { }
            return s;
        }

        public static LinkMeta GetMeta(string url)
        {
            LinkMeta result = new LinkMeta();
            result.Url = url;

            string s = GetHtmlString(url);
            if (s.Equals(""))
            {
                return null;
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(s);

            HtmlNodeCollection titles = doc.DocumentNode.SelectNodes("//title");
            if (titles != null)
            {
                foreach (var title in titles)
                {
                    string titleStr = title.InnerHtml;
                    //Console.WriteLine(titleStr);
                    result.Title = HttpUtility.HtmlDecode(titleStr);
                    break;
                }
            }

            HtmlNodeCollection imgs = doc.DocumentNode.SelectNodes("//img");
            if (imgs != null)
            {
                foreach (var img in imgs)
                {
                    string imgSrc = img.GetAttributeValue("src", "");
                    if (IsUrlRelative(imgSrc))
                    {
                        var urlEndsWithSlash = url.EndsWith("/");
                        var imgSrcStartsWithSlash = imgSrc.StartsWith("/");
                        if (urlEndsWithSlash && imgSrcStartsWithSlash)
                        {
                            imgSrc = url.Substring(0, url.Length - 1) + imgSrc;
                        }
                        else if ((!urlEndsWithSlash) && (!imgSrcStartsWithSlash))
                        {
                            imgSrc = url + "/" + imgSrc;
                        }
                        else
                        {
                            imgSrc = url + imgSrc;
                        }
                    }
                    result.ImgUrl = imgSrc;
                    break;
                }
            }

            result.Description = "";
            HtmlNodeCollection metas = doc.DocumentNode.SelectNodes("//meta");
            if (metas != null)
            {
                foreach (var meta in metas)
                {
                    if (meta.GetAttributeValue("name", "").ToLower().Equals("description"))
                    {
                        string descChunk = meta.GetAttributeValue("content", "");
                        result.Description = result.Description + HttpUtility.HtmlDecode(descChunk);
                    }
                }
            }
            return result;
        }
    }
}
