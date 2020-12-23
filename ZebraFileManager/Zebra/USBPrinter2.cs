using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Sockets;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RJCP.IO.Ports;


namespace ZebraFileManager.Zebra
{
    public class USBPrinter : Printer
    {
        public string PrinterName { get; set; }

        public override bool Connected => true;


        public override bool Connect()
        {
            return Connected;
        }

        public override string RunCommand(string command, bool response = true)
        {
            return Encoding.UTF8.GetString(RunCommand(Encoding.UTF8.GetBytes(command), response));
        }

        object commandLock = new object();

        public override byte[] RunCommand(byte[] command, bool response = true)
        {

            if (!Connected)
            {
                throw new InvalidOperationException("Not connected.");
            }
            lock (commandLock)
            {
                using (var sh = OpenUSBPrinterPort(PrinterName))
                {
                    using (var f = new System.IO.FileStream(sh, System.IO.FileAccess.ReadWrite))
                    {
                        f.Write(command, 0, command.Length);
                    }
                }
                if (response)
                {
                    using (var sh = OpenUSBPrinterPort(PrinterName))
                    using (var f = new System.IO.FileStream(sh, System.IO.FileAccess.ReadWrite))
                    using (var ms = new MemoryStream())
                    {
                        var timeout = 20000;
                        var buffer = new byte[4096];

                        while (true)
                        {

                            uint ioThread = 0;
                            var t = Task.Run(() =>
                            {
                                ioThread = GetCurrentThreadId();
                                var read = f.Read(buffer, 0, buffer.Length);
                                if (read != 0 && ms.CanWrite)
                                    ms.Write(buffer, 0, read);
                            });


                            if (!t.Wait(timeout))
                            {
                                int r = 0;
                                if (ioThread != 0)
                                {
                                    IntPtr tHandle = IntPtr.Zero;
                                    try
                                    {
                                        tHandle = OpenThread(ThreadAccess.All, false, ioThread);
                                        r = CancelSynchronousIo(tHandle);
                                    }
                                    finally
                                    {
                                        if (tHandle != IntPtr.Zero)
                                            CloseHandle(tHandle);
                                    }
                                }
                                break;
                            }
                            timeout = 200;

                        }
                        sh.Close();
                        return ms.ToArray();

                    }
                }
                return null;
            }
        }

