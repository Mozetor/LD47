using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.WaveSpawner {
    /// <summary>
    /// This is a base class for wave spawner. Extend this to add callbacks to adapt the behaviour.
    /// </summary>
    public class WaveSpawner : MonoBehaviour {

        /// <summary> The data describing a set of waves. </summary>
        [Tooltip("The data describing a set of waves.")]
        public WaveData waves;
        /// <summary> The positions, where the entities are spawned. </summary>
        [Tooltip("The positions where the entities are spawned.")]
        public List<Vector3> spawnPoints;
        /// <summary> Will the waves loop forever? </summary>
        [Tooltip("Will the waves loop forever?")]
        public bool infinite;
        /// <summary> Is the spawning of enitities stoppped? </summary>
        protected bool stopped = false;
        /// <summary> The entities currently alive. </summary>
        private List<Spawnable> spawnedEnitities = new List<Spawnable>();
        /// <summary> The wave, that is currently being spawned. </summary>
        private Wave spawnWave;
        /// <summary> The round of waves. </summary>
        private int factor = 1;
        /// <summary> The index of the next wave to be spawned. </summary>
        private int index = 0;
        /// <summary> Have all enemies of the current wave been spawned? </summary>
        private bool allSpawned = true;
        /// <summary> The time until the next group is spawned. </summary>
        private float timeToNextSpawn = 0;
        /// <summar> The countdown to starting the next wave. </summar>
        private float waveCooldown;

        #region callback variables
        /// <summary> This callback is called, when a new wave is starting. </summary>
        private Action<Wave> onWaveStarted;
        /// <summary> This callback is called, when a wave finished. </summary>
        private Action<Wave> onWaveEnded;
        /// <summary> This callback is called, when all waves finished. (Called multiple times if infinite.) </summary>
        private Action onWavesFinished;
        /// <summary> This callback is called, when an entity is spawned. </summary>
        private Action<Spawnable> onEntitySpawned;
        /// <summary> This callback is called, when the countdown is updated. </summary>
        private Action<float, bool> onCountdownUpdated;
        #endregion callback variables

        #region callback registers
        public void AddOnWaveStarted(Action action) {
            onWaveStarted += _ => action();
        }

        public void AddOnWaveStarted(Action<Wave> action) {
            onWaveStarted += action;
        }

        public void AddOnWaveEnded(Action action) {
            onWaveEnded += _ => action();
        }

        public void AddOnWaveEnded(Action<Wave> action) {
            onWaveEnded += action;
        }

        public void AddOnWavesFinished(Action action) {
            onWavesFinished += action;
        }

        public void AddOnEntitySpawned(Action action) {
            onEntitySpawned += _ => action();
        }

        public void AddOnEntitySpawned(Action<Spawnable> action) {
            onEntitySpawned += action;
        }

        public void AddOnCountdownUpdated(Action action) {
            onCountdownUpdated += (_, __) => action();
        }

        public void AddOnCountdownUpdated(Action<float> action) {
            onCountdownUpdated += (f, _) => action(f);
        }

        public void AddOnCountdownUpdated(Action<float, bool> action) {
            onCountdownUpdated += action;
        }
        #endregion callback registers

        /// <summary>
        /// Use this method to add callbacks.
        /// (For example to stop spawning, when the player dies.)
        /// </summary>
        protected virtual void RegisterCallbacks() { }

        /// <summary>
        /// Adjusts a given wave for the use in an infinite spawner.
        /// Creates a new wave out of the given values. (IMPORTANT.)
        /// </summary>
        /// <param name="wave"> The wave as defined via the inspector. </param>
        /// <param name="repetition"> The number of times this wave is spawned. (Starting at 1)</param>
        /// <returns> The adjusted wave. </returns>
        protected virtual Wave GetInfiniteAdjustedWave(Wave wave, int repetition) => wave * repetition;

        private void Start() {
            waveCooldown = waves.waves[index].spawnDelay;
            onCountdownUpdated?.Invoke(waveCooldown, false);
            RegisterCallbacks();
            StartInternal();
        }

        protected virtual void StartInternal() { }

        private void Update() {
            UpdateInternal();

            if (stopped) {
                return;
            }
            if (index != -1) {
                waveCooldown = allSpawned && spawnedEnitities.Count == 0 ? waveCooldown - Time.deltaTime : waves.waves[index].spawnDelay;

                if (waveCooldown < 0) {
                    StartWave();
                }
            }

            timeToNextSpawn -= Time.deltaTime;
            onCountdownUpdated?.Invoke(waveCooldown, spawnedEnitities.Count != 0);

            if (allSpawned || timeToNextSpawn > 0) {
                return;
            }

            SpawnGroup();
        }

        protected virtual void UpdateInternal() { }

        /// <summary>
        /// Start the next wave.
        /// </summary>
        private void StartWave() {
            allSpawned = false;
            var wave = GetInfiniteAdjustedWave(waves.waves[index], factor);
            spawnedEnitities = new List<Spawnable>();
            spawnWave = wave;
            onWaveStarted?.Invoke(wave);
            index = (index + 1) % waves.waves.Length;
            if (index == 0 && !infinite) {
                index = -1;
            }
            else {
                waveCooldown = waves.waves[index].spawnDelay;
            }
        }

        /// <summary>
        /// Ends a wave.
        /// </summary>
        private void EndWave() {
            if (index == 0) {
                onWavesFinished?.Invoke();
                factor++;
            }

            onWaveEnded?.Invoke(GetInfiniteAdjustedWave(waves.waves[index], factor));

            if (index == -1) {
                onWavesFinished?.Invoke();
                stopped = true;
                return;
            }
        }

        /// <summary>
        /// Handles the behaviour when an entity dies.
        /// </summary>
        /// <param name="killed"> The entity that died. </param>
        private void EntityKilled(Spawnable killed) {
            spawnedEnitities.Remove(killed);
            if (spawnedEnitities.Count == 0 && allSpawned) {
                EndWave();
            }
        }

        /// <summary>
        /// Spawns the next group of the active wave.
        /// </summary>
        private void SpawnGroup() {
            var toBeSpawned = new List<Spawnable>();
            int simultaneousSpawns = UnityEngine.Random.Range(spawnWave.minSimultaneousSpawns, spawnWave.maxSimultaneousSpawns + 1);
            int remaining = simultaneousSpawns;
            allSpawned = true;
            foreach (var group in spawnWave.spawnGroups) {
                int spawns = Mathf.Min(remaining, group.amount);
                remaining -= spawns;
                group.amount -= spawns;
                for (int i = 0; i < spawns; i++) {
                    toBeSpawned.Add(group.spawnable);
                }
                if (group.amount > 0) {
                    allSpawned = false;
                }
            }
            foreach (var enemy in toBeSpawned) {
                if (spawnPoints.Count == 0) {
                    Debug.LogError("No spawnpoints were added. Enemies cannot be spawned.");
                    return;
                }
                var spawnPoint = UnityEngine.Random.Range(0, spawnPoints.Count);
                var spawned = Instantiate(enemy, spawnPoints[spawnPoint], Quaternion.identity, this.transform);
                spawned.AddOnDeath(EntityKilled);
                spawned.SetUp(spawnPoint);
                onEntitySpawned?.Invoke(spawned);
                spawnedEnitities.Add(spawned);
            }
            timeToNextSpawn = spawnWave.timeBetweenSpawns;
        }
    }
}
