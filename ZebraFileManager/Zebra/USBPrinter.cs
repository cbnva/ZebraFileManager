using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
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

            int dwError = 0, dwWritten = 0;
            var hPrinter = new IntPtr(0);
            var bSuccess = false; // Assume failure unless you specifically succeed.
            bool NLMDocOpen = false;
            uint junk;
            uint dataRetrieved = 0;
            string tempFile = null;
            lock (commandLock)
            {
                try
                {

                    // Steps as uncovered by http://www.rohitab.com/apimonitor (amazing software, btw):
                    //   1. Open Printer
                    //   2. NLMOpenDoc
                    //   3. Write request to file
                    //   4. NLMPassFileThrough
                    //   5. NLMReadData {repeat until everything is retrieved}
                    if (OpenPrinter(PrinterName.Normalize(), out hPrinter, new PRINTER_DEFAULTS { DesiredAccess = ACCESS_MASK.PRINTER_ALL_ACCESS }))
                    {

                        byte[] data = new byte[4096];
                        uint result;

                        for (int i = 0; i < 10; i++)
                        {

                            // NLMOpenDoc
                            var openData = new byte[33];
                            result = GetPrinterDataA(hPrinter, "NLMOpenDoc", IntPtr.Zero, openData, (uint)openData.Length, ref dataRetrieved);
                            if (result == 0 && openData.Length >= dataRetrieved && Encoding.UTF8.GetString(openData, 0, (int)dataRetrieved) == "NLMOpenOk")
                            {
                                NLMDocOpen = true;
                                break;
                            }
                        }
                        if (!NLMDocOpen)
                            throw new InvalidOperationException("Unable to open the printer.");

                        // Create the temp file
                        tempFile = Path.Combine(Path.GetTempPath(), GetRandomString(16));
                        System.IO.File.WriteAllBytes(tempFile, command);

                        // Send the data to the printer
                        dataRetrieved = 0x00515c27;

                        result = GetPrinterDataA(hPrinter, ("NLMPassFileThrough" + tempFile).Normalize(), IntPtr.Zero, data, (uint)data.Length, ref dataRetrieved);
                        if (result != 0)
                            throw new InvalidOperationException("Unable to send data to printer.");

                        // Read the data
                        using (var ms = new MemoryStream())
                        {
                            while (true)
                            {
                                result = GetPrinterDataA(hPrinter, "NLMReadData", IntPtr.Zero, data, (uint)data.Length, ref dataRetrieved);
                                if (dataRetrieved == 21 && Encoding.UTF8.GetString(data, 0, (int)dataRetrieved).StartsWith("NLMFailed"))
                                {
                                    // End of data
                                    break;
                                }
                                else
                                {
                                    ms.Write(data, 0, (int)dataRetrieved);
                                }
                            }

                            return ms.ToArray();
                        }


                    }
                }
                finally
                {

                    if (NLMDocOpen)
                        GetPrinterDataA(hPrinter, "NLMCloseDoc", IntPtr.Zero, new byte[0], (uint)0, ref dataRetrieved);

                    if (hPrinter != IntPtr.Zero)
                        ClosePrinter(hPrinter);

                    if (System.IO.File.Exists(tempFile))
                        System.IO.File.Delete(tempFile);
                }
                return null;
            }
        }

        static string GetRandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, PRINTER_DEFAULTS pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Ansi, SetLastError = true)]
        static extern uint GetPrinterDataA(IntPtr hPrinter, string pValueName, IntPtr pType, byte[] pData, uint nSize, ref uint pcbNeeded);


        /// <summary>
        /// The PRINTER_DEFAULTS structure specifies the default data type,
        /// environment, initialization data, and access rights for a printer.
        /// </summary>
        /// <seealso href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd162839(v=vs.85).aspx"/>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private class PRINTER_DEFAULTS
        {
            /// <summary>
            /// Pointer to a null-terminated string that specifies the
            /// default data type for a printer.
            /// </summary>
            public IntPtr pDatatype;

            /// <summary>
            /// Pointer to a DEVMODE structure that identifies the
            /// default environment and initialization data for a printer.
            /// </summary>
            public IntPtr pDevMode;

            /// <summary>
            /// Specifies desired access rights for a printer.
            /// The <see cref="OpenPrinter(string, out IntPtr, IntPtr)"/> function uses
            /// this member to set access rights to the printer. These rights can affect
            /// the operation of the SetPrinter and DeletePrinter functions.
            /// </summary>
            public ACCESS_MASK DesiredAccess;
        }

        [Flags]
        private enum ACCESS_MASK : uint
        {
            JOB_ACCESS_ADMINISTER = 0x00000010,
            JOB_ACCESS_READ = 0x00000020,
            JOB_EXECUTE = 0x00020010,
            JOB_READ = 0x00020020,
            JOB_WRITE = 0x00020010,
            JOB_ALL_ACCESS = 0x000F0030,
            PRINTER_ACCESS_ADMINISTER = 0x00000004,
            PRINTER_ACCESS_USE = 0x00000008,
            PRINTER_ACCESS_MANAGE_LIMITED = 0x00000040,
            PRINTER_ALL_ACCESS = 0x000F000C,
            PRINTER_EXECUTE = 0x00020008,
            PRINTER_READ = 0x00020008,
            PRINTER_WRITE = 0x00020008,
            SERVER_ACCESS_ADMINISTER = 0x00000001,
            SERVER_ACCESS_ENUMERATE = 0x00000002,
            SERVER_ALL_ACCESS = 0x000F0003,
            SERVER_EXECUTE = 0x00020002,
            SERVER_READ = 0x00020002,
            SERVER_WRITE = 0x00020003,
            SPECIFIC_RIGHTS_ALL = 0x0000FFFF,
            STANDARD_RIGHTS_ALL = 0x001F0000,
            STANDARD_RIGHTS_EXECUTE = 0x00020000,
            STANDARD_RIGHTS_READ = 0x00020000,
            STANDARD_RIGHTS_REQUIRED = 0x000F0000,
            STANDARD_RIGHTS_WRITE = 0x00020000,
            SYNCHRONIZE = 0x00100000,
            DELETE = 0x00010000,
            READ_CONTROL = 0x00020000,
            WRITE_DAC = 0x00040000,
            WRITE_OWNER = 0x00080000,
            GENERIC_READ = 0x80000000,
            GENERIC_WRITE = 0x40000000,
            GENERIC_EXECUTE = 0x20000000,
            GENERIC_ALL = 0x10000000

        }
    }
}
