using Unk.Util;
using UnityEngine;
using System.Collections.Generic;
using System;
using Unk.Manager;
using System.Linq;
using System.Text.RegularExpressions;
using Unk.Cheats;
using System.Runtime.CompilerServices;
using Photon.Pun;
using static UnityEngine.InputSystem.InputRemoting;

namespace Unk
{
    public static class Extensions
    {
        private static readonly Dictionary<Type, Delegate> TryParseMethods = new Dictionary<Type, Delegate>()
        {
            { typeof(int), new TryParseDelegate<int>(int.TryParse) },
            { typeof(float), new TryParseDelegate<float>(float.TryParse) },
            { typeof(double), new TryParseDelegate<double>(double.TryParse) },
            { typeof(decimal), new TryParseDelegate<decimal>(decimal.TryParse) },
            { typeof(bool), new TryParseDelegate<bool>(bool.TryParse) },
            { typeof(DateTime), new TryParseDelegate<DateTime>(DateTime.TryParse) },
            { typeof(TimeSpan), new TryParseDelegate<TimeSpan>(TimeSpan.TryParse) },
            { typeof(ulong), new TryParseDelegate<ulong>(ulong.TryParse) },
            { typeof(long), new TryParseDelegate<long>(long.TryParse) },
        };

        public static string Subtract(this string s, int num) => s.Substring(0, s.Length - num);

        //public static Camera GetCamera(this PlayerAvatar mainCamera)
        //{
        //    return mainCamera.Reflect().GetValue<Camera>("cam");
        //}
        public static Vector3 GetClosestMonster(this Vector3 point) => GameObjectManager.enemies.OrderBy(x => Vector3.Distance(x.transform.position, point)).FirstOrDefault().transform.position;

        public static void Kill(this Enemy enemy)
        {
            enemy.GetHealth()?.Reflect().Invoke("Death", new Vector3(0, 0, 0));
        }

        public static void Hurt(this Enemy enemy, int damage)
        {
            enemy.GetHealth().Reflect().Invoke("Hurt", damage, new Vector3(0, 0, 0));
        }

        public static void Heal(this Enemy enemy, int heal)
        {
            enemy.GetHealth().Reflect().Invoke("Hurt", -heal, new Vector3(0, 0, 0));
        }

        public static EnemyHealth GetHealth(this Enemy enemy)
        {
            return enemy.Reflect().GetValue<EnemyHealth>("Health");
        }

        public static bool IsDead(this Enemy enemy) => enemy != null && enemy.Reflect().GetValue<EnemyHealth>("Health").Reflect().GetValue<bool>("dead");
        public static bool IsLocalPlayer(this PlayerAvatar player) => player != null && player.Reflect().GetValue<bool>("isLocal");
        public static bool IsDead(this PlayerAvatar player) => player != null && player.Reflect().GetValue<bool>("deadSet");
        public static string GetName(this EnemySetup enemy)
        {
            string name = enemy.name;
            name = name.Replace("Enemy -", "").Trim();  
            name = Regex.Replace(name, @"Enemy Group - \d+", "").Trim();  
            return name;
        }

        public static string GetName(this Enemy enemy) => enemy.Reflect().GetValue<EnemyParent>("EnemyParent").enemyName;
        public static string GetName(this PlayerAvatar player) => string.IsNullOrEmpty(player.Reflect().GetValue<string>("playerName")) ? player.name : player.Reflect().GetValue<string>("playerName");
        public static string Format(this string @string) => @string.Replace("(Clone)", "").Replace("Valuable", "").Trim();
        public static PlayerAvatar GetLocalPlayer(this PlayerAvatar player) => GameObjectManager.players?.FirstOrDefault(p => p != null && p.IsLocalPlayer());
        public static List<PlayerAvatar> GetAlivePlayers(this PlayerAvatar player) => GameObjectManager.players.Where(p => p != null && !p.IsDead()).ToList();
        public static EnemyParent GetEnemyParent(this EnemySetup enemy) => enemy.spawnObjects.Select(o => o?.GetComponent<EnemyParent>()).FirstOrDefault(e => e != null);
        public static PhysGrabObject GetHeldObject(this PlayerAvatar player) => player.physGrabber.Reflect().GetValue<PhysGrabObject>("grabbedPhysGrabObject");
        public static PhysGrabObject GetPhysGrabObject(this ValuableObject item) => item.Reflect().GetValue<PhysGrabObject>("physGrabObject");
        public static PhotonTransformView GetPhotonTransformView(this ValuableObject item) => item.GetPhysGrabObject().Reflect().GetValue<PhotonTransformView>("photonTransformView");

        public static void RevivePlayer(this PlayerAvatar player)
        {
            if (player != null && player.IsDead()) player.photonView.RPC("ReviveRPC", RpcTarget.All, false);
        }

        public static RaycastHit[] SphereCastForward(this Transform transform, float sphereRadius = 1.0f)
        {
            try
            {
                return Physics.SphereCastAll(
                    transform.position + (transform.forward * (sphereRadius + 1.75f)),
                    sphereRadius,
                    transform.forward,
                    float.MaxValue
                );
            }

            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static bool Parse<T>(this string s, out T result) where T : struct, IConvertible, IComparable<T>
        {
            result = default(T);
            bool success = false;

            if (TryParseMethods.TryGetValue(typeof(T), out var method))
                success = ((TryParseDelegate<T>)method)(s, out result);

            return success;
        }

        private delegate bool TryParseDelegate<T>(string input, out T result);
    }
}