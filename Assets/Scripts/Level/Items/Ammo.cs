using UnityEngine;

public class Ammo : Item
{
    [SerializeField] private WeaponType weaponType = WeaponType.Automatic;
    [SerializeField] private int ammoAmount = 150;


    protected override void Start()
    {
        base.Start();
    }

    protected override void PickUp()
    {
        playerController.AddAmmo(weaponType, ammoAmount);
    }
}
