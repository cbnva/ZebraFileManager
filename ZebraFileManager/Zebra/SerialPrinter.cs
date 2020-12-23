using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using RJCP.IO.Ports;


namespace ZebraFileManager.Zebra
{
    public class SerialPrinter : Printer
    {
        public string ComPort { get; set; }

        public override bool Connected => port?.IsOpen == true;

        SerialPortStream port;

        public override bool Connect()
        {
            if (Connected != true)
            {
                port = new SerialPortStream(ComPort);
                port.Open();
            }
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
                port.Write(command, 0, command.Length);
                if (response)
                {

                    var buffer = new byte[port.ReadBufferSize];
                    using (var ms = new MemoryStream())
                    {
                        int read;
                        port.ReadTimeout = 5000;

                        try
                        {
                            while ((read = port.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                port.ReadTimeout = 100;
                                ms.Write(buffer, 0, read);
                            }
                        }
                        catch (SocketException)
                        {
                            // Assume it timed out because the host finished sending information.

                        }
                        return ms.ToArray();
                    }
                }
                return null;
            }
        }
    }
}
