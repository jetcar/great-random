namespace GreatRandom
{
    public class Property 
    {
        private int _intValue;
        private bool _boolValue;
        private string _strValue;

        public int IntValue
        {
            get { return _intValue; }
            set
            {
                if (value == _intValue) return;
                _intValue = value;
            }
        }

        public bool BoolValue
        {
            get { return _boolValue; }
            set
            {
                if (value == _boolValue) return;
                _boolValue = value;
            }
        }

        public string StrValue
        {
            get { return _strValue; }
            set
            {
                if (value == _strValue) return;
                _strValue = value;
            }
        }
       
    }
}
