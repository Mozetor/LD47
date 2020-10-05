using Assets.WaveSpawner.Implementation;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    [Tooltip("Speed of camera")]
    private float cameraSpeed = 20;
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

        transform.position = pos;
    }

    private void SetBackgroundColour(bool light) {
        Debug.Log("is called,light:" + light);
        GetComponent<Camera>().backgroundColor = light ? dayBackground : nightBackground;
    }

}
