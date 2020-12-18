using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ZebraFileManager.Zebra
{
    public class IPPrinter : Printer
    {
        public string Host { get; set; }

        public override bool Connected => socket?.Connected == true;

        Socket socket;

        public override bool Connect()
        {
            if (socket?.Connected != true)
            {

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(Host, 9100);
            }
            return socket.Connected;
        }

        public override string RunCommand(string command, bool response = true)
        {
            return Encoding.UTF8.GetString(RunCommand(Encoding.UTF8.GetBytes(command), response));
        }

        public override byte[] RunCommand(byte[] command, bool response = true)
        {

            if (!Connected)
            {
                throw new InvalidOperationException("Not connected.");
            }

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(Host, 9100);

                socket.Send(command);
                if (response)
                {
                    var buffer = new byte[socket.ReceiveBufferSize];
                    using (var ms = new MemoryStream())
                    {
                        int read;
                        socket.ReceiveTimeout = 5000;
                        
                        try
                        {
                            while ((read = socket.Receive(buffer)) > 0)
                            {
                                socket.ReceiveTimeout = 100;
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
