using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawnerManager : MonoBehaviour
{
    [Header("Monitoring Purpose")]
    [SerializeField]
    private List<MobSpawner> mobSpawners = new List<MobSpawner>();

    private void Awake()
    {
        LoadAssets();
    }

    private void LoadAssets()
    {
        foreach (MobSpawner mobSpawner in GetComponentsInChildren<MobSpawner>())
            mobSpawners.Add(mobSpawner);
    }

    public MobSpawner GetRandomMobSpawner()
    {
        return mobSpawners[Random.Range(0, mobSpawners.Count)];
    }
}
