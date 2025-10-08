using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;


public class InputSettings : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    private const string RebindsKey = "inputRebinds";

    private void OnEnable()
    {
        GameManager.Instance.RegisterInputSettings(this);
    }

    private void Start()
    {
        LoadRebinds();
    }

    public void SaveRebinds()
    {
        string rebinds = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(RebindsKey, rebinds);
        PlayerPrefs.Save();
    }

    public void LoadRebinds()
    {
        string rebinds = PlayerPrefs.GetString(RebindsKey, string.Empty);
        if (!string.IsNullOrEmpty(rebinds))
            inputActions.LoadBindingOverridesFromJson(rebinds);
    }

    public void ResetBindings()
    {
        inputActions.RemoveAllBindingOverrides();
        PlayerPrefs.DeleteKey(RebindsKey);
    }
}
