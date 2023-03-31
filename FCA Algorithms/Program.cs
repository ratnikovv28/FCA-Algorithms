using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace FCA_Algorithms
{

    public class Params
    {
        public List<string> AttrNames { get; set; }
    }

    public class ObjNames
    {
    }

    public class RootObject
    {
        public ObjNames ObjNames { get; set; }
        public Params Params { get; set; }
    }

    public class Data
    {
        public List<int> Inds { get; set; }
    }

    public class DataObject
    {
        public List<Data> Data { get; set; }
    }

    public class CombinedObject
    {
        public ObjNames ObjNames { get; set; }
        public Params Params { get; set; }
        public List<Data> Data { get; set; }
    }

    class FormalContext
    {
        private List<string> _g;
        private List<string> _m;
        private Dictionary<string, List<string>> _i;

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

        public Dictionary<string, List<string>> I
        {
            get { return _i; }
            set { _i = value; }
        }

        public FormalContext(List<string> g, List<string> m, Dictionary<string, List<string>> i)
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

        public List<string> GetObjectsWithAttribute(string attribute)
        {
            List<string> objects = new List<string>();
            foreach (var obj in G)
            {
                if (HasIncidence(obj, attribute))
                {
                    objects.Add(obj);
                }
            }
            return objects;
        }

        public List<string> GetAttributesOfObject(string obj)
        {
            if (I.ContainsKey(obj))
            {
                return I[obj];
            }
            return new List<string>();
        }
    }

    public class Concept
    {
        private List<string> _extent;
        private List<string> _intent;

        public List<string> Extent
        {
            get { return _extent; }
            set { _extent = value; }
        }

        public List<string> Intent
        {
            get { return _intent; }
            set { _intent = value; }
        }

        public Concept(List<string> extent, List<string> intent)
        {
            _extent = extent;
            _intent = intent;
        }
    }
    
    internal class Program
    {
        static void Main(string[] args)
        {
            //// Создаем формальный контекст
            //List<string> g = new List<string>() { "1", "2", "3" };
            //List<string> m = new List<string>() { "a", "b", "c", "d" };
            //Dictionary<string, List<string>> i = new Dictionary<string, List<string>>();
            //i.Add("1", new List<string>() { "a", "c" });
            //i.Add("2", new List<string>() { "b", "c" });
            //i.Add("3", new List<string>() { "a", "b", "c", "d" });
            //FormalContext fc = new FormalContext(g, m, i);

            //Dictionary<Concept, List<Concept>> lattice = AddIntent(fc);

            string fileName = "data.json";
            string jsonString = File.ReadAllText(fileName);

            var combinedObjects = JsonConvert.DeserializeObject<List<CombinedObject>>(jsonString);

            //Создаем формальный контекст на основании входного файла
            var objectWithIntents = combinedObjects[1].Data;

            var G = new List<string>();
            var M = combinedObjects[0].Params.AttrNames;
            var I = new Dictionary<string, List<string>>();

            //Добавляем в матрицу инцидентности формальные понятия
            for (int i = 0; i < objectWithIntents.Count(); i++)
            {
                G.Add((i + 1).ToString());
                I.Add((i + 1).ToString(), objectWithIntents[i].Inds.Select(intent => M[intent]).ToList());
            }

            var fc = new FormalContext(G, M, I);

            var addIntent = AddIntent(fc);

            Console.WriteLine();
        }

        public static Dictionary<Concept, List<Concept>> AddIntent(FormalContext fc)
        {
            Concept bottomConcept = new Concept(new List<string>(), fc.M);

            Dictionary<Concept, List<Concept>> lattice = new Dictionary<Concept, List<Concept>>()
            {
                { bottomConcept, new List<Concept>() }
            };

            foreach (var g in fc.G)
            {
                Concept objectConcept = Add(fc.GetAttributesOfObject(g), bottomConcept, lattice);

                AddGToExtentAbove(g, objectConcept, lattice);
            }

            lattice.Remove(bottomConcept);

            return lattice;
        }

        public static void AddGToExtentAbove(string g, Concept objectConcept, Dictionary<Concept, List<Concept>> lattice)
        {
            if(!objectConcept.Extent.Contains(g))
                objectConcept.Extent.Add(g);

            var parents = lattice[objectConcept];

            foreach (var parent in parents)
            {
                AddGToExtentAbove(g, parent, lattice);
            }

            lattice.Remove(objectConcept);
            lattice.Add(objectConcept, parents);
        }

        public static Concept GetHighestNodeOfIntent(List<string> intent, Concept generatorConcept, Dictionary<Concept, List<Concept>> lattice)
        {
            bool parentIsHighest = true;

            while (parentIsHighest)
            {
                parentIsHighest = false;
                List<Concept> parents = lattice[generatorConcept].ToList();

                if (parents != null)
                {
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
            }

            return generatorConcept;
        }

        public static Concept Add(List<string> intent, Concept generatorConcept, Dictionary<Concept, List<Concept>> lattice)
        {
            generatorConcept = GetHighestNodeOfIntent(intent.ToList(), generatorConcept, lattice);
            if (Enumerable.SequenceEqual(generatorConcept.Intent, intent)) return generatorConcept;

            List<Concept> generatorParents = lattice[generatorConcept].ToList();
            List<Concept> newParents = new List<Concept>();

            if (generatorParents != null)
            {
                for (int i = 0; i < generatorParents.Count; i++)
                {
                    if (!generatorParents[i].Intent.All(item => intent.Contains(item)))
                        generatorParents[i] = Add(generatorParents[i].Intent.Intersect(intent).ToList(), generatorParents[i], lattice);

                    bool addParent = true;

                    foreach (var parent in newParents)
                    {
                        if (generatorParents[i].Intent.All(item => intent.Contains(item)))
                        {
                            addParent = false;
                            break;
                        }
                        else if (parent.Intent.All(item => generatorParents[i].Intent.Contains(item))) 
                            newParents.Remove(parent);
                    }

                    if (addParent) newParents.Add(generatorParents[i]);
                }
            }

            Concept newConcept = new Concept(generatorConcept.Extent.ToList(), intent);

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