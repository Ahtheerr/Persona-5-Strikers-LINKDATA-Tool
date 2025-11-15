using System;
using System.Collections.Generic;
using System.Text;
using Cethleann.Compression.Scramble;

namespace P5SLDT
{
    public class LinkdataParser
    {
        public static long startOffset;
        public static long outSize;
        public static long unknown;

        public static void ParseLinkdata(string idxName, bool isEncrypted)
        {
            var extracted = new Dictionary<(long startOffset, long outSize), string>();
            string binName = idxName.ToLowerInvariant().Replace(".idx", ".bin");
            if (!Path.Exists(binName))
            {
                Console.WriteLine("BIN file is missing.");
                return;
            }
            using (var idxFs = new FileStream(idxName, FileMode.Open))
            using (var binFs = new FileStream(binName, FileMode.Open))
            using (var idxReader = new BinaryReader(idxFs))
            using (var binReader = new BinaryReader(binFs))
            {
                for (int i = 0; idxReader.BaseStream.Position < idxReader.BaseStream.Length; i++)
                {
                    startOffset = idxReader.ReadInt64();
                    outSize = idxReader.ReadInt64();
                    idxReader.ReadInt64(); // outSize repeated
                    unknown = idxReader.ReadInt64(); // I don't know lmao
                    var key = (startOffset, outSize);
                    if (extracted.ContainsKey(key))
                    {
                        Console.WriteLine($"Duplicate entry found at index {i}, skipping extraction.");
                        continue;
                    }
                    extracted[key] = $"Output\\{i}.dat";
                    binReader.BaseStream.Seek(startOffset, SeekOrigin.Begin);
                    byte[] bytes = binReader.ReadBytes((int)outSize);
                    if (isEncrypted)
                    {
                        LinkEncryption.Decrypt(bytes, (uint)i);
                    }
                    if (!Directory.Exists("Output")) Directory.CreateDirectory("Output");
                    File.WriteAllBytes($"Output\\{i}.dat", bytes.ToArray());

                }
                Console.WriteLine($"Extracted {extracted.Count} files to the Output folder.");
            }
        }
        public static void PackLinkdata(string datName, string idxName, bool isEncrypted)
        {
            string binName = idxName.ToLowerInvariant().Replace(".idx", ".bin");
            if (!Path.Exists(binName))
            {
                Console.WriteLine("BIN file is missing.");
                return;
            }
            using (var idxFs = new FileStream(idxName, FileMode.Open))
            using (var binFs = new FileStream(binName, FileMode.Open))
            using (var datFs = new FileStream(datName, FileMode.Open))
            using (var idxWriter = new BinaryWriter(idxFs))
            using (var idxReader = new BinaryReader(idxFs))
            using (var binReader = new BinaryReader(binFs))
            using (var datReader = new BinaryReader(datFs))
            {
                long ID = Convert.ToInt64(Path.GetFileNameWithoutExtension(datName));
                for (int i = 0; idxWriter.BaseStream.Position < idxWriter.BaseStream.Length; i++)
                {
                    startOffset = idxReader.ReadInt64();
                    outSize = idxReader.ReadInt64();
                    idxReader.ReadInt64(); // outSize repeated
                    unknown = idxReader.ReadInt64(); // I don't know lmao
                    if (i == ID)
                    {
                        // IDX LOGIC
                        idxFs.Seek(-24, SeekOrigin.Current);
                        long sizeDifference = outSize - datFs.Length;
                        idxWriter.Write(datFs.Length);
                        idxWriter.Write(datFs.Length);
                        idxReader.ReadInt64(); // unknown remains unchanged
                        // IDX LOGIC

                        // BIN LOGIC
                        var binBytes = new List<byte>();
                        binBytes.AddRange(binReader.ReadBytes((int)startOffset));
                        binFs.Seek(outSize, SeekOrigin.Current);
                        byte[] datBytes = datReader.ReadBytes((int)datFs.Length);
                        if (isEncrypted) LinkEncryption.Encrypt(datBytes, (uint)i);
                        binBytes.AddRange(datBytes);
                        binBytes.AddRange(binReader.ReadBytes((int)(binFs.Length - binFs.Position)));
                        binFs.SetLength(0);
                        binFs.Write(binBytes.ToArray(), 0, binBytes.Count);
                        // BIN LOGIC

                        while (idxWriter.BaseStream.Position < idxWriter.BaseStream.Length)
                        {
                            // IDX LOGIC
                            startOffset = idxReader.ReadInt64();
                            idxFs.Seek(-8, SeekOrigin.Current);
                            idxWriter.Write(startOffset - sizeDifference);
                            idxFs.Seek(24, SeekOrigin.Current);
                            // IDX LOGIC
                        }
                        Console.WriteLine($"Packed {datName} into {binName} and updated {idxName}.");
                    }

                }
            }
        }

    }
}
