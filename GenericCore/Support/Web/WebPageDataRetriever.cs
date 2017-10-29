using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace GenericCore.Support.Web
{
    public static class WebPageDataRetriever
    {
        public static string RetrievePage(string pageUrl)
        {
            pageUrl.AssertNotNull("pageUrl");

            string htmlCode = null;
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                htmlCode = client.DownloadString(pageUrl);
            }

            return htmlCode;
        }

        public static bool DownloadFile(string fileUrl, string localFilePath)
        {
            fileUrl.AssertNotNull("fileUrl");
            localFilePath.AssertNotNull("localFilePath");

            bool succeded = true;

            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile(fileUrl, localFilePath);
                }
                catch(Exception)
                {
                    succeded = false;
                }
            }

            return succeded;
        }
    }
}
