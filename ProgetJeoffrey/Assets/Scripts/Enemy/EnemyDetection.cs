using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    internal System.Action<GameObject, bool> PlayerDetected_Callback;

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Debug.Log("Player detected");

			PlayerDetected_Callback?.Invoke(collision.gameObject, true);
		}
	}

	private void OnTriggerExit(Collider collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Debug.Log("Player vanish");

			PlayerDetected_Callback?.Invoke(null, false);
		}

	}
}
