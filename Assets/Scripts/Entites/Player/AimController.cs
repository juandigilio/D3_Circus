using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] private GameObject sight;
    [SerializeField] private float sightOffset = 1f;
    [SerializeField] private Weapon weapon;

    private PlayerController playerController;
    private Camera mainCamera;
    private Vector3 originalScale;
    private Vector3 invertedScale;
    private Vector2 aimDirection;
    private Vector2 inputDirection;
    private float direction;
    private float lastQuantizedAngle = 0f;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();

        mainCamera = Camera.main;

        originalScale = weapon.transform.localScale;
        invertedScale = new Vector3(-originalScale.x, -originalScale.y, originalScale.z);
    }

    private void FixedUpdate()
    {
        if (playerController.IsPaused()) return;

        Aim();
    }

    private void Aim()
    {
        switch (PlayerSettings.GetInputType())
        {
            case InputType.Mouse:
                {
                    AimToMouse();
                    break;
                }
            case InputType.Separated:
                {
                    AimSeparated();
                    break;
                }
            case InputType.Combinated:
                {
                    AimCombinated();
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private void AimSeparated()
    {
        Vector2 newDirection = aimDirection;
        newDirection.Normalize();
        KeyboardAim(newDirection);
    }

    private void AimCombinated()
    {
        Vector2 newDirection = inputDirection;
        newDirection.Normalize();
        KeyboardAim(newDirection);
    }

    private void AimTo(Vector2 newDirection)
    {
        float rawAngle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
        float quantizedAngle = Mathf.Round(rawAngle / 45f) * 45f;

        if (Mathf.Abs(quantizedAngle - lastQuantizedAngle) < 10f)
            quantizedAngle = lastQuantizedAngle;
        else
            lastQuantizedAngle = quantizedAngle;

        float rad = quantizedAngle * Mathf.Deg2Rad;
        Vector2 quantizedDirection = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized * sightOffset;

        weapon.AimAt(quantizedAngle);
        sight.transform.position += new Vector3(quantizedDirection.x, quantizedDirection.y, 0);
    }

    private void AimToMouse()
    {
        sight.transform.position = weapon.GetFirePointWorldPos();

        Vector3 mousePos = Input.mousePosition;
        Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(mousePos);
        Vector2 dir = (worldMousePos - transform.position);
        dir.Normalize();

        AimTo(dir);

        direction = Mathf.Sign(dir.x);
        playerController.SetDirection(direction);

        UpdateWeaponDirection();
    }

    private void KeyboardAim(Vector2 newDirection)
    {
        float angle;

        sight.transform.position = weapon.GetFirePointWorldPos();

        if (newDirection != Vector2.zero)
        {
            AimTo(newDirection);
        }
        else
        {

            if (direction >= 0)
            {
                angle = 0;
            }
            else
            {
                angle = 180;
            }

            float rad = angle * Mathf.Deg2Rad;
            Vector2 quantizedDirection = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized * sightOffset;
            weapon.AimAt(angle);
            sight.transform.position += new Vector3(quantizedDirection.x, quantizedDirection.y, 0);
            //weapon.AimAt(angle);
            //sight.transform.position += new Vector3(direction * sightOffset, 0, 0);
        }

        UpdateWeaponDirection();
    }

    private void UpdateWeaponDirection()
    {
        if (direction > 0)
        {
            weapon.transform.localScale = originalScale;
        }
        else if (direction < 0)
        {
            weapon.transform.localScale = invertedScale;
        }
    }

    public void SetDirection(float direction)
    {
        this.direction = direction;
    }

    public void SetInputDirection(Vector2 inputDir)
    {
        this.inputDirection = inputDir;
    }

    public void SetAimDirection(Vector2 aimDirection)
    {
        this.aimDirection = aimDirection;
    }

    public void SetCurrentWeapon(Weapon weapon)
    {
        this.weapon = weapon;

        originalScale = weapon.transform.localScale;
        invertedScale = new Vector3(-originalScale.x, -originalScale.y, originalScale.z);
    }
}
