﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace RDP
{
    class RdpHandler
    {
        public static void Rrocess(LogInfo info) {
            if (string.IsNullOrEmpty(info.Username) || string.IsNullOrEmpty(info.Password)) {
                throw new ArgumentNullException("username and password can't be empty");
            }
            RdpConstant constant = new RdpConstant();
            var pwstr = BitConverter.ToString(DataProtection.ProtectData(Encoding.Unicode.GetBytes(info.Password), "")).Replace("-", "");
            var rdpInfo = String.Format(File.ReadAllText(RdpConstant.templatePath), info.Ipaddress, info.Username, pwstr);
            constant.FilePath =info.Name +".rdp";
            File.WriteAllText(constant.FilePath,rdpInfo);
            _mstsc("mstsc "+ constant.FilePath);
        }

        private static void _mstsc(String cmd)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(cmd);
        }
    }
}
