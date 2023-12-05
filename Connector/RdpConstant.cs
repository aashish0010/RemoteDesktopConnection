namespace RDP
{
    class RdpConstant
    {
        public static readonly string IpaddressPatten= @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";

        public string FilePath { get; set; }

        public static readonly string templatePath= "D:\\Aashish\\Personal\\RemoteDesktopConnection\\Connector\\encryption\\TemplateRDP.txt";
    }
}
