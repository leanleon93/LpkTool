namespace LpkTool.Library.LpkData
{
    public enum Region
    {
        EU,
        RU
    }
    public static class RegionHelper
    {
        public static string GetGameMsgString(Region region)
        {
            switch (region)
            {
                case Region.EU:
                    return "GameMsg_English";
                case Region.RU:
                    return "GameMsg";
                default:
                    return "GameMsg";
            }
        }
    }
}
