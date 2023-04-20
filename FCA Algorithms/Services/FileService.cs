using FCA_Algorithms.Models;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace FCA_Algorithms.Services
{
    public class FileService
    {
        private static string addAtomFile = @"..\..\..\Data\ResultOfAddAtom.json";
        private static string addIntentFile = @"..\..\..\Data\ResultOfAddIntent.json";
        private static string addAtomFilteredFile = @"..\..\..\Data\ResultOfFilteredAddAtom.json";
        private static string addIntentFilteredFile = @"..\..\..\Data\ResultOfFilteredAddIntent.json";

        public static FormalContext GetDataFromJsonFile(string contextFilePath)
        {
            if (File.Exists(contextFilePath))
            {
                var jsonString = File.ReadAllText(contextFilePath);
                var combinedObjects = JsonConvert.DeserializeObject<List<CombinedObject>>(jsonString);

                return new FormalContext(combinedObjects[0].ObjNames, combinedObjects[0].Params.AttrNames, combinedObjects[1].Data);
            }
            else
                return null;
        }

        public static void SetDataToJsonFiles(Dictionary<Concept, List<Concept>> addAtom, Dictionary<Concept, List<Concept>> addIntent, int numberOfArguments, bool? filteredFlag)
        {
            var addAtomLattice = new Lattice()
            {
                Data = new RootData()
                {
                    NodesCount = addAtom.Count,
                    ArcsCount = numberOfArguments
                },
                Nodes = new List<NodeData>(),
                Arcs = new List<ArcsData>()
            };
            var addIntentLattice = new Lattice()
            {
                Data = new RootData()
                {
                    NodesCount = addIntent.Count,
                    ArcsCount = numberOfArguments
                },
                Nodes = new List<NodeData>(),
                Arcs = new List<ArcsData>()
            };

            foreach (var node in addAtom)
            {
                addAtomLattice.Nodes.Add(new NodeData()
                {
                    Ext = new NodeData.Data()
                    {
                        Count = node.Key.Extent.Count,
                        Names = node.Key.Extent.ToArray()
                    },
                    Int = new NodeData.Data()
                    {
                        Count = node.Key.Intent.Count,
                        Names = node.Key.Intent.ToArray()
                    },
                });

                foreach (var item in node.Value)
                {
                    addAtomLattice.Arcs.Add(new ArcsData()
                    {
                        S = item.Id,
                        D = node.Key.Id
                    });
                }
            }

            addAtomLattice.Arcs = addAtomLattice.Arcs.OrderBy(a => a.S).ToList();

            foreach (var node in addIntent)
            {
                addIntentLattice.Nodes.Add(new NodeData()
                {
                    Ext = new NodeData.Data()
                    {
                        Count = node.Key.Extent.Count,
                        Names = node.Key.Extent.ToArray()
                    },
                    Int = new NodeData.Data()
                    {
                        Count = node.Key.Intent.Count,
                        Names = node.Key.Intent.ToArray()
                    },
                });

                foreach (var item in node.Value)
                {
                    addIntentLattice.Arcs.Add(new ArcsData()
                    {
                        S = item.Id,
                        D = node.Key.Id
                    });
                }
            }

            addIntentLattice.Arcs = addIntentLattice.Arcs.OrderBy(a => a.S).ToList();

            string jsonDataOfAddAtom = JsonConvert.SerializeObject(addAtomLattice, Formatting.Indented);
            string jsonDataOfAddIntent = JsonConvert.SerializeObject(addIntentLattice, Formatting.Indented);
            if (!filteredFlag.HasValue)
            {
                File.WriteAllText(addAtomFile, jsonDataOfAddAtom);
                File.WriteAllText(addIntentFile, jsonDataOfAddIntent);
            }
            else
            {
                File.WriteAllText(addAtomFilteredFile, jsonDataOfAddAtom);
                File.WriteAllText(addIntentFilteredFile, jsonDataOfAddIntent);
            }
        }
    }
}
