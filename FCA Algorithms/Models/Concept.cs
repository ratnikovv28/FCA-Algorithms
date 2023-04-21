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

        public static bool CheckConcepts(Concept concept1, Concept concept2)
        {
            foreach (string item in concept1.Extent)
            {
                if (!concept2.Extent.Contains(item))
                {
                    return false;
                }
            }

            foreach (string item in concept1.Intent)
            {
                if (!concept2.Intent.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
