using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the sync. of the value of a Slider and Input field
/// </summary>
[System.Serializable]
public class InputSliderController : MonoBehaviour {
    /// <summary> Setting variable name in options. </summary>
    public string optionsName = "name";
    /// <summary> Currnet Value </summary>
    public float Value { get; private set; }
    /// <summary> Slider </summary>
    public Slider slider;
    /// <summary> Input field </summary>
    public TMP_InputField inputField;

    private void Start() {
        ReadValue();
    }

    /// <summary>
    /// Set Value when input field got changed.
    /// </summary>
    public void UpdateInputValue() {
        if(inputField.text.Length == 0 || inputField.text == "0,") {
            return;
        }
        float value = Value;
        try {
            value = float.Parse(inputField.text);
        } catch { }
        SetValues(value);
    }

    /// <summary>
    /// Set Value when slider value got changed.
    /// </summary>
    public void UpdateSliderInput() {
        SetValues(slider.value);
    }

    /// <summary>
    /// Reads setting value from options asset and sets it.
    /// </summary>
    public void ReadValue() {
        Value = OptionsController.instance.GetValue(optionsName);
        SetValues(Value);
    }

    /// <summary>
    /// Set value to given value.
    /// </summary>
    /// <param name="value"> Value to set to </param>
    private void SetValues(float value) {
        value = Mathf.Clamp(value, slider.minValue, slider.maxValue);
        value = Mathf.Round(value * 100) / 100;
        Value = value;
        slider.value = value;
        inputField.text = value.ToString();
        OptionsController.instance.UpdateValue(Value, optionsName);
    }
}
