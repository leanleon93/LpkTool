using LpkTool.Library;
using LpkTool.Library.DbHelpers;
using Newtonsoft.Json;

namespace LpkTool.Examples
{
    internal class ModelSwap
    {
        private static readonly string _inPath = @"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\data2.lpk";
        private static readonly string _outPath = @"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\data2.lpk";
        public static void Swaps()
        {
            using (var itemHelper = ItemHelper.FromFile(_inPath))
            {
                //bard alar skin 322090130 - 332090130
                //322000632 - 332000632 3rd anniversary bard
                //322000023 - bikini top bard 322000418
                //332300578 - white stockings bard
                //332300579 - black stockings bard
                //EFDLItem_PC_MAR_HR_01A_Upper.PC_MAR_HR_01A_Upper - Arcana legendary 2 top
                //332100572 - Arcana legendary 2 bottoms
                //itemHelper.ModelSwap(322090130, 322000417);
                itemHelper.ModelSwap(332090130, 332300579);
                itemHelper.RepackToFile(_outPath);
            }
        }
        public static void Dumps()
        {
            var lpk = Lpk.FromFile(_inPath);

            using var itemHelper = new ItemHelper(lpk);

            var bardOutfits = itemHelper.GetOutfitsForClass("ForReaper");

            var json = JsonConvert.SerializeObject(bardOutfits, Formatting.Indented);
            File.WriteAllText(@"C:\Users\leanw\Documents\quickbms\eu\out\reaperOutfits.json", json);
        }
    }
}
