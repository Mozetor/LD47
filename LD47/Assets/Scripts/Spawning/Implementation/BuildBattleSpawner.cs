using System;
using System.Linq;
using UnityEngine;

namespace Assets.WaveSpawner.Implementation {
    public class BuildBattleSpawner : WaveSpawner {

        public float buildPhaseTime;

        private float timeInBuild;

        private Action<float> onBuildTimeChange;
        private Action onBuildPhaseStart;
        private Action onBuildPhaseEnd;
        private Action onBattlePhaseStart;
        private Action onBattlePhaseEnd;

        public void AddOnBuildTimeChange(Action<float> onChange) {
            onBuildTimeChange += onChange;
        }

        public void AddOnBuildPhaseStart(Action onStart) {
            onBuildPhaseStart += onStart;
        }

        public void AddOnBuildPhaseEnd(Action onEnd) {
            onBuildPhaseEnd += onEnd;
        }

        public void AddOnBattlePhaseStart(Action onStart) {
            onBattlePhaseStart += onStart;
        }

        public void AddOnBattlePhaseEnd(Action onEnd) {
            onBattlePhaseEnd += onEnd;
        }

        protected override void StartInternal() {
            timeInBuild = buildPhaseTime;
            stopped = true;
            onBuildTimeChange?.Invoke(timeInBuild);
            onBuildPhaseStart?.Invoke();
        }

        protected override void UpdateInternal() {
            if (!stopped)
                return;

            timeInBuild -= Time.deltaTime;
            if (timeInBuild <= 0) {
                timeInBuild = 0;
                stopped = false;
                onBuildPhaseEnd?.Invoke();
                onBattlePhaseStart?.Invoke();
            }
            onBuildTimeChange?.Invoke(timeInBuild);
        }

        protected override Wave GetInfiniteAdjustedWave(Wave wave, int repetition) =>
            new Wave(
                wave.name,
                wave.spawnGroups
                    .Select(g => g * repetition)
                    .ToArray(),
                wave.minSimultaneousSpawns,
                wave.maxSimultaneousSpawns,
                wave.timeBetweenSpawns,
                wave.spawnDelay
            );


        protected override void RegisterCallbacks() {
            AddOnWaveEnded(StartBuildTime);
        }

        private void StartBuildTime() {
            stopped = true;
            timeInBuild = buildPhaseTime;
            onBattlePhaseEnd?.Invoke();
            onBuildPhaseStart?.Invoke();
            onBuildTimeChange?.Invoke(timeInBuild);
        }
    }
}