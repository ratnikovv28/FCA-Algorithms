using FCA_Algorithms.Algorithms;
using FCA_Algorithms.Models;
using FCA_Algorithms.Services;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;

namespace FCA_Algorithms
{
    public class Program
    {
        public static long addAtomTimeTicks = 0;
        public static long addIntentTimeTicks = 0;
        public static bool filteringFlag = false;

        public static FormalContext fc;
        public static Dictionary<Concept, List<Concept>> addAtom;
        public static Dictionary<Concept, List<Concept>> addIntent;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("An application for finding maximal rectangles in a formal context using the AddAtom and AddIntent algorithm\n");
                Console.WriteLine("1 - Infomation about algorithms");
                Console.WriteLine("2 - Start algorithms");
                Console.WriteLine("4 - Test algorithms");
                if (filteringFlag)
                    Console.WriteLine("3 - Find context data");
                Console.WriteLine("0 - Exiting the program");
                switch (char.ToLower(Console.ReadKey(true).KeyChar))
                {
                    case '1': ShowInfoAboutAlgorithms(); break;
                    case '2': StartAlgorithms(); break;
                    case '3' when filteringFlag == true: FindContextData(); break;
                    case '4': GetDataOfAlgorithms(); break;
                    case '0': return;
                    default: break;
                }
            }
        }

        public static void GetDataOfAlgorithms()
        {
            int gSize1 = 54;
            int mSize1 = 54;

            int gSize2 = 56;
            int mSize2 = 56;

            int dischargeToFC1 = 5;
            int dischargeToFC2 = 3;
            int dischargeToFC3 = 2;

            int discharge1 = 80;
            int discharge2 = 67;
            int discharge3 = 50;

            Console.Clear();

            for (int i = 15; i < 35; i++)
            {
                Console.WriteLine($"Время генерации формальных контекстов на матрице {gSize1}x{mSize1} с разной разреженностью");

                var sw5 = new Stopwatch();
                sw5.Start();
                var fcMin1 = new FormalContext(gSize1, mSize1, dischargeToFC1);
                sw5.Stop();
                Console.WriteLine($"Разреженность матрицы {discharge1}%: {sw5.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");

                sw5 = new Stopwatch();
                sw5.Start();
                var fcMed1 = new FormalContext(gSize1, mSize1, dischargeToFC2);
                sw5.Stop();
                Console.WriteLine($"Разреженность матрицы {discharge2}%: {sw5.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");

                sw5 = new Stopwatch();
                sw5.Start();
                var fcMax1 = new FormalContext(gSize1, mSize1, dischargeToFC3);
                sw5.Stop();
                Console.WriteLine($"Разреженность матрицы {discharge3}%: {sw5.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");
                                                                                                                                    
                Console.WriteLine($"Время генерации формальных контекстов на матрице {gSize2}x{mSize2} с разной разреженностью");

                var sw6 = new Stopwatch();
                sw6.Start();
                var fcMin2 = new FormalContext(gSize2, mSize2, dischargeToFC1);
                sw6.Stop();
                Console.WriteLine($"Разреженность матрицы {discharge1}%: {sw6.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");

                sw6 = new Stopwatch();
                sw6.Start();
                var fcMed2 = new FormalContext(gSize2, mSize2, dischargeToFC2);
                sw6.Stop();
                Console.WriteLine($"Разреженность матрицы {discharge2}%: {sw6.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");

                sw6 = new Stopwatch();
                sw6.Start();
                var fcMax2 = new FormalContext(gSize2, mSize2, dischargeToFC3);
                sw6.Stop();
                Console.WriteLine($"Разреженность матрицы {discharge3}%: {sw6.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");

                Console.WriteLine();

                var sw7 = new Stopwatch();
                sw7.Start();
                FileService.CreateJsonFileByFormalContext(fcMax1, FileService.formalContextMax1File.Insert(27, (i + 1).ToString()));
                sw7.Stop();
                Console.WriteLine($"Время загрузки формального контекста {gSize1}x{mSize1}, разреженность {discharge1}%: {sw7.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");
                sw7 = new Stopwatch();
                sw7.Start();
                FileService.CreateJsonFileByFormalContext(fcMax2, FileService.formalContextMax2File.Insert(27, (i + 1).ToString()));
                sw7.Stop();
                Console.WriteLine($"Время загрузки формального контекста {gSize1}x{mSize1}, разреженность {discharge2}%: {sw7.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");
                sw7 = new Stopwatch();
                sw7.Start();
                FileService.CreateJsonFileByFormalContext(fcMed1, FileService.formalContextMed1File.Insert(27, (i + 1).ToString()));
                sw7.Stop();
                Console.WriteLine($"Время загрузки формального контекста {gSize1}x{mSize1}, разреженность {discharge3}%: {sw7.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");
                sw7 = new Stopwatch();
                sw7.Start();
                FileService.CreateJsonFileByFormalContext(fcMed2, FileService.formalContextMed2File.Insert(27, (i + 1).ToString()));
                sw7.Stop();
                Console.WriteLine($"Время загрузки формального контекста {gSize2}x{mSize2}, разреженность {discharge1}%: {sw7.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");
                sw7 = new Stopwatch();
                sw7.Start();
                FileService.CreateJsonFileByFormalContext(fcMin1, FileService.formalContextMin1File.Insert(27, (i + 1).ToString()));
                sw7.Stop();
                Console.WriteLine($"Время загрузки формального контекста {gSize2}x{mSize2}, разреженность {discharge2}%: {sw7.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");
                sw7 = new Stopwatch();
                sw7.Start();
                FileService.CreateJsonFileByFormalContext(fcMin2, FileService.formalContextMin2File.Insert(27, (i + 1).ToString()));
                sw7.Stop();
                Console.WriteLine($"Время загрузки формального контекста {gSize2}x{mSize2}, разреженность {discharge3}%: {sw7.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");

                Console.WriteLine();

                Console.WriteLine($"Время работы алгоритма AddAtom на размере матрицы {gSize1}x{mSize1}");

                var sw1 = new Stopwatch();
                sw1.Start();
                var addAtomMin1 = AlgorithmAddAtom.AddAtom(fcMin1);
                sw1.Stop();
                Console.WriteLine($"Разреженность матрицы {discharge1}%: {sw1.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");

                sw1 = new Stopwatch();
                sw1.Start();
                var addAtomMed1 = AlgorithmAddAtom.AddAtom(fcMed1);
                sw1.Stop();
                Console.WriteLine($"Разреженность матрицы {discharge2}%: {sw1.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");

                sw1 = new Stopwatch();
                sw1.Start();
                var addAtomMax1 = AlgorithmAddAtom.AddAtom(fcMax1);
                sw1.Stop();
                Console.WriteLine($"Разреженность матрицы {discharge3}%: {sw1.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");

                Console.WriteLine($"Время работы алгоритма AddIntent на размере матрицы {gSize1}x{mSize1}");

                var sw2 = new Stopwatch();
                sw2.Start();
                var addIntentMin1 = AlgorithmAddIntent.AddIntent(fcMin1);
                sw2.Stop();
                Console.WriteLine($"Разреженность матрицы {discharge1}%: {sw2.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");

                sw2 = new Stopwatch();
                sw2.Start();
                var addIntentMed1 = AlgorithmAddIntent.AddIntent(fcMed1);
                sw2.Stop();
                Console.WriteLine($"Разреженность матрицы {discharge2}%: {sw2.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");

                sw2 = new Stopwatch();
                sw2.Start();
                var addIntentMax1 = AlgorithmAddIntent.AddIntent(fcMax1);
                sw2.Stop();
                Console.WriteLine($"Разреженность матрицы {discharge3}%: {sw2.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");

                Console.WriteLine();

                Console.WriteLine($"Время работы алгоритма AddAtom на размере матрицы {gSize2}x{mSize2}");

                var sw3 = new Stopwatch();
                sw3.Start();
                var addAtomMin2 = AlgorithmAddAtom.AddAtom(fcMin2);
                sw3.Stop();
                Console.WriteLine($"Разреженность матрицы {discharge1}%: {sw3.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");

                sw3 = new Stopwatch();
                sw3.Start();
                var addAtomMed2 = AlgorithmAddAtom.AddAtom(fcMed2);
                sw3.Stop();
                Console.WriteLine($"Разреженность матрицы {discharge2}%: {sw3.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");

                sw3 = new Stopwatch();
                sw3.Start();
                var addAtomMax2 = AlgorithmAddAtom.AddAtom(fcMax2);
                sw3.Stop();
                Console.WriteLine($"Разреженность матрицы {discharge3}%: {sw3.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");

                Console.WriteLine($"Время работы алгоритма AddIntent на размере матрицы {gSize2}x{mSize2}");

                var sw4 = new Stopwatch();
                sw4.Start();
                var addIntentMin2 = AlgorithmAddIntent.AddIntent(fcMin2);
                sw4.Stop();
                Console.WriteLine($"Разреженность матрицы {discharge1}%: {sw4.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");

                sw4 = new Stopwatch();
                sw4.Start();
                var addIntentMed2 = AlgorithmAddIntent.AddIntent(fcMed2);
                sw4.Stop();
                Console.WriteLine($"Разреженность матрицы {discharge2}%: {sw4.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");

                sw4 = new Stopwatch();
                sw4.Start();
                var addIntentMax2 = AlgorithmAddIntent.AddIntent(fcMax2);
                sw4.Stop();
                Console.WriteLine($"Разреженность матрицы {discharge3}%: {sw4.Elapsed.ToString(@"hh\:mm\:ss\.fff")}");

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

            }

            Console.WriteLine("\nPress any button to exit to the main menu");
            Console.ReadKey(true);
        }


        public static void ShowInfoAboutAlgorithms()
        {
            Console.Clear();

            Console.WriteLine("Infomation about algorithms\n");

            Console.WriteLine("AddAtom algorithm - an algorithm for constructing lattices, called by Van Der Merwes EA lattices.\n" +
                "An EA-lattice of context is a lattice of Van Der Merwe concepts, supplemented\nwith individual elements for each attribute and object.\n");

            Console.WriteLine("AddIntent algorithm - an algorithm for constructing grids, is a later version of AddAtom.\n" +
                "Being incremental, it relies on the graph constructed from the first objects of the context to integrate\nthe next object into the lattice." +
                "It takes a lattice of concepts as input, initially adding content\nto the lattice, after adding the objects themselves on top of the reified, created node.\n");

            Console.WriteLine("For this program to work correctly, you need to load the context into the Data folder and name the file context.json");

            Console.WriteLine("\nPress any button to exit to the main menu");
            Console.ReadKey(true);
        }

        public static void StartAlgorithms()
        {
            Console.Clear();

            string contextFilePath = @"..\..\..\Data\context.json";

            //Создаем формальный контекст на основании входного файла
            fc = FileService.GetDataFromJsonFile(contextFilePath);
            if (fc == null)
            {
                Console.WriteLine("There is no context file in the Data folder! Load it before running the algorithm");

                Console.WriteLine("\nPress any button to exit to the main menu");
                Console.ReadKey(true);

                return;
            }

            Console.WriteLine("AddAtom began its execution");
            var sw1 = new Stopwatch();
            sw1.Start();
            addAtom = AlgorithmAddAtom.AddAtom(fc);
            sw1.Stop();
            addAtomTimeTicks = sw1.ElapsedTicks;
            Console.WriteLine("AddAtom has finished its execution\n");

            Console.WriteLine("AddIntent began its execution");
            var sw2 = new Stopwatch();
            sw2.Start();
            addIntent = AlgorithmAddIntent.AddIntent(fc);
            sw2.Stop();
            addIntentTimeTicks = sw2.ElapsedTicks;
            Console.WriteLine("AddIntent has finished its execution\n");

            //Кладем данные работы двух алгоритмов в файлы
            FileService.SetDataToJsonFiles(addAtom, addIntent, fc.M.Count, null);

            Console.WriteLine("Algorithms execution time:");
            Console.WriteLine($"AddAtom ticks: {addAtomTimeTicks}");
            Console.WriteLine($"AddIntent ticks: {addIntentTimeTicks}");

            filteringFlag = true;

            Console.WriteLine("\nPress any button to exit to the main menu");
            Console.ReadKey(true);
        }

        public static void FindContextData()
        {
            Console.Clear();

            var inputObjects = "";
            var inputAttributes = "";
            var flag = true;
            var selectedObjects = new List<string>();
            var selectedAttributes = new List<string>();

            Console.WriteLine("Selecting context data\n");

            while (flag)
            {
                Console.WriteLine("Choose how items will be selected:");
                Console.WriteLine("1 - by objects");
                Console.WriteLine("2 - by attributes");
                Console.WriteLine("3 - by objects and attributes");
                Console.WriteLine("0 - Exit to the main menu");
                switch (char.ToLower(Console.ReadKey(true).KeyChar))
                {
                    case '1':
                        {
                            Console.WriteLine("Enter a list of objects separated by commas");
                            inputObjects = Console.ReadLine();
                            selectedObjects = inputObjects.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(w => w.Trim()).Where(w => fc.G.Contains(w.ToLower())).ToList();
                            flag = false;
                        }
                        break;
                    case '2':
                        {
                            Console.WriteLine("Enter a list of attributes separated by commas");
                            inputAttributes = Console.ReadLine();
                            selectedAttributes = inputAttributes.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(w => w.Trim()).Where(w => fc.M.Contains(w.ToLower())).ToList();
                            flag = false;
                        }
                        break;
                    case '3':
                        {
                            Console.WriteLine("Enter a list of objects separated by commas");
                            inputObjects = Console.ReadLine();
                            selectedObjects = inputObjects.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(w => w.Trim()).Where(w => fc.G.Contains(w.ToLower())).ToList();

                            Console.WriteLine("Enter a list of attributes separated by commas");
                            inputAttributes = Console.ReadLine();
                            selectedAttributes = inputAttributes.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(w => w.Trim()).Where(w => fc.M.Contains(w.ToLower())).ToList();
                            flag = false;
                        }
                        break;
                    case '0': return;
                    default: break;
                }
            }

            var filteringAddAtomData = FilterData(addAtom, selectedObjects, selectedAttributes);
            var filteringAddIntentData = FilterData(addIntent, selectedObjects, selectedAttributes);

            FileService.SetDataToJsonFiles(filteringAddAtomData, filteringAddIntentData, fc.M.Count, true);

            Console.WriteLine("\nPress any button to exit to the main menu");
            Console.ReadKey(true);
        }

        private static Dictionary<Concept, List<Concept>> FilterData(Dictionary<Concept, List<Concept>> dataSet, List<string> selectedObjects, List<string> selectedAttributes)
        {
            var filteredDataSet = new Dictionary<Concept, List<Concept>>();

            foreach (var node in dataSet)
            {
                if (selectedObjects.Count != 0 && selectedAttributes.Count != 0)
                {
                    if (selectedObjects.All(item => node.Key.Extent.Contains(item)) && selectedAttributes.All(item => node.Key.Intent.Contains(item)))
                    {
                        filteredDataSet.Add(node.Key, node.Value);
                    }
                }
                else if (selectedObjects.Count != 0)
                {
                    if (selectedObjects.All(item => node.Key.Extent.Contains(item)))
                    {
                        filteredDataSet.Add(node.Key, node.Value);
                    }
                }
                else if (selectedAttributes.Count != 0)
                {
                    if (selectedAttributes.All(item => node.Key.Intent.Contains(item)))
                    {
                        filteredDataSet.Add(node.Key, node.Value);
                    }
                }
            }

            return filteredDataSet;
        }
    }
}