using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericCore.Serialization.Xml;
using GenericCore.Support;
using System.Collections.Generic;

namespace GenericCore.Test.Serialization
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void TestQuickXmlSerializer()
        {
            QuickXmlSerializerTestClass testObj = new QuickXmlSerializerTestClass { Value = "lol", Number = 1 };
            string xml = QuickXmlSerializer.SerializeObject(testObj);
            Assert.IsNotNull(xml);

            QuickXmlSerializerTestClass deserializedObj = QuickXmlSerializer.DeserializeObject<QuickXmlSerializerTestClass>(xml);
            Assert.AreEqual(testObj, deserializedObj);
        }

        [TestMethod]
        public void TestQuickXmlSerializerDynamic()
        {
            QuickXmlSerializerTestClass testObj = new QuickXmlSerializerTestClass { Value = "lol", Number = 1 };
            string xml = QuickXmlSerializer.SerializeObject(testObj);
            Assert.IsNotNull(xml);

            IDictionary<string, object> deserializedObj = QuickXmlSerializer.GetDynamicObjectFromXml(xml);
            Assert.IsTrue(deserializedObj.ContainsKey("QuickXmlSerializerTestClass"));
        }

        [TestMethod]
        public void TestDynamicXmlDeserializer()
        {
            string xml = @"<Students>
                <Student ID=""100"">
                    <Name>Arul</Name>
                    <Mark>90</Mark>
                </Student>
                <Student ID=""200"">
                    <Name>Arul2</Name>
                    <Mark>80</Mark>
                </Student>
            </Students>";

            dynamic students = DynamicXmlDeserializer.FromString(xml);

            var id1 = students.Student[0].ID;
            var name1 = students.Student[0].Name;
            var mark1 = students.Student[0].Mark;

            var id2 = students.Student[1].ID;
            var name2 = students.Student[1].Name;
            var mark2 = students.Student[1].Mark;

            Assert.IsTrue(id1 == "100" && name1 == "Arul" && mark1 == "90");
            Assert.IsTrue(id2 == "200" && name2 == "Arul2" && mark2 == "80");
        }
    }

    public class QuickXmlSerializerTestClass
    {
        public string Value { get; set; }
        public int Number { get; set; }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ Number.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj.IsNull())
            {
                return false;
            }

            QuickXmlSerializerTestClass typedObj = obj as QuickXmlSerializerTestClass;

            if (typedObj.IsNull())
            {
                return false;
            }

            return Value.Equals(typedObj.Value) && Number.Equals(typedObj.Number);
        }
    }
}
