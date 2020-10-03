using System.Collections.Generic;
using UnityEngine;
using City;

public class TowerSpawner : MonoBehaviour {
    // !!! PLACEHOLDER REPLACE !!!
    /// <summary> Main city </summary>
    private CityController city = new CityController();
    /// <summary> Information of to placed tower </summary>
    // !!! PLACEHOLDER REPLACE !!!
    private TowerPlaceholder towerInformation;
    /// <summary> main cammera </summary>
    private Camera cam;
    /// <summary> Ghost blueprint to indicate placing position </summary>
    private GameObject towerSilhouette;
    /// <summary> Whenever turret silhouette should be active </summary>
    private bool placingActive;
    /// <summary> towers on map </summary>
    private List<TowerPlaceholder> towers = new List<TowerPlaceholder>();

    /// <summary> Percentage of what will be returned on turret selling, 1=100% </summary>
    public float refundOnSell;


    public GameObject testVisual;


    // Start is called before the first frame update
    void Start() {
        towers.Add(new TowerPlaceholder(0, Instantiate(testVisual, Vector3.zero, Quaternion.identity)));
        cam = Camera.main;
        // !!! PLACEHOLDER !!!
        // get city
        // !!!
    }

    // Update is called once per frame
    void Update() {
        // testing -->
        if (Input.GetMouseButtonDown(0) && !placingActive) {
            StartTowerPlacement(new TowerPlaceholder(0, testVisual));
        }
        if (Input.GetMouseButtonDown(1)) {
            CancelTowerPlacement();
        }
        // testing <--
        var i = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 mousePosition = cam.ScreenToWorldPoint(i);
        if (placingActive) {
            MoveSilhouette(mousePosition);
            if (CanPlaceTower(mousePosition)) {
                if (Input.GetMouseButtonDown(0)) {
                    PlaceTower();
                }
            }
        }
    }

    /// <summary> Checks if tower can be placed </summary>
    /// <returns></returns>
    private bool CanPlaceTower(Vector3 screenPosition) {
        if (TestPosition(screenPosition)) {
            if (city.CanBuy(towerInformation.cost)) {
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
        towers.Add(new TowerPlaceholder(towerInformation.cost, Instantiate(towerInformation.goTower, newPos, Quaternion.identity)));
    }

    /// <summary> deconstructs turret, returns a part of its cost </summary>
    private void SellTurret(Vector3 targetPosition) {
        if (!(targetPosition == Vector3.zero)) {
            for (int i = 0; i < towers.Count; i++) {
                if (towers[i].transform.position == targetPosition) {
                    city.Buy((int)Mathf.Round(-(float)towers[i].cost * refundOnSell));
                    TowerPlaceholder towerToRemove = towers[i];
                    towers.Remove(towerToRemove);
                    Destroy(towerToRemove);
                    break;
                }
            }
        }
    }

    /// <summary> Tests if position is viable for turret placement </summary>
    /// <returns></returns>
    private bool TestPosition(Vector3 screenPosition) {
        Vector3 mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        if (/*!worldMap.IsPath(mousePosition.x, mousePosition.y)*/ true) {
            Vector3 cursorTile = new Vector3(Mathf.Round(screenPosition.x), Mathf.Round(screenPosition.y),0);
            for (int i = 0; i < towers.Count; i++) {
                if (towers[i].goTower.transform.position == cursorTile) {
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
        towerSilhouette.transform.position = new Vector3(Mathf.Round(screenPosition.x), Mathf.Round(screenPosition.y), -0.05f);
    }

    /// <summary> put a silhouette of given tower on curser position </summary>
    public void StartTowerPlacement(TowerPlaceholder toPlaceTower) {
        placingActive = true;
        towerInformation = toPlaceTower;
        towerSilhouette = Instantiate(towerInformation.goTower, Vector3.zero, Quaternion.identity);
    }

    /// <summary> cancels placement and removes tower silhouette from curser </summary>
    public void CancelTowerPlacement() {
        placingActive = false;
        Destroy(towerSilhouette);
    }
}
