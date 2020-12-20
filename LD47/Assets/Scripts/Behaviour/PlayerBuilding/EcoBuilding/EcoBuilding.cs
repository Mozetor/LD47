using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Economy;
using City;
using Utils;
using UnityEngine.Experimental.Rendering.Universal;

namespace PlayerBuilding.EcoBuilding {
    public class EcoBuilding : MonoBehaviour, IPlaceable {
        /// <summary> Building name </summary>
        public new string name;
        /// <summary> Cost to sontruct and upgrade a building </summary>
        public BuildCost[] buildCost;
        /// <summary> Cost maintain functionality of a building </summary>
        public UpkeepCost[] upkeepCost;
        /// <summary> Amount of resources added by this building, by level </summary>
        public BuildResource[] resourceGenerated;

        /// <summary> Current level of building </summary>
        private int buildingLevel;

        private void Awake() {
            FindObjectOfType<DayNightCycleController>().AddNightLight(gameObject.GetComponent<Light2D>());
            if (buildCost.Length != resourceGenerated.Length) {
                throw new System.ArgumentException("Upgrade arrays must have same lenght!");
            }
            if (upkeepCost.Length != 0) {
                if (upkeepCost.Length != buildCost.Length) {
                    throw new System.ArgumentException("Upgrade arrays must have same lenght!");
                }
            }
        }

        #region IPlaceableImplementation

        public string GetName() {
            return name;
        }

        public BuildResource[] GetCost() {
            return buildCost[buildingLevel].ResourceCost;
        }

        public GameObject GetObject() {
            return this.gameObject;
        }

        public void FinishPlacement(GameObject placedObject) {
            PlayerBuildingPlacer.AddBuilding(placedObject);
            FindObjectOfType<CityController>().IncreaseIncome(resourceGenerated[buildingLevel]);
        }

        public void PrepareRemoval() {
            FindObjectOfType<CityController>().DecreaseIncome(resourceGenerated[buildingLevel]);
            for (int i = 0; i < upkeepCost[buildingLevel].BalanceCost.Length; i++) {
                BalanceResource bal = upkeepCost[buildingLevel].BalanceCost[i];
                bal.resourceAmount = -bal.resourceAmount;
                FindObjectOfType<CityController>().StrainBalance(bal);
            }
        }

        public bool CanUpgrade() {
            if (buildingLevel + 1 < buildCost.Length) {
                if (FindObjectOfType<CityController>().CanBuyByCost(buildCost[buildingLevel + 1].ResourceCost)) {
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public void Upgrade() {
            if (buildingLevel + 1 >= buildCost.Length) {
                throw new System.ArgumentException("Tried to upgrade past maximum building level");
            }
            // change spirte
            CityController city = FindObjectOfType<CityController>();
            city.Buy(buildCost[buildingLevel + 1].ResourceCost);
            city.DecreaseIncome(resourceGenerated[buildingLevel]);
            for (int i = 0; i < upkeepCost[buildingLevel].BalanceCost.Length; i++) {
                BalanceResource bal = upkeepCost[buildingLevel].BalanceCost[i];
                bal.resourceAmount = -bal.resourceAmount;
                FindObjectOfType<CityController>().StrainBalance(bal);
            }
            buildingLevel++;
            FindObjectOfType<CityController>().StrainBalance(upkeepCost[buildingLevel].BalanceCost);
            city.IncreaseIncome(resourceGenerated[buildingLevel]);
        }

        public bool IsMaxUpgrade() {
            return (buildingLevel + 1 == buildCost.Length) ? true : false;
        }

        public int GetBuildingLevel() {
            return buildingLevel;
        }
        #endregion
    }
}