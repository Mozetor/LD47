using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils {

    public class SceneController : MonoBehaviour {

        public Animator crossfadeAnimator;
        public Animator musicFadeAnimator;

        private void Start() {
            crossfadeAnimator.gameObject.SetActive(true);
        }

        public void ChangeScene(string scene) {
            StartCoroutine(DelayedSceneChange(scene));
        }


        public void ReloadScene() {
            string name = SceneManager.GetActiveScene().name;
            ChangeScene(name);
        }

        public void ExitGame() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        IEnumerator DelayedSceneChange(string sceneName) {
            if (crossfadeAnimator == null) {
                throw new System.ArgumentNullException("Animator was null.");
            }
            crossfadeAnimator.gameObject.SetActive(true);
            crossfadeAnimator.SetTrigger("Start");
            if (musicFadeAnimator) {
                musicFadeAnimator.SetTrigger("Start");
            }
            //OptionsController.instance.isUiBlocking = true;
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(sceneName);
            //OptionsController.instance.isUiBlocking = false;
        }
    }
}