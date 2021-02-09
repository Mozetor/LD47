using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBuilding.Towers {
    [System.Serializable]
    public class TowerDamageData {
        /// <summary> Tower damage </summary>
        public int damage;
        /// <summary> Tower range </summary>
        public float range;
        /// <summary> Time between attacks </summary>
        public float attackCooldown;
    }
}