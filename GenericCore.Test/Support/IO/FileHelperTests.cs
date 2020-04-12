using GenericCore.Support;
using GenericCore.Support.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.Test.Support.IO
{
    [TestClass]
    public class FileHelperTests
    {
        private readonly string _dirPath = @"C:\temp";
        private readonly string _fileName = @"a.txt";
        private string _filePath => _dirPath.CombinePaths(_fileName);
        private readonly string _textContent = "Lorem ipsum dolor sit amet";
        private readonly string[] _lineContent = new string[] { "Lorem", "ipsum", "dolor" };
        private readonly byte[] _byteContent = Encoding.UTF8.GetBytes("Lorem ipsum dolor sit amet");
        private readonly string[] _appendLineContent = new string[] { "consectetur", "adipiscing", "elit" };
        private readonly string _appendContent = "consectetur adipiscing elit";

        [TestMethod]
        public async Task ReadAllTextAsync()
        {
            if (File.Exists(_filePath)) File.Delete(_filePath);
            File.WriteAllText(_filePath, _textContent);
            Assert.AreEqual(_textContent, await FileHelper.ReadAllTextAsync(_filePath));
            File.Delete(_filePath);
        }

        [TestMethod]
        public async Task ReadAllLinesAsync()
        {
            if (File.Exists(_filePath)) File.Delete(_filePath);
            File.WriteAllLines(_filePath, _lineContent);
            CollectionAssert.AreEqual(_lineContent, await FileHelper.ReadAllLinesAsync(_filePath));
            File.Delete(_filePath);
        }

        [TestMethod]
        public async Task ReadAllBytesAsync()
        {
            if (File.Exists(_filePath)) File.Delete(_filePath);
            File.WriteAllBytes(_filePath, _byteContent);
            CollectionAssert.AreEqual(_byteContent, await FileHelper.ReadAllBytesAsync(_filePath));
            File.Delete(_filePath);
        }

        [TestMethod]
        public async Task WriteAllBytesAsync()
        {
            if (File.Exists(_filePath)) File.Delete(_filePath);
            await FileHelper.WriteAllBytesAsync(_filePath, _byteContent);
            CollectionAssert.AreEqual(_byteContent, File.ReadAllBytes(_filePath));
            File.Delete(_filePath);
        }

        [TestMethod]
        public async Task WriteAllLinesAsync()
        {
            if (File.Exists(_filePath)) File.Delete(_filePath);
            await FileHelper.WriteAllLinesAsync(_filePath, _lineContent);
            CollectionAssert.AreEqual(_lineContent, File.ReadAllLines(_filePath));
            File.Delete(_filePath);
        }

        [TestMethod]
        public async Task WriteAllTextAsync()
        {
            if (File.Exists(_filePath)) File.Delete(_filePath);
            await FileHelper.WriteAllTextAsync(_filePath, _textContent);
            Assert.AreEqual(_textContent, File.ReadAllText(_filePath));
            File.Delete(_filePath);
        }

        [TestMethod]
        public async Task AppendAllLinesAsync()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }

            File.WriteAllLines(_filePath, _lineContent);
            await FileHelper.AppendAllLinesAsync(_filePath, _appendLineContent);
            CollectionAssert.AreEqual(_lineContent.Union(_appendLineContent).ToArray(), File.ReadAllLines(_filePath));
            File.Delete(_filePath);
        }

        [TestMethod]
        public async Task AppendAllTextAsync()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }

            File.WriteAllText(_filePath, _textContent);

            await FileHelper.AppendAllTextAsync(_filePath, _appendContent);
            Assert.AreEqual($"{_textContent}{_appendContent}", File.ReadAllText(_filePath));
            File.Delete(_filePath);
        }

        [TestMethod]
        public async Task DeleteAsync()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, _textContent);
            }

            await FileHelper.DeleteAsync(_filePath);
            Assert.IsFalse(File.Exists(_filePath));
        }

        [TestMethod]
        public async Task MoveSameDriveAsync()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, _textContent);
            }

            string destDirPath = _dirPath.CombinePaths("Temp2");
            string destFilePath = destDirPath.CombinePaths(_fileName);

            if (!Directory.Exists(destDirPath))
            {
                Directory.CreateDirectory(destDirPath);
            }

            await FileHelper.MoveAsync(_filePath, destFilePath);

            Assert.IsTrue(File.Exists(destFilePath));

            File.Delete(destFilePath);
            Directory.Delete(destDirPath);
        }

        [TestMethod]
        public async Task MoveDifferentDriveAsync()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, _textContent);
            }

            const string destDirPath = "E:\\Temp";
            string destFilePath = destDirPath.CombinePaths(_fileName);

            if (!Directory.Exists(destDirPath))
            {
                Directory.CreateDirectory(destDirPath);
            }

            await FileHelper.MoveAsync(_filePath, destFilePath);

            Assert.IsTrue(File.Exists(destFilePath));

            File.Delete(destFilePath);
            Directory.Delete(destDirPath);
        }

        [TestMethod]
        public async Task MoveNetworkDriveAsync()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, _textContent);
            }

            const string destDirPath = "Z:\\Temp\\Temp2";
            string destFilePath = destDirPath.CombinePaths(_fileName);

            if (!Directory.Exists(destDirPath))
            {
                Directory.CreateDirectory(destDirPath);
            }

            await FileHelper.MoveAsync(_filePath, destFilePath);

            Assert.IsTrue(File.Exists(destFilePath));

            File.Delete(destFilePath);
            Directory.Delete(destDirPath);
        }

        [TestMethod]
        public async Task CopyAsync()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, _textContent);
            }

            string destDirPath = _dirPath.CombinePaths("Temp2");
            string destFilePath = destDirPath.CombinePaths(_fileName);

            if (!Directory.Exists(destDirPath))
            {
                Directory.CreateDirectory(destDirPath);
            }

            await FileHelper.CopyAsync(_filePath, destFilePath);

            Assert.IsTrue(File.Exists(destFilePath));
            Assert.AreEqual(File.ReadAllText(_filePath), File.ReadAllText(destFilePath));

            File.Delete(_filePath);
            File.Delete(destFilePath);
            Directory.Delete(destDirPath);
        }

        [TestMethod]
        public async Task CopyOverrideAsync()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, _textContent);
            }

            string destDirPath = _dirPath.CombinePaths("Temp2");
            string destFilePath = destDirPath.CombinePaths(_fileName);

            if (!Directory.Exists(destDirPath))
            {
                Directory.CreateDirectory(destDirPath);
            }
            if (!File.Exists(destFilePath))
            {
                File.WriteAllText(destFilePath, _appendContent);
            }

            await FileHelper.CopyAsync(_filePath, destFilePath, true);

            Assert.IsTrue(File.Exists(destFilePath));
            Assert.AreEqual(File.ReadAllText(_filePath), File.ReadAllText(destFilePath));

            File.Delete(destFilePath);
            Directory.Delete(destDirPath);
        }
    }
}
