using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    [Tooltip("Speed of camera")]
    private float cameraSpeed = 20;

    private void Update() {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x + Input.GetAxis("Horizontal") * Time.deltaTime * cameraSpeed, -25, 25);
        pos.y = Mathf.Clamp(pos.y + Input.GetAxis("Vertical") * Time.deltaTime * cameraSpeed, -25, 25);

        transform.position = pos;
    }

}
