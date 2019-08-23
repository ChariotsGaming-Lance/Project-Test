using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    private Rigidbody _rb;
    private Rigidbody rb
    {
        get
        {
            if (null == _rb)
                _rb = GetComponent<Rigidbody>();
            return _rb;
        }
    }

    [Header("Customizables")]
    [SerializeField]
    private float speed = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Move(float x, float z)
        
    {
            rb.AddForce(new Vector3(x, 0, z), ForceMode.Impulse);
    }

    public void Brake()
    {
        rb.AddForce(new Vector3(-rb.velocity.x, 0, -rb.velocity.z), ForceMode.Impulse);
    }
}
