using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WpfApplication1;

namespace GreatRandom
{
    public class KenoLoader : INumbersGenerator
    {
        private int _amount = 20;
        private string _path = "..\\..\\..\\..\\..\\WpfApplication2\\WpfApplication1\\test.xml";
        private int _currentIndex = 0;
        public int Maxnumber { get { return 64; } }
        public bool HaveNext { get { return CurrentIndex < results.Count - 1; } }

        public bool isLast
        {
            get { return CurrentIndex == results.Count - 2; }
        }

        public ObservableCollection<Result> results { get; set; }

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set { _currentIndex = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public void Load()
        {
            StreamReader reader = new StreamReader(Path);
            results = Deserialize<ObservableCollection<Result>>(reader.ReadToEnd());
            reader.Close();
        }

        public static T Deserialize<T>(string xml)
        {
            var xs = new XmlSerializer(typeof(T));
            return (T)xs.Deserialize(new StringReader(xml));
        }

        public HashSet<int> Generate()
        {
            var numbers = new HashSet<int>();
            var result = results[CurrentIndex++];
            for (int i = 0; i < result.Results.Length; i++)
            {
                numbers.Add(result.Results[i]);
            }
            if (CurrentIndex == results.Count-1)
            {
                CurrentIndex = 0;
            }
            return numbers;

        }
    }

}
