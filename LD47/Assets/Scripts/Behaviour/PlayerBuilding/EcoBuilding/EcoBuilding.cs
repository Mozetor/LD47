using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBuilding.EcoBuilding {
    public class EcoBuilding : MonoBehaviour, IPlaceable {

        public new string name;
        public int cost;

        public int GetCost() {
            return cost;
        }

        public GameObject GetObject() {
            return this.gameObject;
        }

        public void FinishPlacement(GameObject placedObject) {
            PlayerBuildingPlacer.AddBuilding(placedObject);
        }
    }
}