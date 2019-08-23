using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class SpineScript : MonoBehaviour
{
    private PlayerControl _playerControl;
    private PlayerControl playerControl
    {
        get
        {
            if (null == _playerControl)
                _playerControl = GetComponent<PlayerControl>();
            return _playerControl;
        }
    }

    //player spine stuff
    private SkeletonAnimation skeletonAnim;
    private const string idleAnim = "idle";
    private const string attackAnim = "frantic";
    private const string walkAnim = "run";
    private const string chargeAnim1 = "attack charge";
    private const string chargeAnim2 = "attack charge 2";
    private const string chargeAnim3 = "attack charge 3";

    //bool inputUp, inputDown, inputLeft, inputRight, onGround;
    bool onGround;

    const int movementTrack = 0;
    const int actionTrack = 1;

    public GameObject grndTremor;

    void Awake()
    {
        skeletonAnim = GetComponent<SkeletonAnimation>();
        onGround = true;
        grndTremor.SetActive(false);
        //skeletonAnim.AnimationState.Complete += OnAnimationComplete;
    }

    void Start()
    {
        skeletonAnim.AnimationState.Complete += OnAnimationComplete;

        skeletonAnim.AnimationState.SetAnimation(movementTrack, idleAnim, true);

        //skeletonAnim.AnimationState.Data.SetMix(idleAnim, walkAnim, 0.15f);
        //skeletonAnim.AnimationState.Data.SetMix(walkAnim, idleAnim, 0.15f);
    }

    void OnDestroy()
    {
        skeletonAnim.AnimationState.Complete -= OnAnimationComplete;
    }

    void Update()
    {
        if (playerControl.GetPlayerStat().lockControls)
        {
            //print("lock test 2");
            return;
        }
        
        //get x and y values from player control
        float x = playerControl.GetX();
        float y = playerControl.GetY();

        //these values represent input, so as long as they are not 0; the character should move/animate
        if (Mathf.Approximately(x, 0) && Mathf.Approximately(y, 0))
        {
            //small bugs, sometimes when spam z it will do wrong anim
            if (playerControl.GetPlayerStat().isCharging)
            {
                //charging
                if (skeletonAnim.AnimationName != chargeAnim2)
                    skeletonAnim.AnimationState.SetAnimation(movementTrack, chargeAnim2, true);
                grndTremor.SetActive(true);
            }
            else
            {
                Idle();
                grndTremor.SetActive(false);
            }
        }
        else
        {
            if (x > 0)
                FlipCharacter(InputDirection.Right);
            else if (x < 0)
                FlipCharacter(InputDirection.Left);


            //if (onGround && skeletonAnim.AnimationName != walkAnim)
            //{
            //    skeletonAnim.AnimationState.SetAnimation(movementTrack, walkAnim, true);
            //}
            if(onGround)
            {
                //check player state to change anim
                if(Input.GetKey(KeyCode.Z))
                //if(playerControl.GetPlayerStat().isCharging)
                {
                    //charging
                    //skeletonAnim.AnimationState.SetAnimation(movementTrack, attackAnim, false);
                    //skeletonAnim.AnimationState.SetAnimation(movementTrack, chargeAnim1, true);
                    if(skeletonAnim.AnimationName != chargeAnim2)
                        skeletonAnim.AnimationState.SetAnimation(movementTrack, chargeAnim2, true);
                    //skeletonAnim.AnimationState.SetAnimation(movementTrack, chargeAnim3, false);
                    grndTremor.SetActive(true);
                }
                else if(Input.GetKeyUp(KeyCode.Z))
                //else if(playerControl.GetPlayerStat().dashing)
                {
                    //lock into dash anim
                    if (skeletonAnim.AnimationName != chargeAnim3)
                        skeletonAnim.AnimationState.SetAnimation(movementTrack, chargeAnim3, false);
                    grndTremor.SetActive(false);
                }
                else
                {
                    if (skeletonAnim.AnimationName != walkAnim)
                        skeletonAnim.AnimationState.SetAnimation(movementTrack, walkAnim, true);
                    grndTremor.SetActive(false);
                }
            }
        }
    }

    void Idle()
    {
        if (skeletonAnim.AnimationName != idleAnim)
        {
            skeletonAnim.AnimationState.SetAnimation(movementTrack, idleAnim, true);
        }
    }

    void OnAnimationComplete(TrackEntry pTrackEntry)
    {

    }

    void FlipCharacter(InputDirection pDirection)
    {
        Vector3 tScale = transform.localScale;
        switch (pDirection)
        {
            case InputDirection.Left:
                tScale.x = -1;
                break;
            case InputDirection.Right:
                tScale.x = 1;
                break;
            default:
                break;
        }
        if (tScale != transform.localScale)
        {
            transform.localScale = tScale;
        }
    }

    //void UpdateCurrentInputValues()
    //{
    //    inputUp = Input.GetKey("up");
    //    inputDown = Input.GetKey("down");
    //    inputLeft = Input.GetKey("left");
    //    inputRight = Input.GetKey("right");
    //}

    enum InputDirection
    {
        Up,
        Down,
        Left,
        Right,
    }
}