using System;
using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;
    [SerializeField] private TextMeshProUGUI ammo;
    [SerializeField] private TextMeshProUGUI availableLifes;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();
    }

    private void FixedUpdate()
    {
        UpdateAmmo();
        UpdateLifes();
    }

    private void UpdateAmmo()
    {
        ammo.text = "Ammo" + playerController.CurrentWeaponAmmo();
    }

    private void UpdateLifes()
    {
        availableLifes.text = playerController.AvailableLives() + "/" + playerController.MaxLives(); 
    }
}
