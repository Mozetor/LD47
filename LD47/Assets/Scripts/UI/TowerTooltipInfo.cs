using Assets.Enemies;
using Assets.Towers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.ToolTip.Implementation {
    public class TowerTooltipInfo : ToolTipInfo {

        public Tower tower;


        public override Color GetColor() => Color.white;

        public override string GetToolTipText() {
            if (tower.projectile is RocketTowerProjectile rocket) {
                return string.Format(
                    "<b>{0}</b>\n" +
                    "cost:  {1}\n" +
                    "targets: {2}\n" +
                    "range: {3}\n" +
                    "damage: {4}\n" +
                    "firerate: {5:F2}\n" +
                    "explosion radius: {6}",
                    tower.name,
                    tower.cost,
                    TargetsAsString(tower.targets),
                    tower.range,
                    tower.damage,
                    1 / tower.attackCooldown,
                    rocket.explosionRange
                );
            } else {
                return string.Format(
                    "<b>{0}</b>\n" +
                    "cost:  {1}\n" +
                    "targets: {2}\n" +
                    "range: {3}\n" +
                    "damage: {4}\n" +
                    "firerate: {5:F2}",
                    tower.name,
                    tower.cost,
                    TargetsAsString(tower.targets),
                    tower.range,
                    tower.damage,
                    1 / tower.attackCooldown
                );
            }

        }

        public override bool ShowToolTip() => tower != null;

        private string TargetsAsString(List<EnemyType> targets) => 
            targets
                .Distinct()
                .Select(TypeToString)
                .Aggregate((f, s) => $"{f}, {s}");

        private string TypeToString(EnemyType type) {
            switch (type) {
                case EnemyType.GROUNDED:
                    return "Grounded";
                case EnemyType.AIRBORN:
                    return "Airborne";
                default: throw new ArgumentException($"Unknown enemy type '{type}'");
            }
        }
    }
}
