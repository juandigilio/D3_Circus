using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private InputSystemUIInputModule uiInputModule;
    [SerializeField] GameObject UI;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider sfxVolume;

    [SerializeField] private List<GameObject> keyBoard = new List<GameObject>();
    [SerializeField] private GameObject keyBoardCombinated;
    [SerializeField] private GameObject keyBoardSeparated;
    [SerializeField] private GameObject keyBoardMouse;

    [SerializeField] private List<GameObject> gamepad = new List<GameObject>();
    [SerializeField] private GameObject gamepadCombinated;
    [SerializeField] private GameObject gamepadSeparated;

    private PlayerInput playerInput;

    private const string masterKey = "MasterVolume";
    private const string musicKey = "MusicVolume";
    private const string sfxKey = "SFXVolume";

    private void Start()
    {
        dropdown.ClearOptions();

        string[] enumNames = Enum.GetNames(typeof(InputType));
        dropdown.AddOptions(new System.Collections.Generic.List<string>(enumNames));
        dropdown.value = (int)PlayerInfo.GetInputType();
        dropdown.onValueChanged.AddListener(OnDropdownChanged);


        masterVolume.value = PlayerPrefs.GetFloat(masterKey, 1f); ;
        musicVolume.value = PlayerPrefs.GetFloat(musicKey, 1f);
        sfxVolume.value = PlayerPrefs.GetFloat(sfxKey, 1f);

        AkUnitySoundEngine.SetRTPCValue("Master_Volume", masterVolume.value);
        AkUnitySoundEngine.SetRTPCValue("Music_Volume", musicVolume.value);
        AkUnitySoundEngine.SetRTPCValue("SFX_Volume", sfxVolume.value);

        masterVolume.onValueChanged.AddListener(OnMasterChanged);
        musicVolume.onValueChanged.AddListener(OnMusicChanged);
        sfxVolume.onValueChanged.AddListener(OnSFXChanged);

        playerInput = GameManager.Instance.GetPlayerInput();
    }

    private void FixedUpdate()
    {
        UpdateControls();
    }

    private void OnDropdownChanged(int index)
    {
        InputType selected = (InputType)index;
        Debug.Log("Seleccionado: " + selected);

        PlayerInfo.SetInputType(selected);
    }

    private void OnMasterChanged(float value)
    {
        AkUnitySoundEngine.SetRTPCValue("Master_Volume", value);
        PlayerPrefs.SetFloat(masterKey, value);
    }

    private void OnMusicChanged(float v)
    {
        AkUnitySoundEngine.SetRTPCValue("Music_Volume", v);
        PlayerPrefs.SetFloat(musicKey, v);
    }

    private void OnSFXChanged(float v)
    {
        AkUnitySoundEngine.SetRTPCValue("SFX_Volume", v);
        PlayerPrefs.SetFloat(sfxKey, v);
    }

    private void TurnOn(List<GameObject> turnOn)
    {
        TurnOffAll();

        foreach (GameObject text in turnOn)
        {
            text.SetActive(true);
        }
    }

    private void TurnOffAll()
    {
        keyBoardCombinated.SetActive(false);
        keyBoardSeparated.SetActive(false);
        keyBoardMouse.SetActive(false);
        gamepadCombinated.SetActive(false);
        gamepadSeparated.SetActive(false);

        foreach (GameObject text in keyBoard)
        {
            text.SetActive(false);
        }
        foreach (GameObject text in gamepad)
        {
            text.SetActive(false);
        }
    }

    private void UpdateControls()
    {
        InputType selected = PlayerInfo.GetInputType();
        string currentDevice = playerInput.currentControlScheme;

        if (currentDevice == "Gamepad" && PlayerInfo.GetInputType() == InputType.Mouse)
        {
            PlayerInfo.SetInputType(InputType.Separated);

            dropdown.value = (int)InputType.Separated;
            dropdown.RefreshShownValue();
        }

        if (currentDevice == "Gamepad")
        {
            TurnOn(gamepad);
        }
        else
        {
            TurnOn(keyBoard);
        }

        switch (selected)
        {
            case InputType.Combined:
            {
                if (currentDevice == "Gamepad")
                {
                    gamepadCombinated.SetActive(true);
                }
                else
                {
                    keyBoardCombinated.SetActive(true);
                }
                break;
            }
            case InputType.Separated:
            {
                if (currentDevice == "Gamepad")
                {
                    gamepadSeparated.SetActive(true);
                }
                else
                {
                    keyBoardSeparated.SetActive(true);
                }
                break;
            }
            case InputType.Mouse:
            {
                if (currentDevice == "Gamepad")
                {
                    break;
                }
                else
                {
                    keyBoardMouse.SetActive(true);
                }
                break;
            }
            default:
            {
                break;
            }
        }
    }
}
