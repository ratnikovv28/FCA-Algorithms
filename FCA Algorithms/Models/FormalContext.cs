namespace FCA_Algorithms.Models
{
    public class FormalContext
    {
        private List<string> _g;
        private List<string> _m;
        private Dictionary<string, List<string>> _i;
        //private List<CombinedObject> _combinedObjects;

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

        public FormalContext(List<string> G, List<string> M, List<Data> I)
        {
            var objectWithIntents = I;
            var objNames = G.Select(o => o.ToLower()).ToList();

            _g = new List<string>();
            _m = M;
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

        public FormalContext()
        {
            var rnd = new Random();

            int gCount = rnd.Next(2, 20);
            int mCount = rnd.Next(2, 20);

            var objects = new List<string>();
            _g = objects;
            var attributes = new List<string>();
            _m = attributes;
            var data = new List<Data>();

            _i = new Dictionary<string, List<string>>();

            for (int i = 0; i <= gCount; i++)
            {
                objects.Add((i + 1).ToString());
            }

            for (int i = 0; i <= mCount; i++)
            {
                char letter = (char)('a' + i);
                attributes.Add(letter.ToString());
            }

            for (int i = 0; i <= gCount; i++)
            {
                var rnd1 = new Random();
                int length = rnd1.Next(0, 7); // генерируем длину списка от 1 до 10

                var dependency = new List<int>();
                var prev = rnd1.Next(0, mCount - 1); // генерируем первое случайное число от 0 до mCount(количества атрибутов)
                dependency.Add(prev);
                for (int j = 0; j < length; j++)
                {
                    if (prev + 1 != mCount)
                    {
                        var next = rnd1.Next(prev + 1, mCount);
                        dependency.Add(next);
                        prev = next;
                        if (prev == mCount && i + 1 == length) break;
                    }
                    else
                        break;
                }

                data.Add(new Data()
                {
                    Inds = dependency
                });
            }

            //Добавляем в матрицу инцидентности формальные понятия
            for (int i = 0; i < data.Count; i++)
            {
                _i.Add((i + 1).ToString(), data[i].Inds.Select(intent => M[intent]).ToList());
            }
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
    }
}
