using Assets.Enemies;
using Assets.Towers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.ToolTip.Implementation {
    public class EcoBuildingTooltipInfo : ToolTipInfo {

        public PlayerBuilding.EcoBuilding.EcoBuilding building;


        public override Color GetColor() => Color.white;

        public override string GetToolTipText() {

            return string.Format(
                "<b>{0}</b>\n" +
                "cost:  {1}\n" +
                "aditional info:\n" +
                "{2}\n",
                building.name,
                building.cost,
                "Im a placeholder!"
            );
        }

        public override bool ShowToolTip() => building != null;
    }
}
