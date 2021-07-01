namespace Mafiator.Common.SiteSetting
{

    public class GateWay
    {
        public MellatAccount MellatBank { get; set; }
        public ZarinPalBank ZarinPalBank { get; set; }
    }

    public class MellatAccount
    {
        public long TerminalId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }

    public class ZarinPalBank
    {
        public string MerchantId { get; set; }
        public bool IsSandbox { get; set; }
    }


}