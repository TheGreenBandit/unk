using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using Unk.Manager;
using Unk.Menu.Core;
using Unk.Util;
using Object = UnityEngine.Object;

namespace Unk.Menu.Tab
{
    internal class EnemyTab : MenuTab
    {
        public EnemyTab() : base("Enemies") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 scrollPos2 = Vector2.zero;
        private Vector2 scrollPos3 = Vector2.zero;
        private int selectedTab = 0;
        private readonly string[] tabs = ["Enemy List", "Spawn Enemies"];
        private static int selectedEnemy = -1;
        private static int selectedEnemySetup = -1;
        private string s_spawnAmount = "1";
        private int damage = 0;
        private int heal = 0;
        private float freeze = 1;

        public override void Draw()
        {
            GUILayout.BeginVertical(GUILayout.Width(UnkMenu.Instance.contentWidth - UnkMenu.Instance.spaceFromLeft));
            selectedTab = GUILayout.Toolbar(selectedTab, tabs);


            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical(GUILayout.Width(UnkMenu.Instance.contentWidth * 0.3f - UnkMenu.Instance.spaceFromLeft));
            EnemyList();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(UnkMenu.Instance.contentWidth * 0.7f - UnkMenu.Instance.spaceFromLeft));
            scrollPos2 = GUILayout.BeginScrollView(scrollPos2);

            switch (selectedTab)
            {
                case 0:
                    GeneralActions();
                    EnemyActions();
                    break;
                case 1:
                    EnemySpawnerContent();
                    break;
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

        }

        private void EnemyList()
        {
            switch (selectedTab)
            {
                case 0:
                    if (!GameObjectManager.enemies.Exists(e => e.GetInstanceID() == selectedEnemy)) selectedEnemy = -1;
                    DrawList<Enemy>("Enemy List", GameObjectManager.enemies.OrderBy(e => e.GetName()).ToList(), e => e.IsDead(), e => e.GetName(), ref scrollPos, ref selectedEnemy);
                    break;
                case 1:
                    if (!GetEnemies().Exists(e => e.GetInstanceID() == selectedEnemySetup)) selectedEnemySetup = -1;
                    DrawList<EnemySetup>("Enemy Type", GetEnemies().OrderBy(e => e.GetName()).ToList(), _ => false, e => e.GetName(), ref scrollPos3, ref selectedEnemySetup);
                   break;
            }
        }

        private void DrawList<T>(string title, IEnumerable<T> objects, Func<T, bool> conditional, Func<T, string> label, ref Vector2 scroll, ref int instanceID) where T : Object
        {
            float width = UnkMenu.Instance.contentWidth * 0.3f - UnkMenu.Instance.spaceFromLeft * 2;
            float height = UnkMenu.Instance.contentHeight - 45;

            Rect rect = new Rect(0, 30, width, height);
            GUI.Box(rect, title);

            GUILayout.BeginVertical(GUILayout.Width(width), GUILayout.Height(height));
            GUILayout.Space(25);
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            foreach (T item in objects)
            {
                if (conditional(item)) continue;

                if (instanceID == -1) instanceID = item.GetInstanceID();

                if (instanceID == item.GetInstanceID()) GUI.contentColor = Settings.c_espEnemies.GetColor();

                if (GUILayout.Button(label(item), GUI.skin.label)) instanceID = item.GetInstanceID();

                GUI.contentColor = Settings.c_menuText.GetColor();
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void GeneralActions()
        {
            UI.Header("General Actions");
            UI.Button("Kill All", () => GameObjectManager.enemies.Where(e => e != null && !e.IsDead()).ToList().ForEach(e => e.Kill()));
        }

        private void EnemyActions()
        {
            Enemy enemy = GetSelectedEnemy();
            if (enemy == null) return;

            UI.Header("Selected Monster Actions");

            UI.Button("Kill", () => enemy.Kill());
            UI.TextboxAction("Damage", ref damage, 3,
                new UIButton("Amount", () => enemy.Hurt(damage))
            );
            UI.TextboxAction("Heal", ref heal, 3,
                new UIButton("Amount", () => enemy.Heal(heal))
            );
            UI.TextboxAction("Freeze", ref freeze, 3,
                new UIButton("Time", () => enemy.Freeze(freeze))
            );
        }

        private void EnemySpawnerContent()
        {
            if (selectedEnemySetup == -1) return;
            EnemySetup enemySetup = GetEnemies().Find(x => x.GetInstanceID() == selectedEnemySetup);
            if (enemySetup == null) return;

            if (!PhotonNetwork.IsMasterClient)
            {
                UI.Label("Host is required", Settings.c_menuText);
                return;
            }

            UI.Header("Enemy Spawner Content");

            UI.Label("Selected Enemy:", enemySetup.GetName(), Settings.c_menuText);
            UI.Textbox("Spawn Amount", ref s_spawnAmount, @"[^0-9]");

            UI.Button("Spawn", () => SpawnEnemy(enemySetup, int.Parse(s_spawnAmount)));
        }

        private Enemy GetSelectedEnemy()
        {
            return GameObjectManager.enemies.FirstOrDefault(x => x.GetInstanceID() == selectedEnemy);
        }

        private List<EnemySetup> GetEnemies()
        {
            HashSet<EnemySetup> enemies = new HashSet<EnemySetup>();
            List<EnemyParent> enemiesDifficulty1 = EnemyDirector.instance.enemiesDifficulty1.Where(o => o != null && o.GetEnemyParent() != null).Select(o => o.GetEnemyParent()).ToList();
            List<EnemyParent> enemiesDifficulty2 = EnemyDirector.instance.enemiesDifficulty2.Where(o => o != null && o.GetEnemyParent() != null).Select(o => o.GetEnemyParent()).ToList();
            List<EnemyParent> enemiesDifficulty3 = EnemyDirector.instance.enemiesDifficulty3.Where(o => o != null && o.GetEnemyParent() != null).Select(o => o.GetEnemyParent()).ToList();
            enemiesDifficulty1.Concat(enemiesDifficulty2).Concat(enemiesDifficulty3).Where(ep => !enemies.Any(e => e.GetEnemyParent()?.enemyName == ep.enemyName)).ToList().ForEach(ep =>
            {
                EnemySetup EnemySetup = EnemyDirector.instance.enemiesDifficulty1.FirstOrDefault(o => o.GetEnemyParent() == ep) ?? EnemyDirector.instance.enemiesDifficulty2.FirstOrDefault(o => o.GetEnemyParent() == ep) ?? EnemyDirector.instance.enemiesDifficulty3.FirstOrDefault(o => o.GetEnemyParent() == ep);
                if (EnemySetup != null) enemies.Add(EnemySetup);
            });
            return enemies.ToList();
        }

        private void SpawnEnemy(EnemySetup enemy, int amount)
        {
            if (LevelGenerator.Instance == null || enemy == null) return;
            RoomVolume roomVolume = Object.FindObjectsOfType<RoomVolume>().FirstOrDefault(i => i.Truck);
            if (roomVolume?.transform == null) return;
            LevelPoint levelPoint = LevelGenerator.Instance.LevelPathPoints.OrderByDescending(p => Vector3.Distance(p.transform.position, roomVolume.transform.position)).FirstOrDefault();
            if (levelPoint?.transform == null) return;
            for (int i = 0; i < amount; i++) LevelGenerator.Instance.Reflect().InvokeCustom("EnemySpawn", BindingFlags.Instance | BindingFlags.NonPublic, enemy, levelPoint.transform.position);
        }
    }
}
