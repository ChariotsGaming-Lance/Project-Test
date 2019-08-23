using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private enum TargetingMode { PLAYER, BOMB };
    private TargetingMode target;
    private PlayerPhysics _playerPhysics;
    private PlayerPhysics playerPhysics
    {
        get
        {
            if (null == _playerPhysics)
                _playerPhysics = GetComponent<PlayerPhysics>();
            return _playerPhysics;
        }
    }

    private Rigidbody _rb;
    public GameObject shield;
    public GameObject shieldRotater;

    public void Start()
    {
        damager.gameObject.SetActive(false);
        shield.gameObject.SetActive(false);
    }

    [Header("Required")]
    [SerializeField]
    private GUIHUD guiHud = null;
    [SerializeField]
    private Damager damager;
    [SerializeField]
    private Bomb bomb;
    [SerializeField]
    private float attackDuration = 1.0f;

    [Header("Customizables")]
    [SerializeField]
    private PlayerStat playerStat;

    private float x = 0.0f;
    private float y = 0.0f;

    public float maxStrength = 20.0f;
    private float cs = 0.0f;

    private void Awake()
    {
        damager.RunThisOnTrigger += OnHitSuccessful;
        damager.gameObject.SetActive(false);
        _rb = GetComponent<Rigidbody>();
        playerStat = new PlayerStat();
    }

    //TODO: migrate non-physics code to Update()
    //NOTE: fixedupdate should be for physics ONLY.
    //https://forum.unity.com/threads/the-truth-about-fixedupdate.231637/
    private void FixedUpdate()
    {
        shieldRotater.transform.LookAt(shieldRotater.transform.position + new Vector3(x, 0, y));

        //reduce cooldown timer
        //playerStat.cooldown -= Time.timeScale;

        //print(_rb.velocity.ToString() + "eakuyjghsdbasjhbd");
        if (playerStat.lockControls)
            return;
        if (target == TargetingMode.BOMB)
        {
            if (!bomb.gameObject.activeSelf) bomb.gameObject.SetActive(true);
            bomb.Move(x, y);
        }

        if (guiHud.joyStick.GetPointerUp())
        {
            //playerPhysics.Brake();
        }

        if (playerStat.isCharging)
        {
//            print (playerStat.chargeStrength);
            if (playerStat.chargeStrength < maxStrength)
                playerStat.chargeStrength += 0.5f;
            //print(playerStat.chargeStrength);
            playerPhysics.Brake();
            //here can add shield stuff
            shield.gameObject.SetActive(true);
            //shield.transform.LookAt(new Vector3(x, 0, y));
            //shield.transform.Rotate();
        }
        //for some reason the moment it goes in here the velocity becomes 0
        else if (playerStat.attack)
        {
            //print(_rb.velocity.ToString());
            if (Mathf.Approximately(x, 0) && Mathf.Approximately(y, 0))
            {
                //no joystick input, do not attack and keep cooldown
                playerStat.attack = false;
            }
            else
            {
                //joystick input, character attacks
                //if(playerStat.cooldown == 0.0f)
                StartCoroutine(Attack());
                //after attack, reset value
                //supposed to have a timer
                playerStat.attack = false;
            }
            //deactivate shield
            shield.SetActive(false);
        }
        //shifted move to here, so when holding charge, dont move
        else
        {
            playerPhysics.Move(x, y);
            //deactivate shield
            shield.SetActive(false);
        }

        //ATTACKING CONDITION
        //if (!isCharging && !Mathf.Approximately(chargeStrength, 0))
        //{
        //    //do attack
        //    //StartCoroutine(Attack());
        //}
    }

    private void Update()
    {
        if (playerStat.lockControls)
            return;

        x = guiHud.joyStick.Horizontal;
        y = guiHud.joyStick.Vertical;
        //if (Input.GetKeyDown(KeyCode.Z)) StartCoroutine(Attack());
        if (Input.GetKey(KeyCode.Z))
        {
            //print("pressed");
            //playerStat.chargeStrength = 0;
            playerStat.isCharging = true;
            playerStat.shield = true;
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            playerStat.isCharging = false;
            playerStat.attack = true;
            playerStat.shield = false;
            _Attack();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (target == TargetingMode.PLAYER) target = TargetingMode.BOMB;
            else if (target == TargetingMode.BOMB) target = TargetingMode.PLAYER;
            print(target);
        }
    }

    public void _Attack()
    {
        print(canAttack + "askdghj");
        if (canAttack)
            StartCoroutine(Attack());
    }

    bool canAttack = false;

    public IEnumerator Attack()
    {
        canAttack = false;
        playerStat.dashing = true;
        //damager.gameObject.SetActive(true);
        //print("Attack");
        playerPhysics.Attack(x, y, playerStat.chargeStrength);
        //playerPhysics.Attack(x, y);

        playerStat.lockControls = true;
        //velocity is 0 somehow
        yield return new WaitForSeconds(attackDuration);
        //print(_rb.velocity.ToString() + "sdkfgasjkhfbd");

        while (_rb.velocity.magnitude > _playerPhysics.getSpeed())
            yield return new WaitForSeconds(0.01f);
        //damager.gameObject.SetActive(false);
        playerStat.lockControls = false;
        playerStat.dashing = false;
        //print("lock test");
        playerStat.chargeStrength = 0;
        canAttack = true;
        //print(canAttack);
    }

    private void OnHitSuccessful(Collider col)
    {
        Debug.Log("Success Hit");
        playerStat.score += 1;
        print(playerStat.score);
    }

    public float GetX()
    {
        return x;
    }

    public float GetY()
    {
        return y;
    }

    public PlayerStat GetPlayerStat()
    {
        return playerStat;
    }
}

[System.Serializable]
public class PlayerStat
{
    [Header("Customizables")]
    public int score = 0;
    public int scoreRequired = 0;
    //need variables for charging, cooldown
    public float chargeStrength = 0.0f;
    public bool isCharging = false;
    public bool shield = false;
    public bool attack = false;
    public bool dashing = false;
    public bool lockControls = false;
    public float maxCharge = 20f;
}