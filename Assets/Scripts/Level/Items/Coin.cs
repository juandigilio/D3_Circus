using UnityEngine;

public class Coin : Item
{
    [SerializeField] private int scoreValue = 500;

    protected override void Start()
    {
        base.Start();
    }

    protected override void PickUp()
    {
        GameManager.Instance.GetLevelManager().AddItemScore(scoreValue);
    }
}
