using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace P5SLDT
{
    public class CsvParser
    {
        public static CsvConfiguration csvConfig = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false
        };
        public static void ParseCsv(string csvFile)
        {
            using (var csvFs = new FileStream(csvFile, FileMode.Open))
            using (var reader = new StreamReader(csvFs))
            using (var csv = new CsvReader(reader, csvConfig))
            using (var datFs = new FileStream(csvFile.Replace(Path.GetExtension(csvFile), ".dat"), FileMode.Create))
            using (var writer = new BinaryWriter(datFs))
            {
                writer.Write((int)0x16121900);
                int lineCount = 0;
                int sectionCount = 0;
                var springField = new List<byte>();
                csv.Read();
                string type1_0 = csv.GetField<string>(1);
                csv.Read();
                string type1_1 = csv.GetField<string>(1);
                lineCount += 2;
                while (csv.Read())
                {
                    lineCount++;
                }
                writer.Write(lineCount);
                csvFs.Seek(0, SeekOrigin.Begin);
                csv.Read();
                if (type1_0 == "0" && type1_1 == "1")
                {
                    sectionCount = csv.Parser.Record.Length * 2 + 2;
                    writer.Write(sectionCount);
                    datFs.Seek(0x40, SeekOrigin.Begin);
                    for (int i = 0; i < lineCount; i++)
                    {
                        for (int j = 0; j < sectionCount / 2 - 1; j++)
                        {
                            if (j == 0)
                            {
                                writer.Write(springField.Count);
                                springField.AddRange(Encoding.UTF8.GetBytes(csv.GetField<string>(j).Replace("[1B]", "\u001b") + "\u0000"));
                                continue;
                            }
                            writer.Write(csv.GetField<short>(j));
                        }
                        csv.Read();
                    }
                    writer.Write(springField.ToArray());
                    Console.WriteLine($"CSV file parsed to {Path.GetFileName(datFs.Name)}");
                    return;
                }
                sectionCount = (csv.Parser.Record.Length + 1) * 4;
                writer.Write(sectionCount);
                datFs.Seek(0x40, SeekOrigin.Begin);
                for (int i = 0; i < lineCount; i++)
                {
                    for (int j = 0; j < sectionCount / 4; j++)
                    {
                        if (j == sectionCount / 4 - 1)
                        {
                            writer.Write(i);
                            break;
                        }
                        writer.Write(springField.Count);
                        springField.AddRange(Encoding.UTF8.GetBytes(csv.GetField<string>(j).Replace("[1B]", "\u001b") + "\u0000"));
                    }
                    csv.Read();
                }
                writer.Write(springField.ToArray());
                Console.WriteLine($"CSV file parsed to {Path.GetFileName(datFs.Name)}");
            }
        }
    }
}
