using FCA_Algorithms.Models;

namespace FCA_Algorithms.Algorithms
{
    public class AlgorithmAddAtom
    {
        public static Dictionary<Concept, List<Concept>> AddAtom(FormalContext fc)
        {
            var bottomConcept = new Concept(new List<string>(), fc.M);

            var lattice = new Dictionary<Concept, List<Concept>>()
            {
                { bottomConcept, new List<Concept>() }
            };

            foreach (var g in fc.G)
            {
                Add(fc.GetAttributesOfObject(g), g, bottomConcept, lattice);
            }

            if (lattice[bottomConcept].Count == 1 && bottomConcept.Intent.All(item => lattice.ElementAt(1).Key.Intent.Contains(item)))
            {
                lattice.Remove(bottomConcept);
            }

            return lattice;
        }

        public static Concept Add(List<string> intent, string g, Concept generatorConcept, Dictionary<Concept, List<Concept>> lattice)
        {
            var candidateParents = lattice[generatorConcept].ToList();
            var nodeHighests = new List<Concept>(); //newParents

            for (int i = 0; i < candidateParents.Count(); i++)
            {
                var newIntent = candidateParents[i].Intent.Intersect(intent).ToList();
                var highest = GetHighestNodeOfIntent(newIntent.ToList(), candidateParents[i], lattice);

                if (!Enumerable.SequenceEqual(highest.Intent, newIntent))
                {
                    highest = Add(newIntent.ToList(), g, highest, lattice);
                }
                else
                {
                    AddGToExtentAbove(g, highest, lattice);
                }

                if (Enumerable.SequenceEqual(intent, newIntent))
                    return highest;

                var addHighest = true;

                if (nodeHighests.Count != 0)
                {
                    var nodeHighestsCopy = nodeHighests.ToList();
                    foreach (var nodeHighest in nodeHighests)
                    {
                        if (highest.Intent.All(item => nodeHighest.Intent.Contains(item)))
                        {
                            addHighest = false;
                            break;
                        }
                        else if (nodeHighest.Intent.All(item => highest.Intent.Contains(item)))
                            nodeHighestsCopy.Remove(nodeHighest);
                    }
                    nodeHighests = nodeHighestsCopy.ToList();
                }

                if (addHighest) nodeHighests.Add(highest);
            }

            var newConcept = new Concept(generatorConcept.Extent.Union(new List<string>() { g }).ToList(), intent);

            lattice.Add(newConcept, new List<Concept>());

            foreach (var nodeHighest in nodeHighests)
            {
                lattice[generatorConcept].Remove(nodeHighest);
                lattice[newConcept].Add(nodeHighest);
            }

            lattice[generatorConcept].Add(newConcept);

            return newConcept;
        }

        public static void AddGToExtentAbove(string g, Concept objectConcept, Dictionary<Concept, List<Concept>> lattice)
        {
            var parents = lattice[objectConcept];

            if (!objectConcept.Extent.Contains(g))
                objectConcept.Extent.Add(g);

            foreach (var parent in parents)
            {
                AddGToExtentAbove(g, parent, lattice);
            }

            lattice[objectConcept] = parents;
        }

        public static Concept GetHighestNodeOfIntent(List<string> intent, Concept generatorConcept, Dictionary<Concept, List<Concept>> lattice)
        {
            bool parentIsHighest = true;

            while (parentIsHighest)
            {
                parentIsHighest = false;
                var parents = lattice[generatorConcept].ToList();

                foreach (var parent in parents)
                {
                    if (intent.All(item => parent.Intent.Contains(item)))
                    {
                        generatorConcept = parent;
                        parentIsHighest = true;
                        break;
                    }
                }
            }

            return generatorConcept;
        }
    }
}
