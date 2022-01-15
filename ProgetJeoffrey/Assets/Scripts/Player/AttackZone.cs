using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    private float force = 5.0f;
    private BoxCollider Collider = null;

    void Start()
    {
        Collider = GetComponent<BoxCollider>();
    }

    internal void ActiveAttack()
	{
        Collider.enabled = true;
        // DEBUG
        transform.GetChild(0).gameObject.SetActive(true);
        // DEBUG
        StartCoroutine(ColliderStayActive());
	}

    IEnumerator ColliderStayActive()
	{
        yield return new WaitForSeconds(0.2f);
        Collider.enabled = false;
        // DEBUG
        transform.GetChild(0).gameObject.SetActive(false);
        // DEBUG
    }

    private void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.CompareTag("Enemy"))
		{
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.right * force, ForceMode.Impulse);
		}
	}
}
