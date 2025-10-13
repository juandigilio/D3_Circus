using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Slider volume;
    private const string VolumeKey = "MasterVolume";

    void Start()
    {
        dropdown.ClearOptions();

        string[] enumNames = Enum.GetNames(typeof(InputType));
        dropdown.AddOptions(new System.Collections.Generic.List<string>(enumNames));

        dropdown.onValueChanged.AddListener(OnDropdownChanged);


        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        volume.value = savedVolume;
        //pasar esta linea a wwise
        AudioListener.volume = savedVolume;

        volume.onValueChanged.AddListener(OnVolumeChanged);
    }

    private void OnDropdownChanged(int index)
    {
        InputType selected = (InputType)index;
        Debug.Log("Seleccionado: " + selected);

        PlayerSettings.SetInputType(selected);
    }

    private void OnVolumeChanged(float value)
    {
        //pasar a wwise
        AudioListener.volume = value;
        PlayerPrefs.SetFloat(VolumeKey, value);
    }
}
