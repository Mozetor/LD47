using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utils {

    public class GameSpeedController : MonoBehaviour {

        public Image speed1, speed2, speed3;

        private void Awake() {
            Time.timeScale = 1f;
            UpdateUI();
        }

        public void SetGameSpeed(float speed) {
            Time.timeScale = speed;
            UpdateUI();
        }

        private void UpdateUI() {
            speed1.color = Time.timeScale == 1 ? Color.green : Color.white;
            speed2.color = Time.timeScale == 2 ? Color.green : Color.white;
            speed3.color = Time.timeScale == 3 ? Color.green : Color.white;
        }
    }
}