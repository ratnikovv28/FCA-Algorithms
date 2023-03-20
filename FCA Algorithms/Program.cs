using System;
using System.Collections.Generic;
using System.Linq;

namespace FCA_Algorithms
{
    //class FormalContext
    //{
    //    private List<string> G;
    //    private List<string> M;
    //    private Dictionary<string, HashSet<string>> I;

    //    public List<string> MM
    //    {
    //        get { return M; }
    //        set { M = value; }
    //    }

    //    public List<string> GG
    //    {
    //        get { return G; }
    //        set { G = value; }
    //    }

    //    public FormalContext(List<string> g, List<string> m, Dictionary<string, HashSet<string>> i)
    //    {
    //        G = g;
    //        M = m;
    //        I = i;
    //    }

    //    public bool HasAttribute(string attribute)
    //    {
    //        return M.Contains(attribute);
    //    }

    //    public bool HasObject(string obj)
    //    {
    //        return G.Contains(obj);
    //    }

    //    public bool HasIncidence(string obj, string attribute)
    //    {
    //        if (I.ContainsKey(obj))
    //        {
    //            return I[obj].Contains(attribute);
    //        }
    //        return false;
    //    }

    //    public HashSet<string> GetObjectsWithAttribute(string attribute)
    //    {
    //        HashSet<string> objects = new HashSet<string>();
    //        foreach (var obj in G)
    //        {
    //            if (HasIncidence(obj, attribute))
    //            {
    //                objects.Add(obj);
    //            }
    //        }
    //        return objects;
    //    }

    //    public HashSet<string> GetAttributesOfObject(string obj)
    //    {
    //        if (I.ContainsKey(obj))
    //        {
    //            return I[obj];
    //        }
    //        return new HashSet<string>();
    //    }
    //}

    public class FormalContext
    {
        public List<string> Objects { get; set; }
        public List<string> Attributes { get; set; }
        public Dictionary<string, HashSet<string>> Context { get; set; }

        public FormalContext(List<string> objects, List<string> attributes, Dictionary<string, HashSet<string>> context)
        {
            Objects = objects;
            Attributes = attributes;
            Context = context;
        }

        public HashSet<string> GetExtent(string attribute)
        {
            if (!Attributes.Contains(attribute))
                throw new ArgumentException("Attribute not found in the formal context.");

            var extent = new HashSet<string>();
            foreach (var obj in Objects)
            {
                if (Context[obj].Contains(attribute))
                    extent.Add(obj);
            }

            return extent;
        }

        public HashSet<string> GetIntent(string obj)
        {
            if (!Objects.Contains(obj))
                throw new ArgumentException("Object not found in the formal context.");

            var intent = new HashSet<string>();
            foreach (var attr in Attributes)
            {
                if (Context[obj].Contains(attr))
                    intent.Add(attr);
            }

            return intent;
        }

        public bool IsConcept(string obj, string attr)
        {
            return Context[obj].Contains(attr);
        }

        public Lattice<string> GenerateLattice()
        {
            var lattice = new Lattice<string>();

            var extentDict = new Dictionary<HashSet<string>, string>();
            foreach (var attr in Attributes)
            {
                var extent = GetExtent(attr);
                if (!extentDict.ContainsKey(extent))
                    extentDict[extent] = attr;
            }

            foreach (var obj in Objects)
            {
                var intent = GetIntent(obj);
                var extent = extentDict[intent];

                var node = new Node<string>(extent);
                lattice.AddNode(node);

                if (node.Parents.Count == 0)
                    lattice.RootNodes.Add(node);

                foreach (var parent in node.Parents)
                    parent.Children.Add(node);
            }

            return lattice;
        }
    }

    public class Node<T>
    {
        public T Value { get; set; }
        public List<Node<T>> Parents { get; set; }
        public List<Node<T>> Children { get; set; }

        public Node(T value)
        {
            Value = value;
            Parents = new List<Node<T>>();
            Children = new List<Node<T>>();
        }
    }

    public class Lattice<T>
    {
        public List<Node<T>> RootNodes { get; set; }
        public List<Node<T>> Nodes { get; set; }

