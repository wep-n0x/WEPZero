namespace Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    public class OutPacket
    {
        private int operationCode;
        private List<string> blocks;

        public OutPacket(int opc)
        {
            this.operationCode = opc;
            this.blocks = new List<string>();
        }

        public void AddBlock(object o) {
            this.blocks.Add(o.ToString().Replace('\x20', '\x1D'));
        }

        public byte[] GetOutput() {
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.TickCount + " ");
            sb.Append(this.operationCode + " ");
            foreach(string sBlock in blocks.ToArray())
            {
                sb.Append(sBlock + " ");
            }
            sb.Append("\n\0");

            return ASCIIEncoding.GetEncoding("Windows-1250").GetBytes(sb.ToString());
        }
    }
}
