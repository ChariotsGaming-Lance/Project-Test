using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroesManager : MonoBehaviour
{
    [Header("Required")]
    [SerializeField]
    private Transform headBombs = null;

    [Header("Monitoring Purpose")]
    [SerializeField]
    private List<Hero> heroes = new List<Hero>();

    private void Awake()
    {
        RegisterPlayers();
    }

    private void RegisterPlayers()
    {
        foreach (Hero hero in GetComponentsInChildren<Hero>())
        {
            heroes.Add(hero);
            hero.GetHeadBomb().Init(headBombs);
        }
    }

    public List<Hero> GetAllHeroes()
    {
        return heroes;
    }
}
