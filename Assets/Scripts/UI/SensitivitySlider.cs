using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TextMeshProUGUI textMesh;

    public void SetSensitivity()
    {
        Controls.lookSensitvity = slider.value;
        textMesh.text = "Sensitivity: " + (Mathf.RoundToInt(slider.value * 100) / 100f).ToString();
    }
}
