using Assets.WaveSpawner.Implementation;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    [Tooltip("Speed of camera")]
    private float cameraSpeed = 20;
    [Tooltip("Size of Sidescroll bar in pixels")]
    public float mouseSideScrollBar = 50;
    [Tooltip("speed of Sidescroll by mouse")]
    public float mouseSidescrollSpeed = 0.1f;
    [Tooltip("speed of Depthscroll")]
    public float depthSidescrollSpeed = 150f;
    [Tooltip("limit how far can be scrolled: x=close, y=far")]
    public Vector2 depthSidescrollLimit;

    private void Update() {
        Vector3 pos = transform.position;

        pos += ScreenSideScrolling();
        pos.x = Mathf.Clamp(pos.x + Input.GetAxis("Horizontal") * Time.deltaTime * cameraSpeed, -25, 25);
        pos.y = Mathf.Clamp(pos.y + Input.GetAxis("Vertical") * Time.deltaTime * cameraSpeed, -25, 25);
        pos.z = Mathf.Clamp(pos.z + Input.mouseScrollDelta.y * Time.deltaTime * depthSidescrollSpeed, depthSidescrollLimit.x, depthSidescrollLimit.y);

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


}
