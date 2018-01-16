namespace Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
     
    public class InPacket
    {
        private ushort _operationCode;
        private long _timestamp;
        private List<string> _blocks;

        public void Set(byte[] _input) {
            string strInput = ASCIIEncoding.GetEncoding("Windows-1250").GetString(_input);
            string[] strBlocks = strInput.Split(' ');

            this._timestamp = Convert.ToInt64(strBlocks[0]);
            this._operationCode = Convert.ToUInt16(strBlocks[1]);

            string[] newBlocks = new string[strBlocks.Length - 2];
            Array.Copy(strBlocks, 2, newBlocks, 0, strBlocks.Length - 2);

            for(byte I = 0; I < newBlocks.Length; I++) {
                this._blocks.Add(newBlocks[I]);
            }
        }

        public ushort GetOPC() { return this._operationCode; }

        public string GetString(int Idx) { return this._blocks[Idx]; }
        public int GetInt(int Idx) { return Convert.ToInt32(this.GetString(Idx)); }
        public int GetUShort(int Idx) { return Convert.ToUInt16(this.GetString(Idx)); }  
    }
}
