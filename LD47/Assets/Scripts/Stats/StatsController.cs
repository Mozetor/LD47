using Assets.Enemies;
using Assets.WaveSpawner;
using Assets.WaveSpawner.Implementation;
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
            Enemy e = (Enemy)spawnable;
            stats.enemiesKilled++;
            switch (e.enemyName) {
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
                default:
                    throw new NotImplementedException("Enemy name: " + e.enemyName + " not implemented!");
            }
        }
    }
}