using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Handlers
{
    public class CP_EQUIPMENT :Networking.PacketHandler
    {
        public static void MakeEquipmentPacket(WRClient wc, GameFramework.Classes _class, string _equipment) {
            Core.OutPacket mPacket = new Core.OutPacket(29970);
            mPacket.AddBlock(1);
            mPacket.AddBlock((int)_class);
            mPacket.AddBlock(_equipment);
            byte[] mBuffer = mPacket.GetOutput();
            for (int I = 0; I < mBuffer.Length; I++)
                mBuffer[I] ^= Core.BuildConfig.GameKey_Server;
            wc.ClientSocket.Send(mBuffer);
        }
        public override void Handle()
        {
            bool equipItem = !(this.packet.GetInt(0) == 1);
            GameFramework.Classes bTargetClass = (GameFramework.Classes)this.packet.GetInt(1);

            if (bTargetClass < GameFramework.Classes.COUNT)
            {
                string weaponCode = this.packet.GetString(4).ToUpper();
                int targetSlot = 0;

                if (weaponCode.Length == 4)
                {
                    if (targetSlot < 8)
                    {
                        if (equipItem)
                        {
                            targetSlot = this.packet.GetInt(5);

                            string eqClass = string.Empty;
                            switch (bTargetClass)
                            {
                                case GameFramework.Classes.Engineer:
                                    this.client.Inventory.Engineer[targetSlot] = weaponCode;
                                    eqClass = string.Join(",", this.client.Inventory.Engineer);
                                    break;
                                case GameFramework.Classes.Medic:
                                    this.client.Inventory.Medic[targetSlot] = weaponCode;
                                    eqClass = string.Join(",", this.client.Inventory.Medic);
                                    break;
                                case GameFramework.Classes.Sniper:
                                    this.client.Inventory.Sniper[targetSlot] = weaponCode;
                                    eqClass = string.Join(",", this.client.Inventory.Sniper);
                                    break;
                                case GameFramework.Classes.Assault:
                                    this.client.Inventory.Assault[targetSlot] = weaponCode;
                                    eqClass = string.Join(",", this.client.Inventory.Assault);
                                    break;
                                case GameFramework.Classes.Heavy:
                                    this.client.Inventory.Heavy[targetSlot] = weaponCode;
                                    eqClass = string.Join(",", this.client.Inventory.Heavy);
                                    break;
                            }

                            this.client.SaveInventory();
                            MakeEquipmentPacket(this.client, bTargetClass, eqClass);
                        } else {
                            targetSlot = this.packet.GetInt(3);

                            string eqClass = string.Empty;
                            switch (bTargetClass)
                            {
                                case GameFramework.Classes.Engineer:
                                    this.client.Inventory.Engineer[targetSlot] = "^";
                                    eqClass = string.Join(",", this.client.Inventory.Engineer);
                                    break;
                                case GameFramework.Classes.Medic:
                                    this.client.Inventory.Medic[targetSlot] = "^";
                                    eqClass = string.Join(",", this.client.Inventory.Medic);
                                    break;
                                case GameFramework.Classes.Sniper:
                                    this.client.Inventory.Sniper[targetSlot] = "^";
                                    eqClass = string.Join(",", this.client.Inventory.Sniper);
                                    break;
                                case GameFramework.Classes.Assault:
                                    this.client.Inventory.Assault[targetSlot] = "^";
                                    eqClass = string.Join(",", this.client.Inventory.Assault);
                                    break;
                                case GameFramework.Classes.Heavy:
                                    this.client.Inventory.Heavy[targetSlot] = "^";
                                    eqClass = string.Join(",", this.client.Inventory.Heavy);
                                    break;
                            }

                            this.client.SaveInventory();
                            MakeEquipmentPacket(this.client, bTargetClass, eqClass);
                        }
                    }
                }
            }
        }
    }
}
