
using System;
using UnityEngine;

public class HealthUp : Item
{
    [SerializeField] private int healthAmount = 20;

    protected override void Start()
    {
        base.Start();
    }

    protected override void PickUp()
    {
        playerController.HealthUp(healthAmount);
    }
}
