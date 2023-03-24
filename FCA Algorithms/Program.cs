using System;
using System.Collections.Generic;
using System.Linq;

namespace FCA_Algorithms
{
    class FormalContext
    {
        private List<string> _g;
        private List<string> _m;
        private Dictionary<string, HashSet<string>> _i;

        public List<string> M
        {
            get { return _m; }
            set { _m = value; }
        }

        public List<string> G
        {
            get { return _g; }
            set { _g = value; }
        }

        public Dictionary<string, HashSet<string>> I
        {
            get { return _i; }
            set { _i = value; }
        }

        public FormalContext(List<string> g, List<string> m, Dictionary<string, HashSet<string>> i)
        {
            G = g;
            M = m;
            I = i;
        }

        public bool HasAttribute(string attribute)
        {
            return M.Contains(attribute);
        }

        public bool HasObject(string obj)
        {
            return G.Contains(obj);
        }

        public bool HasIncidence(string obj, string attribute)
        {
            if (I.ContainsKey(obj))
            {
                return I[obj].Contains(attribute);
            }
            return false;
        }

        public HashSet<string> GetObjectsWithAttribute(string attribute)
        {
            HashSet<string> objects = new HashSet<string>();
            foreach (var obj in G)
            {
                if (HasIncidence(obj, attribute))
                {
                    objects.Add(obj);
                }
            }
            return objects;
        }

        public HashSet<string> GetAttributesOfObject(string obj)
        {
            if (I.ContainsKey(obj))
            {
                return I[obj];
            }
            return new HashSet<string>();
        }
    }

    public class Concept
    {
        private string _extent;
        private string _intent;

        public string Extent
        {
            get { return _extent; }
            set { _extent = value; }
        }

        public string Intent
        {
            get { return _intent; }
            set { _intent = value; }
        }

        public Concept(string extent, string intent)
        {
            _extent = extent;
            _intent = intent;
        }
    }
    
    internal class Program
    {
        static void Main(string[] args)
        {
            // Создаем формальный контекст
            List<string> g = new List<string>() { "1", "2", "3" };
            List<string> m = new List<string>() { "a", "b", "c", "d" };
            Dictionary<string, HashSet<string>> i = new Dictionary<string, HashSet<string>>();
            i.Add("1", new HashSet<string>() { "a", "c" });
            i.Add("2", new HashSet<string>() { "b", "c" });
            i.Add("3", new HashSet<string>() { "a", "b", "c", "d" });
            FormalContext fc = new FormalContext(g, m, i);

            Dictionary<Concept, List<Concept>> lattice = AddIntent(fc);
        }

        public static Dictionary<Concept, List<Concept>> AddIntent(FormalContext fc)
        {
            Concept bottomConcept = new Concept("", String.Join("", fc.M.ToArray()));

            Dictionary<Concept, List<Concept>> lattice = new Dictionary<Concept, List<Concept>>()
            {
                { bottomConcept, new List<Concept>() }
            };

            foreach (var g in fc.G)
            {
                Concept objectConcept = Add(String.Join("", fc.GetAttributesOfObject(g)), bottomConcept, lattice);

                AddGToExtentAbove(g, objectConcept, lattice);
            }

            lattice.Remove(bottomConcept);

            return lattice;
        }

        public static void AddGToExtentAbove(string g, Concept objectConcept, Dictionary<Concept, List<Concept>> lattice)
        {
            if(!objectConcept.Extent.Contains(g))
                objectConcept.Extent += g;

            var parents = lattice[objectConcept];

            foreach (var parent in parents)
            {
                AddGToExtentAbove(g, parent, lattice);
            }

            lattice.Remove(objectConcept);
            lattice.Add(objectConcept, parents);
        }

        public static Concept GetHighestNodeOfIntent(string intent, Concept generatorConcept, Dictionary<Concept, List<Concept>> lattice)
        {
            bool parentIsHighest = true;

            while (parentIsHighest)
            {
                parentIsHighest = false;
                List<Concept> parents = lattice[generatorConcept];

                if (parents != null)
                {
                    foreach (var parent in parents)
                    {
                        if (parent.Intent.Contains(intent))
                        {
                            generatorConcept = parent;
                            parentIsHighest = true;
                            break;
                        }
                    }
                }
            }

            return generatorConcept;
        }

        public static Concept Add(string intent, Concept generatorConcept, Dictionary<Concept, List<Concept>> lattice)
        {
            generatorConcept = GetHighestNodeOfIntent(intent, generatorConcept, lattice);
            if (generatorConcept.Intent == intent) return generatorConcept;

            List<Concept> generatorParents = lattice[generatorConcept].ToList();
            List<Concept> newParents = new List<Concept>();

            if (generatorParents != null)
            {
                for (int i = 0; i < generatorParents.Count; i++)
                {
                    if (!intent.Contains(generatorParents[i].Intent))
                        generatorParents[i] = Add(new string(generatorParents[i].Intent.Intersect(intent).ToArray()), generatorParents[i], lattice);

                    bool addParent = true;

                    foreach (var parent in newParents)
                    {
                        if (intent.Contains(generatorParents[i].Intent))
                        {
                            addParent = false;
                            break;
                        }
                        else if (generatorParents[i].Intent.Contains(parent.Intent)) 
                            newParents.Remove(parent);
                    }

                    if (addParent) newParents.Add(generatorParents[i]);
                }
            }

            Concept newConcept = new Concept(generatorConcept.Extent, intent);

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