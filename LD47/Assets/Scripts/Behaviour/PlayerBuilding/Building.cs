using City;
using Economy;
using PlayerBuilding;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Utils;

public abstract class Building : MonoBehaviour, IPlaceable, IDamageable {

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
        if (buildingLevel + 1 < buildCost.Length) {
            if (FindObjectOfType<CityController>().CanBuyByCost(buildCost[buildingLevel + 1].ResourceCost)) {
                return true;
            }
            else return false;
        }
        else return false;
    }

    public abstract void FinishPlacement(GameObject placedObject);

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
        return this.gameObject;
    }

    public bool IsMaxUpgrade() {
        return (buildingLevel + 1 == buildCost.Length) ? true : false;
    }

    public abstract void PrepareRemoval();

    public abstract void Upgrade();

    public void Damage(int damage) {
        Hp -= damage;
        if (Hp > HpMax) {
            Hp = HpMax;
        }
        if (Hp <= 0) {
            Debug.LogWarning("building destruction not fully implemented!");
            // removal from list not implemented
            PrepareRemoval();
        }
    }

    public int GetHp() {
        return Hp;
    }
}
