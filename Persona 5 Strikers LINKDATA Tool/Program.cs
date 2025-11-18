using System;
using System.Collections.Generic;
using System.Text;

namespace P5SLDT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                    Console.WriteLine(Arguments.tutorial);
                    return;
            }
            if (args[0].ToLowerInvariant() == "-linkdata")
            {
                bool isEncrypted = false;
                bool notEverything = true;
                bool engFiles = false;
                if (Path.GetExtension(args[1]).ToLowerInvariant() == ".idx")
                {
                    if (args.Contains("-dec")) isEncrypted = true;
                    if (args.Contains("-everything")) notEverything = false;
                    if (args.Contains("-eng")) engFiles = true;
                    LinkdataParser.ParseLinkdata(args[1], isEncrypted, notEverything, engFiles);
                    return;
                }
                else if (Path.GetExtension(args[1]).ToLowerInvariant() == ".dat")
                {
                    if (Path.GetExtension(args[2]).ToLowerInvariant() == ".idx")
                    {
                        if (args.Contains("-enc")) isEncrypted = true;
                        LinkdataParser.PackLinkdata(args[1], args[2], isEncrypted);
                        return;
                    }

                }
            }
            else if (args[0].ToLowerInvariant() == "-dat")
            {
                if (!Path.Exists(args[1]))
                {
                    Console.WriteLine("File not found: " + args[1]);
                    return;
                }
                using (var datFs = new FileStream(args[1], FileMode.Open))
                using (var datReader = new BinaryReader(datFs))
                {
                    if (datReader.ReadInt32() != 0x16121900)
                    {
                        Console.WriteLine("Not a valid DAT file.");
                        return;
                    }
                    DatParser.ParseDatFile(args[1], datReader);
                    return;
                }
            }
            else if (args[0].ToLowerInvariant() == "-csv")
            {
                if (!Path.Exists(args[1]))
                {
                    Console.WriteLine("File not found: " + args[1]);
                    return;
                }
                CsvParser.ParseCsv(args[1]);
                return;
            }
            else
            {
                Console.WriteLine(Arguments.tutorial);
                return;
            }

        }
    }
}
