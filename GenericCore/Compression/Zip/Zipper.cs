using GenericCore.Support;
using Ionic.Zip;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GenericCore.Compression.Zip
{
    public static class Zipper
    {
        public static byte[] ZipStringToBytes(string stringToZip)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(stringToZip);
            return ZipBytes(buffer);
        }

        public static string ZipString(string stringToZip)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(stringToZip);
            return ZipBytesToString(buffer);
        }

        public static string ZipBytesToString(byte[] bytesToZip)
        {
            byte[] zippedData = ZipBytes(bytesToZip);
            return Convert.ToBase64String(zippedData);
        }

        public static byte[] ZipBytes(byte[] bytesToZip)
        {
            if (bytesToZip.IsNullOrEmptyList())
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(bytesToZip, 0, bytesToZip.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(bytesToZip.Length), 0, gZipBuffer, 0, 4);

            return gZipBuffer;
        }

        public static string UnZipString(string zippedString)
        {
            byte[] unzippedBytes = UnZipStringToBytes(zippedString);
            return Encoding.UTF8.GetString(unzippedBytes);
        }

        public static string UnZipBytesToString(byte[] zippedBytes)
        {
            byte[] unzippedBytes = UnZipBytes(zippedBytes);
            return Encoding.UTF8.GetString(unzippedBytes);
        }

        public static byte[] UnZipStringToBytes(string zippedString)
        {
            byte[] buffer = Convert.FromBase64String(zippedString);
            return UnZipBytes(buffer);
        }

        public static byte[] UnZipBytes(byte[] zippedBytes)
        {
            if (zippedBytes.IsNullOrEmptyList())
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(zippedBytes, 0);
                memoryStream.Write(zippedBytes, 4, zippedBytes.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return buffer;
            }
        }

        //public static void CreateZipFileFromFileList(string zipFileName, IList<string> fileNames)
        //{
        //    CreateZipFileFromFileList(zipFileName, fileNames, null, false);
        //}

        //public static void CreateZipFileFromFileList(string zipFileName, IList<string> fileNames, IList<string> descriptions, bool flatPath)
        //{
        //    List<FileInfo> files = new List<FileInfo>();
        //    foreach (var file in fileNames.Distinct())
        //    {
        //        if (!File.Exists(file))
        //        {
        //            continue;
        //        }

        //        FileInfo fi = new FileInfo(file);

        //        if ((fi.Attributes & FileAttributes.Directory) == 0)
        //        {
        //            files.Add(fi);
        //        }
        //    }

        //    CreateZipFileFromFileListImpl(zipFileName, files, descriptions, true, flatPath);
        //}

        public static byte[] CreateZipFromFileContentList(IList<Tuple<string, byte[]>> fileContentList)
        {
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.CompressionLevel = CompressionLevel.Default;

                    int i = 0;

                    foreach (var item in fileContentList)
                    {
                        ZipEntry entry = null;
                        entry = zip.AddEntry(item.Item1, item.Item2);
                        ++i;

                    }
                    MemoryStream ms = new MemoryStream();
                    zip.Save(ms);
                    return ms.GetBuffer();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public static byte[] CreateZipContentFromFolder(string directory, string entryDirectory, string pattern, bool recursive)
        //{
        //    try
        //    {
        //        using (ZipFile zip = new ZipFile())
        //        {
        //            zip.CompressionLevel = CompressionLevel.Default;

        //            IList<string> fileList = Utilities.GetFilesInFolder(directory, pattern, recursive);

        //            IList<string> fileListWithNewPath = ModifyFilesPath(fileList, entryDirectory);

        //            int index = 0;

        //            foreach (var item in fileList)
        //            {
        //                byte[] content = File.ReadAllBytes(item);
        //                ZipEntry entry = null;
        //                entry = zip.AddEntry(fileListWithNewPath[index], content);
        //                index++;
        //            }
        //            MemoryStream ms = new MemoryStream();
        //            zip.Save(ms);
        //            return ms.GetBuffer();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        private static IList<string> ModifyFilesPath(IList<string> fileList, string entryDirectory)
        {
            IList<string> listToReturn = new List<string>();

            foreach (var file in fileList)
            {
                listToReturn.Add("C:" + file.Substring(file.IndexOf(Path.Combine(Path.DirectorySeparatorChar.ToString(), entryDirectory))));
            }
            return listToReturn;
        }

        //private static void CreateZipFileFromFileListImpl(string zipFileName, IList<FileInfo> fileNames, IList<string> comments, bool overwriteIfExists, bool flatPath)
        //{
        //    const string zipExt = ".zip";
        //    try
        //    {
        //        //if file name extension terminates with something different from '.zip' 
        //        //this will change it because it is needed from ionic.zip as save file param.
        //        if (Path.HasExtension(zipFileName))
        //        {
        //            string ext = Path.GetExtension(zipFileName);
        //            zipFileName = (ext.Equals(zipExt)) ? zipFileName : Path.ChangeExtension(zipFileName, zipExt);
        //        }
        //        else
        //        {
        //            zipFileName += zipExt;
        //        }

        //        using (ZipFile zip = new ZipFile())
        //        {
        //            zip.CompressionLevel = CompressionLevel.Default;

        //            int i = 0;
        //            foreach (FileInfo filename in fileNames)
        //            {
        //                ZipEntry entry = null;
        //                if (flatPath)
        //                {
        //                    var content = File.ReadAllBytes(filename.FullName);
        //                    entry = zip.AddEntry(Utilities.FlatFileName(filename), content);
        //                }
        //                else
        //                {
        //                    entry = zip.AddFile(filename.FullName);
        //                }
        //                entry.Comment = comments.Return(x => x.ElementAtOrDefault(i).ToEmptyIfNull(), string.Empty);
        //                ++i;
        //            }

        //            if (File.Exists(zipFileName))
        //            {
        //                if (overwriteIfExists)
        //                {
        //                    File.Delete(zipFileName);
        //                }
        //                else
        //                {
        //                    throw new Exception("File {0} already exists.".FormatWith(zipFileName));
        //                }
        //            }
        //            zip.Save(zipFileName);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public static IList<byte[]> GetFileContentFromZipFile(string filePath, string fileName = null)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The file hasn't been found", filePath);
            }

            return GetFileContentFromZipFile(File.ReadAllBytes(filePath), fileName);
        }

        public static IList<byte[]> GetFileContentFromZipFile(byte[] fileContent, string fileName = null)
        {
            fileContent.AssertNotNull("fileContent");

            IList<byte[]> files = new List<byte[]>();

            using (MemoryStream ms = new MemoryStream(fileContent))
            using (ZipFile zip = ZipFile.Read(ms))
            {
                foreach (ZipEntry e in zip)
                {
                    if (fileName == null || e.FileName == fileName)
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            e.Extract(m);
                            files.Add(m.ToArray());
                        }
                    }
                }
            }

            return files;
        }
    }
}
