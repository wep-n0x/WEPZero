namespace Game.Networking
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PacketHandler
    {
        public Core.InPacket packet;
        public WRClient client;

        public void Set(WRClient _wc, byte[] _input)
        {
            this.packet = new Core.InPacket();
            this.packet.Set(_input);

            this.client = _wc;
        }

        public virtual void Handle() { }
    }
}
