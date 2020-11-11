using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBuilding {
    public interface IPlaceable {
        int GetCost();
        GameObject GetObject();
        void FinishPlacement(GameObject placedObject);
    }
}