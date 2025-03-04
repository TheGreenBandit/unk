using UnityEngine;
using Unk.Manager;
using Unk.Menu.Core;
using Unk.Util;

namespace Unk.Menu.Tab
{
    internal class EnemyTab : MenuTab
    {
        public EnemyTab() : base("Enemies") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 scrollPos2 = Vector2.zero;
        public static Enemy selectedEnemy;
        int damage = 0;
        int heal = 0;
        float freeze = 1;

        public override void Draw()
        {
            GUILayout.BeginVertical(GUILayout.Width(UnkMenu.Instance.contentWidth * 0.3f - UnkMenu.Instance.spaceFromLeft));
            LivingEnemyList();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(UnkMenu.Instance.contentWidth * 0.7f - UnkMenu.Instance.spaceFromLeft));
            scrollPos2 = GUILayout.BeginScrollView(scrollPos2);
            GeneralActions();
            EnemyActions();
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
            UI.Header("Selected Monster Actions");
            EnemyHealth health = selectedEnemy.Reflect().GetValue<EnemyHealth>("Health");
            UI.Button("Kill", () => health.Reflect().Invoke("Death", new Vector3(0, 0, 0)));
            UI.TextboxAction("Damage", ref damage, 3,
                new UIButton("Amount", () => health.Reflect().Invoke("Hurt", damage, new Vector3(0, 0, 0)))
            );
            UI.TextboxAction("Heal", ref damage, 3,
                new UIButton("Amount", () => health.Reflect().Invoke("Hurt", -heal, new Vector3(0, 0, 0)))
            );
            UI.TextboxAction("Freeze", ref freeze, 3,
                new UIButton("Time", () => selectedEnemy.Freeze(freeze))
            );
        }

        private void LivingEnemyList()
        {
            float width = UnkMenu.Instance.contentWidth * 0.3f - UnkMenu.Instance.spaceFromLeft * 2;
            float height = UnkMenu.Instance.contentHeight - 50;

            Rect rect = new Rect(0, 30, width, height);
            GUI.Box(rect, "Enemy List");

            GUILayout.BeginVertical(GUILayout.Width(width), GUILayout.Height(height));

            GUILayout.Space(25);
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            foreach (Enemy enemy in GameObjectManager.enemies)
            {
                if (selectedEnemy is null) selectedEnemy = enemy;

                if (selectedEnemy.GetInstanceID() == enemy.GetInstanceID()) GUI.contentColor = Settings.c_espEnemies.GetColor();

                if (GUILayout.Button(enemy.GetName(), GUI.skin.label)) selectedEnemy = enemy;

                GUI.contentColor = Settings.c_menuText.GetColor();
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}
