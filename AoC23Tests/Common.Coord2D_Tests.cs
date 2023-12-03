using AoC23.Common;

namespace AoC23Tests.Common
{
    [TestClass]
    public class Common_Coord2D
    {
        private static IEnumerable<object[]> TestData_Sum()
        {
            yield return new object[] { new Coord2D(1,2), new Coord2D(3, 4), new Coord2D(4, 6)};
            yield return new object[] { new Coord2D(99, 99), new Coord2D(-98, -97), new Coord2D(1, 2) };
        }

        private static IEnumerable<object[]> TestData_Substract()
        {
            yield return new object[] { new Coord2D(1, 2), new Coord2D(3, 4), new Coord2D(-2, -2) };
            yield return new object[] { new Coord2D(99, 99), new Coord2D(-10, -20), new Coord2D(109, 119) };
        }

        private static IEnumerable<object[]> TestData_ProductByScalar()
        {
            yield return new object[] { new Coord2D(1, 2), 5, new Coord2D(5, 10) };
            yield return new object[] { new Coord2D(10, 30), -2, new Coord2D(-20, -60) };
        }

        private static IEnumerable<object[]> TestData_DivideByScalar()
        {
            yield return new object[] { new Coord2D(15, 45), 5, new Coord2D(3, 9) };
            yield return new object[] { new Coord2D(22, -99), -11, new Coord2D(-2, 9) };
        }

        private static IEnumerable<object[]> TestData_Equality()
        {
            yield return new object[] { new Coord2D(15, 45), new Coord2D(15, 45), true };
            yield return new object[] { new Coord2D(15, 45), new Coord2D(15, 46), false };
            yield return new object[] { new Coord2D(15, 45), new Coord2D(16, 46), false };
            yield return new object[] { new Coord2D(15, 45), new Coord2D(16, 45), false };
        }

        private static IEnumerable<object[]> TestData_Deconstruct()
        {
            yield return new object[] { new Coord2D(15, 45), (15,45) };
            yield return new object[] { new Coord2D(-11, 34), (-11, 34)  };
        }

        private static IEnumerable<object[]> TestData_Manhattan()
        {
            yield return new object[] { new Coord2D(1, 1), new Coord2D(3, 5), 6 };
            yield return new object[] { new Coord2D(-1, -1), new Coord2D(-3, 5), 8 };
        }

        private static IEnumerable<object[]> TestData_Neighbors()
        {
            yield return new object[] { new Coord2D(1, 1), new List<Coord2D>() { new Coord2D(0, 1), new Coord2D(2, 1),
                                                                                 new Coord2D(1, 0), new Coord2D(1, 2)} };
            yield return new object[] { new Coord2D(-3, 5), new List<Coord2D>() { new Coord2D(-4, 5), new Coord2D(-2, 5),
                                                                                  new Coord2D(-3, 4), new Coord2D(-3, 6)} };
        }

        private static IEnumerable<object[]> TestData_Neighbors8()
        {
            yield return new object[] { new Coord2D(1, 1), new List<Coord2D>() { new Coord2D(0, 0), new Coord2D(0, 1), new Coord2D(0, 2),
                                                                                 new Coord2D(1, 0),                    new Coord2D(1, 2),
                                                                                 new Coord2D(2, 0), new Coord2D(2, 1), new Coord2D(2, 2)} };
            
            yield return new object[] { new Coord2D(-3, 5), new List<Coord2D>() {new Coord2D(-4, 4), new Coord2D(-4, 5), new Coord2D(-4, 6),
                                                                                 new Coord2D(-3, 4),                    new Coord2D(-3, 6),
                                                                                 new Coord2D(-2, 4), new Coord2D(-2, 5), new Coord2D(-2, 6)} };
        }

        [DataTestMethod]
        [DynamicData(nameof(TestData_Sum), DynamicDataSourceType.Method)]
        public void Should_Sum_2_Coords(Coord2D coord_a, Coord2D coord_b, Coord2D expected)
        {
            var result = coord_a + coord_b;
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestData_Substract), DynamicDataSourceType.Method)]
        public void Should_Substract_2_Coords(Coord2D coord_a, Coord2D coord_b, Coord2D expected)
        {
            var result = coord_a - coord_b;
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestData_ProductByScalar), DynamicDataSourceType.Method)]
        public void Should_Multiply_Coord_By_Scalar(Coord2D coord_a, int scalar, Coord2D expected)
        {
            var result = scalar * coord_a;
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestData_ProductByScalar), DynamicDataSourceType.Method)]
        public void Should_Multiply_Coord_By_Scalar_Inverse_Order(Coord2D coord_a, int scalar, Coord2D expected)
        {
            var result = coord_a * scalar;
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestData_DivideByScalar), DynamicDataSourceType.Method)]
        public void Should_Divide_Coord_By_Scalar(Coord2D coord_a, int scalar, Coord2D expected)
        {
            var result = coord_a / scalar;
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestData_Equality), DynamicDataSourceType.Method)]
        public void Should_Compare_Equality(Coord2D coord_a, Coord2D coord_b, bool expected)
        {
            var result = coord_a == coord_b;
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestData_Equality), DynamicDataSourceType.Method)]
        public void Should_Compare_Difference(Coord2D coord_a, Coord2D coord_b, bool expected)
        {
            var result = coord_a != coord_b;
            Assert.AreEqual(!expected, result);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestData_Manhattan), DynamicDataSourceType.Method)]
        public void Should_Calculate_Manhattan_Distance(Coord2D coord_a, Coord2D coord_b, int expected)
        {
            var result = coord_a.Manhattan(coord_b);
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestData_Deconstruct), DynamicDataSourceType.Method)]
        public void Should_Deconstruct_To_Tuple(Coord2D coord_a, (int a, int b) expected)
        {
            (int aa, int bb) = coord_a;
            Assert.IsTrue(aa == expected.a && bb == expected.b);
        }

        
        [DataTestMethod]
        [DynamicData(nameof(TestData_Neighbors), DynamicDataSourceType.Method)]
        public void Should_Find_Neighbors(Coord2D coord_a, List<Coord2D> expected)
        {
            var neighs = coord_a.GetNeighbors().ToList();
            var test = neighs.Count == expected.Count;

            foreach (var neighbor in neighs)
                test &= expected.Contains(neighbor);

            Assert.IsTrue(test);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestData_Neighbors8), DynamicDataSourceType.Method)]
        public void Should_Find_Neighbors8(Coord2D coord_a, List<Coord2D> expected)
        {
            var neighs = coord_a.GetNeighbors8().ToList();
            var test = neighs.Count == expected.Count;

            foreach (var neighbor in neighs)
                test &= expected.Contains(neighbor);

            Assert.IsTrue(test);
        }
    }
}