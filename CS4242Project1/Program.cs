using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CS4242Project1
{
    class Program
    {
        private static Tree tree;

        static void Main(string[] args)
        {
            String input= "";
            String input2 = "";
            String results = "";
            String file = readFile();
            parseXML(file, null);

            while (!input2.Equals("1") && !input2.Equals("2"))
            {
                Console.WriteLine("Do you want to search by depth (1) or breadth (2)? : ");
                input2 = Console.ReadLine();
            }
            
            Console.WriteLine("The goal of the program is to verify the xml file about to be parsed is done correctly.");
            Console.WriteLine("All you have to do is enter a 'behavior' and confirm the correct repsonse as the output.");
            Console.WriteLine("Let's begin. (enter 9 to get a list of 'behaviors' to enter and see 'tree.png' to see valid responses.");

            while (!input.ToLower().Equals("quit"))
            {
                Console.WriteLine("Event ('quit' to exit) : ");
                input = Console.ReadLine();

                if (input.ToLower().Equals("9"))
                {
                    Console.WriteLine("----Valid behaviors----");
                    Console.WriteLine("Incoming Projectile");
                    Console.WriteLine("Combat");
                    Console.WriteLine("Idle");
                    Console.WriteLine("Melee");
                    Console.WriteLine("Ranged");
                    Console.WriteLine();
                }
                else
                {
                    if (input2.Equals('1'))
                    { results = tree.depthSearch(input); }
                    else
                    { results = tree.breadthSearch(input); }

                    if (results != null)
                        Console.WriteLine(results);
                    else
                        Console.WriteLine("Unable to find a response for " + input);
                }
            }
        }

        private static String parseXML(String contents, Node parent)
        {
            if (isClosingRoot(getMatch(contents, "\\<(.*?)\\>")))
                return contents;
            
            if (tree == null)
            {
                // Pass value without <>
                tree = new Tree(new Node(getMatch(contents, "\\<(.*?)\\>").Groups[1].Value));

                // Remove root from contents including <>
                int index = contents.IndexOf(getMatch(contents, "\\<(.*?)\\>").Value);
                contents = contents.Remove(index, getMatch(contents, "\\<(.*?)\\>").Value.Length);
                contents = parseXML(contents, tree.root);
            }
            while (contents.ToLower().Contains("node"))
            {
                // Get tag with and without <>
                String full = getMatch(contents, "\\<(.*?)\\>").Value;
                String sub = getMatch(contents, "\\<(.*?)\\>").Groups[1].Value;

                if (isClosingNode(getMatch(contents, "\\<(.*?)\\>")))
                {
                    // remove closing tag
                    int index = contents.IndexOf(getMatch(contents, "\\<(.*?)\\>").Value);
                    contents = contents.Remove(index, getMatch(contents, "\\<(.*?)\\>").Value.Length);
                    // Go back up tree
                    contents = parseXML(contents, parent.parent);
                }
                else if (full.ToLower().Contains("node") && !full.ToLower().Contains("/"))
                {
                    Node child = new Node(getMatch(sub, "\"(.*?)\"").Groups[1].Value);
                    child.parent = parent;
                    parent.children.Add(child);

                    // Remove root from contents
                    int index = contents.IndexOf(full);
                    contents = contents.Remove(index, full.Length);
                    contents = parseXML(contents, child);
                }
                // node without children
                else
                {
                    Node child = new Node(getMatch(sub, "response=\"(.*?)\"").Groups[1].Value);
                    child.parent = parent;
                    parent.children.Add(child);

                    // Remove root from contents
                    int index = contents.IndexOf(full);
                    contents = contents.Remove(index, full.Length);
                    contents = parseXML(contents, parent);
                }
            }

            return contents;
        }

        /// <summary>
        /// Detemrines if the the </root> tag has been detected
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private static bool isClosingRoot(Match match)
        {
            if (match.Groups[1].Value.ToLower().Contains("root") && match.Groups[1].Value.Contains("/"))
                return true;
            return false;
        }

        private static bool isClosingNode(Match match)
        {
            String text = match.Groups[1].Value.ToLower();

            // if tag is </n it is a closing
            if (text.Contains("node") && text.Contains("/") && text.IndexOf('/') < text.IndexOf('n'))
                return true;
            return false;
        }

        private static Match getMatch(String input, String pattern)
        {
            return Regex.Match(input, pattern);
        }

        /// <summary>
        /// Returns the contents of the xml file
        /// </summary>
        /// <returns></returns>
        private static String readFile()
        {
            StreamReader file = new StreamReader("sample.xml");
            String fileContents = file.ReadToEnd();
            file.Close();

            return fileContents;
        }
    }
}
