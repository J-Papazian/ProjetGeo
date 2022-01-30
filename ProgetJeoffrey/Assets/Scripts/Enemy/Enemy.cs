using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyData datas = null;
    [SerializeField] EnemyDetection detection = null;

    private GameObject player = null;
    private bool goToPlayer = false;

    private Rigidbody rigidBody = null;

    internal CurrentEnemyData currentEnemyData;
    internal struct CurrentEnemyData {
        internal int life;
        internal int damage;
        internal float speed;
        internal float radiusDetection;
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        detection.PlayerDetected_Callback += PlayerDetected;
        detection.SetRadius(datas.radiusDetection);

        currentEnemyData.life = datas.life;
        currentEnemyData.damage = datas.damage;
        currentEnemyData.speed = datas.speed;
        currentEnemyData.radiusDetection = datas.radiusDetection;
    }

	private void Update()
	{
        if (goToPlayer)
            GoToPlayer(player);
	}

	void GoToPlayer (GameObject player)
	{
        Vector3 direction = player.transform.position - transform.position;

        direction = (player.transform.position - transform.position).normalized * datas.speed;

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

    internal void TakeDamage(int damage)
    {
        currentEnemyData.life -= damage;

        if (currentEnemyData.life <= 0)
            EnemyManager.Instance.EnemyDied(gameObject);
    }
}
