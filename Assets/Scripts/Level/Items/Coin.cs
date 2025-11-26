using UnityEngine;

public class Coin : Item
{
    [SerializeField] private AutoAnimator animator;
    [SerializeField] private int scoreValue = 500;
    [SerializeField] private bool isSmallCoin = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void PickUp()
    {
        GameManager.Instance.GetLevelManager().AddItemScore(scoreValue);

        if (isSmallCoin)
        {
            GameManager.Instance.GetUIAudio().PlaySmallCoinSound();
        }
        else
        {
            GameManager.Instance.GetUIAudio().PlayBigCoinSound();
        }
    }

    public void TurnOff()
    {
        animator.TurnOff();
    }
}
