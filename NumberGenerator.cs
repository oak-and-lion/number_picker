using System;
using System.IO;
using System.Collections.Generic;

namespace number_picker
{
    class NumberGenerator
    {
        private int _min;
        private int _max;

        private const string DEFAULT_FILE = "numbers.seed";
        private const string ALREADY_PICKED_FILE = "numbers.picked";

        private List<int> _alreadyPicked;

        private Random _random;

        public NumberGenerator(int min, int max)
        {
            _min = min;
            _max = max;
            _alreadyPicked = new List<int>();
            _random = new Random();
            readPickedNumbers();
        }

        public NumberGenerator(string file)
        {            
            _alreadyPicked = new List<int>();
            _random = new Random();
            readSeedFile(file);
            readPickedNumbers();
        }

        public NumberGenerator()
        {            
            _alreadyPicked = new List<int>();
            _random = new Random();
            readSeedFile(DEFAULT_FILE);
            readPickedNumbers();
        }

        private void checkInitial(string file)
        {
            if (!File.Exists(file))
            {
                Reset(file);
            }
        }

        private void readSeedFile(string file)
        {
            try
            {
                checkInitial(file);
                // Open the text file using a stream reader.
                using (var sr = new StreamReader(file))
                {
                    // Read the stream as a string, and write the string to the console.
                    string[] numbers = sr.ReadToEnd().Split(",");
                    _min = Convert.ToInt32(numbers[0]);
                    _max = Convert.ToInt32(numbers[1]);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        private void readPickedNumbers()
        {
            if (!File.Exists(ALREADY_PICKED_FILE))
            {
                using (var sw = new StreamWriter(ALREADY_PICKED_FILE))
                {
                    sw.Write(String.Empty);
                }
            }

            try
            {
                using (var sr = new StreamReader(ALREADY_PICKED_FILE))
                {
                    string[] numbers = sr.ReadToEnd().Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string number in numbers)
                    {
                        _alreadyPicked.Add(Convert.ToInt32(number));
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        public int PickNumber()
        {
            int result = _random.Next(_min, _max);

            while (_alreadyPicked.Contains(result))
            {
                result = _random.Next(_min, _max);
            }

            _alreadyPicked.Add(result);
            WritePickedNumbers(_alreadyPicked);
            return result;
        }

        public void WritePickedNumbers(List<int> numbers)
        {
            List<string> output = new List<string>();
            foreach (int i in numbers)
            {
                output.Add(i.ToString());
            }

            using (StreamWriter sw = new StreamWriter(ALREADY_PICKED_FILE))
            {
                sw.Write(String.Join(',', output.ToArray()));
            }
        }

        public void Reset()
        {
            Reset(DEFAULT_FILE);
        }

        private void Reset(string file)
        {
            try
            {
                File.Delete(ALREADY_PICKED_FILE);
            }
            catch { }
            try
            {
                File.Delete(DEFAULT_FILE);
            }
            catch { }
            _alreadyPicked.Clear();
            using (StreamWriter sw = new StreamWriter(DEFAULT_FILE))
            {
                sw.Write("1,365");
            }
            readSeedFile(DEFAULT_FILE);
        }

        public override string ToString()
        {
            return string.Format("Min: {0}, Max: {1}", _min.ToString(), _max.ToString());
        }
    }
}