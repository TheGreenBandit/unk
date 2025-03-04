using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
                    DrawList<Enemy>("Enemy List", GameObjectManager.enemies.OrderBy(e => e.GetName()).ToList(), e => e.Reflect().GetValue<EnemyHealth>("Health").Reflect().GetValue<bool>("dead"), e => e.GetName(), ref scrollPos, ref selectedEnemy);
                    break;
                    //case 1:
                    //if (!GameUtil.GetEnemyTypes().Exists(e => e.GetInstanceID() == selectedEnemyType)) selectedEnemyType = -1;
                    //DrawList<EnemyType>("EnemyTab.EnemyTypes", GameUtil.GetEnemyTypes().OrderBy(e => e.name).ToList(), _ => false, e => e.name, ref scrollPos3, ref selectedEnemyType);
                    //break;
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
            UI.Label("Features Coming Soon!");

        }

        private void EnemyActions()
        {
            Enemy enemy = GetSelectedEnemy();

            if (enemy == null) return;

            UI.Header("Selected Monster Actions");
            EnemyHealth health = enemy.Reflect().GetValue<EnemyHealth>("Health");
            if (health != null)
            {
                UI.Button("Kill", () => health.Reflect().Invoke("Death", new Vector3(0, 0, 0)));
                UI.TextboxAction("Damage", ref damage, 3,
                    new UIButton("Amount", () => health.Reflect().Invoke("Hurt", damage, new Vector3(0, 0, 0)))
                );
                UI.TextboxAction("Heal", ref damage, 3,
                    new UIButton("Amount", () => health.Reflect().Invoke("Hurt", -heal, new Vector3(0, 0, 0)))
                );
                UI.TextboxAction("Freeze", ref freeze, 3,
                    new UIButton("Time", () => enemy.Freeze(freeze))
                );
            }
        }

        private void EnemySpawnerContent()
        {
            UI.Header("Enemy Spawner Content");

        }

        private Enemy GetSelectedEnemy()
        {
            return GameObjectManager.enemies.FirstOrDefault(x => x.GetInstanceID() == selectedEnemy);
        }
    }
}
