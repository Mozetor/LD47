using Assets.WaveSpawner.Implementation;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    [Tooltip("Speed of camera")]
    private float cameraSpeed = 20;
    [Tooltip("Size of Sidescroll bar in pixels")]
    public float mouseSideScrollBar = 50;
    [Tooltip("speed of Sidescroll by mouse")]
    public float mouseSidescrollSpeed = 0.5f;
    [Tooltip("speed of Depthscroll")]
    public float depthSidescrollSpeed = 0.5f;
    [Tooltip("limit how far can be scrolled: x=close, y=far")]
    public Vector2 depthSidescrollLimit;
    [Tooltip("Camera background color on day")]
    public Color dayBackground;
    [Tooltip("Camera background color on night")]
    public Color nightBackground;

    private void Awake() {
        var spawner = FindObjectOfType<BuildBattleSpawner>();
        spawner.AddOnBuildPhaseEnd(() => SetBackgroundColour(false));
        spawner.AddOnBattlePhaseEnd(() => SetBackgroundColour(true));
    }

    private void Update() {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x + Input.GetAxis("Horizontal") * Time.deltaTime * cameraSpeed, -25, 25);
        pos.y = Mathf.Clamp(pos.y + Input.GetAxis("Vertical") * Time.deltaTime * cameraSpeed, -25, 25);

        if (pos.z <= depthSidescrollLimit.y && Input.mouseScrollDelta.y > 0 || pos.z >= depthSidescrollLimit.x && Input.mouseScrollDelta.y < 0) {
            pos.z += Input.mouseScrollDelta.y * depthSidescrollSpeed;
        }

        pos += ScreenSideScrolling();


        transform.position = pos;
    }

    private Vector3 ScreenSideScrolling() {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 scrollDirection = Vector2.zero;
        if (mousePosition.x < mouseSideScrollBar) {
            scrollDirection.x -= mouseSidescrollSpeed;
        }
        else if (mousePosition.x > Screen.width - mouseSideScrollBar) {
            scrollDirection.x += mouseSidescrollSpeed;
        }
        if (mousePosition.y < mouseSideScrollBar) {
            scrollDirection.y -= mouseSidescrollSpeed;
        }
        else if (mousePosition.y > Screen.height - mouseSideScrollBar) {
            scrollDirection.y += mouseSidescrollSpeed;
        }
        return scrollDirection;
    }

    private void SetBackgroundColour(bool light) {
        GetComponent<Camera>().backgroundColor = light ? dayBackground : nightBackground;
    }

}
