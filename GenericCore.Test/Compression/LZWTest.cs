using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using GenericCore.Compression.LZW;
using System.Text;

namespace GenericCore.Test.Compression
{
    [TestClass]
    public class LZWTest
    {
        [TestMethod]
        public void TestLZW()
        {
            string inputStr = @"

            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque accumsan venenatis lorem et fringilla. Nulla facilisi. Phasellus commodo leo eros, in luctus tortor commodo sit amet. Praesent molestie, nisl ac placerat venenatis, diam magna sagittis velit, non placerat tortor massa vel leo. Pellentesque vel ipsum nunc. Cras mattis dolor odio, nec posuere nisi maximus quis. Aliquam erat volutpat. Curabitur malesuada mauris eu ex molestie, nec molestie urna dignissim. Nunc sodales fringilla cursus. Nam ac odio varius mauris euismod semper.

            Vestibulum est sem, faucibus et nulla sit amet, fringilla molestie purus. Sed dui tellus, ultricies vel lectus a, lobortis sollicitudin eros. Donec id arcu a nunc interdum pretium. Duis at viverra eros. Proin ornare, purus in pretium commodo, ligula orci aliquet lorem, a interdum lorem augue vel purus. Interdum et malesuada fames ac ante ipsum primis in faucibus. Vestibulum ultricies eu erat sed interdum. Ut in tempor dolor. Maecenas sed euismod elit. Sed nec dictum quam. Integer at fringilla tellus, eu elementum sapien. Integer sollicitudin ex lacus, eget laoreet sapien bibendum nec. Aliquam venenatis dictum dui volutpat dictum.

            Fusce semper nisl in elit pharetra porta. Duis condimentum tristique placerat. Quisque placerat efficitur elit, ac interdum nisl ornare non. Ut dictum sit amet ex vel semper. Nam turpis eros, faucibus non fringilla vitae, elementum ut ligula. Sed mauris turpis, fringilla a egestas ut, aliquam ac mauris. Nam porta, nisl in ultrices finibus, urna ex pellentesque risus, eget ullamcorper ipsum sapien eget felis. Integer consequat lorem a commodo tempor.

            Curabitur fringilla felis quis viverra elementum. Donec felis nibh, efficitur sit amet augue in, efficitur dictum felis. Curabitur eu fringilla orci. Vivamus mattis diam enim, sit amet suscipit nibh blandit non. Nulla a diam id enim posuere consequat. Mauris ornare est eget risus lacinia mattis. Nam sed volutpat urna. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nam malesuada mi at interdum bibendum. Duis quis venenatis tortor, sit amet rhoncus nunc. Fusce aliquam fermentum imperdiet. Donec est nunc, vehicula ac suscipit finibus, faucibus at lectus. Donec quis tincidunt tortor. Sed a diam id mauris tempor pretium. Etiam dictum porttitor blandit. Cras enim ligula, blandit a dolor et, tempus auctor massa.

            Etiam dictum, augue quis sagittis pulvinar, sapien odio egestas velit, consectetur maximus ex lectus id odio. Nunc sit amet purus non neque cursus imperdiet eu a nisl. Nam pellentesque commodo sodales. Suspendisse vitae scelerisque purus. Proin sed magna a elit auctor molestie. Duis dapibus eleifend odio, pellentesque finibus felis. Curabitur ac gravida mauris, eget ornare ex. Integer quis dui sodales, elementum felis ut, consequat lorem. ";

            //string inputStr = "abc";
            byte[] inputBytes = Encoding.ASCII.GetBytes(inputStr);

            byte[] compressed = LZW.Compress(inputStr);
            string decompressedStr = LZW.Decompress(compressed);

            //string decompressedStr = Encoding.ASCII.GetString(decompressedBytes);

            //CollectionAssert.AreEqual(inputBytes, decompressedBytes);
            Assert.AreEqual(inputStr, decompressedStr);
        }
    }
}
