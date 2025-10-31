
public class HealthUp : Item
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void PickUp()
    {
        playerController.HealthUp();
    }
}
