using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    private ObjectExpander _headExpander;
    private ObjectExpander headExpander
    {
        get
        {
            if (null == _headExpander)
                _headExpander = GetComponentInChildren<ObjectExpander>();
            return _headExpander;
        }
    }

    private Damager _damager;
    private Damager damager
    {
        get
        {
            if (null == _damager)
            {
                _damager = GetComponentInChildren<Damager>();
                _damager.gameObject.SetActive(false);
            }
            return _damager;
        }
    }

    [Header("Required")]
    [SerializeField]
    private WaypointManager waypointManager = null;

    [Header("Customizables")]
    [SerializeField]
    private HeroStats stats = new HeroStats();

    [Header("Monitoring Purpose")]
    [SerializeField]
    private Waypoint currentTargetedWaypoint = null;
    [SerializeField]
    private List<Collider> selfColliders = new List<Collider>();

    private void Awake()
    {
        LoadAssets();
        headExpander.Init(stats.scoreRequired);
    }

    public WaypointManager GetWaypointManager()
    {
        return waypointManager;
    }

    public Damager GetDamager()
    {
        return damager;
    }

    public ObjectExpander GetHeadBomb()
    {
        return headExpander;
    }

    public HeroStats GetStats()
    {
        return stats;
    }

    private void LoadAssets()
    {
        foreach (Collider col in GetComponentsInChildren<Collider>())
            selfColliders.Add(col);
    }
}

[System.Serializable]
public class HeroStats
{
    [Header("Customizables")]
    public int score = 0;
    public int scoreRequired = 0;
    //need variables for charging, cooldown
    public float chargeStrength = 0.0f;
    public bool isCharging = false;
    public bool shield = false;
    //used with cooldown, so that the char doesnt change direction while attacking
    public bool isAttacking = false;
    public bool dashing = false;
    public bool lockControls = false;
    public float maxCharge = 20f;
}