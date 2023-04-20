using FCA_Algorithms.Algorithms;
using FCA_Algorithms.Models;
using FCA_Algorithms.Services;

namespace FCA_Algorithms.Tests
{
    public class Tests
    {
        private FormalContext _fc;

        [SetUp]
        public void Setup()
        {
            string testingContextFilePath = @"..\..\..\Data\TestingContext.json";
            
            // Инициализируем формальный контекст для тестирования при помощи JSON файла для тестирования
            _fc = FileService.GetDataFromJsonFile(testingContextFilePath);
        }

        [Test]
        public void TestAddAtom_EmptyFormalContext()
        {
            // Arrange
            var fc = new FormalContext(new List<string>(), new List<string>());

            // Act
            var result = AddAtom(fc);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.ContainsKey(new Concept(new List<string>(), fc.M, 0)));
            Assert.AreEqual(0, result[new Concept(new List<string>(), fc.M, 0)].Count);
        }

        [Test]
        public void AddAtom_()
        {
            var lattice = AlgorithmAddAtom.AddAtom(_fc);

            // Проверяем наличие нижнего концепта в решетке
            Assert.True(lattice.ContainsKey(new Concept(new List<string>(), _fc.M, 0)));

            // Проверяем, что у нижнего концепта нет родителей
            Assert.IsEmpty(lattice[new Concept(new List<string>(), _fc.M, 0)]);
        }
    }
}