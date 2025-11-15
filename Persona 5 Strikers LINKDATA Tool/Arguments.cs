using System;
using System.Collections.Generic;
using System.Text;

namespace P5SLDT
{
    public class Arguments
    {
        public static string[] Args0 = { "-dat", "-csv", "-linkdata" };

        public static string tutorial =
    "Usage:\n" +
    $"{"-dat [FilePath].dat",-50} .dat -> .csv\n" +
    $"{"-csv [FilePath].csv",-50} .csv -> .dat\n" +
    $"{"-linkdata [FilePath].IDX -dec",-50} Extracts files from LINKDATA.BIN ( -dec = PC )\n" +
    $"{"-linkdata [FilePath].dat [FilePath].idx -enc",-50} Packs files into LINKDATA.BIN ( -enc = PC )";

    }
}
