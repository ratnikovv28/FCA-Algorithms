namespace FCA_Algorithms.Models
{
    public class Concept
    {
        private List<string> _extent;
        private List<string> _intent;
        private int _id;

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

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public Concept(List<string> extent, List<string> intent, int id)
        {
            _extent = extent;
            _intent = intent;
            _id = id;
        }
    }
}
