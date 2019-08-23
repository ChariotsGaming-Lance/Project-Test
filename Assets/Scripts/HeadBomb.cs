using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBomb : MonoBehaviour
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
    private float speed = 4f;
    [SerializeField]
    private float heightLimit = 20f;

    private Transform bombParent = null;

    public void Init(Transform bombParent)
    {
        this.bombParent = bombParent;
    }

    public void Ascend()
    {
        StartCoroutine(AscendContainer());
    }

    public void DropHeadBombs()
	{
		rb.isKinematic = false;
		rb.useGravity = true;
	}

    private IEnumerator AscendContainer()
    {
        WaitForSeconds updateFrequency = new WaitForSeconds(0.1F);
        rb.isKinematic = false;
        transform.SetParent(bombParent);

        while (transform.position.y < heightLimit)
        {
            if (rb.velocity.magnitude < speed * 1.5f)
                rb.AddForce(Vector3.up * speed, ForceMode.Impulse); 
            yield return updateFrequency;
        }
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, other.gameObject.transform.position.y, transform.position.z);
        }
    }
}
