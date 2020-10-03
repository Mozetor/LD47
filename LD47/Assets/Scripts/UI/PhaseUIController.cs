using Assets.WaveSpawner;
using Assets.WaveSpawner.Implementation;
using TMPro;
using UnityEngine;

public class PhaseUIController : MonoBehaviour {

    private const string BUILD_TIME_FORMAT = "{0} seconds remaining";
    private const string BATTLE_FORMAT = "{0} enemies remaining";

    public TextMeshProUGUI battleText;
    public Transform battleUI;
    public TextMeshProUGUI buildText;
    public Transform buildUI;
    private int enemies;

    private void Awake() {
        var spawner = FindObjectOfType<BuildBattleSpawner>();
        spawner.AddOnBuildPhaseStart(() => buildUI.gameObject.SetActive(true));
        spawner.AddOnBuildPhaseEnd(() => buildUI.gameObject.SetActive(false));
        spawner.AddOnBattlePhaseStart(() => battleUI.gameObject.SetActive(true));
        spawner.AddOnBattlePhaseEnd(() => battleUI.gameObject.SetActive(false));
        spawner.AddOnEntitySpawned(EnemySpawned);
        spawner.AddOnBuildTimeChange(BuildTimeChanged);
        battleText.text = string.Format(BATTLE_FORMAT, enemies);
    }

    private void EnemySpawned(Spawnable spawnable) {
        spawnable.AddOnDeath(EnemyDied);
        battleText.text = string.Format(BATTLE_FORMAT, ++enemies);
    }

    private void EnemyDied() {
        battleText.text = string.Format(BATTLE_FORMAT, --enemies);
    }

    private void BuildTimeChanged(float buildTime) {
        var bTime = Mathf.FloorToInt(buildTime);
        buildText.text = string.Format(BUILD_TIME_FORMAT, bTime);
    }
}
