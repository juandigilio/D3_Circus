using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject machineGun;
    [SerializeField] private GameObject rifle;
    [SerializeField] private TextMeshProUGUI ammo;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();
    }

    private void FixedUpdate()
    {
        UpdateWeaponState();
        UpdateLifes();
    }

    private void UpdateWeaponState()
    {
        if (playerController.GetCurrentWeaponType() == WeaponType.Pistol)
        {
            pistol.SetActive(true);
            machineGun.SetActive(false);
            rifle.SetActive(false);
        }
        else if (playerController.GetCurrentWeaponType() == WeaponType.Automatic)
        {
            pistol.SetActive(false);
            machineGun.SetActive(true);
            rifle.SetActive(false);
        }
        else if (playerController.GetCurrentWeaponType() == WeaponType.Rifle)
        {
            pistol.SetActive(false);
            machineGun.SetActive(false);
            rifle.SetActive(true);
        }

        ammo.text = "" + playerController.CurrentWeaponAmmo();
    }

    private void UpdateLifes()
    {
        healthBar.fillAmount = playerController.HealthPercentage();
    }
}
