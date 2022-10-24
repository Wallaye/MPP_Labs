using Faker.Core.Config;
using Faker.Core.Generators;
using Faker.Core.Tests.ConfigClasses;
using System.Collections;

namespace Faker.Core.Tests
{
    public class Tests
    {
        private Faker _faker;
        [SetUp]
        public void Setup()
        {
            _faker = new();
        }

        [Test]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(byte))]
        [TestCase(typeof(short))]
        [TestCase(typeof(int))]
        [TestCase(typeof(long))]
        [TestCase(typeof(float))]
        [TestCase(typeof(double))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(string))]
        [TestCase(typeof(bool))]
        [TestCase(typeof(char))]
        [TestCase(typeof(TestInit))]
        [TestCase(typeof(TestCtorStruct))]
        [TestCase(typeof(List<List<int>>))]

        public void CreatePrimitiveTest(Type type)
        {
            Assert.DoesNotThrow(() => _faker.Create(type));
        }

        [Test]
        [TestCase(typeof(byte))]
        [TestCase(typeof(short))]
        [TestCase(typeof(int))]
        [TestCase(typeof(long))]
        [TestCase(typeof(float))]
        [TestCase(typeof(double))]
        [TestCase(typeof(string))]
        [TestCase(typeof(bool))]
        [TestCase(typeof(char))]
        public void CreatePrimitiveNotDefaultValue(Type type)
        {
            Assert.That(_faker.Create(type), Is.Not.EqualTo(UserTypeGenerator.GetDefaultValue(type)));
        }

        [Test]
        public void CreateInitedUserType()
        {
            TestInit testClass = _faker.Create<TestInit>();

            Assert.Multiple(() =>
            {
                Assert.NotZero(testClass.Int);
                Assert.NotZero(testClass.Byte);
                Assert.NotNull(testClass.String);
                Assert.True(testClass.Bool);
                Assert.NotNull(testClass.b);
                Assert.That(testClass.b.symbol, Is.Not.EqualTo('\0'));
            });
        }

        [Test]
        public void CreateWithCycleDependencies()
        {
            Assert.DoesNotThrow(() => _faker.Create<C>());
        }

        [Test]
        public void CreateCycleParentInited()
        {
            C testClass = _faker.Create<C>();
            Assert.Multiple(() =>
            {
                Assert.NotNull(testClass.d.e.c);
                Assert.NotNull(testClass.d.e.c.s);
            });
        }

        [Test]
        public void CreateSelectConstructor()
        {
            TestCtor testClass = _faker.Create<TestCtor>();
            Assert.NotZero(testClass.C);
        }

        [Test]
        public void CreateTestStructCtor()
        {
            TestCtorStruct ctorStruct = _faker.Create<TestCtorStruct>();

            Assert.Multiple(() =>
            {
                Assert.NotNull(ctorStruct.String);
                Assert.NotZero(ctorStruct.Int);
            });
        }

        [Test]
        public void CreateTestInitStruct()
        {
            TestInitStruct initStruct = _faker.Create<TestInitStruct>();

            Assert.Multiple(() =>
            {
                Assert.NotZero(initStruct.Double);
                Assert.True(initStruct.Bool);
            });
        }

        [Test]
        public void CreateListValueCheckTest()
        {
            bool containsZeros = _faker.Create<List<int>>()
                .Where(p => p == 0).Count() > 0;

            Assert.False(containsZeros);
        }

        [Test]
        [TestCase(typeof(List<List<List<int>>>))]
        [TestCase(typeof(List<List<decimal>>))]
        public void CreateListTest(Type type)
        {
            var list = (IList)_faker.Create(type);

            Assert.Multiple(() =>
            {
                Assert.IsNotEmpty(list);
                Assert.IsNotEmpty((IList)list[0]);
            });
        }

        [Test]
        public void ConfigTest()
        {
            var config = new FakerConfig();
            config.Add<User, int, AgeGenerator>(configTest => configTest.Age);
            config.Add<User, string, NameGenerator>(configTest => configTest.Name);
            var faker = new Faker(config);
            User configTest = faker.Create<User>();

            Assert.Multiple(() =>
            {
                Assert.That(configTest.Name, Is.EqualTo("Name"));
                Assert.That(configTest.Age, Is.InRange(1,99));
            });
        }

    }
}