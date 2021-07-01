using System;
using System.Collections.Generic;
using System.IO;

namespace number_picker
{
    class unitTests
    {
        private const string DEFAULT_FILE = "numbers.seed";
        NumberGenerator numberGenerator;
        List<TestResult> results;

        public void Run()
        {
            results = new List<TestResult>();
            int failedTests = 0;

            createNumberGenerator();
            writePickedNumbers();
            PickNumber();

            foreach (TestResult result in results)
            {
                if (!result.DidPass)
                {
                    Console.WriteLine(string.Format("Test: {0} failed", result.Name));
                    failedTests++;
                }
            }

            Console.WriteLine(string.Format("Total tests: {0}", results.Count));
            Console.WriteLine(string.Format("Failed Tests: {0}", failedTests));
        }

        private void initialize(string file)
        {
            numberGenerator = new NumberGenerator(file);
        }

        private void createNumberGenerator()
        {
            TestResult testResult = new TestResult("createNumberGenerator");

            const string EXPECTED = "Min: 1, Max: 365";
            initialize(DEFAULT_FILE);
            string result = numberGenerator.ToString();
            testResult.DidPass = EXPECTED == result;
            results.Add(testResult);
        }

        private void writePickedNumbers()
        {
            List<int> pickedNumbers = new List<int>();
            pickedNumbers.Add(1);
            pickedNumbers.Add(2);
            TestResult testResult = new TestResult("writePickedNumbers");

            initialize(DEFAULT_FILE);
            numberGenerator.WritePickedNumbers(pickedNumbers);

            using (StreamReader sr = new StreamReader("numbers.picked"))
            {
                String output = sr.ReadToEnd();
                testResult.DidPass = output.Equals("1,2");
            }

            results.Add(testResult);
        }

        private void PickNumber()
        {
            TestResult testResult = new TestResult("writePickedNumbers");

            initialize(DEFAULT_FILE);
            int number = numberGenerator.PickNumber();

            using (StreamReader sr = new StreamReader("numbers.picked"))
            {
                String output = sr.ReadToEnd();
                testResult.DidPass = output.Contains(number.ToString());
            }

            results.Add(testResult);
        }
    }

    class TestResult
    {
        public TestResult(string name)
        {
            DidPass = false;
            Name = name;
        }
        public string Name;
        public bool DidPass;
    }
}