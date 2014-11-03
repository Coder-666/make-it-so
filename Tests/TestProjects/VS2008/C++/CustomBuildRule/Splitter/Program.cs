﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Splitter
{
    class Program
    {
        /// <summary>
        /// We take a file, arg[0], and an output folder, arg[1].
        /// We split the file into a header and .cpp and write them
        /// to the output folder.
        /// </summary>
        static void Main(string[] args)
        {
            StreamWriter headerFile = null;
            StreamWriter cppFile = null;
            try
            {
                string file = args[0];
                string outputFolder = args[1];

                string outputHeaderFile = outputFolder + "/" + Path.GetFileNameWithoutExtension(file) + ".h";
                string outputCPPFile = outputFolder + "/" + Path.GetFileNameWithoutExtension(file) + ".cpp";
                headerFile = new StreamWriter(outputHeaderFile, false);
                cppFile = new StreamWriter(outputCPPFile, false);

                bool inHeaderSection = false;
                bool inCPPSection = false;

                string[] lines = File.ReadAllLines(file);
                foreach (string line in lines)
                {
                    if (line.Contains("##START-HEADER##") == true)
                    {
                        inHeaderSection = true;
                        inCPPSection = false;
                        headerFile.WriteLine("// GENERATED BY SPLITTER...");
                        continue;
                    }
                    if (line.Contains("##END-HEADER##") == true)
                    {
                        inHeaderSection = false;
                        continue;
                    }
                    if (line.Contains("##START-CPP##") == true)
                    {
                        inCPPSection = true;
                        inHeaderSection = false;
                        cppFile.WriteLine("// GENERATED BY SPLITTER...");
                        continue;
                    }
                    if (line.Contains("##END-CPP##") == true)
                    {
                        inCPPSection = false;
                        continue;
                    }

                    if (inHeaderSection == true)
                    {
                        headerFile.WriteLine(line);
                    }
                    if (inCPPSection == true)
                    {
                        cppFile.WriteLine(line);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (headerFile != null) headerFile.Close();
                if (cppFile != null) cppFile.Close();
            }
        }
    }
}
