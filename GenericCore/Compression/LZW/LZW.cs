using GenericCore.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCore.Compression.LZW
{
    public static class LZW
    {
        public static byte[] Compress(byte[] uncompressed)
        {
            // build the dictionary
            Dictionary<string, byte> dictionary = new Dictionary<string, byte>();

            if(uncompressed.Any(x => x > 127))
            {
                throw new FormatException("Only ASCII characters are permitted");
            }

            for (int i = 0; i < 127; i++)
            {
                dictionary.Add(((char)i).ToString(), (byte)i);
            }

            string w = string.Empty;
            List<byte> compressed = new List<byte>();

            foreach (char c in uncompressed)
            {
                string wc = w + c;
                if (dictionary.ContainsKey(wc))
                {
                    w = wc;
                }
                else
                {
                    // write w to output
                    compressed.Add(dictionary[w]);
                    // wc is a new sequence; add it to the dictionary
                    dictionary.Add(wc, (byte)dictionary.Count);
                    w = c.ToString();
                }
            }

            // write remaining output if necessary
            if (!string.IsNullOrEmpty(w))
            {
                compressed.Add(dictionary[w]);
            }

            return compressed.ToArray();
        }

        public static byte[] Decompress(byte[] compressed)
        {
            IList<byte> compressedList = new List<byte>(compressed);

            // build the dictionary
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            for (int i = 0; i < 127; i++)
            {
                dictionary.Add(i, ((char)i).ToString());
            }

            string w = dictionary[compressedList[0]];
            compressedList.RemoveAt(0);
            StringBuilder decompressed = new StringBuilder(w);

            string entry = null;
            foreach (int k in compressedList)
            {
                entry = null;
                if (dictionary.ContainsKey(k))
                {
                    entry = dictionary[k];
                }
                else if (k == dictionary.Count)
                {
                    entry = w + w[0];
                }

                decompressed.Append(entry);

                // new sequence; add it to the dictionary
                dictionary.Add(dictionary.Count, w + entry[0]);

                w = entry;
            }

            return Encoding.ASCII.GetBytes(decompressed.ToString());
        }
    }
}
