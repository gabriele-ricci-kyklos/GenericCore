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
        #region Zipper

        public static string ZipString(string stringToZip)
        {
            if (stringToZip == null)
            {
                return null;
            }

            if (stringToZip.IsNullOrEmpty())
            {
                return string.Empty;
            }

            byte[] buffer = Encoding.UTF8.GetBytes(stringToZip);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        public static string UnZipString(string zippedString)
        {
            if (zippedString == null)
            {
                return null;
            }

            if (zippedString.IsNullOrEmpty())
            {
                return string.Empty;
            }

            byte[] gZipBuffer = Convert.FromBase64String(zippedString);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        public static void UnZipSingleFileContentToStream(byte[] fileContent, Stream outstream)
        {
            fileContent.AssertNotNull(nameof(fileContent));

            using (MemoryStream ms = new MemoryStream(fileContent))
            using (ZipFile zipFile = ZipFile.Read(ms))
            {
                (zipFile.Count == 1).AssertIsTrue("Zip contains more than one file");
                ZipEntry zipEntry = zipFile.First();
                zipEntry.IsDirectory.AssertIsFalse("Zip content is a folder!");
                zipEntry.Extract(outstream);
            }
        }

        public static Stream ZipSingleFileContent(byte[] fileContent, string entryName)
        {
            fileContent.AssertNotNull(nameof(fileContent));

            var ms = new MemoryStream();
            using (var zipStream = new ZipFile())
            {
                zipStream.AddEntry(entryName, fileContent);
                zipStream.Save(ms);
            }
            ms.Position = 0;
            return ms;
        }

        public static byte[] ZipByteArray(byte[] fileContent, string entryName)
        {
            using (MemoryStream ms = ZipSingleFileContent(fileContent, entryName) as MemoryStream)
            {
                return ms.ToArray();
            }
        }

        public static void CreateZipFileFromFileList(string zipFileName, IList<string> fileNames)
        {
            CreateZipFileFromFileList(zipFileName, fileNames, null, false);
        }

        public static void CreateZipFileFromFileListWithFileNameOnly(string zipFileName, IList<string> fileNames)
        {
            CreateZipFileFromFileList(zipFileName, fileNames, null, true, true);
        }

        public static void CreateZipFileFromFileList(string zipFileName, IList<string> fileNames, IList<string> descriptions, bool flatPath, bool fileNameOnly = false)
        {
            List<FileInfo> files = new List<FileInfo>();
            foreach (var file in fileNames.Distinct())
            {
                if (!File.Exists(file))
                {
                    continue;
                }

                FileInfo fi = new FileInfo(file);

                if ((fi.Attributes & FileAttributes.Directory) == 0)
                {
                    files.Add(fi);
                }
            }

            //CreateZipFileFromFileListImpl(zipFileName, files, descriptions, true, flatPath);
            CreateZipFileFromFileListImpl(zipFileName, files, descriptions, true, flatPath, fileNameOnly);
        }

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
                    return ms.ToArray();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static byte[] CreateZipContentFromFolder(string directory, string entryDirectory, string pattern, bool recursive)
        {
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.CompressionLevel = CompressionLevel.Default;

                    IList<string> fileList = GetFilesInFolder(directory, pattern, recursive);

                    IList<string> fileListWithNewPath = ModifyFilesPath(fileList, entryDirectory);

                    int index = 0;

                    foreach (var item in fileList)
                    {
                        byte[] content = File.ReadAllBytes(item);
                        ZipEntry entry = null;
                        entry = zip.AddEntry(fileListWithNewPath[index], content);
                        index++;
                    }
                    MemoryStream ms = new MemoryStream();
                    zip.Save(ms);
                    return ms.ToArray();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

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
        private static void CreateZipFileFromFileListImpl(string zipFileName, IList<FileInfo> fileNames, IList<string> comments, bool overwriteIfExists, bool flatPath, bool onlyFileName)
        {
            const string zipExt = ".zip";
            try
            {
                //if file name extension terminates with something different from '.zip' 
                //this will change it because it is needed from ionic.zip as save file param.
                if (Path.HasExtension(zipFileName))
                {
                    string ext = Path.GetExtension(zipFileName);
                    zipFileName = (ext.Equals(zipExt)) ? zipFileName : Path.ChangeExtension(zipFileName, zipExt);
                }
                else
                {
                    zipFileName += zipExt;
                }

                Dictionary<string, int> fileSet = new Dictionary<string, int>();

                using (ZipFile zip = new ZipFile())
                {
                    zip.CompressionLevel = CompressionLevel.Default;

                    int i = 0;
                    foreach (FileInfo filename in fileNames)
                    {
                        ZipEntry entry = null;
                        if (flatPath)
                        {
                            var content = File.ReadAllBytes(filename.FullName);
                            //entry = zip.AddEntry(Utilities.FlatFileName(filename), content);

                            string fileEntryName = string.Empty;
                            if (onlyFileName)
                            {
                                int counter;
                                if (fileSet.TryGetValue(filename.Name, out counter))
                                {
                                    ++counter;
                                    fileEntryName = $"{Path.GetFileNameWithoutExtension(filename.Name)}_{counter}{filename.Extension}";
                                    fileSet[filename.Name] = counter;
                                }
                                else
                                {
                                    fileEntryName = filename.Name;
                                    fileSet.Add(filename.Name, 0);
                                }
                            }
                            else
                            {
                                fileEntryName = FlatFileName(filename);
                            }

                            entry = zip.AddEntry(fileEntryName, content);
                        }
                        else
                        {
                            entry = zip.AddFile(filename.FullName);
                        }
                        entry.Comment = comments.Return(x => x.ElementAtOrDefault(i).ToEmptyIfNull(), string.Empty);
                        ++i;
                    }

                    if (File.Exists(zipFileName))
                    {
                        if (overwriteIfExists)
                        {
                            File.Delete(zipFileName);
                        }
                        else
                        {
                            throw new ArgumentException($"File {zipFileName} already exists");
                        }
                    }
                    zip.Save(zipFileName);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region UnZipper

        public static IList<Tuple<string, byte[]>> GetFileContentsFromZipFile(string filePath, string fileName = null)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The file hasn't been found", filePath);
            }

            byte[] fileContent = File.ReadAllBytes(filePath);

            return GetFileContentsFromZipFile(fileContent, fileName);
        }

        public static IList<Tuple<string, byte[]>> GetFileContentsFromZipFile(byte[] fileContent, string fileName = null)
        {
            fileContent.AssertNotNull("fileContent");

            IList<Tuple<string, byte[]>> files = new List<Tuple<string, byte[]>>();

            using (MemoryStream ms = new MemoryStream(fileContent))
            using (ZipFile zip = ZipFile.Read(ms))
            {
                foreach (ZipEntry e in zip)
                {
                    if (!e.IsDirectory && (fileName == null || e.FileName == fileName))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            e.Extract(m);
                            files.Add(Tuple.Create(e.FileName, m.ToArray()));
                        }
                    }
                }
            }

            return files;
        }

        #endregion

        private static IList<string> GetFilesInFolder(string file, string searchPattern, bool recursive)
        {
            if (!File.Exists(file) && !Directory.Exists(file))
            {
                return new List<string>();
            }

            if (Directory.Exists(file))
            {
                return
                    Directory
                    .GetFiles(file, searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                    .ToList();
            }

            return file.AsList();
        }

        private static string FlatFileName(FileInfo fi)
        {
            return fi.FullName.Replace(Path.DirectorySeparatorChar, '_').Replace(":", string.Empty);
        }
    }
}
