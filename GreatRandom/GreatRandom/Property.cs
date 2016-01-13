using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GreatRandom.Annotations;

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
