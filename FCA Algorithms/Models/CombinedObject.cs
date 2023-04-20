namespace FCA_Algorithms.Models
{
    public class CombinedObject
    {
        public List<string> ObjNames { get; set; }
        public Params Params { get; set; }
        public List<Data> Data { get; set; }
    }

    public class Params
    {
        public List<string> AttrNames { get; set; }
    }

    public class ObjNames
    {
        public List<string> Names { get; set; }
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
}
