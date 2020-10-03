using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlaceholder : MonoBehaviour {

        

    public int cost;
    public GameObject goTower;

    public TowerPlaceholder(int cost, GameObject goTower) {
        this.cost = cost;
        this.goTower = goTower;
    }
}
