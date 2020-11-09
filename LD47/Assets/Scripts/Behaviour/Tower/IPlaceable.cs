using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IPlaceable {
    public int GetCost();
    public GameObject GetObject();
    public void FinishPlacement(GameObject placedObject);
}
