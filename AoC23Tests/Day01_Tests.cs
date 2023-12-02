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

        [DataTestMethod]
        [DataRow("1abc2three", 13)]
        [DataRow("fivefv1abc2ffffnine", 59)]
        [DataRow("3ghjg5ghfourjghjg", 34)]
        [DataRow("twokjhkjhkjh6mnvmnv4", 24)]
        public void First_and_Last_2Digits_Letters(string input, int expected)
        {
            var reader = new CalibrationReader();
            var result = reader.ParseCalibration(input, true);
            Assert.AreEqual(expected, result);
        }


        [DataTestMethod]
        [DataRow("1abc2three", 13)]
        [DataRow("fivefv1abc2ffffnine", 59)]
        [DataRow("3ghjg5ghfourjghjg", 34)]
        [DataRow("twokjhkjhkjh6mnvmnv4", 24)]
        public void First_and_Last_MoreThan2Digits_Letters(string input, int expected)
        {
            var reader = new CalibrationReader();
            var result = reader.ParseCalibration(input, true);
            Assert.AreEqual(expected, result);
        }


        [DataTestMethod]
        [DataRow("trebsevenuchet", 77)]
        [DataRow("fourhjkhkjh", 44)]
        [DataRow("ghjgghjghjgeight", 88)]
        public void First_and_Last_Only1Digit_Letters(string input, int expected)
        {
            var reader = new CalibrationReader();
            var result = reader.ParseCalibrationWithLetters(input);
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("two1nine", 29)]
        [DataRow("eightwothree", 83)]
        [DataRow("abcone2threexyz", 13)]
        [DataRow("xtwone3four", 24)]
        [DataRow("4nineeightseven2", 42)]
        [DataRow("zoneight234", 14)]
        [DataRow("7pqrstsixteen", 76)]
        public void First_and_Last_SmpleInput_Letters(string input, int expected)
        {
            var reader = new CalibrationReader();
            var result = reader.ParseCalibrationWithLetters(input);
            Assert.AreEqual(expected, result);
        }


    }


}
