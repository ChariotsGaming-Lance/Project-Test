using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    private Animator _animator;
    private Animator animator
    {
        get
        {
            if (null == _animator)
                _animator = GetComponent<Animator>();
            return _animator;
        }
    }

    [Header("Customizables")]
    [SerializeField]
    private int mobAmount = 20;
    [SerializeField]
    private float timer = 50f;

    [Header("Required")]
    [SerializeField]
    private WaypointManager waypointManager = null;
    [SerializeField]
    private MobSpawnerManager mobSpawnerManager = null;
    [SerializeField]
    private GUIHUD guiHUD = null;
    [SerializeField]
    private HeroesManager heroesManager = null;

    [Header("Monitoring Purpose")]
    private int currentMobAmout = 0;
    private LevelState levelState = new LevelState();


    private void Awake()
    {
        guiHUD.bombControls.SetActive(false);
        levelState.timer = timer;
        GamePhase();
    }

    private void GamePhase()
    {
        StartCoroutine(GamePhaseContainer());
    }

    private IEnumerator GamePhaseContainer()
    {
        WaitForSeconds updateFrequency = new WaitForSeconds(1);
        while (levelState.timer > 0)
        {
            yield return updateFrequency;
            levelState.timer -= 1;
            guiHUD.annoucements.text = "Time Remaining: " + (int)levelState.timer + " sec";
        }

        LaunchHeadBombs();

        guiHUD.annoucements.text = "Initiating Bombing Phase";
        yield return new WaitForSeconds(1F);
        animator.SetTrigger("bombing camera");
        yield return new WaitForSeconds(1F);
        guiHUD.annoucements.text = "Drag the cross hair to target";

        guiHUD.mainControls.SetActive(false);
        guiHUD.bombControls.SetActive(true);

        yield return new WaitForSeconds(3f);
        levelState.timer = 10f;
        while (levelState.timer > 0)
        {
            yield return updateFrequency;
            levelState.timer -= 1;
            guiHUD.annoucements.text = "Time's running out: " + (int)levelState.timer + " sec";
        }

        guiHUD.annoucements.text = "Returning to Game Phase";
        animator.SetTrigger("main camera");
        yield return new WaitForSeconds(1.5f);
        guiHUD.annoucements.text = "Dodge the incoming bombs!";
       
        guiHUD.bombControls.SetActive(false);
        guiHUD.mainControls.SetActive(true);
    }

    private void LaunchHeadBombs()
    {
        foreach (Hero hero in heroesManager.GetAllHeroes())
            hero.GetHeadBomb().Ascend();
    }
}

public class LevelState
{
    public float timer = 0;
}
