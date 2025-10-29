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
        public Transform firePoint_Down ;

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
    [SerializeField] private float frameRate = 10f;

    private float timer;
    private int currentFrame;

    private SpriteRenderer currentTorso;
    private List<SpriteRenderer> currentLegs = new List<SpriteRenderer>();
    private CurrentWeaponSet currentWeaponSet = new CurrentWeaponSet();
    private CurrentWeaponAnimation currentWeaponAnimation = new CurrentWeaponAnimation();
    private bool isRunning;


    private void Start()
    {
        currentWeaponSet.Start();
        currentWeaponAnimation.Start();

        HideAll();
        SetWeapon(0);
        ShowStand();
    }

    private void Update()
    {
        Animate();
    }


    public void SetWeaponDirection(Vector2 direction)
    {
        switch (direction)
        {
            case Vector2 d when( d.x == 0 && d.y == 1):
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

    public void SetRunning(bool running)
    {
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

    private IEnumerator ShootAnimationCoroutine()
    {
        List<SpriteRenderer> anim = currentWeaponAnimation.animation;

        if (anim.Count < 2)
        {
            anim[0].enabled = true;
            yield break;
        }

        foreach (SpriteRenderer sprite in anim)
        {
            sprite.enabled = false;
        }


        for (int i = 1; i < anim.Count; i++)
        {
            anim[i].enabled = true;

            if (i > 1)
            {
                anim[i - 1].enabled = false;
            }
            yield return new WaitForSeconds(1f / frameRate);
        }

        anim[anim.Count - 1].enabled = false;
        anim[0].enabled = true;
    }

    private void Animate()
    {
        if (isRunning)
        {
            AnimateLegs();
        }
    }

    private void AnimateLegs()
    {
        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            timer = 0f;
            foreach (SpriteRenderer sprite in currentLegs)
            {
                sprite.enabled = false;
            }

            currentFrame++;

            if (currentFrame >= currentLegs.Count)
            {
                currentFrame = 0;
            }
            currentLegs[currentFrame].enabled = true;
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

        currentWeaponAnimation.animation = weapon_Animation;
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

        currentWeaponSet.up = up;
        currentWeaponSet.firePoint_Up = firePoint_Up;
        currentWeaponSet.front_Up = front_Up;
        currentWeaponSet.firePoint_Front_Up = firePoint_Front_Up;
        currentWeaponSet.front = front;
        currentWeaponSet.firePoint_Front = firePoint_Front;
        currentWeaponSet.front_Down = front_Down;
        currentWeaponSet.firePoint_Front_Down = firePoint_Front_Down;
        currentWeaponSet.down = down;
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
