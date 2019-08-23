using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public delegate void AdditionalInstructions(Collider incomingCollider);
    public AdditionalInstructions RunThisOnTrigger;

    public float knockbackForce = 5.0f;

    [Header("Monitoring Purpose")]
    [SerializeField]
    private List<Collider> filteredColliders = new List<Collider>();

    private void Init(List<Collider> colliders)
    {
        foreach (Collider col in colliders)
            filteredColliders.Add(col);
    }

    void OnCollisionEnter(Collision collision)
    {
        //print("collide");
        if (collision.gameObject.tag == "Enemy")
        {
            //sRunThisOnTrigger?.Invoke();

            print("Hit Enemy");
            
            // Calculate Angle Between the collision point and the player
            Vector3 dir = collision.contacts[0].point - transform.position;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the enemy
            collision.gameObject.GetComponent<Rigidbody>().AddForce(dir * knockbackForce, ForceMode.Impulse);

        }
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Enemy")
    //    {
    //        if (null != RunThisOnTrigger)
    //            RunThisOnTrigger();

    //        print("Hit Enemy");
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (IsNotInFilteredColliders(other))
        {
            if (null != RunThisOnTrigger)
                RunThisOnTrigger(other);
        }
    }

    private bool IsNotInFilteredColliders(Collider incomingCollider)
    {
        foreach (Collider col in filteredColliders)
            if (incomingCollider == col)
                return false;
        return true;
    }
}
