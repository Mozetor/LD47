using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Stats {

    public class StatsUIController : MonoBehaviour {

        public UIStats uiStats;

        private void Start() {
            CountStats s = StatsController.stats;
            uiStats.roundsSurvived.text = s.roundsSurvived.ToString();
            uiStats.allMoneyMade.text = s.allMoneyMade.ToString();
            uiStats.allMoneyUsed.text = s.allMoneyUsed.ToString();
            uiStats.moneyUsedForTurret.text = s.moneyUsedForTurret.ToString();
            uiStats.moneyUsedForIncome.text = s.moneyUsedForIncome.ToString();
            uiStats.enemiesKilled.text = s.enemiesKilled.ToString();
            uiStats.meleeEnemiesKilled.text = s.meleeEnemiesKilled.ToString();
            uiStats.rangedEnemiesKilled.text = s.rangedEnemiesKilled.ToString();
            uiStats.suicideEnemiesKilled.text = s.suicideEnemiesKilled.ToString();
            uiStats.flyingEnemiesKilled.text = s.suicideEnemiesKilled.ToString();
        }

    }
}