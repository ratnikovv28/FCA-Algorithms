using FCA_Algorithms.Algorithms;
using FCA_Algorithms.Models;
using FCA_Algorithms.Services;
using Newtonsoft.Json;

namespace FCA_Algorithms.Tests
{
    public class Tests
    {
        private FormalContext _fc1;
        private FormalContext _fc2;
        private FormalContext _fc3;

        [SetUp]
        public void Setup()
        {
            _fc1 = new FormalContext(
                new List<string>()
                {
                    "1", "2", "3", "4", "5"
                },
                new List<string>()
                {
                    "A", "B", "C", "D", "E", "F",
                },
                new List<Data>()
                {
                    new Data()
                    {
                        Inds = new List<int>()
                        {
                            0, 2, 4
                        }
                    },
                    new Data()
                    {
                        Inds = new List<int>()
                        {
                            0, 1, 5
                        }
                    },
                    new Data()
                    {
                        Inds = new List<int>()
                        {
                            2, 3, 4
                        }
                    },
                    new Data()
                    {
                        Inds = new List<int>()
                        {
                            0, 3, 5
                        }
                    },
                    new Data()
                    {
                        Inds = new List<int>()
                        {
                            1, 4
                        }
                    }
                });

            _fc2 = new FormalContext(
                new List<string>()
                {
                    "1", "2", "3", "4", "5"
                },
                new List<string>()
                {
                    "A", "B", "C", "D"
                },
                new List<Data>()
                {
                    new Data()
                    {
                        Inds = new List<int>()
                        {
                            0, 2, 3
                        }
                    },
                    new Data()
                    {
                        Inds = new List<int>()
                        {
                            0, 1, 3
                        }
                    },
                    new Data()
                    {
                        Inds = new List<int>()
                        {
                            2, 3
                        }
                    },
                    new Data()
                    {
                        Inds = new List<int>()
                        {
                            0, 3
                        }
                    },
                    new Data()
                    {
                        Inds = new List<int>()
                        {
                            1, 3
                        }
                    }
                });

            _fc3 = new FormalContext(
                new List<string>()
                {
                    "1", "2", "3", "4", "5"
                },
                new List<string>()
                {
                    "A", "B", "C", "D"
                },
                new List<Data>()
                {
                    new Data()
                    {
                        Inds = new List<int>()
                        {
                            0, 1, 2, 3
                        }
                    },
                    new Data()
                    {
                        Inds = new List<int>()
                        {
                            1, 3
                        }
                    },
                    new Data()
                    {
                        Inds = new List<int>()
                        {
                            2, 3
                        }
                    },
                    new Data()
                    {
                        Inds = new List<int>()
                        {
                            1
                        }
                    },
                    new Data()
                    {
                        Inds = new List<int>()
                        {
                            1, 3
                        }
                    }
                });
        }


        //Тест на нулевой формальный контекст
        [Test]
        public void TestAddAtom_EmptyFormalContext()
        {
            // Arrange
            var fc = new FormalContext(new List<string>(), new List<string>(), new List<Data>());

            // Act
            var result = AlgorithmAddAtom.AddAtom(fc);

            // Assert
            Assert.IsNotNull(result); 
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(0, result.ElementAt(0).Value.Count);
        }

        //Тест алгоритма AddAtom
        [Test]
        public void TestAddAtom()
        {
            // Arrange
            

            // Act
            var result1 = AlgorithmAddAtom.AddAtom(_fc1);
            var result2 = AlgorithmAddAtom.AddAtom(_fc2);
            var result3 = AlgorithmAddAtom.AddAtom(_fc3);

            // Assert
            Assert.AreEqual(11, result1.Count);
            Assert.AreEqual(7, result2.Count);
            Assert.AreEqual(6, result3.Count);
        }

        //Тест алгоритма AddIntent
        [Test]
        public void TestAddIntent()
        {
            // Arrange

            // Act
            var result1 = AlgorithmAddIntent.AddIntent(_fc1);
            var result2 = AlgorithmAddIntent.AddIntent(_fc2);
            var result3 = AlgorithmAddIntent.AddIntent(_fc3);

            // Assert
            Assert.AreEqual(11, result1.Count);
            Assert.AreEqual(7, result2.Count);
            Assert.AreEqual(6, result3.Count);
        }

        //Random testing technique
        [Test]
        public void TestEachOther()
        {
            var rnd = new Random();
            int length = rnd.Next(0, 5000);
            for (int i = 0; i < length; i++)
            {
                // Arrange
                var fc = new FormalContext();

                // Act
                var addAtom = AlgorithmAddAtom.AddAtom(fc);
                var addIntent = AlgorithmAddIntent.AddIntent(fc);

                string jsonDataOfAddAtom = JsonConvert.SerializeObject(addAtom, Formatting.Indented);
                string jsonDataOfAddIntent = JsonConvert.SerializeObject(addIntent, Formatting.Indented);

                // Assert
                Assert.AreEqual(true, jsonDataOfAddAtom == jsonDataOfAddIntent);
            }
        }
    }
}