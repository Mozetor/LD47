using Assets.WaveSpawner.Implementation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Utils {

    public class DayNightCycleController : MonoBehaviour {

        public float duration;
        public List<Light2D> dayLights = new List<Light2D>();
        public List<Light2D> nightLights = new List<Light2D>();

        private BuildBattleSpawner spawner;

        private void Start() {
            spawner = FindObjectOfType<BuildBattleSpawner>();
            dayLights.ForEach((l) => spawner.AddOnBuildPhaseStart(() => StartCoroutine(Cycle(l, true))));
            dayLights.ForEach((l) => spawner.AddOnBattlePhaseStart(() => StartCoroutine(Cycle(l, false))));

            nightLights.ForEach((l) => spawner.AddOnBuildPhaseEnd(() => StartCoroutine(Cycle(l, true))));
            nightLights.ForEach((l) => spawner.AddOnBattlePhaseEnd(() => StartCoroutine(Cycle(l, false))));
        }

        IEnumerator Cycle(Light2D light, bool on) {
            if (light != null) {
                float currentDuration = 0;
                while (currentDuration < duration) {
                    float value = Mathf.Lerp(0, 1, on ? currentDuration / duration : (duration - currentDuration) / duration);
                    light.intensity = Mathf.Clamp(value, .05f, 1f);
                    currentDuration += Time.deltaTime;
                    yield return null;
                }
            }
        }

        public void AddNightLight(Light2D l) {
            spawner.AddOnBuildPhaseEnd(() => StartCoroutine(Cycle(l, true)));
            spawner.AddOnBattlePhaseEnd(() => StartCoroutine(Cycle(l, false)));
        }

    }
}