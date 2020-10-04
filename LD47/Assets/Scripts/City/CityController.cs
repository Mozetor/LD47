using Assets.WaveSpawner.Implementation;
using Stats;
using System;
using TMPro;
using UnityEngine;
using Utils;

namespace City {

    public class CityController : MonoBehaviour {

        [Header("Variables")]
        [SerializeField]
        [Tooltip("Current money amount")]
        private int money = 100;
        [SerializeField]
        [Tooltip("Current health amount")]
        private int health = 100;
        [SerializeField]
        [Tooltip("Current money income")]
        private int moneyIncome = 100;
        [SerializeField]
        [Tooltip("Money increase of upgrade")]
        private int upgradeIncomeIncrease = 10;
        [SerializeField]
        [Tooltip("Money cost to upgrade income")]
        private int upgradeCost = 50;

        [Header("UI")]
        [Tooltip("Text to display money")]
        public TextMeshProUGUI moneyText;
        [Tooltip("Text to display income")]
        public TextMeshProUGUI incomeText;
        [Tooltip("Text to display health")]
        public TextMeshProUGUI healthText;
        [Tooltip("Text to display upgrade income increase")]
        public TextMeshProUGUI upgradeIncomeIncreaseText;
        [Tooltip("Text to display upgrade cost")]
        public TextMeshProUGUI upgradeCostText;

        private void Awake() {
            var spawner = FindObjectOfType<BuildBattleSpawner>();
            spawner.AddOnBuildPhaseStart(AddMoney);
            spawner.AddOnBuildPhaseStart(() => StatsController.stats.roundsSurvived++);
            UpdateUI();
            StatsController.ResetStats();
        }


        /// <summary>
        /// Upgrade income if possible.
        /// </summary>
        public void UpgradeIncome() {
            if (Buy(upgradeCost)) {
                StatsController.stats.moneyUsedForIncome += upgradeCost;
                moneyIncome += upgradeIncomeIncrease;
                UpdateUI();
            }
        }
        /// <summary>
        /// Buy something for given amount if possible.
        /// </summary>
        /// <param name="moneyCost"> Money cost </param>
        /// <returns> Whether is was possible to buy </returns>
        public bool Buy(int moneyCost) {
            if (CanBuy(moneyCost)) {
                money -= moneyCost;
                UpdateUI();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Whether money is enough to buy something
        /// </summary>
        /// <param name="moneyCost"></param>
        /// <returns></returns>
        public bool CanBuy(int moneyCost) => money >= moneyCost;
        /// <summary>
        /// Damages the city with given amount.
        /// </summary>
        /// <param name="damage"> Amount of damage </param>
        public void Damage(int damage) {
            health -= damage;
            UpdateUI();
            if (health <= 0) {
                FindObjectOfType<SceneController>().ChangeScene("GameOver");
            }
        }


        /// <summary>
        /// Add income to current money.
        /// </summary>
        private void AddMoney() {
            money += moneyIncome;
            StatsController.stats.allMoneyMade += moneyIncome;
            UpdateUI();
        }
        /// <summary>
        /// Update all values displayed in the UI.
        /// </summary>
        private void UpdateUI() {
            if (!moneyText || !incomeText || !healthText || !upgradeIncomeIncreaseText || !upgradeCostText) {
                throw new ArgumentNullException("Some UI text components are not assigned!");
            }
            moneyText.text = money.ToString();
            incomeText.text = moneyIncome.ToString();
            healthText.text = health.ToString();
            upgradeIncomeIncreaseText.text = upgradeIncomeIncrease.ToString();
            upgradeCostText.text = upgradeCost.ToString();
        }
    }
}