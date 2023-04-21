using FCA_Algorithms.Models;

namespace FCA_Algorithms.Algorithms
{
    public class AlgorithmAddIntent
    {
        public static Dictionary<Concept, List<Concept>> AddIntent(FormalContext fc)
        {
            var bottomConcept = new Concept(new List<string>(), fc.M);

            var lattice = new Dictionary<Concept, List<Concept>>()
            {
                { bottomConcept, new List<Concept>() }
            };

            foreach (var g in fc.G)
            {
                var objectConcept = Add(fc.GetAttributesOfObject(g), bottomConcept, lattice);

                AddGToExtentAbove(g, objectConcept, lattice);
            }

            return lattice;
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

        public static Concept Add(List<string> intent, Concept generatorConcept, Dictionary<Concept, List<Concept>> lattice)
        {
            generatorConcept = GetHighestNodeOfIntent(intent.ToList(), generatorConcept, lattice);
            if (Enumerable.SequenceEqual(generatorConcept.Intent, intent)) return generatorConcept;

            var generatorParents = lattice[generatorConcept].ToList();
            var newParents = new List<Concept>();

            if (generatorParents != null)
            {
                for (int i = 0; i < generatorParents.Count(); i++)
                {
                    if (!generatorParents[i].Intent.All(item => intent.Contains(item)))
                    {
                        generatorParents[i] = Add(generatorParents[i].Intent.Intersect(intent).ToList(), generatorParents[i], lattice);
                    }

                    var addParent = true;

                    var newParentsCopy = newParents.ToList();
                    foreach (var parent in newParents)
                    {

                        if (generatorParents[i].Intent.All(item => parent.Intent.Contains(item)))
                        {
                            addParent = false;
                            break;
                        }
                        else if (parent.Intent.All(item => generatorParents[i].Intent.Contains(item)))
                            newParentsCopy.Remove(parent);
                    }
                    newParents = newParentsCopy.ToList();

                    if (addParent) 
                        newParents.Add(generatorParents[i]);
                }
            }

            var newConcept = new Concept(generatorConcept.Extent.ToList(), intent);

            lattice.Add(newConcept, new List<Concept>());

            if (newParents != null)
            {
                foreach (var parent in newParents)
                {
                    lattice[generatorConcept].Remove(parent);
                    lattice[newConcept].Add(parent);
                }
            }

            lattice[generatorConcept].Add(newConcept);

            return newConcept;
        }
    }
}
