using UnityEngine;

/// <summary>
/// Manages the option asset.
/// </summary>
public class OptionsController : MonoBehaviour {

    /// <summary> Current instance </summary>
    public static OptionsController instance;

    /// <summary> Options asset reference </summary>
    public Options options;
    /// <summary> A UI element is blocking the screen </summary>
    public bool isUiBlocking;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this);
        }
        else {
            instance = this;
        }
    }

    /// <summary>
    /// Changes value a setting in options to given value.
    /// </summary>
    /// <param name="value"> Value of the setting </param>
    /// <param name="variableName"> Name of the setting </param>
    public void UpdateValue(float value, string variableName) {
        switch (variableName) {
            case "masterVolume":
                options.masterVolume = value;
                break;
            case "sfxVolume":
                options.sfxVolume = value;
                break;
            case "musicVolume":
                options.musicVolume = value;
                break;
            default:
                throw new System.NotImplementedException("VariableName " + variableName + " not implemented.");
        }
    }

    /// <summary>
    /// Get current value from options.
    /// </summary>
    /// <param name="variableName"> Name of the variable in options </param>
    /// <returns> Value of setting in options </returns>
    public float GetValue(string variableName) {
        switch (variableName) {
            case "masterVolume":
                return options.masterVolume;
            case "sfxVolume":
                return options.sfxVolume;
            case "musicVolume":
                return options.musicVolume;
            default:
                throw new System.NotImplementedException("VariableName " + variableName + " not implemented.");
        }
    }

    /// <summary>
    /// Resets option values to default.
    /// </summary>
    public void ResetValues() {
        options.masterVolume = 1f;
        options.sfxVolume = 1f;
        options.musicVolume = 1f;
        InputSliderController[] inputSliderControllers = (InputSliderController[])FindObjectsOfType(typeof(InputSliderController));
        for (int i = 0; i < inputSliderControllers.Length; i++) {
            inputSliderControllers[i].ReadValue();
        }
    }
}
