using AoC23.Day01;

namespace AoC23Tests
{
    [TestClass]
    public class Day01_Tests
    {
        [DataTestMethod]
        [DataRow("1abc2", 12)]
        [DataRow("fv1abc2ffff", 12)]
        [DataRow("3ghjg5ghjghjg", 35)]
        [DataRow("kjhkjhkjh6mnvmnv4", 64)]
        public void First_and_Last_2Digits(string input, int expected)
        {
            var reader = new CalibrationReader();
            var result = reader.ParseCalibration(input);
            Assert.AreEqual(expected, result);
        }


        [DataTestMethod]
        [DataRow("1a3b4c2", 12)]
        [DataRow("fv1a33b4c2ffff", 12)]
        [DataRow("3gh6jg5ghjghjg", 35)]
        [DataRow("kjhkjhkjh6mnv7m7nv4", 64)]
        public void First_and_Last_MoreThan2Digits(string input, int expected)
        {
            var reader = new CalibrationReader();
            var result = reader.ParseCalibration(input);
            Assert.AreEqual(expected, result);
        }


        [DataTestMethod]
        [DataRow("treb7uchet", 77)]
        [DataRow("4hjkhkjh", 44)]
        [DataRow("ghjgghjghjg8", 88)]
        public void First_and_Last_Only1Digit(string input, int expected)
        {
            var reader = new CalibrationReader();
            var result = reader.ParseCalibration(input);
            Assert.AreEqual(expected, result);
        }
    }


}
