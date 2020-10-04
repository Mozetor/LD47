using UnityEngine;

/// <summary>
/// The Settings of the game.
/// </summary>
[CreateAssetMenu()]
public class Options : ScriptableObject {
    public float masterVolume = 1f;
    public float sfxVolume = 1f;
    public float musicVolume = 1f;
}
