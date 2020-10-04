using Assets.WaveSpawner;
using Assets.WaveSpawner.Implementation;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private readonly List<Spawnable> spawned = new List<Spawnable>();

    void Awake() {
        var spawner = FindObjectOfType<BuildBattleSpawner>();
        spawner.AddOnBuildPhaseStart(() => Debug.Log("Build Phase Started"));
        spawner.AddOnBuildPhaseEnd(() => Debug.Log("Build Phase Ended"));
        spawner.AddOnBattlePhaseStart(() => Debug.Log("Battle Phase Started"));
        spawner.AddOnBattlePhaseEnd(() => Debug.Log("Battle Phase Ended"));
        spawner.AddOnWaveStarted(() => Debug.Log("Wave started"));
        spawner.AddOnWaveEnded(() => Debug.Log("Wave ended"));
        spawner.AddOnEntitySpawned(s => Debug.LogFormat("'{0}' was spawned", s.name));
    }

    // Update is called once per frame
    void Update()
    {
    }
}
