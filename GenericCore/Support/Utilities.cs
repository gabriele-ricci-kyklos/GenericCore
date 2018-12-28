using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.Support
{
    public static class Utilities
    {
        public static string GetFileNameFromUriString(string uri)
        {
            uri.AssertHasText(nameof(uri));

            if (!Uri.TryCreate(uri, UriKind.Absolute, out Uri uriObj))
            {
                uriObj = new Uri(new Uri("http://fake.com"), uri);
            }

            return Path.GetFileName(uriObj.LocalPath);
        }
    }
}
