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
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI score;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();
    }

    private void FixedUpdate()
    {
        UpdateWeaponState();
        UpdatePlayerInfo();
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

    private void UpdatePlayerInfo()
    {
        healthBar.fillAmount = playerController.HealthPercentage();

        float totalTime = GameManager.Instance.GetLevelManager().GetTotalTime();
        int minutes = Mathf.FloorToInt(totalTime / 60);
        int seconds = Mathf.FloorToInt(totalTime % 60);

        timer.text = $"{minutes:00}:{seconds:00}.{(totalTime % 1f) * 10:0}";
        score.text = "" + GameManager.Instance.GetLevelManager().GetCurrentScore();
    }
}
