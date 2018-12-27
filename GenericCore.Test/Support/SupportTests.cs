using GenericCore.Support;
using GenericCore.Support.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.Test.Support.Strings
{
    [TestClass]
    public class SupportTests
    {
        [TestMethod]
        public void CounterTest()
        {
            Assert.AreEqual(1, Counter.Next);
            Assert.AreEqual(2, Counter.Next);
            Assert.AreEqual(3, Counter.Next);

            Assert.AreEqual(3, Counter.Last);
        }

        [TestMethod]
        public void PatternMatcherTest()
        {
            PatternMatcher<int> getRentPrice = 
                new PatternMatcher<int>()
                    .Case<MotorCycle>(bike => 100 + bike.Cylinders * 10)
                    .Case<Bicycle>(30)
                    .Case<Car>(car => car.EngineType == EngineType.Diesel, car => 220 + car.Doors * 20)
                    .Case<Car>(car => car.EngineType == EngineType.Gasoline, car => 200 + car.Doors * 20)
                    .Default(0);

            object[] vehicles = new object[] {
                new Car { EngineType = EngineType.Diesel, Doors = 4 },
                new Car { EngineType = EngineType.Gasoline, Doors = 3 },
                new Bicycle(),
                new MotorCycle { Cylinders = 2 },
                new MotorCycle { Cylinders = 3 },
            };

            Assert.AreEqual(300, getRentPrice.Match(vehicles[0]));
            Assert.AreEqual(260, getRentPrice.Match(vehicles[1]));
            Assert.AreEqual(30, getRentPrice.Match(vehicles[2]));
            Assert.AreEqual(120, getRentPrice.Match(vehicles[3]));
            Assert.AreEqual(130, getRentPrice.Match(vehicles[4]));
        }

        [TestMethod]
        public void IOUtilities_EmptyFolder_Test()
        {
            IOUtilities.EmptyFolder(@"C:\temp");
        }

        [TestMethod]
        public void IOUtilities_CopyDirectory_Test()
        {
            IOUtilities.CopyFolderTo(@"C:\temp\folder1", @"C:\temp\folder2", true, true);
        }
    }

    public enum EngineType
    {
        Diesel,
        Gasoline
    }

    public class Bicycle
    {
        public int Cylinders;
    }

    public class Car
    {
        public EngineType EngineType;
        public int Doors;
    }

    public class MotorCycle
    {
        public int Cylinders;
    }
}
