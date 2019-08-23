using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
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
    private float speed = 4;
    [SerializeField]
    private float minForce = 5;

    public void Move(float x, float z)
    {
        Vector3 movementVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (movementVelocity.magnitude < speed * 1.5F)
            rb.AddForce(new Vector3(x, 0, z), ForceMode.Impulse);
    }

    public void Brake()
    {
        rb.AddForce (new Vector3(-rb.velocity.x, 0, -rb.velocity.z), ForceMode.Impulse);
    }



    public void Attack(float x, float z, float force)
    {
        //print(x + "-" + z + "-" + force);
        if (force < minForce)
            rb.AddForce(new Vector3(x, 0, z) * minForce, ForceMode.Impulse);
        else
            rb.AddForce(new Vector3(x, 0, z) * force, ForceMode.Impulse);
    }

    public float getSpeed()
    {
        return speed;
    }
}
