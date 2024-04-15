using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZebraFileManager.Zebra
{
    public class PrinterMessage
    {
        string stringContents;

        public byte[] ByteContents { get; set; }

        /// <summary>
        /// If true, `StringContents` contains hexadecimal representation of `ByteContents`. If false, `StringContents` contains textual contents.
        /// </summary>
        public bool IsBinaryString { get; private set; }

        public string StringContents
        {
            get
            {
                if (stringContents == null && ByteContents != null)
                {
                    try
                    {
                        stringContents = Encoding.UTF8.GetString(ByteContents);
                        IsBinaryString = false;
                    }
                    catch
                    {

                        var sb = new StringBuilder();
                        sb.AppendLine($"BINARY: {ByteContents.Length} bytes\r\n");
                        var bytesPerLine = 32;
                        for (int i = 0; i < ByteContents.Length; i += bytesPerLine)
                        {
                            for (int j = i; j < Math.Min(i + bytesPerLine, ByteContents.Length); j++)
                            {
                                sb.AppendFormat("{0:X2} ", ByteContents[j]);
                            }
                            sb.Remove(sb.Length - 1, 1);
                            sb.AppendLine();
                        }
                        stringContents = sb.ToString();

                        IsBinaryString = true;
                    }
                }

                return stringContents;
            }
            set
            {
                stringContents = value;
            }
        }

        public DateTime TimeGenerated { get; set; }

        public PrinterMessageType Direction { get; set; }

    }

    public enum PrinterMessageType
    {
        Send,
        Receive
    }

}
