using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private Player currentPlayer;
    private List<Player> players = new List<Player>();

    private Vector3 position;
    private CameraController mainCamera = null;
    internal bool canSwitch = true;

    // Debug
	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.P) && canSwitch || Input.GetButtonDown("Switch") && canSwitch)
            SwitchCharacter();
	}

	internal void InitPlayer (Player newPlayer, bool activePlayer)
	{
        players.Add(newPlayer);

        if (activePlayer)
        {
            currentPlayer = newPlayer;
            if (mainCamera == null)
                mainCamera = Camera.main.GetComponent<CameraController>();
            mainCamera.SwitchPlayer(currentPlayer.transform);
        }
	}

    internal void SwitchCharacter()
    {
        mainCamera.SwitchPlayer(null);
        canSwitch = false;
        position = currentPlayer.transform.position;
        currentPlayer.SwitchOut();
        currentPlayer.OutAnim(CallCharacter);
        OrganizePlayerList();
        currentPlayer = players[0];
    }

    internal void FinishSwitch ()
	{
        mainCamera.SwitchPlayer(currentPlayer.transform);
        canSwitch = true;
    }

    private void CallCharacter ()
	{
        currentPlayer.InAnim(position, currentPlayer.SwitchIn);
	}

    private void OrganizePlayerList ()
	{
        players.Remove(currentPlayer);
        players.Add(currentPlayer);
	}
}
