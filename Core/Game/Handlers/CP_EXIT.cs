namespace Game.Handlers
{
    public class CP_EXIT : Networking.PacketHandler
    {
        public override void Handle()
        {
            this.client.Disconnect();
        }
    }
}
