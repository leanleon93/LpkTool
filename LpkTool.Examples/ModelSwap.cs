using LpkTool.Library;
using LpkTool.Library.DbHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpkTool.Examples
{
    internal class ModelSwap
    {
        private static string _inPath = @"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\data.lpk";
        private static string _outPath = @"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\data.lpk";
        public static void Swaps()
        {
            using (var itemHelper = ItemHelper.FromFile(_inPath))
            {
                //bard alar skin
                //322000632 - 332000632 3rd anniversary bard
                //322000023 - bikini top bard
                //332300578 - white stockings bard
                //332300579 - black stockings bard
                //EFDLItem_PC_MAR_HR_01A_Upper.PC_MAR_HR_01A_Upper - Arcana legendary 2 top
                //332100572 - Arcana legendary 2 bottoms
                itemHelper.ModelSwap(322100006, "EFDLItem_PC_MAR_HR_01A_Upper.PC_MAR_HR_01A_Upper");
                itemHelper.ModelSwap(322130089, "EFDLItem_PC_MAR_HR_01A_Upper.PC_MAR_HR_01A_Upper");
                itemHelper.ModelSwap(332100006, 332100572);
                itemHelper.RepackToFile(_outPath);
            }
        }
        public static void Dumps()
        {
            var lpk = Lpk.FromFile(_inPath);

            using var itemHelper = new ItemHelper(lpk);

            var bardOutfits = itemHelper.GetOutfitsForClass("ForArcana");

            var json = JsonConvert.SerializeObject(bardOutfits, Formatting.Indented);
            File.WriteAllText(@"C:\Users\leanw\Documents\quickbms\eu\out\arcanaOutfits.json", json);
        }
    }
}
