using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NLog;

namespace EssentialTrainingApp
{
    class Program
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public static List<string> Words;

        static void Main(string[] args)
        {
            logger.Trace("The program started.");
            Words = new List<string>();
            Words.Add("Bread");
            Words.Add("Hot sauce");
            Words.Add("Yeet");
            //CrazyMathProblem();
            ReadTextFile();
            Console.ReadLine();

        }

        private static void ReadTextFile()
        {
            try
            {
                using (var sr = new StreamReader(@"C:\temp\test.txt"))
                {
                    string contents = sr.ReadToEnd();
                    Console.WriteLine(contents);
                }
            }
            catch(System.IO.FileNotFoundException ex)
            {
                Console.WriteLine("Could not find the file in the specified directory.");
                logger.Error("Could not find the file: " + ex.Message);
            }
            catch(System.IO.DirectoryNotFoundException ex)
            {
                Console.WriteLine("Couldn't find the directory");
                logger.Error("Could not find the directory: " + ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unknown error: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Yah yah yah yeet");
            }
        }

        private static int CrazyMathProblem()
        {
            var income = 1000;
            for(var i = 10; i > 0; i--)
            {
                income = income = (income / i);
            }
            return income;
        }
    }
}
