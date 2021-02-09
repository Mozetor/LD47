using City;
using Economy;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Utils;

namespace PlayerBuilding {

    public abstract class Building : MonoBehaviour, IPlaceable<Building>, IUpgradeable, IDamageable {

        /// <summary> Building name </summary>
        public new string name;
        /// <summary> Cost to contruct and upgrade a building </summary>
        public BuildCost[] buildCost;
        /// <summary> Cost maintain functionality of a building </summary>
        public UpkeepCost[] upkeepCost;
        /// <summary> Current level of building </summary>
        protected int buildingLevel;
        /// <summary> Current health points of building </summary>
        protected int Hp;
        /// <summary> Maximum health points of building </summary>
        public int HpMax;

        private void Awake() {
            FindObjectOfType<DayNightCycleController>().AddNightLight(gameObject.GetComponent<Light2D>());
        }

        public bool CanUpgrade() {
            return buildingLevel + 1 < buildCost.Length && FindObjectOfType<CityController>().CanBuyByCost(buildCost[buildingLevel + 1].ResourceCost);
        }

        public int GetBuildingLevel() {
            return buildingLevel;
        }

        public BuildResource[] GetCost() {
            return buildCost[buildingLevel].ResourceCost;
        }

        public string GetName() {
            return name;
        }

        public GameObject GetObject() {
            return gameObject;
        }

        public bool IsMaxUpgrade() {
            return buildingLevel + 1 == buildCost.Length;
        }

        public abstract void PrepareRemoval();

        public abstract void Upgrade();

        public void Damage(int damage) {
            Hp = Mathf.Min(Hp - damage, HpMax);
            if (Hp <= 0) {
                FindObjectOfType<ConstructionController>().RemoveBuilding(this);
            }
        }

        public int GetHealth() {
            return Hp;
        }

        public List<Vector2Int> GetOffsetPositions() {
            return new List<Vector2Int>() { Vector2Int.zero };
        }

        public Building GetT() => this;
    }
}