        public Lattice()
        {
            RootNodes = new List<Node<T>>();
            Nodes = new List<Node<T>>();
        }

        public void AddNode(Node<T> node)
        {
            Nodes.Add(node);
        }
    }


    class Concept
    {
        private string _extent;
        private string _intent;

        public Concept(string extent, string intent)
        {
            _extent = extent;
            _intent = intent;
        }

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
    }
    
    internal class Program
    {
        static void Main(string[] args)
        {
            //// Создаем формальный контекст
            //List<string> g = new List<string>() { "a", "b", "c" };
            //List<string> m = new List<string>() { "x", "y", "z" };
            //Dictionary<string, HashSet<string>> i = new Dictionary<string, HashSet<string>>();
            //i.Add("a", new HashSet<string>() { "x", "y" });
            //i.Add("b", new HashSet<string>() { "x", "z" });
            //i.Add("c", new HashSet<string>() { "y", "z" });
            //FormalContext fc = new FormalContext(g, m, i);

            //// Проверяем, содержится ли атрибут "x" в контексте
            //bool hasAttribute = fc.HasAttribute("x");
            //Console.WriteLine(hasAttribute); // True

            //// Проверяем, содержится ли объект "d" в контексте
            //bool hasObject = fc.HasObject("d");
            //Console.WriteLine(hasObject); // False

            //// Проверяем, содержится ли отношение (a, y) в контексте
            //bool hasIncidence = fc.HasIncidence("a", "y");
            //Console.WriteLine(hasIncidence); // True

            //// Получаем объекты, которые имеют атрибут "x"
            //HashSet<string> objectsWithAttribute = fc.GetObjectsWithAttribute("x");
            //foreach (var obj in objectsWithAttribute)
            //{
            //    Console.WriteLine(obj);
            //}
            //// Результат:
            //// a
            //// b

            //// Получаем атрибуты объекта "a"
            //HashSet<string> attributesOfObject = fc.GetAttributesOfObject("a");
            //foreach (var attribute in attributesOfObject)
            //{
            //    Console.WriteLine(attribute);
            //}
            //// Результат:
            //// x
            //// y

            //Console.WriteLine();

            var objects = new List<string> { "a", "b", "c", "d", "e" };
            var attributes = new List<string> { "1", "2", "3", "4", "5" };
            var context = new Dictionary<string, HashSet<string>>
            {
                { "a", new HashSet<string> { "1", "2", "3" } },
                { "b", new HashSet<string> { "2", "3", "4" } },
                { "c", new HashSet<string> { "3", "4", "5" } },
                { "d", new HashSet<string> { "4", "5" } },
                { "e", new HashSet<string> { "5" } }
            };

            var formalContext = new FormalContext(objects, attributes, context);
            var lattice = formalContext.GenerateLattice();
        }

        //public void AddIntent(FormalContext fc)
        //{
        //    Concept bottomConcept = new Concept("", String.Join("", fc.MM.ToArray()));

        //    foreach(var g in fc.GG)
        //    {
        //        Concept objectConcept = Add(fc.GetObjectsWithAttribute(g).ToString(), bottomConcept);

        //    }
        //}

        //public Concept GetHighestNodeOfIntent(string intent, Concept generatorConcept)
        //{
        //    bool parentIsHighest = true;

        //    while (parentIsHighest)
        //    {
        //        parentIsHighest = false;
        //        List<Concept> parents = GetParents();
        //        foreach(var parent in parents)
        //        {
        //            if (parent.Intent.Contains(intent))
        //            {
        //                generatorConcept = parent;
        //                parentIsHighest = true;
        //                break;
        //            }
        //        }
        //    }

        //    return generatorConcept;
        //}

        //public List<Concept> GetParents(Concept generatorConcept)
        //{
        //}

        //public Concept Add(string intent, Concept generatorConcept)
        //{
        //    generatorConcept = GetHighestNodeOfIntent(intent, generatorConcept);
        //    if (generatorConcept.Intent == intent) return generatorConcept;

        //    List<Concept> generatorParents = GetParents(generatorConcept);


        //}
    }
}