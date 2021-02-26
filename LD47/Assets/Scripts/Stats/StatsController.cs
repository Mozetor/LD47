using Assets.WaveSpawner;
using Assets.WaveSpawner.Implementation;
using Enemies;
using System;
using UnityEngine;

namespace Stats {

    public class StatsController : MonoBehaviour {
        private static StatsController instance;
        public static CountStats stats;

        private void Awake() {
            if (instance != null) {
                Destroy(this);
            }
            instance = this;
            DontDestroyOnLoad(this);
            var spawner = FindObjectOfType<BuildBattleSpawner>();
            spawner.AddOnEntitySpawned(s => s.AddOnDeath(e => AddEnemyKill(e)));
        }

        public static void ResetStats() {
            stats = new CountStats();
        }

        private void AddEnemyKill(Spawnable spawnable) {
            NavEnemy2D e = (NavEnemy2D)spawnable;
            stats.enemiesKilled++;
            switch (e.settings.enemyName) {
                case "melee":
                    stats.meleeEnemiesKilled++;
                    break;
                case "ranged":
                    stats.rangedEnemiesKilled++;
                    break;
                case "flying":
                    stats.flyingEnemiesKilled++;
                    break;
                case "suicide":
                    stats.suicideEnemiesKilled++;
                    break;
                case "tank":
                    stats.tankEnemiesKilled++;
                    break;
                default:
                    throw new NotImplementedException("Enemy name: " + e.settings.enemyName + " not implemented!");
            }
        }
    }
}