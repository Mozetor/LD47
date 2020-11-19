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
        /// <summary> Building Cost </summary>
        public BuildCost[] cost;
        /// <summary> Amount of resources added by this building, by level </summary>
        public BuildResource[] resourceGenerated;

        /// <summary> Current level of building </summary>
        private int buildingLevel;

        private void Awake() {
            FindObjectOfType<DayNightCycleController>().AddNightLight(gameObject.GetComponent<Light2D>());
            if (cost.Length != resourceGenerated.Length) {
                throw new System.ArgumentException("Upgrade arrays must have same lenght!");
            }
        }

        #region IPlaceableImplementation

        public string GetName() {
            return name;
        }

        public BuildResource[] GetCost() {
            return cost[buildingLevel].ResourceCost;
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
        }

        public bool CanUpgrade() {
            if (buildingLevel + 1 < cost.Length) {
                if (FindObjectOfType<CityController>().CanBuyByCost(cost[buildingLevel + 1].ResourceCost)) {
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public void Upgrade() {
            if (buildingLevel + 1 >= cost.Length) {
                throw new System.ArgumentException("Tried to upgrade past maximum building level");
            }
            // change spirte
            CityController city = FindObjectOfType<CityController>();
            city.Buy(cost[buildingLevel + 1].ResourceCost);
            city.DecreaseIncome(resourceGenerated[buildingLevel]);
            buildingLevel++;
            city.IncreaseIncome(resourceGenerated[buildingLevel]);
        }

        public bool IsMaxUpgrade() {
            if (buildingLevel + 1 == cost.Length) {
                return true;
            }
            else return false;
        }

        public int GetBuildingLevel() {
            return buildingLevel;
        }
        #endregion
    }
}