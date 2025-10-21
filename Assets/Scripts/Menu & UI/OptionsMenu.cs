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
    [SerializeField] private Slider volume;

    [SerializeField] private List<GameObject> keyBoard = new List<GameObject>();
    [SerializeField] private GameObject keyBoardCombinated;
    [SerializeField] private GameObject keyBoardSeparated;
    [SerializeField] private GameObject keyBoardMouse;

    [SerializeField] private List<GameObject> gamepad = new List<GameObject>();
    [SerializeField] private GameObject gamepadCombinated;
    [SerializeField] private GameObject gamepadSeparated;

    private PlayerInput playerInput;

    private const string VolumeKey = "MasterVolume";

    private void Start()
    {
        dropdown.ClearOptions();

        string[] enumNames = Enum.GetNames(typeof(InputType));
        dropdown.AddOptions(new System.Collections.Generic.List<string>(enumNames));
        dropdown.value = (int)PlayerSettings.GetInputType();
        dropdown.onValueChanged.AddListener(OnDropdownChanged);


        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        volume.value = savedVolume;
        //pasar esta linea a wwise
        AudioListener.volume = savedVolume;

        volume.onValueChanged.AddListener(OnVolumeChanged);

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

        PlayerSettings.SetInputType(selected);
    }

    private void OnVolumeChanged(float value)
    {
        //pasar a wwise
        AudioListener.volume = value;
        PlayerPrefs.SetFloat(VolumeKey, value);
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
        InputType selected = PlayerSettings.GetInputType();
        string currentDevice = playerInput.currentControlScheme;

        if (currentDevice == "Gamepad" && PlayerSettings.GetInputType() == InputType.Mouse)
        {
            PlayerSettings.SetInputType(InputType.Separated);

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
            case InputType.Combinated:
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
