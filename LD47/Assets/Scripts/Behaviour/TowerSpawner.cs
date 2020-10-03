using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using City;

public class TowerSpawner : MonoBehaviour {

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
    /// <summary> postions already used by towers </summary>
    private List<Vector2> occupiedPositions = new List<Vector2>();


    public GameObject testVisual;


    // Start is called before the first frame update
    void Start() {
        // City Position
        occupiedPositions.Add(new Vector2(0, 0));
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
        towers.Add(new TowerPlaceholder(towerInformation.cost, Instantiate(towerInformation.goTower, towerSilhouette.transform.position, Quaternion.identity)));
    }

    /// <summary> Tests if position is viable for turret placement </summary>
    /// <returns></returns>
    private bool TestPosition(Vector3 screenPosition) {
        Vector3 mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        if (/*!worldMap.IsPath(mousePosition.x, mousePosition.y)*/ true) {
            Vector2 cursorTile = new Vector2((int)screenPosition.x, (int)screenPosition.y);
            for (int i = 0; i < occupiedPositions.Count; i++) {
                Debug.Log("cursorTile:" + cursorTile);
                if (occupiedPositions[i] == cursorTile) {
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
        towerSilhouette.transform.position = new Vector3((int)screenPosition.x, (int)screenPosition.y, 0);
    }

    /// <summary> put a silhouette of given tower on curser position </summary>
    public void StartTowerPlacement(TowerPlaceholder toPlaceTower) {
        placingActive = true;
        towerInformation = toPlaceTower;
        towerSilhouette = Instantiate(towerInformation.goTower, Camera.main.ScreenToViewportPoint(Input.mousePosition), Quaternion.identity);
    }

    /// <summary> cancels placement and removes tower silhouette from curser </summary>
    public void CancelTowerPlacement() {
        placingActive = false;
        Destroy(towerSilhouette);
    }
}
