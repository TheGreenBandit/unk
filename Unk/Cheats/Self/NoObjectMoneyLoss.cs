using Photon.Pun;
using Unk.Cheats.Core;
using Unk.Manager;
using Unk.Util;

namespace Unk.Cheats
{
    internal class NoObjectMoneyLoss : ToggleCheat
    {
        public override void Update()//make it so items no longer break also, probably patch out impacts
        {
            foreach (ValuableObject item in GameObjectManager.items)
            {
                if (item is null || !Enabled) continue;
                if (item.Reflect().GetValue<float>("dollarValueCurrent") != item.valuePreset.valueMax)////we want max standard value, not MAX value eg 999999999
                    item.Reflect().GetValue<PhotonView>("photonView").RPC("DollarValueSetRPC", RpcTarget.All, item.valuePreset.valueMax);
            }
        }
    }
}