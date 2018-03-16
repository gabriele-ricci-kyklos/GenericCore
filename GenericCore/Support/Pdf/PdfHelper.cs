using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GenericCore.Support.Pdf
{
    // Uses a modified version of the library PdfSharp to solve a known bug: http://forum.pdfsharp.net/viewtopic.php?p=583#p583
    public static class PdfHelper
    {
        public static byte[] MergePdfFilesInASingleDocument(IList<string> files)
        {
            IList<byte[]> fileBytes = new List<byte[]>();

            foreach (string file in files)
            {
                if (!File.Exists(file))
                {
                    throw new FileNotFoundException($"The file '{file}' has not been found");
                }

                fileBytes.Add(File.ReadAllBytes(file));
            }

            byte[] mergedPdf = MergePdfFilesInASingleDocument(fileBytes.ToArray());
            return mergedPdf;
        }

        public static void MergePdfFilesInASingleDocument(IList<string> files, string destFilePath)
        {
            IList<byte[]> fileBytes = new List<byte[]>();

            foreach (string file in files)
            {
                if(!File.Exists(file))
                {
                    throw new FileNotFoundException($"The file '{file}' has not been found");
                }

                fileBytes.Add(File.ReadAllBytes(file));
            }

            byte[] mergedPdf = MergePdfFilesInASingleDocument(fileBytes.ToArray());
            File.WriteAllBytes(destFilePath, mergedPdf);
        }

        public static void MergePdfFilesInASingleDocument(byte[][] files, string destFilePath)
        {
            if (!Path.GetExtension(destFilePath).Equals(".pdf"))
            {
                destFilePath = Path.ChangeExtension(destFilePath, ".pdf");
            }

            byte[] mergedPdf = MergePdfFilesInASingleDocument(files);
            File.WriteAllBytes(destFilePath, mergedPdf);
        }

        public static byte[] MergePdfFilesInASingleDocument(byte[][] files)
        {
            byte[] outputBytes = null;
            PdfDocument outputDocument = new PdfDocument();

            try
            {
                // Show consecutive pages facing. Requires Acrobat 5 or higher.
                outputDocument.PageLayout = PdfPageLayout.TwoColumnLeft;

                foreach (byte[] file in files)
                {
                    using (Stream stream = new MemoryStream(file))
                    {
                        // we create a reader for the document
                        PdfDocument inputDocument = PdfReader.Open(stream, PdfDocumentOpenMode.Import);

                        // Iterate pages
                        int count = inputDocument.PageCount;
                        for (int idx = 0; idx < count; idx++)
                        {
                            PdfPage page = inputDocument.Pages[idx];
                            outputDocument.AddPage(page);
                        }
                    }
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    outputDocument.Save(stream);
                    outputBytes = stream.ToArray();
                }
            }
            finally
            {
                outputDocument.Close();
            }

            return outputBytes;
        }
    }
}