        [Flags]
        private enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200),
            All = TERMINATE | SUSPEND_RESUME | GET_CONTEXT | SET_CONTEXT | SET_INFORMATION | QUERY_INFORMATION | SET_THREAD_TOKEN | IMPERSONATE | DIRECT_IMPERSONATION,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SP_DEVICE_INTERFACE_DATA
        {
            public uint cbSize;
            public Guid InterfaceClassGuid;
            public uint Flags;
            public IntPtr Reserved;
        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        private struct SP_DEVICE_INTERFACE_DETAIL_DATA  // Only used for Marshal.SizeOf. NOT!
        {
            public uint cbSize;
            public char DevicePath;
        }

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetupDiGetClassDevs([In(), MarshalAs(UnmanagedType.LPStruct)] System.Guid ClassGuid, string Enumerator, IntPtr hwndParent, uint Flags);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, uint MemberIndex, ref SP_DEVINFO_DATA DeviceInfoData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, [In()] ref SP_DEVINFO_DATA DeviceInfoData, [In(), MarshalAs(UnmanagedType.LPStruct)] System.Guid InterfaceClassGuid, uint MemberIndex, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, [In()] ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, IntPtr DeviceInterfaceDetailData, uint DeviceInterfaceDetailDataSize, out uint RequiredSize, IntPtr DeviceInfoData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern int SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, int dwShareMode, IntPtr lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int CancelSynchronousIo(IntPtr hThread);

        private const uint DIGCF_PRESENT = 0x00000002U;
        private const uint DIGCF_DEVICEINTERFACE = 0x00000010U;
        private const int ERROR_INSUFFICIENT_BUFFER = 122;

        private const int FILE_SHARE_READ = 1;
        private const int FILE_SHARE_WRITE = 2;
        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const int OPEN_EXISTING = 3;

        private static readonly Guid GUID_DEVINTERFACE_USBPRINT = new Guid(0x28d78fad, 0x5a12, 0x11D1, 0xae, 0x5b, 0x00, 0x00, 0xf8, 0x03, 0xa8, 0xc2);
        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        private static string GetUSBInterfacePath(string SystemDeviceInstanceID)
        {
            if (string.IsNullOrEmpty(SystemDeviceInstanceID)) throw new ArgumentNullException("SystemDeviceInstanceID");

            IntPtr hdi = SetupDiGetClassDevs(GUID_DEVINTERFACE_USBPRINT, SystemDeviceInstanceID, IntPtr.Zero, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
            if (hdi.Equals(INVALID_HANDLE_VALUE)) throw new System.ComponentModel.Win32Exception();

            try
            {
                SP_DEVINFO_DATA device_data = new SP_DEVINFO_DATA();
                device_data.cbSize = (uint)Marshal.SizeOf(typeof(SP_DEVINFO_DATA));

                if (SetupDiEnumDeviceInfo(hdi, 0, ref device_data) == 0) throw new System.ComponentModel.Win32Exception();  // Only one device in the set

                SP_DEVICE_INTERFACE_DATA interface_data = new SP_DEVICE_INTERFACE_DATA();
                interface_data.cbSize = (uint)Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA));

                if (SetupDiEnumDeviceInterfaces(hdi, ref device_data, GUID_DEVINTERFACE_USBPRINT, 0, ref interface_data) == 0) throw new System.ComponentModel.Win32Exception();   // Only one interface in the set


                // Get required buffer size
                uint required_size = 0;
                SetupDiGetDeviceInterfaceDetail(hdi, ref interface_data, IntPtr.Zero, 0, out required_size, IntPtr.Zero);

                int last_error_code = Marshal.GetLastWin32Error();
                if (last_error_code != ERROR_INSUFFICIENT_BUFFER) throw new System.ComponentModel.Win32Exception(last_error_code);

                IntPtr interface_detail_data = Marshal.AllocCoTaskMem((int)required_size);

                try
                {

                    // FIXME, don't know how to calculate the size.
                    // See https://stackoverflow.com/questions/10728644/properly-declare-sp-device-interface-detail-data-for-pinvoke

                    switch (IntPtr.Size)
                    {
                        case 4:
                            Marshal.WriteInt32(interface_detail_data, 4 + Marshal.SystemDefaultCharSize);
                            break;
                        case 8:
                            Marshal.WriteInt32(interface_detail_data, 8);
                            break;

                        default:
                            throw new NotSupportedException("Architecture not supported.");
                    }

                    if (SetupDiGetDeviceInterfaceDetail(hdi, ref interface_data, interface_detail_data, required_size, out required_size, IntPtr.Zero) == 0) throw new System.ComponentModel.Win32Exception();

                    // TODO: When upgrading to .NET 4, replace that with IntPtr.Add
                    return Marshal.PtrToStringAuto(new IntPtr(interface_detail_data.ToInt64() + Marshal.OffsetOf(typeof(SP_DEVICE_INTERFACE_DETAIL_DATA), "DevicePath").ToInt64()));

                }
                finally
                {
                    Marshal.FreeCoTaskMem(interface_detail_data);
                }
            }
            finally
            {
                SetupDiDestroyDeviceInfoList(hdi);
            }
        }

        public static List<string> GetUSBPrinterPorts()
        {
            var results = new List<string>();

            ManagementObjectSearcher s = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity");
            foreach (ManagementObject device in s.Get())
            {
                // Try Name, Caption and/or Description (they seem to be same most of the time).
                string Name = (string)device.GetPropertyValue("Name");

                // >>>>>>>>>>>>>>>>>>>> Query String ...
                if (Name == "USB Printing Support")
                {
                    try
                    {
                        var pnp = device.GetPropertyValue("PNPDeviceID") as string;
                        if (pnp != null)
                            results.Add(pnp);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("ERROR: {0}", e.Message);
                    }
                }
            }
            return results;
        }


        static string GetUSBPathFromPNPID(string PrinterName)
        {
            return GetUSBInterfacePath(PrinterName);
        }

        static Microsoft.Win32.SafeHandles.SafeFileHandle OpenUSBPrinterPort(string PNPID)
        {
            return new Microsoft.Win32.SafeHandles.SafeFileHandle(CreateFile(GetUSBPathFromPNPID(PNPID), GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero), true);
        }


    }
}
