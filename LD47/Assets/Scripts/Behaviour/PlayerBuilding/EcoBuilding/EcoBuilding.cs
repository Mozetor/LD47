using City;
using Economy;

namespace PlayerBuilding.EcoBuilding {
    public class EcoBuilding : Building {
        /// <summary> Amount of resources added by this building, by level </summary>
        public BuildResource[] resourceGenerated;

        private void Awake() {
            if (buildCost.Length != resourceGenerated.Length) {
                throw new System.ArgumentException("Upgrade arrays must have same lenght!");
            }
            if (upkeepCost.Length != 0) {
                if (upkeepCost.Length != buildCost.Length) {
                    throw new System.ArgumentException("Upgrade arrays must have same lenght!");
                }
            }
        }

        private void Start() {
            FindObjectOfType<CityController>().IncreaseIncome(resourceGenerated[buildingLevel]);
            if (upkeepCost.Length != 0) {
                for (int i = 0; i < upkeepCost[buildingLevel].BalanceCost.Length; i++) {
                    FindObjectOfType<CityController>().StrainBalance(upkeepCost[buildingLevel].BalanceCost[i]);
                }
            }
        }

        public override void PrepareRemoval() {
            FindObjectOfType<CityController>().DecreaseIncome(resourceGenerated[buildingLevel]);
            if (upkeepCost.Length != 0) {
                for (int i = 0; i < upkeepCost[buildingLevel].BalanceCost.Length; i++) {
                    BalanceResource bal = upkeepCost[buildingLevel].BalanceCost[i];
                    bal.resourceAmount = -bal.resourceAmount;
                    FindObjectOfType<CityController>().StrainBalance(bal);
                }
            }
        }

        public override void Upgrade() {
            if (buildingLevel + 1 >= buildCost.Length) {
                throw new System.ArgumentException("Tried to upgrade past maximum building level");
            }
            // change spirte
            CityController city = FindObjectOfType<CityController>();
            city.Buy(buildCost[buildingLevel + 1].ResourceCost);
            city.DecreaseIncome(resourceGenerated[buildingLevel]);
            if (upkeepCost.Length != 0) {
                for (int i = 0; i < upkeepCost[buildingLevel].BalanceCost.Length; i++) {
                    BalanceResource bal = upkeepCost[buildingLevel].BalanceCost[i];
                    bal.resourceAmount = -bal.resourceAmount;
                    FindObjectOfType<CityController>().StrainBalance(bal);
                }
            }
            buildingLevel++;
            if (upkeepCost.Length != 0) {
                FindObjectOfType<CityController>().StrainBalance(upkeepCost[buildingLevel].BalanceCost);
            }
            city.IncreaseIncome(resourceGenerated[buildingLevel]);
        }
    }
}