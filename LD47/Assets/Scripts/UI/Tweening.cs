using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweening : MonoBehaviour {

    [Tooltip("Duration of the effect")]
    public float duration;
    [Tooltip("UI transforms that are only visible in build phase")]
    public List<Transform> buildPhaseUI = new List<Transform>();
    [Tooltip("UI transforms that are only visible in battle phase")]
    public List<Transform> battlePhaseUI = new List<Transform>();

    private void Start() {
        // buildPhaseUI:  build menu, upgrade menu, ...
        // battlePhaseUI: tower abilities, ...

        // TODO: buildPhaseUI.ForEach((t) => 'start of building phase' action += () => StartCoroutine(Scale(t, true)));
        //       buildPhaseUI.ForEach((t) => 'end of building phase' action += () => StartCoroutine(Scale(t, false)));
        //       battlePhaseUI.ForEach((t) => 'start of battle phase' action += () => StartCoroutine(Scale(t, true)));
        //       battlePhaseUI.ForEach((t) => 'end of battle phase' action += () => StartCoroutine(Scale(t, false)));
    }

    private IEnumerator Scale(Transform transform, bool expand) {
        if (expand) {
            transform.gameObject.SetActive(true);
        }
        float currentDuration = 0;
        while (currentDuration < duration) {
            float value = Mathf.Lerp(0, 1, expand ? currentDuration / duration : (duration - currentDuration) / duration);
            transform.localScale = new Vector3(value, value, 1);
            currentDuration += Time.deltaTime;
            yield return null;
        }
        if (!expand) {
            transform.gameObject.SetActive(false);
        }
    }

}
