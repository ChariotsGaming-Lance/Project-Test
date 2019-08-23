using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobPhysics : MonoBehaviour
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

    public  void Attack(float force)
    {
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }
    /*
    public void MoveToward()
    {
        if (rb.velocity.) 
    }
    */
}
