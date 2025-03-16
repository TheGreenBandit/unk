using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unk.Util
{
    internal class ItemUtil
    {
        public static Item GetItemByName(string name)
        {
            var itemPair = StatsManager.instance.itemDictionary.ToList().Find(x => x.Value.name == name);
            return itemPair.Equals(default(KeyValuePair<string, Item>)) ? null : itemPair.Value;
        }
        public static Vector3 GetSpawnPos() => SemiFunc.MainCamera().transform.SphereCastForward()[0].point;
        public static void SpawnItem(string name) => SpawnItem(name, GetSpawnPos());
        public static void SpawnItem(string name, Vector3 pos, int amount = 1, bool equip = false)
        {
            pos = PlayerAvatar.instance.playerTransform.position;
            Debug.Log($"Attemping Spawn: {name}, pos {pos}, amount {amount}, equip {equip}");
            for (int i = 0; i < amount; i++)
            {
                GameObject obj = GetItemByName(name).prefab;
                if (PhotonNetwork.IsConnected)
                {
                    Debug.Log("Online");
                    obj = PhotonNetwork.Instantiate("Valuables/" + name, pos, Quaternion.identity);
                    if (!PhotonNetwork.IsMasterClient)
                        Object.Instantiate(obj, pos, Quaternion.identity); //test load scripts? maybe for local player only but see what happens
                }
                else
                {
                    Debug.Log("Offline");
                    obj = Object.Instantiate(obj, pos, Quaternion.identity);
                }
                if (equip)
                    obj.GetComponent<ItemEquippable>().RequestEquip(Inventory.instance.GetFirstFreeInventorySpotIndex());

            }

        }
    }
}
