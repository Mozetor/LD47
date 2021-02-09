using Buildings.Economy;
using Buildings.Resources;
using UnityEngine;

namespace Assets.ToolTip.Implementation {
    public class EcoBuildingTooltipInfo : ToolTipInfo {

        public EcoBuilding building;


        public override Color GetColor() => Color.white;

        public override string GetToolTipText() {

            return string.Format(
                "<b>{0}</b>\n" +
                "cost:\n{1}" +
                "Generates:\n{2}",
                building.name,
                CostArrayToString(building.buildCost[0].ResourceCost),
                building.resourceGenerated[0].resourceType + ": " +
                building.resourceGenerated[0].resourceAmount + "\n"
            );
            ;
        }

        private string CostArrayToString(BuildResource[] cost) {
            string s = "";
            for (int i = 0; i < cost.Length; i++) {
                s += cost[i].resourceType + ": ";
                s += cost[i].resourceAmount + "\n";
            }
            return s;
        }

        public override bool ShowToolTip() => building != null;
    }
}
