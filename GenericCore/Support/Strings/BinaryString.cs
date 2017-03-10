using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCore.Support.Strings
{
    public class BinaryString
    {
        private Lazy<string> _binaryString;
        
        public string OriginalString { get; set; }

        public BinaryString(string str)
        {
            OriginalString = str;
            _binaryString = new Lazy<string>(() => ToBinaryString(OriginalString, string.Empty));
        }

        public override bool Equals(object obj)
        {
            if (obj.IsNull())
            {
                return false;
            }

            BinaryString binStr = obj as BinaryString;
            if (binStr.IsNull())
            {
                return false;
            }

            return OriginalString.Equals(binStr.OriginalString);
        }

        public override int GetHashCode()
        {
            return OriginalString.GetHashCode();
        }

        public override string ToString()
        {
            return _binaryString.Value;
        }

        private string ToBinaryString(string str, string separator)
        {
            separator = separator.ToEmptyIfNull();

            if (str.IsNullOrEmpty())
            {
                return string.Empty;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(str);
            string binaryStr =
                bytes
                    .Select
                    (
                        x =>
                            Convert
                                .ToString(x, 2)
                                .PadLeft(8, '0')
                    )
                    .StringJoin(separator);

            return binaryStr;
        }
    }
}
