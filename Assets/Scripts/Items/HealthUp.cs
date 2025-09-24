
public class HealthUp : Item
{
    protected override void Start()
    {
        base.Start();
        //playerController = GameManager.Instance.GetComponent<PlayerController>();
    }

    protected override void PickUp()
    {
        playerController.HealthUp();
    }
}
