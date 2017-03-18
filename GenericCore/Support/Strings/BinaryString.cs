using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCore.Support.Strings
{
    public class BinaryString
    {
        private string _separator;
        private Encoding _encoding;
        private Lazy<IEnumerable<string>> _binaryString;
        
        public byte[] Bytes { get; set; }
        public string OriginalString
        {
            get
            {
                if(Bytes.IsNullOrEmptyList())
                {
                    return string.Empty;
                }

                return _encoding.GetString(Bytes);
            }
        }

        public BinaryString(string str, Encoding encoding = null)
        {
            str.AssertNotNull("str");

            if (encoding.IsNull())
            {
                encoding = Encoding.UTF8;
            }

            byte[] bytes = encoding.GetBytes(str);
            Initialize(bytes, encoding);
        }

        public BinaryString(byte[] byteArray, Encoding encoding = null)
        {
            if (encoding.IsNull())
            {
                encoding = Encoding.UTF8;
            }

            Initialize(byteArray, encoding);
        }

        public BinaryString InvertBinaries()
        {
            string binaryStr = ToString();
            StringBuilder buffer = new StringBuilder();

            foreach (char c in binaryStr)
            {
                buffer.Append((c == '1') ? '0' : '1');
            }

            string invertedBinaryStr = buffer.ToString();
            byte[] bytes = GetByteArray(invertedBinaryStr);

            return new BinaryString(bytes);
        }

        public BinaryString ReverseBinaries()
        {
            string binaryStr = ToString();
            string[] strArray = binaryStr.ToCharArray().Select(x => x.ToString()).ToArray();
            Array.Reverse(strArray);
            byte[] bytes = GetByteArray(strArray.StringJoin(string.Empty));

            return new BinaryString(bytes);
        }

        #region Object override

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
            return ToString(string.Empty);
        }

        public string ToString(string separator)
        {
            return 
                _binaryString
                .Value
                .ToEmptyIfNull()
                .StringJoin(separator.ToEmptyIfNull());
        }

        #endregion

        #region Private Methods

        private void Initialize(byte[] byteArray, Encoding encoding)
        {
            byteArray.AssertNotNull("byteArray");

            Bytes = byteArray;

            _encoding = encoding;
            _binaryString = new Lazy<IEnumerable<string>>(() => ToBinaryString());
            _separator = string.Empty;
        }

        private IEnumerable<string> ToBinaryString()
        {
            if (Bytes.IsNullOrEmptyList())
            {
                return new string[0];
            }

            IEnumerable<string> binaryStr =
                Bytes
                    .Select
                    (
                        x =>
                            Convert
                                .ToString(x, 2)
                                .PadLeft(8, '0')
                    );

            return binaryStr;
        }

        private byte[] GetByteArray(string binaryStr)
        {
            int numOfBytes = binaryStr.Length / 8;
            byte[] bytes = new byte[numOfBytes];

            for (int i = 0; i < numOfBytes; ++i)
            {
                bytes[i] = Convert.ToByte(binaryStr.Substring(8 * i, 8), 2);
            }

            return bytes;
        }

        #endregion
    }
}
