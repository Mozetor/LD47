using Assets.WaveSpawner.Implementation;
using City;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Buildings {
    public class PlayerBuildingPlacer : MonoBehaviour {

        /// <summary> Main city </summary>
        private CityController city;
        /// <summary> Information of to placed building </summary>
        private IPlaceable<Building> objectToPlace;
        /// <summary> Main camera </summary>
        private Camera mainCamera;
        /// <summary> Ghost blueprint to indicate placing position </summary>
        private GameObject buildingSilhouette;
        /// <summary> Whenever building silhouette should be active </summary>
        private bool placingActive;
        /// <summary> Is the game in the build phase </summary>
        private bool inBuildPhase;
        /// <summary> Is sell selected </summary>
        private bool SellingActive;
        // !!! TODO: remove !!!
        /// <summary> towers on map, !!! THIS SHOULDNT BE IN THE PLACER, REMOVE !!! </summary>
        private static readonly List<GameObject> playerBuildings = new List<GameObject>();

        /// <summary> Percentage of money, that will be returned on building selling, 1=100% </summary>
        [Tooltip("Percentage of what will be returned on turret selling, 1=100%")]
        [Range(0f, 1f)]
        public float refundOnSell;

        private void Awake() {
            var spawner = FindObjectOfType<BuildBattleSpawner>();
            spawner.AddOnBuildPhaseStart(() => inBuildPhase = true);
            spawner.AddOnBuildPhaseEnd(() => { inBuildPhase = false; CancelTowerPlacement(); });
        }

        private void Start() {
            mainCamera = Camera.main;
            city = FindObjectOfType<CityController>();
        }

        private void Update() {
            if (!inBuildPhase) {
                return;
            }

            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 25));
            // Checks if selling is active
            if (!EventSystem.current.IsPointerOverGameObject() && SellingActive && Input.GetMouseButton(0) && !placingActive) {
                Vector3 cursorTile = new Vector3(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y), 0);
                SellTurret(cursorTile);
            }
            // Checks if placing is active
            if (placingActive && !SellingActive) {
                MoveSilhouette(mousePosition);
                if (!EventSystem.current.IsPointerOverGameObject() && CanPlaceTower(mousePosition) && Input.GetMouseButtonDown(0)) {
                    PlaceBuilding();
                }
            }
        }

        /// <summary>
        /// Add building to building list !!! THIS SHOULDNT BE IN THE PLACER, REMOVE !!! 
        /// </summary>
        /// <param name="newBuilding"></param>
        public static void AddBuilding(GameObject newBuilding) {
            playerBuildings.Add(newBuilding);
        }

        /// <summary>
        /// Checks If building can be placedm, also changes colour accordingly
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns> If building ca be placed</returns>
        private bool CanPlaceTower(Vector3 screenPosition) {
            if (TestPosition(screenPosition)) {
                if (city.CanBuyByCost(objectToPlace.GetCost())) {
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

        /// <summary>
        /// Places building into map
        /// </summary>
        private void PlaceBuilding() {
            Vector3 newPos = new Vector3(buildingSilhouette.transform.position.x, buildingSilhouette.transform.position.y, 0);
            GameObject newObject = Instantiate(objectToPlace.GetObject(), newPos, Quaternion.identity);
            if (newObject.GetComponent<LineRenderer>() != null) {
                newObject.GetComponent<LineRenderer>().enabled = false;
            }
            //objectToPlace.FinishPlacement(newObject);
            city.Buy(objectToPlace.GetCost());
        }

        /// <summary>
        /// Sell building and add part of its cost to city
        /// </summary>
        /// <param name="targetPosition"></param>
        private void SellTurret(Vector3 targetPosition) {
            if (!(targetPosition == Vector3.zero)) {
                for (int i = 0; i < playerBuildings.Count; i++) {
                    if (playerBuildings[i].transform.position == targetPosition) {
                        IPlaceable<Building> buildingToSell = playerBuildings[i].GetComponent<IPlaceable<Building>>();
                        city.AddResource(buildingToSell.GetCost(), refundOnSell);
                        buildingToSell.PrepareRemoval();
                        GameObject buildingToRemove = playerBuildings[i];
                        playerBuildings.Remove(buildingToRemove);
                        Destroy(buildingToRemove.gameObject);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Tests if position is viable for buidling placement
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns> Position is viable</returns>
        private bool TestPosition(Vector3 screenPosition) {
            if (true/*!worldController.IsPath(Mathf.RoundToInt(screenPosition.x), Mathf.RoundToInt(screenPosition.y))*/) {
                Vector3 cursorTile = new Vector3(Mathf.Round(screenPosition.x), Mathf.Round(screenPosition.y), 0);
                for (int i = 0; i < playerBuildings.Count; i++) {
                    if (playerBuildings[i].transform.position == cursorTile) {
                        return false;
                    }
                }
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Sets building silhouette sprite colour
        /// </summary>
        /// <param name="color"></param>
        private void ChangeSilhouetteColour(Color color) {
            buildingSilhouette.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        }

        /// <summary>
        /// Moves building silhouette to curser position
        /// </summary>
        /// <param name="screenPosition"></param>
        private void MoveSilhouette(Vector3 screenPosition) {
            buildingSilhouette.transform.position = new Vector3(Mathf.Round(screenPosition.x), Mathf.Round(screenPosition.y), 0);
        }

        /// <summary>
        /// Creates building Silhouette and starts placement
        /// </summary>
        /// <param name="toPlaceObject"></param>
        public void StartTowerPlacement(IPlaceable<Building> toPlaceObject) {
            if (inBuildPhase) {
                CancelTowerPlacement();
                placingActive = true;
                SellingActive = false;
                objectToPlace = toPlaceObject;
                buildingSilhouette = Instantiate(toPlaceObject.GetObject(), Vector3.zero, Quaternion.identity);
            }
        }

        /// <summary>
        /// Cancels building placement and removes silhouette from curser
        /// </summary>
        public void CancelTowerPlacement() {
            placingActive = false;
            if (buildingSilhouette != null) {
                Destroy(buildingSilhouette);
            }
            objectToPlace = null;
        }

        /// <summary>
        /// Start sell mode and cancels placement if enabeled
        /// </summary>
        public void StartSellMode() {
            CancelTowerPlacement();
            SellingActive = true;
        }

        /// <summary>
        /// Stops sellmode
        /// </summary>
        public void CancelSellMode() {
            SellingActive = false;
        }

        /// <summary>
        /// Returns if placer is in place mode
        /// </summary>
        /// <returns></returns>
        public bool GetTurretPlaceStatus() {
            return placingActive;
        }

        /// <summary>
        /// Returns if placer is in sell mode
        /// </summary>
        /// <returns></returns>
        public bool GetSellStatus() {
            return SellingActive;
        }
    }
}