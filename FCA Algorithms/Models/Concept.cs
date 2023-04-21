namespace FCA_Algorithms.Models
{
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
}
