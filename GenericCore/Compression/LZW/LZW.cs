using GenericCore.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCore.Compression.LZW
{
    /*
     * https://rosettacode.org/wiki/LZW_compression#C.23
     * Originally taken the code and strongly edited; now rollbacked to the original version and left for legacy
     * TODO: find a way to represent the output in bytes: https://stackoverflow.com/questions/49210099/how-do-i-represent-an-lzw-output-in-bytes
     * TODO: accept byte array in input
    */

    public static class LZW
    {
        public static List<int> Compress(string uncompressed)
        {
            IDictionary<string, int> dictionary = new Dictionary<string, int>();

            for (int i = 0; i < 256; ++i)
            {
                dictionary.Add(((char)i).ToString(), i);
            }

            string w = string.Empty;
            List<int> compressed = new List<int>();

            foreach (char c in uncompressed)
            {
                string wc = w + c;
                if (dictionary.ContainsKey(wc))
                {
                    w = wc;
                }
                else
                {
                    compressed.Add(dictionary[w]);
                    dictionary.Add(wc, dictionary.Count);
                    w = c.ToString();
                }
            }
            
            if (!string.IsNullOrEmpty(w))
            {
                compressed.Add(dictionary[w]);
            }

            return compressed;
        }

        public static string Decompress(List<int> compressed)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            for (int i = 0; i < 256; i++)
            {
                dictionary.Add(i, ((char)i).ToString());
            }

            string w = dictionary[compressed[0]];
            compressed.RemoveAt(0);
            StringBuilder decompressed = new StringBuilder(w);

            foreach (int k in compressed)
            {
                string entry = null;

                if (dictionary.ContainsKey(k))
                {
                    entry = dictionary[k];
                }
                else if (k == dictionary.Count)
                {
                    entry = w + w[0];
                }

                decompressed.Append(entry);
                
                dictionary.Add(dictionary.Count, w + entry[0]);

                w = entry;
            }

            return decompressed.ToString();
        }
    }
}
