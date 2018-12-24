using GenericCore.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.Compression
{
    public class LZ77String
    {
        static char _referencePrefix = '`';
        static int _referenceIntBase = 96;
        static int _referenceIntFloorCode = ' ';
        static int _referenceIntCeilCode = _referenceIntFloorCode + _referenceIntBase;
        static int _maxStringDistance = (int)Math.Pow(_referenceIntBase, 2) - 1;
        static int _minStringLength = 5;
        static int _maxStringLength = (int)Math.Pow(_referenceIntBase, 1) - 1 + _minStringLength;
        static int _defaultWindowLength = 9220;
        static int _maxWindowLength = _maxStringDistance + _minStringLength;
        
        public static string CompressStr(string data)
        {
            LZ77String lz = new LZ77String();
            return lz.Compress(data, -1);
        }
        public static string DecompressStr(string data)
        {
            LZ77String lz = new LZ77String();
            return lz.Decompress(data);
        }

        public string Compress(string data)
        {
            return Compress(data, -1);
        }

        public string Compress(string data, int windowLength)
        {
            if (windowLength == -1)
            {
                windowLength = _defaultWindowLength;
            }

            if (windowLength > _maxWindowLength)
            {
                throw new ArgumentException("Window length is too large.");
            }

            string compressed = string.Empty;

            int pos = 0;
            int lastPos = data.Length - _minStringLength;

            while (pos < lastPos)
            {
                //Stopwatch w = Stopwatch.StartNew();

                int searchStart = Math.Max(pos - windowLength, 0);
                int matchLength = _minStringLength;
                bool foundMatch = false;
                int bestMatchDistance = _maxStringDistance;
                int bestMatchLength = 0;
                string newCompressed = null;

                while ((searchStart + matchLength) < pos)
                {
                    int sourceWindowEnd = Math.Min(searchStart + matchLength, data.Length);
                    int targetWindowEnd = Math.Min(pos + matchLength, data.Length);

                    string m1 = data.Substring(searchStart, sourceWindowEnd - searchStart);
                    string m2 = data.Substring(pos, targetWindowEnd - pos);

                    bool isValidMatch = m1.Equals(m2) && matchLength < _maxStringLength;

                    if (isValidMatch)
                    {
                        matchLength++;
                        foundMatch = true;
                    }
                    else
                    {
                        int realMatchLength = matchLength - 1;

                        if (foundMatch && (realMatchLength > bestMatchLength))
                        {
                            bestMatchDistance = pos - searchStart - realMatchLength;
                            bestMatchLength = realMatchLength;
                        }

                        matchLength = _minStringLength;
                        searchStart++;
                        foundMatch = false;
                    }
                }

                if (bestMatchLength != 0)
                {
                    newCompressed = _referencePrefix
                            + EncodeReferenceInt(bestMatchDistance, 2)
                            + EncodeReferenceLength(bestMatchLength);

                    pos += bestMatchLength;
                }
                else
                {
                    if (data[pos] != _referencePrefix)
                    {
                        newCompressed = string.Empty + data[pos];
                    }
                    else
                    {
                        newCompressed = string.Empty + _referencePrefix + _referencePrefix;
                    }

                    pos++;
                }

                compressed += newCompressed;

                //Console.WriteLine("{0}", w.ElapsedMilliseconds);
            }

            return compressed + data.Substring(pos).Replace(_referencePrefix.ToString(), string.Format("{0}{1}", _referencePrefix, _referencePrefix));
        }

        public string Decompress(string data)
        {
            string decompressed = "";
            int pos = 0;

            while (pos < data.Length)
            {

                char currentChar = data[pos];

                if (currentChar != _referencePrefix)
                {
                    decompressed += currentChar;
                    pos++;
                }
                else
                {

                    char nextChar = data[pos + 1];

                    if (nextChar != _referencePrefix)
                    {

                        int distance = DecodeReferenceInt(data.Substring(pos + 1, 2), 2);
                        int length = DecodeReferenceLength(data.Substring(pos + 3, 1));
                        int start = decompressed.Length - distance - length;
                        int end = start + length;

                        decompressed += decompressed.Substring(start, end - start);
                        pos += _minStringLength - 1;

                    }
                    else
                    {
                        decompressed += _referencePrefix;
                        pos += 2;
                    }
                }
            }

            return decompressed;
        }

        private string EncodeReferenceInt(int value, int width)
        {
            if ((value >= 0) && (value < (Math.Pow(_referenceIntBase, width) - 1)))
            {
                string encoded = string.Empty;

                while (value > 0)
                {
                    char c = (char)((value % _referenceIntBase) + _referenceIntFloorCode);
                    encoded = string.Format("{0}{1}", c, encoded);//string.Empty + c + encoded;
                    value = (int)Math.Floor((double)value / _referenceIntBase);
                }

                int missingLength = width - encoded.Length;

                for (int i = 0; i < missingLength; i++)
                {
                    char c = (char)_referenceIntFloorCode;
                    encoded = string.Empty + c + encoded;
                }

                return encoded;
            }
            else
            {
                throw new ArgumentException(string.Format("Reference int out of range: {0} (width = {1})", value, width));
            }
        }

        private string EncodeReferenceLength(int length)
        {
            return EncodeReferenceInt(length - _minStringLength, 1);
        }

        private int DecodeReferenceInt(string data, int width)
        {
            int value = 0;

            for (int i = 0; i < width; i++)
            {

                value *= _referenceIntBase;

                int charCode = (int)data[i];

                if ((charCode >= _referenceIntFloorCode)
                        && (charCode <= _referenceIntCeilCode))
                {

                    value += charCode - _referenceIntFloorCode;

                }
                else
                {

                    throw new ArgumentException("Invalid char code in reference int: " + charCode);
                }
            }

            return value;
        }

        private int DecodeReferenceLength(string data)
        {
            return DecodeReferenceInt(data, 1) + _minStringLength;
        }
    }
}
