using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerAnimator : MonoBehaviour
{
    public struct CurrentWeaponSet
    {
        public List<SpriteRenderer> up;
        public Transform firePoint_Up;
        public List<SpriteRenderer> front_Up;
        public Transform firePoint_Front_Up;
        public List<SpriteRenderer> front;
        public Transform firePoint_Front;
        public List<SpriteRenderer> front_Down;
        public Transform firePoint_Front_Down;
        public List<SpriteRenderer> down;
        public Transform firePoint_Down;

        public void Start()
        {
            up = new List<SpriteRenderer>();
            front_Up = new List<SpriteRenderer>();
            front = new List<SpriteRenderer>();
            front_Down = new List<SpriteRenderer>();
            down = new List<SpriteRenderer>();
        }

        public void Clear()
        {
            foreach (SpriteRenderer sprite in up)
            {
                sprite.enabled = false;
            }
            up.Clear();

            foreach (SpriteRenderer sprite in front_Up)
            {
                sprite.enabled = false;
            }
            front_Up.Clear();

            foreach (SpriteRenderer sprite in front)
            {
                sprite.enabled = false;
            }
            front.Clear();

            foreach (SpriteRenderer sprite in front_Down)
            {
                sprite.enabled = false;
            }
            front_Down.Clear();

            foreach (SpriteRenderer sprite in down)
            {
                sprite.enabled = false;
            }
            down.Clear();
        }
    }

    public struct CurrentWeaponAnimation
    {
        public List<SpriteRenderer> animation;
        public Transform firePoint;

        public void Start()
        {
            animation = new List<SpriteRenderer>();
        }

        public void Clear()
        {
            foreach (SpriteRenderer sprite in animation)
            {
                sprite.enabled = false;
            }
            animation.Clear();
        }
    }


    [SerializeField] private PlayerController playerController;

    [Header("Gun 1")]
    [SerializeField] private List<SpriteRenderer> gun_1_Up = new List<SpriteRenderer>();
    [SerializeField] private Transform gun_1_FirePoint_Up;
    [SerializeField] private List<SpriteRenderer> gun_1_Front_Up = new List<SpriteRenderer>();
    [SerializeField] private Transform gun_1_FirePoint_Front_Up;
    [SerializeField] private List<SpriteRenderer> gun_1_Front = new List<SpriteRenderer>();
    [SerializeField] private Transform gun_1_FirePoint_Front;
    [SerializeField] private List<SpriteRenderer> gun_1_Front_Down = new List<SpriteRenderer>();
    [SerializeField] private Transform gun_1_FirePoint_FrontDown;
    [SerializeField] private List<SpriteRenderer> gun_1_Down = new List<SpriteRenderer>();
    [SerializeField] private Transform gun_1_FirePoint_Down;

    [Header("Gun 2")]
    [SerializeField] private List<SpriteRenderer> gun_2_Up = new List<SpriteRenderer>();
    [SerializeField] private Transform gun_2_FirePoint_Up;
    [SerializeField] private List<SpriteRenderer> gun_2_Front_Up = new List<SpriteRenderer>();
    [SerializeField] private Transform gun_2_FirePoint_Front_Up;
    [SerializeField] private List<SpriteRenderer> gun_2_Front = new List<SpriteRenderer>();
    [SerializeField] private Transform gun_2_FirePoint_Front;
    [SerializeField] private List<SpriteRenderer> gun_2_Front_Down = new List<SpriteRenderer>();
    [SerializeField] private Transform gun_2_FirePoint_FrontDown;
    [SerializeField] private List<SpriteRenderer> gun_2_Down = new List<SpriteRenderer>();
    [SerializeField] private Transform gun_2_FirePoint_Down;

    [Header("Gun 3")]
    [SerializeField] private List<SpriteRenderer> gun_3_Up = new List<SpriteRenderer>();
    [SerializeField] private Transform gun_3_FirePoint_Up;
    [SerializeField] private List<SpriteRenderer> gun_3_Front_Up = new List<SpriteRenderer>();
    [SerializeField] private Transform gun_3_FirePoint_Front_Up;
    [SerializeField] private List<SpriteRenderer> gun_3_Front = new List<SpriteRenderer>();
    [SerializeField] private Transform gun_3_FirePoint_Front;
    [SerializeField] private List<SpriteRenderer> gun_3_Front_Down = new List<SpriteRenderer>();
    [SerializeField] private Transform gun_3_FirePoint_FrontDown;
    [SerializeField] private List<SpriteRenderer> gun_3_Down = new List<SpriteRenderer>();
    [SerializeField] private Transform gun_3_FirePoint_Down;

    [Header("Torso")]
    [SerializeField] private SpriteRenderer torso_Front_Up;
    [SerializeField] private SpriteRenderer torso_Front;
    [SerializeField] private SpriteRenderer torso_Front_Down;
    [SerializeField] private SpriteRenderer torso_Hit;

    [Header("Legs")]
    [SerializeField] private SpriteRenderer legs_Stand;
    [SerializeField] private List<SpriteRenderer> legs_Running = new List<SpriteRenderer>();
    [SerializeField] private List<SpriteRenderer> legs_Jumping = new List<SpriteRenderer>();

    [Header("Settings")]
    [SerializeField] private float legsFrameRate = 0.1f;
    [SerializeField] private float jumpFrameRate = 0.01f;
    [SerializeField] private float weaponFrameRate = 0.2f;

    private float legsTimer;
    private float jumpTimer;
    private int currentLegsFrame;
    private int currentJumpFrame;

    private bool isJumping;
    private bool isGrounded;

    private SpriteRenderer currentTorso;
    private List<SpriteRenderer> currentLegs = new List<SpriteRenderer>();
    private CurrentWeaponSet currentWeaponSet = new CurrentWeaponSet();
    private CurrentWeaponAnimation currentWeaponAnimation = new CurrentWeaponAnimation();
    private bool isRunning;
    private Vector2 lastDirection = new Vector2();


    private void Start()
    {
        legsTimer = 0f;
        jumpTimer = 0;
        currentLegsFrame = 0;
        currentJumpFrame = 0;
        isGrounded = false;
        isJumping = false;
        isRunning = false;

        currentWeaponSet.Start();
        currentWeaponAnimation.Start();

        HideAll();
        SetWeapon(0);
        ShowStand();
    }

    private void FixedUpdate()
    {
        CheckGround(playerController.IsGrounded());
        Animate();
    }

    public void SetWeaponDirection(Vector2 direction)
    {
        if (direction == lastDirection) return;

        lastDirection = direction;

        switch (lastDirection)
        {
            case Vector2 d when (d.x == 0 && d.y == 1):
            {
                SetCurrentWeaponAnimation(currentWeaponSet.up, currentWeaponSet.firePoint_Up);
                SetTorso(torso_Front_Up);
                break;
            }
            case Vector2 d when (d.x == 1 && d.y == 1):
            {
                SetCurrentWeaponAnimation(currentWeaponSet.front_Up, currentWeaponSet.firePoint_Front_Up);
                SetTorso(torso_Front_Up);
                break;
            }
            case Vector2 d when (d.x == 1 && d.y == 0) || (d.x == 0 && d.y == 0):
            {
                SetCurrentWeaponAnimation(currentWeaponSet.front, currentWeaponSet.firePoint_Front);
                SetTorso(torso_Front);
                break;
            }
            case Vector2 d when (d.x == 1 && d.y == -1):
            {
                SetCurrentWeaponAnimation(currentWeaponSet.front_Down, currentWeaponSet.firePoint_Front_Down);
                SetTorso(torso_Front_Down);
                break;
            }
            case Vector2 d when (d.x == 0 && d.y == -1):
            {
                SetCurrentWeaponAnimation(currentWeaponSet.down, currentWeaponSet.firePoint_Down);
                SetTorso(torso_Front_Down);
                break;
            }
            default:
            {
                break;
            }
        }
    }

    public void SetWeapon(int weapon)
    {
        switch (weapon)
        {
            case 0:
                {
                    ActivateWeapon(
                    gun_1_Up, gun_1_FirePoint_Up,
                    gun_1_Front_Up, gun_1_FirePoint_Front_Up,
                    gun_1_Front, gun_1_FirePoint_Front,
                    gun_1_Front_Down, gun_1_FirePoint_FrontDown,
                    gun_1_Down, gun_1_FirePoint_Down
                    );
                    break;
                }
            case 1:
                {
                    ActivateWeapon(
                    gun_2_Up, gun_2_FirePoint_Up,
                    gun_2_Front_Up, gun_2_FirePoint_Front_Up,
                    gun_2_Front, gun_2_FirePoint_Front,
                    gun_2_Front_Down, gun_2_FirePoint_FrontDown,
                    gun_2_Down, gun_2_FirePoint_Down
                    );
                    break;
                }
            case 2:
                {
                    ActivateWeapon(
                    gun_3_Up, gun_3_FirePoint_Up,
                    gun_3_Front_Up, gun_3_FirePoint_Front_Up,
                    gun_3_Front, gun_3_FirePoint_Front,
                    gun_3_Front_Down, gun_3_FirePoint_FrontDown,
                    gun_3_Down, gun_3_FirePoint_Down
                    );
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public Vector3 GetFirePoint()
    {
        return currentWeaponAnimation.firePoint.position;
    }

    public void AnimateShoot()
    {
        StopAllCoroutines();
        StartCoroutine(ShootAnimationCoroutine());
    }

    public void SetRunning(bool running)
    {
        if (running == isRunning) return;

        isRunning = running;

        if (isRunning)
        {
            SetLegs(legs_Running);
        }
        else
        {
            SetLegs(legs_Stand);
        }
    }

    public void AnimateJump()
    {
        isJumping = true;

        SetLegs(legs_Jumping);
    }

    private void CheckGround(bool grounded)
    {
        bool wasGrounded = isGrounded;

        isGrounded = grounded;

        if (isGrounded && !wasGrounded)
        {
            isJumping = false; 
            currentJumpFrame = 0;

            if (isRunning)
            {
                SetLegs(legs_Running);
                return;
            }
            else
            {
                SetLegs(legs_Stand);
            }
        }
        else if (!isGrounded && !isJumping)
        {
            SetLegs(legs_Jumping[0]);
        }
    }

    private void UpdateJumpAnimation()
    {
        if (!isJumping) return;

        if (currentJumpFrame >= currentLegs.Count)
        {
            currentJumpFrame = currentLegs.Count;
            return;
        }

        jumpTimer += Time.deltaTime;

        if (jumpTimer >= jumpFrameRate)
        {
            jumpTimer = 0f;

            foreach (SpriteRenderer sprite in currentLegs)
            {
                sprite.enabled = false;
            }

            Debug.Log($"Current jump frame: " + currentJumpFrame);
            Debug.Log($"List size: " + currentLegs.Count);
            currentLegs[currentJumpFrame].enabled = true;

            currentJumpFrame++;
        }
    }

    private IEnumerator ShootAnimationCoroutine()
    {
        foreach (SpriteRenderer sprite in currentWeaponAnimation.animation)
        {
            sprite.enabled = false;
        }

        for (int i = 1; i < currentWeaponAnimation.animation.Count; i++)
        {
            currentWeaponAnimation.animation[i - 1].enabled = false;
            currentWeaponAnimation.animation[i].enabled = true;

            yield return new WaitForSeconds(weaponFrameRate);
        }

        foreach (SpriteRenderer sprite in currentWeaponAnimation.animation)
        {
            sprite.enabled = false;
        }

        currentWeaponAnimation.animation[0].enabled = true;
    }

    private void Animate()
    {
        if (isRunning)
        {
            AnimateLegs();
        }

        UpdateJumpAnimation();
    }

    private void AnimateLegs()
    {
        if (isJumping || !isGrounded) return;

        legsTimer += Time.deltaTime;

        if (legsTimer >= legsFrameRate)
        {
            legsTimer = 0f;

            foreach (SpriteRenderer sprite in currentLegs)
            {
                sprite.enabled = false;
            }

            currentLegsFrame++;

            if (currentLegsFrame >= currentLegs.Count)
            {
                currentLegsFrame = 0;
            }
            currentLegs[currentLegsFrame].enabled = true;
        }
    }

    private void ShowStand()
    {
        SetCurrentWeaponAnimation(gun_1_Front, gun_1_FirePoint_Front);
        SetLegs(legs_Stand);
        SetTorso(torso_Front);
    }

    private void SetLegs(List<SpriteRenderer> legs)
    {
        foreach (SpriteRenderer sprite in currentLegs)
        {
            sprite.enabled = false;
        }

        currentLegs.Clear();
        currentLegs.AddRange(legs);

        currentJumpFrame = 0;
        currentLegsFrame = 0;

        currentLegs[0].enabled = true;
    }

    private void SetLegs(SpriteRenderer leg)
    {
        foreach (SpriteRenderer sprite in currentLegs)
        {
            sprite.enabled = false;
        }

        currentLegs.Clear();
        currentLegs.Add(leg);

        currentLegs[0].enabled = true;
    }

    private void SetTorso(SpriteRenderer torso)
    {
        if (currentTorso)
        {
            currentTorso.enabled = false;
        }
        currentTorso = torso;
        currentTorso.enabled = true;
    }

    private void SetCurrentWeaponAnimation(List<SpriteRenderer> weapon_Animation, Transform firepoint)
    {
        currentWeaponAnimation.Clear();

        currentWeaponAnimation.animation.AddRange(weapon_Animation);
        currentWeaponAnimation.animation[0].enabled = true;
        currentWeaponAnimation.firePoint = firepoint;
    }

    private void ActivateWeapon(
        List<SpriteRenderer> up, Transform firePoint_Up,
        List<SpriteRenderer> front_Up, Transform firePoint_Front_Up,
        List<SpriteRenderer> front, Transform firePoint_Front,
        List<SpriteRenderer> front_Down, Transform firePoint_Front_Down,
        List<SpriteRenderer> down, Transform firePoint_Down
        )
    {
        currentWeaponSet.Clear();

        currentWeaponSet.up.AddRange(up);
        currentWeaponSet.firePoint_Up = firePoint_Up;
        currentWeaponSet.front_Up.AddRange(front_Up);
        currentWeaponSet.firePoint_Front_Up = firePoint_Front_Up;
        currentWeaponSet.front.AddRange(front);
        currentWeaponSet.firePoint_Front = firePoint_Front;
        currentWeaponSet.front_Down.AddRange(front_Down);
        currentWeaponSet.firePoint_Front_Down = firePoint_Front_Down;
        currentWeaponSet.down.AddRange(down);
        currentWeaponSet.firePoint_Down = firePoint_Down;

        SetCurrentWeaponAnimation(front, firePoint_Front);
    }

    private void HideTorso()
    {
        torso_Front_Up.enabled = false;
        torso_Front.enabled = false;
        torso_Front_Down.enabled = false;
        torso_Hit.enabled = false;
    }

    private void HideLegs()
    {
        legs_Stand.enabled = false;

        foreach (var sprite in legs_Running)
        {
            sprite.enabled = false;
        }

        foreach (var sprite in legs_Jumping)
        {
            sprite.enabled = false;
        }
    }

    private void HideGunSet(params List<SpriteRenderer>[] animations)
    {
        foreach (List<SpriteRenderer> animation in animations)
        {
            foreach (SpriteRenderer sprite in animation)
            {
                sprite.enabled = false;
            }
        }
    }

    private void HideGuns()
    {
        HideGunSet(gun_1_Up, gun_1_Front_Up, gun_1_Front, gun_1_Front_Down, gun_1_Down);
        HideGunSet(gun_2_Up, gun_2_Front_Up, gun_2_Front, gun_2_Front_Down, gun_2_Down);
        HideGunSet(gun_3_Up, gun_3_Front_Up, gun_3_Front, gun_3_Front_Down, gun_3_Down);
    }

    private void HideAll()
    {
        HideTorso();

        HideLegs();

        HideGuns();
    }
}
