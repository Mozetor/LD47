using Assets.WaveSpawner.Implementation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlayerBuilding {
    public class TowerUiController : MonoBehaviour {

        /// <summary> Main Camera </summary>
        private Camera mainCamera;
        /// <summary> Tower placer controller </summary>
        private PlayerBuildingPlacer towerPlacer;

        /// <summary> Ui of building placement selection </summary>
        public GameObject canvasTurretBuildMenue;

        /// <summary> Ui of selected turret </summary>
        public GameObject canvasTurretSelection;
        /// <summary> If turret ui is already updated </summary>
        private bool updatedTowerUi;

        /// <summary> Activeley selected building </summary>
        private GameObject selectedBuilding;
        /// <summary> Contains info and methods to manipulate selected building </summary>
        private IPlaceable selectedPlaceable;

        private void Awake() {
            var spawner = FindObjectOfType<BuildBattleSpawner>();
            spawner.AddOnBuildPhaseEnd(CloseBuildUi);
        }

        // Start is called before the first frame update
        void Start() {
            mainCamera = Camera.main;
            towerPlacer = FindObjectOfType<PlayerBuildingPlacer>();
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetMouseButtonDown(0) && !towerPlacer.GetTurretPlaceStatus() && !EventSystem.current.IsPointerOverGameObject()) {
                SelectTurret();
            }
            if (Input.GetMouseButtonDown(1) && canvasTurretBuildMenue.transform.GetChild(1).gameObject.activeInHierarchy) {
                CloseBuildUi();
            }
            if (selectedBuilding != null) {
                UpdateTowerUi();
            }

        }


        public void SelectTurret() {
            if (selectedBuilding != null) {
                UnSelectTurret();
            }
            Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit;
            Vector3 positionOnZ0 = mouseRay.origin - Camera.main.transform.position.z * mouseRay.direction / mouseRay.direction.z;
            if (!EventSystem.current.IsPointerOverGameObject() && (hit = Physics2D.Linecast(positionOnZ0, positionOnZ0, LayerMask.GetMask("Tower")))) {
                if (hit.transform.CompareTag("Tower")) {
                    selectedBuilding = hit.transform.gameObject;
                    canvasTurretSelection.SetActive(true);
                    selectedPlaceable = selectedBuilding.GetComponent<IPlaceable>();
                    if (selectedBuilding.GetComponent<Tower.Tower>() != null) {
                        Tower.Tower selecedTurretInformation = selectedBuilding.GetComponent<Tower.Tower>();
                        // Range indicator
                        SetCircleHighlight(hit.transform.gameObject, selecedTurretInformation.range, 0.15f, new Color32(0, 191, 255, 255));
                    }
                    // Turret highlight
                    SetCircleHighlight(hit.transform.GetChild(0).gameObject, hit.transform.lossyScale.x / 2, 0.2f, Color.yellow);
                }
            }
        }


        private void UnSelectTurret() {
            if (selectedBuilding.GetComponent<LineRenderer>() != null) {
                selectedBuilding.GetComponent<LineRenderer>().enabled = false;
            }
            if (selectedBuilding.transform.GetChild(0).GetComponent<LineRenderer>() != null) {
                selectedBuilding.transform.GetChild(0).GetComponent<LineRenderer>().enabled = false;
            }
            updatedTowerUi = false;
            selectedBuilding = null;
            selectedPlaceable = null;
            canvasTurretSelection.SetActive(false);
        }

        private void SetCircleHighlight(GameObject go, float radius, float lineWidth, Color color) {
            LineRenderer line = go.GetComponent<LineRenderer>();
            bool recalculate = false;
            if (line == null) {
                line = go.AddComponent<LineRenderer>();
                line.useWorldSpace = false;
                line.loop = true;
                line.positionCount = 180;
            }
            if (line.sharedMaterial == null) {
                line.material = new Material(Shader.Find("Sprites/Default"));
            }
            // check if radius or lineWidth have changed
            if (line.startWidth != lineWidth / 2 || line.GetPosition(0) != new Vector3(0, radius, -0.1f) || line.endColor != color) {
                recalculate = true;
            }
            if (!line.enabled) {
                line.enabled = true;
            }
            if (recalculate) {
                line.startWidth = lineWidth / 2;
                line.endWidth = lineWidth / 2;
                line.endColor = color;
                line.startColor = color;
                Vector3[] circlePoints = new Vector3[180];
                for (int i = 0; i < 180; i++) {
                    circlePoints[i] = new Vector3(Mathf.Sin(Mathf.Deg2Rad * i * 2) * radius, Mathf.Cos(Mathf.Deg2Rad * i * 2) * radius, -0.1f);
                }
                line.SetPositions(circlePoints);
            }
        }

        private void UpdateTowerUi() {
            if (!updatedTowerUi) {
                canvasTurretSelection.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = selectedPlaceable.GetName();
                // show tower stats here
                updatedTowerUi = true;
            }
            // !!! CAN BE IMPROVED !!!
            // CanUpgrade should be called only once per day, not on update
            if (!selectedPlaceable.IsMaxUpgrade()) {
                canvasTurretSelection.transform.GetChild(2).gameObject.SetActive(true);
                if (selectedPlaceable.CanUpgrade()) {
                    canvasTurretSelection.transform.GetChild(2).GetComponent<Image>().color = Color.green;
                }
                else {
                    canvasTurretSelection.transform.GetChild(2).GetComponent<Image>().color = Color.red;
                }
            }
            else {
                canvasTurretSelection.transform.GetChild(2).gameObject.SetActive(false);
            }
        }

        #region UiInteractions
        /// <summary> Expands build menue </summary>
        public void ExpandBuildUi() {
            BuildUI(true);
        }

        /// <summary> Closes build menue and cancels building prozess </summary>
        public void CloseBuildUi() {
            BuildUI(false);
            if (towerPlacer.GetTurretPlaceStatus()) {
                towerPlacer.CancelTowerPlacement();
            }
            if (towerPlacer.GetSellStatus()) {
                towerPlacer.CancelSellMode();
            }
        }

        private void BuildUI(bool expand) {
            canvasTurretBuildMenue.transform.GetChild(0).gameObject.SetActive(!expand);
            canvasTurretBuildMenue.transform.GetChild(1).gameObject.SetActive(expand);
            canvasTurretBuildMenue.transform.GetChild(2).gameObject.SetActive(!expand);
            canvasTurretBuildMenue.transform.GetChild(3).gameObject.SetActive(expand);
        }

        /// <summary> Starts placing of a new tower </summary>
        /// <param name="newTower"></param>
        public void StartTowerPlacement(GameObject silhouette) {
            // Range indicator
            if (silhouette.GetComponent<Tower.Tower>() != null) {
                SetCircleHighlight(silhouette, silhouette.GetComponent<Tower.Tower>().range, 0.15f, new Color32(0, 191, 255, 255));
            }
            towerPlacer.StartTowerPlacement(silhouette.GetComponent<IPlaceable>());
        }

        /// <summary> Starts placing of a new tower </summary>
        /// <param name="newTower"></param>
        public void StartTowerSelling() {
            canvasTurretBuildMenue.transform.GetChild(0).gameObject.SetActive(false);
            canvasTurretBuildMenue.transform.GetChild(1).gameObject.SetActive(true);
            canvasTurretBuildMenue.transform.GetChild(2).gameObject.SetActive(false);
            canvasTurretBuildMenue.transform.GetChild(3).gameObject.SetActive(false);
            towerPlacer.StartSellMode();
        }

        /// <summary> Upgrades selected building if possible </summary>
        public void UpgradeSelectedBuilding() {
            if (selectedPlaceable.CanUpgrade()) {
                selectedPlaceable.Upgrade();
            }
        }

        #endregion
    }
}