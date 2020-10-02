using UnityEngine;

/// <summary>
/// The Settings of the game.
/// </summary>
[CreateAssetMenu()]
public class Options : ScriptableObject {
    public float masterVolume = 1f;
    public float sfxVolume = 1f;
    public float musicVolume = 1f;

    [Range(10f, 15f)]
    public float distance = 15f;
    [Range(0f, 1f)]
    public float sensitivity = 1f;

}
