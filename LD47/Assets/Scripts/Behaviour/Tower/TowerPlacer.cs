using System.Collections.Generic;
using UnityEngine;
using City;
using Assets.WaveSpawner.Implementation;
using System.Linq;
using Stats;

public class TowerPlacer : MonoBehaviour {

    public Tower test;

    public GameObject testIcon;

    /// <summary> Main city </summary>
    private CityController city;
    /// <summary> Information of to placed tower </summary>
    private Tower towerToPlace;
    /// <summary> Main Camera </summary>
    private Camera mainCamera;
    /// <summary> Ghost blueprint to indicate placing position </summary>
    private GameObject towerSilhouette;
    /// <summary> Whenever turret silhouette should be active </summary>
    private bool placingActive;
    /// <summary> Is the game in the build phase </summary>
    private bool inBuildPhase;
    /// <summary> Is the game in the build phase </summary>
    private bool SellingActive;
    /// <summary> towers on map </summary>
    private readonly List<Tower> towers = new List<Tower>();

    /// <summary> Percentage of what will be returned on turret selling, 1=100% </summary>
    [Tooltip("Percentage of what will be returned on turret selling, 1=100%")]
    public float refundOnSell;

    private void Awake() {
        var spawner = FindObjectOfType<BuildBattleSpawner>();
        spawner.AddOnBuildPhaseStart(() => inBuildPhase = true);
        spawner.AddOnBuildPhaseEnd(() => { inBuildPhase = false; CancelTowerPlacement(); });
    }

    // Start is called before the first frame update
    private void Start() {
        mainCamera = Camera.main;
        city = FindObjectOfType<CityController>();
    }

    // Update is called once per frame
    private void Update() {


        if (!inBuildPhase) {
            return;
        }
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (SellingActive && inBuildPhase && Input.GetMouseButton(0) && !placingActive) {
            Vector3 cursorTile = new Vector3(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y), 0);
            SellTurret(cursorTile);
        }

        if (placingActive && inBuildPhase && !SellingActive) {
            MoveSilhouette(mousePosition);
            if (CanPlaceTower(mousePosition) && Input.GetMouseButtonDown(0)) {
                PlaceTower();
            }
        }
    }

    /// <summary> Checks if tower can be placed </summary>
    /// <returns></returns>
    private bool CanPlaceTower(Vector3 screenPosition) {
        if (TestPosition(screenPosition)) {
            if (city.CanBuy(towerToPlace.cost)) {
                ChangeSilhouetteColour(Color.green);
                return true;
            }
            else {
                ChangeSilhouetteColour(Color.yellow);
                return false;
            }
        }
        else {
            ChangeSilhouetteColour(Color.red);
            return false;
        }
    }

    /// <summary> Places tower into map </summary>
    /// <returns></returns>
    private void PlaceTower() {
        Vector3 newPos = new Vector3(towerSilhouette.transform.position.x, towerSilhouette.transform.position.y, 0);
        towers.Add(Instantiate(towerToPlace, newPos, Quaternion.identity));
        StatsController.stats.moneyUsedForTurret += towerToPlace.cost;
        city.Buy(towerToPlace.cost);
    }

    /// <summary> deconstructs turret, returns a part of its cost </summary>
    private void SellTurret(Vector3 targetPosition) {
        if (!(targetPosition == Vector3.zero)) {
            for (int i = 0; i < towers.Count; i++) {
                if (towers[i].transform.position == targetPosition) {
                    city.Buy(Mathf.RoundToInt(-towers[i].cost * refundOnSell));
                    Tower towerToRemove = towers[i];
                    towers.Remove(towerToRemove);
                    Destroy(towerToRemove.gameObject);
                    break;
                }
            }
        }
    }

    /// <summary> Tests if position is viable for turret placement </summary>
    /// <returns></returns>
    private bool TestPosition(Vector3 screenPosition) {
        if (/*!worldMap.IsPath(screenPosition.x, screenPosition.y)*/ true) {
            Debug.LogWarning("turret placer is mising worldMap checks");
            Vector3 cursorTile = new Vector3(Mathf.Round(screenPosition.x), Mathf.Round(screenPosition.y), 0);
            for (int i = 0; i < towers.Count; i++) {
                if (towers[i].transform.position == cursorTile) {
                    return false;
                }
            }
            return true;
        }
        else {
            return false;
        }
    }

    /// <summary> Sets turret silhouette sprite colour </summary>
    /// <param name="color"></param>
    private void ChangeSilhouetteColour(Color color) {
        towerSilhouette.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
    }

    /// <summary> changes turret silhouette position to curser position </summary>
    private void MoveSilhouette(Vector3 screenPosition) {
        towerSilhouette.transform.position = new Vector3(Mathf.Round(screenPosition.x), Mathf.Round(screenPosition.y), 0);
    }

    /// <summary> put a silhouette of given tower on curser position </summary>
    public void StartTowerPlacement(Tower toPlaceTower, GameObject silhouette) {
        if (inBuildPhase) {
            CancelTowerPlacement();
            placingActive = true;
            SellingActive = false;
            towerToPlace = toPlaceTower;
            towerSilhouette = Instantiate(silhouette, Vector3.zero, Quaternion.identity);
        }
    }

    /// <summary> cancels placement and removes tower silhouette from curser </summary>
    public void CancelTowerPlacement() {
        placingActive = false;
        if (towerSilhouette != null) {
            Destroy(towerSilhouette);
        }
        towerToPlace = null;
    }

    public void StartSellMode() {
        CancelTowerPlacement();
        SellingActive = true;
    }

    public void CancelSellMode() {
        SellingActive = false;
    }

    /// <summary> returns true if placing is active </summary>
    /// <returns></returns>
    public bool GetTurretPlaceStatus() {
        return placingActive;
    }

    public bool GetSellStatus() {
        return SellingActive;
    }
}
