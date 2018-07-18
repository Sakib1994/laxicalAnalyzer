using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laxicalAnalyzer
{
    class CustomLex
    {
        private string[] keywords = {"include","int", "main", "printf",
        "scanf", "stdio", "return "};
        private string[] specialCharacter = { "#", ".", ",", "<", ">","(",")","\"", "{", "}" };

        private string[] operators = {"+","-","*","/","%", "=" };
        string[] Delimiter = { ";" };

        public string Parse(string item)
        {

            StringBuilder str = new StringBuilder();
            int ok;
            if (Int32.TryParse(item, out ok))
            {

                str.Append("(numerical constant, " + item + ") ");
                return str.ToString();

            }

            if (item.Equals("\r\n"))
            {
                Console.WriteLine("\n------------------------------------------------------");
                return "\r\n";
            }

            if (CheckKeyword(item) == true)
            {
                str.Append(item + "    --------> keyword " );
                return str.ToString();
            }

            if (CheckSpecialCharacter(item) == true)
            {
                str.Append(item + "   --------> Special Character");
                return str.ToString();
            }
            if (CheckOperator(item) == true)
            {
                str.Append(item + "   --------> operator");
                return str.ToString();
            }
            if (CheckDelimiter(item) == true)
            {
                str.Append(item + "   --------> Delimiter");
                return str.ToString();
            }

            str.Append(item + "   --------> identifier"); 
            return str.ToString();



        }
        private bool CheckOperator(string str)
        {
            return operators.Contains(str);
        }
        private bool CheckSpecialCharacter(string str)
        {
            return specialCharacter.Contains(str);
        }

        private bool CheckDelimiter(string str)
        {
            return Delimiter.Contains(str);
        }
        private bool CheckKeyword(string str)
        {
            return keywords.Contains(str); 
        }
        public string GetNextLexicalAtom(ref string item)
        {
            StringBuilder token = new StringBuilder();
            for (int i = 0; i < item.Length; i++)
            {
                if (CheckDelimiter(item[i].ToString()))
                {
                    if (i + 1 < item.Length && CheckDelimiter(item.Substring(i, 2)))
                    {
                        token.Append(item.Substring(i, 2));
                        item = item.Remove(i, 2);
                        return Parse(token.ToString());
                    }
                    else
                    {
                        token.Append(item[i]);
                        item = item.Remove(i, 1);
                        return Parse(token.ToString());
                    }
                }
                else if (item[i] == ' ')
                {
                    item = item.Remove(i, 1);
                    return "Space Removed";
                }
                else if (CheckSpecialCharacter(item[i].ToString()))
                {
                    token.Append(item[i]);
                    item = item.Remove(i, 1);
                    return Parse(token.ToString());
                }
                else if (CheckOperator(item[i].ToString()))
                {
                    if (i + 1 < item.Length && (CheckOperator(item.Substring(i, 2))))
                    {
                        if (i + 2 < item.Length && CheckOperator(item.Substring(i, 3)))
                        {
                            token.Append(item.Substring(i, 3));
                            item = item.Remove(i, 3);
                            return Parse(token.ToString());
                        }
                        else
                        {
                            token.Append(item.Substring(i, 2));
                            item = item.Remove(i, 2);
                            return Parse(token.ToString());
                        }
                    }
                    else
                    {
                        int ok;
                        if (item[i] == '-' && Int32.TryParse(item[i + 1].ToString(), out ok))
                            continue;
                        token.Append(item[i]);
                        item = item.Remove(i, 1);
                        return Parse(token.ToString());
                    }

                }
                else
                {
                    if (item[i + 1].ToString().Equals(" ") || CheckDelimiter(item[i + 1].ToString()) == true ||
                        CheckOperator(item[i + 1].ToString()) == true || CheckSpecialCharacter(item[i + 1].ToString())==true)
                    {
                        int j = i + 2;
                        if (Parse(item.Substring(0, i + 1)).Contains("numerical constant") && item[i + 1] == '.')
                        {

                            while (item[j].ToString().Equals(" ") == false && CheckDelimiter(item[j].ToString()) == false && CheckOperator(item[j].ToString()) == false)
                                j++;
                            int ok;
                            if (Int32.TryParse(item.Substring(i + 2, j - i - 2), out ok))
                            {
                                token.Append("(numerical constant, ").Append(item.Substring(0, j)).Append(") ");
                                item = item.Remove(0, j);
                                return token.ToString();
                            }

                        }
                        token.Append(item.Substring(0, i + 1));
                        item = item.Remove(0, i + 1);
                        return Parse(token.ToString());
                    }
                }

            }
            return null;
        }
    }
}
