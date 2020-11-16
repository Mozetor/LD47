using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Economy;
using City;

namespace PlayerBuilding.EcoBuilding {
    public class EcoBuilding : MonoBehaviour, IPlaceable {
        /// <summary> Building name </summary>
        public new string name;
        /// <summary> Building Cost </summary>
        public int cost;

        /// <summary> Amount of resources </summary>
        public int resourceAmount;
        /// <summary> Type of resource </summary>
        public ResourceType resourceType;

        private void Produce(int amount) {

            switch (resourceType) {
                case ResourceType.MONEY:
                    FindObjectOfType<CityController>().UpgradeIncome(amount);
                    break;
                case ResourceType.WOOD:
                    Debug.LogWarning("Wood not implemented!");
                    break;
                default:
                    throw new System.ArgumentException("ResourceType " + resourceType + " not found");
            }
        }

        #region IPlaceableImplementation
        public int GetCost() {
            return cost;
        }

        public GameObject GetObject() {
            return this.gameObject;
        }

        public void FinishPlacement(GameObject placedObject) {
            PlayerBuildingPlacer.AddBuilding(placedObject);
            Produce(resourceAmount);
        }
        public void PrepareRemoval() {
            Produce(-resourceAmount);
        }

        public bool CanUpgrade() {
            throw new System.NotImplementedException();
        }

        public void Upgrade() {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}