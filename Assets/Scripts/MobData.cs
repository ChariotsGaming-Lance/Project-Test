using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MobsData : MonoBehaviour
{
    public List<MobData> mobData = new List<MobData>();
}

public class MobData
{
    public Mob mob = null;
    public List<Mob> activeMobs = new List<Mob>();
    public List<Mob> inactiveMobs = new List<Mob>();
}
