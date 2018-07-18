using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace laxicalAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = System.IO.File.ReadAllText("Program.c");
            CustomLex analyzer = new CustomLex();

            if (text != null)
            {
                string[] lines = Regex.Split(text, "\r\n");
                int lineCount = 0;
                foreach (string line in lines)
                {
                    lineCount++;
                    //string line = lines[i] + "\r\n";
                    Console.WriteLine("Line = {0}", lineCount);
                    Console.WriteLine("Contents: "+line);
                    text = line.Trim(' ', '\t');
                    while (line != null) { 
                        string token = analyzer.GetNextLexicalAtom(ref text);
                        if (token != null)
                        {
                            Console.Write(token+"\n");
                        }
                        else
                        {
                            break;
                        }
                    }
                    Console.WriteLine("\n------------------------------------------------------");
                }
            }

            System.Console.ReadLine();
        }
    }
}
