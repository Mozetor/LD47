using Assets.Enemies;
using Assets.WaveSpawner;
using Assets.WaveSpawner.Implementation;
using System.Linq;
using TMPro;
using UnityEngine;

public class WaveInformation : MonoBehaviour {

    public TextMeshProUGUI EnemyTypeTxt;
    public TextMeshProUGUI EnemyAmountTxt;


    private void Awake() {
        var spawner = FindObjectOfType<BuildBattleSpawner>();
        UpdateUi(spawner.waves.waves[0]);
        spawner.AddOnWaveEnded(UpdateUi);
    }

    void UpdateUi(Wave wave) {
        EnemyTypeTxt.text = string.Format(
        "<b>{0}</b>\n" +
        "<b>{1}</b>\n" +
        "<b>{2}</b>\n" +
        "{3}",

        "Next wave:",
        wave.name,
        "Enemies",
        EnemyNameAsString(wave.spawnGroups));

        EnemyAmountTxt.text = string.Format(
         "\n \n" +
         "{0}\n" +
         "{1}",

         "Count",
         EnemyCountAsString(wave.spawnGroups));
    }



    private string EnemyNameAsString(SpawnGroup[] spawnGroups) =>
           spawnGroups
               .Select(SpawnGroupToString)
               .Aggregate((f, s) => $"{f}\n{s}");

    private string EnemyCountAsString(SpawnGroup[] spawnGroups) =>
          spawnGroups
              .Select(SpawnAmountToString)
              .Aggregate((f, s) => $"{f}\n{s}");

    private string SpawnGroupToString(SpawnGroup spawnGroup) {
        return $"{spawnGroup.name}";
    }

    private string SpawnAmountToString(SpawnGroup spawnGroup) {
        return $"{spawnGroup.amount}";
    }
}
