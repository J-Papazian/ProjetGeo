using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Player : MonoBehaviour
{
    [SerializeField] CharacterData datas = null;
    [SerializeField] bool activePlayer = false;
    [Header("Attack")]
    [SerializeField] float reloadAttack = 0.5f;
    [SerializeField] ParticleSystem attackFX = null;
    [SerializeField] AttackZone attackZone = null;
    [Header("Animation helper")]
    [SerializeField] Transform inTarget;
    [SerializeField] float inDuration = 1.0f;
    [SerializeField] Transform outTarget;
    [SerializeField] float outDuration = 1.0f;
    [Header("Renderer")]
    [SerializeField] SpriteRenderer spriteRenderer = null;

    private Player3DController playerController;
    private CharacterController controller;

    private bool canAttacking = true;

    internal CurrentCharacterData currentCharacterData;
    internal struct CurrentCharacterData 
    {
        internal int damage;
    }

    private void Start()
    {
        PlayerManager.Instance.InitPlayer(this, activePlayer);

        playerController = GetComponent<Player3DController>();
        controller = GetComponent<CharacterController>();

        playerController.Attack_Callback += Attack;

        currentCharacterData.damage = datas.damage;
    }

	internal Tween OutAnim (TweenCallback callback)
	{
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(outTarget.position, outDuration));

        sequence.OnComplete(() => { callback?.Invoke(); spriteRenderer.enabled = false; });

        return sequence;
	}

    internal Tween InAnim (Vector3 position, TweenCallback callback)
	{
        transform.position = position;
        transform.position += new Vector3(0.0f, 10.0f, 0.0f);

        spriteRenderer.enabled = true;

        float offset = 0.0f;
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.position - Vector3.up * 100.0f, out hit);
        if (hit.collider != null)
            offset = (transform.position.y - (controller.height / 2.0f) - hit.transform.position.y - hit.transform.GetComponent<Collider>().bounds.size.y / 2.0f);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMoveY(transform.position.y - offset, inDuration));

        sequence.OnComplete(() => callback?.Invoke());

        return sequence;
    }

    internal void SwitchOut ()
	{
        playerController.enabled = false;
        controller.enabled = false;
    }

    internal void SwitchIn()
    {
        playerController.enabled = true;
        controller.enabled = true;

        PlayerManager.Instance.FinishSwitch();
    }

    private void Attack ()
	{
        if (!canAttacking)
            return;

        canAttacking = false;
        attackZone.ActiveAttack();
        attackFX.Play();
        StartCoroutine(ReloadAttack());
    }

    IEnumerator ReloadAttack ()
	{
        yield return new WaitForSeconds(reloadAttack);
        canAttacking = true;
    }

	private void OnDestroy()
	{
        playerController.Attack_Callback -= Attack;
    }
}
