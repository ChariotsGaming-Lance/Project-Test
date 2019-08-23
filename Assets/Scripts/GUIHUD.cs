using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUIHUD : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField]
    private Joystick _joyStick = null;
    public Joystick joyStick
    {
        get
        {
            return _joyStick;
        }
    }
    public Transform crossHair = null;
    public TextMeshProUGUI annoucements = null;
    public GameObject mainControls = null;
    public GameObject bombControls = null;
}