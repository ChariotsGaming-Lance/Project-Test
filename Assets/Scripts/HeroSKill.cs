using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSkill : MonoBehaviour
{
    private Mob _mob;
    private Mob mob
    {
        get
        {
            if (null == _mob)
                _mob = GetComponent<Mob>();
            return _mob;
        }
    }

    private MobPhysics _mobPhysics;
    private MobPhysics mobPhysics
    {
        get
        {
            if (null == _mobPhysics)
                _mobPhysics = GetComponent<MobPhysics>();
            return _mobPhysics;
        }
    }

    [Header("Customizables")]
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private float forceMultiplier = 1f;
  
    private void Awake()
    {
        mob.GetDamager().RunThisOnTrigger += OnHit;
    }

    public void LaunchAttack(float amount)
    {
        Debug.Log("Force");
        mobPhysics.Attack(amount * forceMultiplier);
       
    }

    private void OnHit(Collider col)
    {
        Mob incomingMob = col.GetComponent<Mob>();
        if (null != incomingMob)
        {
            mob.GetHeadBomb().Expand(damage);
            col.GetComponent<Mob>().GetHeadBomb().Shrink(damage);
        }
    }
}
