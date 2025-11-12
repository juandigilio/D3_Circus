using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] private GameObject sight;
    [SerializeField] private float sightOffset = 1f;
    [SerializeField] private Weapon weapon;

    private PlayerController playerController;
    private PlayerAnimator animator;
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
        animator = playerController.GetPlayerAnimator();

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
        sight.transform.position = GameManager.Instance.GetWeaponsManager().GetCurrentFirePoint().position;
    
        switch (PlayerInfo.GetInputType())
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
            case InputType.Combined:
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

        if (aimDirection == Vector2.zero)
            animator.SetWeaponDirection(aimDirection);
    }

    private void AimCombinated()
    {
        Vector2 newDirection = inputDirection;
        newDirection.Normalize();
        KeyboardAim(newDirection);

        if (inputDirection == Vector2.zero)
            animator.SetWeaponDirection(inputDirection);
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

        quantizedAngle = NormalizeAngle(quantizedAngle);
        animator.SetWeaponDirection(NormalizeQuantizedAngle(quantizedAngle));
    }

    private void AimToMouse()
    {
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
        }

        UpdateWeaponDirection();
    }

    private Vector2 NormalizeQuantizedAngle(float quantizedAngle)
    {
        Vector2 animatorDir = Vector2.zero;

        if (quantizedAngle > -5f && quantizedAngle < 5f) animatorDir = new Vector2(1, 0);
        else if (quantizedAngle >= 22.5f && quantizedAngle < 67.5f) animatorDir = new Vector2(1, 1);
        else if (quantizedAngle >= 67.5f && quantizedAngle < 112.5f) animatorDir = new Vector2(0, 1);
        else if (quantizedAngle >= 112.5f && quantizedAngle < 157.5f) animatorDir = new Vector2(1, 1);
        else if (quantizedAngle >= 157.5f || quantizedAngle < -157.5f) animatorDir = new Vector2(1, 0);
        else if (quantizedAngle >= -157.5f && quantizedAngle < -112.5f) animatorDir = new Vector2(1, -1);
        else if (quantizedAngle >= -112.5f && quantizedAngle < -67.5f) animatorDir = new Vector2(0, -1);
        else if (quantizedAngle >= -67.5f && quantizedAngle < -22.5f) animatorDir = new Vector2(1, -1);

        return animatorDir;
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

    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        else if (angle < -180f) angle += 360f;
        return angle;
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
