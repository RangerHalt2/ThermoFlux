using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderScript : MonoBehaviour // Renamed to avoid ambiguity with UnityEngine.UI.Slider
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _slidertext;
    public float reqLevels => _slider.value;
    public int IntSliderValue => Mathf.RoundToInt(_slider.value);
    private const string SLIDER_VALUE_KEY = "SliderValue";

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(SLIDER_VALUE_KEY))
        {
            _slider.value = PlayerPrefs.GetFloat(SLIDER_VALUE_KEY);
            _slidertext.text = _slider.value.ToString("0"); // Update text
        }

        _slider.onValueChanged.AddListener((v) => {
            _slidertext.text = v.ToString("0"); // Corrected name here
            PlayerPrefs.SetFloat(SLIDER_VALUE_KEY, v); // Save the value
            PlayerPrefs.Save(); // Important: Save PlayerPrefs to disk
            BootstrappedData.Instance.reqLevels = v;
            Debug.Log($"SliderScript: Saved SliderValue to BootstrappedData: {v}");
            //BootstrappedData.Instance.SliderValue = BootstrappedData.Instance.reqLevels;
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
