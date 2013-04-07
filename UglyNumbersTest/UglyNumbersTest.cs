using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UglyNumbersTest
{
    [TestClass]
    public class UglyNumbersTest
    {
        PrivateType program;
        [TestInitialize]
        public void init()
        {
            program = new PrivateType(typeof(Program));
        }
        
        [TestMethod]
        public void CreateBasePossibilitiesTest()
        {
            var actual = (List<List<string>>)program.InvokeStatic("CreateBasePossibilities", "123");
            Assert.AreEqual(1, actual[0].Count);
            Assert.IsTrue(actual[1][1].Contains("+"));
            Assert.IsTrue(actual[1][2].Contains("-"));
            Assert.IsTrue(actual[2][1].Contains("+"));
            Assert.IsTrue(actual[2][2].Contains("-"));

            actual = (List<List<string>>)program.InvokeStatic("CreateBasePossibilities", "1");
            Assert.AreEqual(1, actual.Count);

            actual = (List<List<string>>)program.InvokeStatic("CreateBasePossibilities", "12345");
            Assert.AreEqual(1, actual[0].Count);
            Assert.IsTrue(actual[1][1].Contains("+"));
            Assert.IsTrue(actual[1][2].Contains("-"));
            Assert.IsTrue(actual[2][1].Contains("+"));
            Assert.IsTrue(actual[2][2].Contains("-"));
            Assert.IsTrue(actual[3][1].Contains("+"));
            Assert.IsTrue(actual[3][2].Contains("-"));
            Assert.IsTrue(actual[4][1].Contains("+"));
            Assert.IsTrue(actual[4][2].Contains("-"));
        }

        [TestMethod]
        public void CreatePossiblitiesTest()
        {
            var input = (List<List<string>>)program.InvokeStatic("CreateBasePossibilities", "12345");
            var output = program.InvokeStatic("CreatePossibilities", new object [] {input});
            Assert.IsTrue(((List<string>)output).Count == 81);
        }

        [TestMethod]
        public void EvaluatesToUglyTest()
        {
            var inputUgly = "1+234-5+6"; //ugly
            var inputNotUgly = "123+4-56"; //not ugly

            var output = program.InvokeStatic("EvaluatesToUgly", inputUgly);
            Assert.IsTrue(Convert.ToInt32(output) == 1, "Should be ugly");

            output = program.InvokeStatic("EvaluatesToUgly", inputNotUgly);
            Assert.IsTrue(Convert.ToInt32(output) == 0, "Should be not ugly");
        }

        [TestMethod]
        public void FindUglyNumbersTest()
        {
            var output = program.InvokeStatic("FindUglyNumbers", "1");
            Assert.IsTrue(Convert.ToInt32(output) == 0, "Output is " + output.ToString() + " should be 1");

            output = program.InvokeStatic("FindUglyNumbers", "9");
            Assert.IsTrue(Convert.ToInt32(output) == 1, "Output is " + output.ToString() + " should be 1");

            output = program.InvokeStatic("FindUglyNumbers", "011");
            Assert.IsTrue(Convert.ToInt32(output) == 6, "Output is " + output.ToString() + " should be 6");

            output = program.InvokeStatic("FindUglyNumbers", "12345");
            Assert.IsTrue(Convert.ToInt32(output) == 64, "Output is " + output.ToString() + " should be 64");

            output = program.InvokeStatic("FindUglyNumbers", "3747766429");
            Assert.IsTrue(Convert.ToInt32(output) == 14913, "Output is " + output.ToString() + " should be 14913");

            var testInputs = new List<string>() {"0", "1","9","13","011","277","413","0876","7192","7329","8126","9893","12345","17214","20204","160176","1566843","9716463","54275347","84091195","91920744","216589640","845048810","3747766429","4802642960","8224853717","9712606483","0000000000277"};
            foreach (var item in testInputs)
            {
                output = program.InvokeStatic("FindUglyNumbers", item);    
            }
            
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void GetCartesianProductTest()
        {
            var list1 = new List<string>() {"1"};
            var list2 = new List<string>() {"+2", "-2", "2"};
            
            var actual = program.InvokeStatic("GetCartesianProduct", new object[] {list1, list2});
            var expected = new List<string>() { "1+2", "1-2", "12" };

            CollectionAssert.AreEqual(expected, (List<string>)actual);    
        }
    }
}
