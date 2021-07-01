using System;

namespace number_picker
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isTesting = false;
            
            if (args.Length > 0)
            {
                if (args[0].Equals("test"))
                {
                    isTesting = true;
                }
            }
            if (!isTesting)
            {
                NumberGenerator numberGenerator = new NumberGenerator();
                if (args.Length > 0 && args[0].Equals("reset"))
                {
                    numberGenerator.Reset();
                }

                Console.WriteLine(numberGenerator.PickNumber());
            }
            else{
                unitTests tests = new unitTests();
                tests.Run();
            }
        }
    }
}
