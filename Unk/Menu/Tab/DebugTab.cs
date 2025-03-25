using Photon.Pun;
using System.Linq;
using UnityEngine;
using Unk.Cheats;
using Unk.Cheats.Core;
using Unk.Manager;
using Unk.Menu.Core;
using Unk.Util;

namespace Unk.Menu.Tab
{
    internal class DebugTab : MenuTab
    {
        public DebugTab() : base("Debug") { }
        private Vector2 scrollPos = Vector2.zero;

        public override void Draw()
        {
            GUILayout.BeginVertical();
            MenuContent();
            GUILayout.EndVertical();         
        }

        private void MenuContent()
        {
            //scrollPos = GUILayout.BeginScrollView(scrollPos);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Master Client: ");
            GUILayout.FlexibleSpace();
            GUILayout.Label(PhotonNetwork.IsMasterClient ? "Yes" : "No");
            GUILayout.EndHorizontal();
            UI.Button("Log RPCS", () => { PhotonNetwork.PhotonServerSettings.RpcList.ToList().ForEach(r => Debug.Log(r));
            });

            UI.Button("Join Random Game",() => { PhotonNetwork.JoinRandomOrCreateRoom(); });
            UI.Button("Spawn again", () => { PlayerAvatar.instance.playerTransform.position = new Vector3(0, 0, 0); });
            UI.Button("Spawn Item", () =>
            {
                PhotonNetwork.Instantiate("Valuables/" + AssetManager.instance.surplusValuableSmall.name, PlayerAvatar.instance.transform.position, new Quaternion(0, 0, 0, 0));
            });
            //UI.Button("Spawn Item", () =>
            //{
            //    typeof(PhotonNetwork).Reflect().Invoke("NetworkInstantiate", new InstantiateParameters(name, PlayerAvatar.instance.transform.position, Quaternion.identity, 0, null, )("Valuables/" + AssetManager.instance.surplusValuableSmall.name, PlayerAvatar.instance.transform.position, new Quaternion(0, 0, 0, 0));
            //});


            UI.Button("Test items", () =>
            {
                PunManager.instance.Reflect().GetValue<PhotonView>("photonView").RPC("UpdateStatRPC", RpcTarget.All, "itemsPurchased", "Handgun", 1);
                LevelGenerator.Instance.PhotonView.RPC("ItemSetup", RpcTarget.All);
            });
            //	public Dictionary<string, Item> itemDictionary = new Dictionary<string, Item>(); //get item names from here
            if (StatsManager.instance.itemDictionary != null)
            {

                for (int i = 0; i < StatsManager.instance.itemDictionary.Count; i++)
                    UI.Button(StatsManager.instance.itemDictionary.ElementAt(i).Key, () => 
                    {

                        PunManager.instance.AddingItem(StatsManager.instance.itemDictionary.ElementAt(i).Key,
                            StatsManager.instance.GetIndexThatHoldsThisItemFromItemDictionary(
                                StatsManager.instance.itemDictionary.ElementAt(i).Key), -1, null); });
            }
            //UI.Button("AddingItemRPC test", () => { PunManager.instance.AddingItem("name", StatsManager.instance.GetIndexThatHoldsThisItemFromItemDictionary("anem"), -1, null); });

            UI.Button("Revive all", () =>
            {
                GameObjectManager.players.Where(p => p != null && p.IsDead()).ToList().ForEach(p => p.Revive());
            });

            UI.Button("Revive localPlayer", () =>
            {
                PlayerAvatar.instance.GetLocalPlayer().Revive();
            });

            // use for target enemy.SetChaseTarget

            UI.Button("Raycast", () =>
            {
                string debugMessage = "";
                foreach (RaycastHit hit in SemiFunc.MainCamera().transform.SphereCastForward())
                {
                    Collider collider = hit.collider;
                    debugMessage += $"Hit: {collider.name} => {collider.gameObject.name} => Layer {LayerMask.LayerToName(collider.gameObject.layer)} {collider.gameObject.layer}\n";
                }
                Debug.Log(debugMessage);
            });

            UI.Button("Unload test", Loader.Unload);

            //GUILayout.EndScrollView();
        }
    }

}
