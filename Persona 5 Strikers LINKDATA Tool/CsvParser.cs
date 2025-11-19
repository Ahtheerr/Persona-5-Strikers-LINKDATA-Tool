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
                var allRecords = new List<string[]>();
                var springField = new List<byte>();
                while (csv.Read())
                {
                    allRecords.Add(csv.Parser.Record);
                }
                lineCount = allRecords.Count;
                writer.Write(lineCount);
                if (allRecords[0].Length > 1 && allRecords[0][1] == "0" && allRecords[1][1] == "1")
                {
                    sectionCount = allRecords[0].Length * 2 + 2;
                    writer.Write(sectionCount);
                    datFs.Seek(0x40, SeekOrigin.Begin);
                    for (int i = 0; i < lineCount; i++)
                    {
                        for (int j = 0; j < allRecords[0].Length; j++)
                        {
                            if (j == 0)
                            {
                                writer.Write(springField.Count);
                                springField.AddRange(Encoding.UTF8.GetBytes(allRecords[i][j].Replace("[1B]", "\u001b") + "\u0000"));
                            }
                            else writer.Write(Convert.ToInt16(allRecords[i][j]));
                        }
                    }
                    writer.Write(springField.ToArray());
                    Console.WriteLine($"CSV file packed to {Path.GetFileName(datFs.Name)}");
                    return;
                }
                sectionCount = allRecords[0].Length * 4 + 4;
                writer.Write(sectionCount);
                datFs.Seek(0x40, SeekOrigin.Begin);
                for (int i = 0; i < lineCount; i++)
                {
                    for (int j = 0; j < allRecords[0].Length + 1; j++)
                    {
                        if (j == allRecords[0].Length)
                        {
                            writer.Write(i);
                            break;
                        }
                        writer.Write(springField.Count);
                        springField.AddRange(Encoding.UTF8.GetBytes(allRecords[i][j].Replace("[1B]", "\u001b") + "\u0000"));
                    }
                }
                writer.Write(springField.ToArray());
                Console.WriteLine($"CSV file packed to {Path.GetFileName(datFs.Name)}");
            }
        }
    }
}
