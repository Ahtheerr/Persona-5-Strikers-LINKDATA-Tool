using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace P5SLDT
{
    public class DatParser
    {
        public static CsvConfiguration csvConfig = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false
        };
        public static void ParseDatFile(string filePath, BinaryReader reader)
        {
            string csvName = Path.GetFullPath(filePath).Replace(Path.GetExtension(filePath), ".csv");
            using (var fs = new FileStream(csvName, FileMode.Create))
            using (var writer = new StreamWriter(fs))
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                int lineNum = reader.ReadInt32();
                int sectionNum = reader.ReadInt32();
                int textSectionStart = lineNum * sectionNum + 0x40;
                reader.BaseStream.Seek(0x44, SeekOrigin.Begin);
                if (sectionNum > 8 && reader.ReadInt16() == 0)
                {
                    reader.BaseStream.Seek(0x40, SeekOrigin.Begin);
                    for (int i = 0; i < lineNum; i++)
                    {
                        for (int j = 0; j < sectionNum; j += 2)
                        {
                            if (j == 0)
                            {
                                int pointer = reader.ReadInt32();
                                long currentPos = reader.BaseStream.Position;
                                reader.BaseStream.Seek(pointer + textSectionStart, SeekOrigin.Begin);
                                var bytes = new List<byte>();
                                while (true)
                                {
                                    byte b = reader.ReadByte();
                                    if (b == 0x00)
                                    {
                                        csv.WriteField(Encoding.UTF8.GetString(bytes.ToArray()).Replace("\u001b", "[1B]"));
                                        break;
                                    }
                                    bytes.Add(b);
                                }
                                reader.BaseStream.Seek(currentPos, SeekOrigin.Begin);
                                j += 2;
                            }
                            else csv.WriteField(reader.ReadInt16());
                        }
                        csv.NextRecord();
                    }
                    Console.WriteLine($"DAT file parsed to {Path.GetFileName(csvName)}");
                    return;
                }
                reader.BaseStream.Seek(0x40, SeekOrigin.Begin);
                for (int i = 0; i < lineNum; i++)
                {
                    for (int j = 0; j < sectionNum; j += 4)
                    {
                        if (j == sectionNum - 4)
                        {
                            reader.ReadInt32();
                            break;
                        }
                        int pointer = reader.ReadInt32();
                        long currentPos = reader.BaseStream.Position;
                        reader.BaseStream.Seek(pointer + textSectionStart, SeekOrigin.Begin);
                        var bytes = new List<byte>();
                        while (true)
                        {
                            byte b = reader.ReadByte();
                            if (b == 0x00)
                            {
                                csv.WriteField(Encoding.UTF8.GetString(bytes.ToArray()).Replace("\u001b", "[1B]"));
                                break;
                            }
                            bytes.Add(b);
                        }
                        reader.BaseStream.Seek(currentPos, SeekOrigin.Begin);
                    }
                    csv.NextRecord();
                }
                Console.WriteLine($"DAT file parsed to {Path.GetFileName(csvName)}");

            }
        }
    }
}
