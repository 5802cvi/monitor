using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitorCreator;
using System.Collections.Generic;
using MonitorCreator.Enumerations;

namespace UnitTest
{
    [TestClass]
    public class TestMonitorCreator
    {
        [TestMethod]
        public void CheckParametrConstructor()
        {
            ParameterData parData = new ParameterData("name", 234);

            Assert.AreEqual(parData.Name, "name");
            Assert.AreEqual(parData.Value, 234);
        }

        [TestMethod]
        public void CheckParametrData()
        {
            ParameterData parData = new ParameterData("name","dsds", 234);

            Assert.AreEqual(parData.Description, "dsds");
            Assert.AreEqual(parData.Name, "name");
            Assert.AreEqual(parData.Value, 234);
        }

        [TestMethod]
        public void CheckNormalValueProperty()
        {
            Sketch sketchData = new Sketch();
            sketchData.NormalValue = 20;
            double a = sketchData.NormalValue;
            Assert.AreEqual(sketchData.NormalValue, a);
        }

        [TestMethod]
        public void TestParameterValid()
        {

            var parameterData = new ModelParameters();

            var parameters = new Dictionary<Parameter, ParameterData>
            {
            // Высота монитора.
                {
                Parameter.BodyHeight, new ParameterData(Parameter.BodyHeight.ToString(), 15, new PointF(10,30))
                },

            };
            List<string> error = parameterData.CheckData(parameters);
            if (error.Count != 0)
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        public void TestParameterInValid()
        {

            var parameterData = new ModelParameters();

            var parameters = new Dictionary<Parameter, ParameterData>
            {
            // Высота монитора.
                {
                Parameter.BodyHeight, new ParameterData(Parameter.BodyHeight.ToString(), 5, new PointF(10, 30))
                },

            };
            List<string> error = parameterData.CheckData(parameters);
            if (error.Count == 0)
            {
                Assert.Fail();
            }
        }
    }
}
