using Assets.WaveSpawner.Implementation;
using Stats;
using System;
using TMPro;
using UnityEngine;
using Utils;
using Economy;


namespace City {

    public class CityController : MonoBehaviour {

        [Header("Variables")]
        [SerializeField]
        [Tooltip("Current health amount")]
        private int health = 100;
        [SerializeField]
        [Tooltip("Current resource amount")]
        private BuildResource[] resources = new BuildResource[Enum.GetNames(typeof(ResourceType)).Length];
        [SerializeField]
        [Tooltip("Current resource income")]
        private BuildResource[] resourceIncome = new BuildResource[Enum.GetNames(typeof(ResourceType)).Length];

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
            spawner.AddOnBuildPhaseStart(() => AddResource(resourceIncome));
            spawner.AddOnBuildPhaseStart(() => StatsController.stats.roundsSurvived++);
            UpdateUI();
            StatsController.ResetStats();
        }


        /// <summary>
        /// Increses income.
        /// </summary>
        public void IncreaseIncome(BuildResource increaseIncome) {
            for (int i = 0; i < resourceIncome.Length; i++) {
                if (increaseIncome.resourceType == resourceIncome[i].resourceType) {
                    resourceIncome[i].resourceAmount += increaseIncome.resourceAmount;
                    UpdateUI();
                    break;
                }
            }

        }

        /// <summary>
        /// Reduces income.
        /// </summary>
        public void DecreaseIncome(BuildResource decreseIncome) {
            for (int i = 0; i < resourceIncome.Length; i++) {
                if (decreseIncome.resourceType == resourceIncome[i].resourceType) {
                    resourceIncome[i].resourceAmount -= decreseIncome.resourceAmount;
                    UpdateUI();
                    break;
                }
            }

        }
        #region ResourceManagement

        /// <summary>
        /// Buy something for given amount if possible.
        /// </summary>
        /// <param name="resourceCost"> Money cost </param>
        /// <returns> Whether is was possible to buy </returns>
        public bool Buy(BuildResource[] resourceCost) {
            bool canBuy = true;
            for (int i = 0; i < resourceCost.Length; i++) {
                if (!CanBuyByCost(resourceCost[i])) {
                    canBuy = false;
                }
            }
            if (canBuy) {
                for (int i = 0; i < resourceCost.Length; i++) {
                    SubtractResource(resourceCost[i]);
                }
                return true;
            }
            else {
                return false;
            }
        }
        /// <summary>
        /// Whether resource is enough to buy something.
        /// </summary>
        /// <param name="moneyCost"></param>
        /// <returns> Whether enought resources are avaible </returns>
        public bool CanBuyByCost(BuildResource resourceCost) {
            for (int i = 0; i < resources.Length; i++) {
                if (resources[i].resourceType == resourceCost.resourceType) {
                    if (resources[i].resourceAmount >= resourceCost.resourceAmount) {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
            throw new System.NotImplementedException("resource " + resourceCost.resourceType + " not found");
        }
        /// <summary>
        /// Whether resource is enough to buy something.
        /// </summary>
        /// <param name="resourceCost"></param>
        /// <returns> Whether enought resources are avaible </returns>
        public bool CanBuyByCost(BuildResource[] resourceCost) {
            bool canBuy = true;
            for (int i = 0; i < resourceCost.Length; i++) {
                if (!CanBuyByCost(resourceCost[i])) {
                    canBuy = false;
                }
            }
            return canBuy;
        }
        /// <summary>
        /// Adds given amount of resource, use other method to subtract
        /// </summary>
        /// <param name="resource"> resource, that should be added </param>
        /// <returns> Return false if negative amount is given </returns>
        public bool AddResource(BuildResource resource) {
            if (resource.resourceAmount < 0) {
                return false;
            }
            for (int i = 0; i < resources.Length; i++) {
                if (resources[i].resourceType == resource.resourceType) {
                    resources[i].resourceAmount += resource.resourceAmount;
                    UpdateUI();
                    return true;
                }
            }
            throw new System.NotImplementedException("resource " + resource.resourceType + " not found");
        }
        /// <summary>
        /// Adds given amount of resources, use other method to subtract
        /// </summary>
        /// <param name="resource"> resource, that should be added </param>
        /// <returns> Return false if negative amount is given </returns>
        public void AddResource(BuildResource[] resource) {
#if UNITY_EDITOR
            // checks input value error, editor only
            for (int i = 0; i < resource.Length; i++) {
                if (resource[i].resourceAmount < 0) {
                    throw new System.ArgumentException("cant add negative values, use SubtractResource");
                }
            }
#endif
            for (int i = 0; i < resource.Length; i++) {
                AddResource(resource[i]);
            }
        }
        /// <summary>
        /// Adds given amount of resource, use other method to subtract
        /// </summary>
        /// <param name="resource"> resource, that should be added </param>
        /// <param name="modifier"> multiplier added on resource amount, can be negative</param>
        /// <returns> Return false if negative amount is given </returns>
        public bool AddResource(BuildResource resource, float modifier) {
            if (resource.resourceAmount < 0) {
                return false;
            }
            for (int i = 0; i < resources.Length; i++) {
                if (resources[i].resourceType == resource.resourceType) {
                    resources[i].resourceAmount += (Mathf.RoundToInt(resource.resourceAmount * modifier));
                    UpdateUI();
                    return true;
                }
            }
            throw new System.NotImplementedException("resource " + resource.resourceType + " not found");
        }
        /// <summary>
        /// Adds given amount of resource, use other method to subtract
        /// </summary>
        /// <param name="resource"> resource, that should be added </param>
        /// <param name="modifier"> multiplier added on resource amount, can be negative</param>
        /// <returns> Return false if negative amount is given </returns>
        public void AddResource(BuildResource[] resource, float modifier) {
#if UNITY_EDITOR
            // checks input value error, editor only
            for (int i = 0; i < resource.Length; i++) {
                if (resource[i].resourceAmount < 0) {
                    throw new System.ArgumentException("cant add negative values, use SubtractResource");
                }
            }
#endif
            for (int i = 0; i < resource.Length; i++) {
                AddResource(resource[i], modifier);
            }
        }
        /// <summary>
        /// Subtracts given amount of resource, wont subtract if no resources are avaible and return false
        /// </summary>
        /// <param name="resource"> resource, that should be subtracted </param>
        /// <returns> Whether enought resources are avaible </returns>
        public bool SubtractResource(BuildResource resource) {
            for (int i = 0; i < resources.Length; i++) {
                if (resources[i].resourceType == resource.resourceType) {
                    if (resources[i].resourceAmount >= +resource.resourceAmount) {
                        resources[i].resourceAmount -= resource.resourceAmount;
                        UpdateUI();
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
            throw new System.NotImplementedException("resource " + resource.resourceType + " not found");
        }
        #endregion
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
        /// Update all values displayed in the UI.
        /// </summary>
        private void UpdateUI() {
            if (!moneyText || !incomeText || !healthText) {
                throw new ArgumentNullException("Some UI text components are not assigned!");
            }
            moneyText.text = resources[(int)ResourceType.MONEY].resourceAmount.ToString();
            incomeText.text = resourceIncome[(int)ResourceType.MONEY].resourceAmount.ToString();
            healthText.text = health.ToString();
        }
    }
}