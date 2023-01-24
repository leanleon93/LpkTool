using LpkTool.Library;
using LpkTool.Library.LpkData;

namespace LostArkLanguagePatch
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 4) return;
            var lpkTargetPath = args[0];
            var lpkTargetRegion = args[1];
            var lpkSourcePath = args[2];
            var lpkSourceRegion = args[3];
            if (!lpkTargetPath.EndsWith(".lpk") || !File.Exists(lpkTargetPath)) return;
            if (!lpkSourcePath.EndsWith(".lpk") || !File.Exists(lpkSourcePath)) return;
            Region targetRegion = (Region)Enum.Parse(typeof(Region), lpkTargetRegion);
            Region sourceRegion = (Region)Enum.Parse(typeof(Region), lpkSourceRegion);
            var lpkTarget = Lpk.FromFile(lpkTargetPath, targetRegion);
            var lpkSource = Lpk.FromFile(lpkSourcePath, sourceRegion);
            var patcher = new LanguagePatcher(lpkTarget, lpkSource, lpkTargetPath);
            patcher.Patch();

        }
    }
}