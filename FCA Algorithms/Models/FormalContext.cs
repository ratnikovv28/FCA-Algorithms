namespace FCA_Algorithms.Models
{
    public class FormalContext
    {
        private List<string> _g;
        private List<string> _m;
        private Dictionary<string, List<string>> _i;
        private List<CombinedObject> _combinedObjects;

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

        public FormalContext(List<CombinedObject> combinedObjects)
        {
            _combinedObjects = combinedObjects;
            CreateFormalConcept();
        }

        public bool HasAttribute(string attribute) => M.Contains(attribute);

        public bool HasObject(string obj) => G.Contains(obj);

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

        private void CreateFormalConcept()
        {
            var objectWithIntents = _combinedObjects[1].Data;
            var objNames = _combinedObjects[0].ObjNames.Select(o => o.ToLower()).ToList();

            _g = new List<string>();
            _m = _combinedObjects[0].Params.AttrNames;
            _i = new Dictionary<string, List<string>>();

            if (objNames == null || (objNames != null && objNames.Count() != objectWithIntents.Count()))
            {
                //Добавляем в матрицу инцидентности формальные понятия
                for (int i = 0; i < objectWithIntents.Count(); i++)
                {
                    _g.Add((i + 1).ToString());
                    _i.Add((i + 1).ToString(), objectWithIntents[i].Inds.Select(intent => M[intent]).ToList());
                }
            }
            else
            {
                _g = objNames;
                //Добавляем в матрицу инцидентности формальные понятия
                for (int i = 0; i < objectWithIntents.Count(); i++)
                {
                    _i.Add(G[i], objectWithIntents[i].Inds.Select(intent => M[intent]).ToList());
                }
            }
        }
    }
}
