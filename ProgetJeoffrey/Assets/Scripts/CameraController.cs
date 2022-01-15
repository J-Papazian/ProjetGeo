using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector3 offset = Vector3.zero;

    private Transform player;

    void Update()
    {
        if (player != null)
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
    }

    internal void SwitchPlayer (Transform newPlayer)
	{
        player = newPlayer;
	}
}
