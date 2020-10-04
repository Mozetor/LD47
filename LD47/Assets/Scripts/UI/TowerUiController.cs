using Assets.WaveSpawner.Implementation;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerUiController : MonoBehaviour {

    /// <summary> Main Camera </summary>
    private Camera mainCamera;
    /// <summary> Tower placer controller </summary>
    private TowerPlacer towerPlacer;

    /// <summary> Ui of building placement selection </summary>
    public GameObject canvasTurretBuildMenue;

    /// <summary> Activeley selected turret </summary>
    private GameObject selectedTurret;

    private void Awake() {
        var spawner = FindObjectOfType<BuildBattleSpawner>();
        spawner.AddOnBuildPhaseEnd(CloseBuildUi);
    }

    // Start is called before the first frame update
    void Start() {
        mainCamera = Camera.main;
        towerPlacer = FindObjectOfType<TowerPlacer>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0) && !towerPlacer.GetTurretPlaceStatus()) {
            SelectTurret();
        }
        if (Input.GetMouseButtonDown(1) && canvasTurretBuildMenue.transform.GetChild(1).gameObject.activeInHierarchy) {
            CloseBuildUi();
        }

    }


    public void SelectTurret() {
        if (selectedTurret != null) {
            UnSelectTurret();
        }
        Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit;
        Vector3 positionOnZ0 = mouseRay.origin - Camera.main.transform.position.z * mouseRay.direction / mouseRay.direction.z;
        if (!EventSystem.current.IsPointerOverGameObject() && (hit = Physics2D.Linecast(positionOnZ0, positionOnZ0, LayerMask.GetMask("Tower")))) {
            if (hit.transform.CompareTag("Tower")) {
                selectedTurret = hit.transform.gameObject;
                Tower selecedTurretInformation = selectedTurret.GetComponent<Tower>();
                // Range indicator
                SetCircleHighlight(hit.transform.gameObject, selecedTurretInformation.range, 0.15f, Color.green);
                // Turret highlight
                SetCircleHighlight(hit.transform.GetChild(0).gameObject, hit.transform.lossyScale.x / 2, 0.2f, Color.yellow);
            }
        }
    }


    private void UnSelectTurret() {
        if (selectedTurret.GetComponent<LineRenderer>() != null) {
            selectedTurret.GetComponent<LineRenderer>().enabled = false;
        }
        if (selectedTurret.transform.GetChild(0).GetComponent<LineRenderer>() != null) {
            selectedTurret.transform.GetChild(0).GetComponent<LineRenderer>().enabled = false;
        }
        selectedTurret = null;
    }

    private void SetCircleHighlight(GameObject go, float radius, float lineWidth, Color color) {
        LineRenderer line = go.GetComponent<LineRenderer>();
        bool recalculate = false;
        if (line == null) {
            line = go.AddComponent<LineRenderer>();
            line.useWorldSpace = false;
            line.loop = true;
            line.positionCount = 180;
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
        towerPlacer.StartTowerPlacement(silhouette.GetComponent<Tower>(), silhouette);
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

    #endregion
}
