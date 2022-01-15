using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyDetection detection = null;
    [SerializeField] float speed = 5.0f;

    private GameObject player = null;
    private bool goToPlayer = false;

    private Rigidbody rigidBody = null;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        detection.PlayerDetected_Callback += PlayerDetected;
    }

	private void Update()
	{
        if (goToPlayer)
            GoToPlayer(player);
	}

	void GoToPlayer (GameObject player)
	{
        Vector3 direction = player.transform.position - transform.position;

        direction = (player.transform.position - transform.position).normalized * speed;

        rigidBody.velocity = direction;
    }

    private void PlayerDetected (GameObject player, bool detected)
	{
        this.player = player;
        goToPlayer = detected;
	}

	private void OnDestroy()
	{
        detection.PlayerDetected_Callback -= PlayerDetected;
	}
}
