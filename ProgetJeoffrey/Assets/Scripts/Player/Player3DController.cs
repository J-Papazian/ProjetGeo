using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3DController : MonoBehaviour
{
	[SerializeField] float speed = 5.0f;
	[SerializeField] float maxSpeed = 7.0f;
	[SerializeField] float gravity = 9.81f;
	[SerializeField] float jumpTakeOffSpeed = 7.0f;

	private bool grounded = false;
	private float verticalSpeed = 0.0f;

	private CharacterController controller = null;
	private Animator animator = null;

	internal System.Action Attack_Callback;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		controller = GetComponent<CharacterController>();
	}

	private void Update()
	{
		Move();
		Action();
	}

	private void Move()
	{
		// move
		float horizontal = Input.GetAxisRaw("Horizontal");
		if (horizontal > 0.01f)
			transform.rotation = Quaternion.Euler(Vector3.up * 180);
		else if (horizontal < -0.01f)
			transform.rotation = Quaternion.Euler(Vector3.zero);

        // grounded
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * (controller.height + 0.5f) / 2.0f, Color.red);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1 * (controller.height + 0.5f) / 2.0f))
            grounded = true;
        else
            grounded = false;

        // gravity
        if (controller.isGrounded)
            verticalSpeed = 0.0f;
        else if (!grounded)
            verticalSpeed -= gravity * Time.deltaTime;

        // jump
        if (Input.GetButtonDown("Jump") && grounded) {
            verticalSpeed = jumpTakeOffSpeed;
            grounded = false;
        } else if (Input.GetButtonUp("Jump")) {
            if (verticalSpeed > 0)
                verticalSpeed = verticalSpeed * 0.5f;
        }

		//if (controller.isGrounded && Input.GetButton("Jump")) {
		//	moveDirection.y = jumpSpeed;
		//}
		//moveDirection.y -= gravity * Time.deltaTime;
		//controller.Move(moveDirection * Time.deltaTime);

		// compute
		Vector3 direction = new Vector3(horizontal, 0.0f, 0.0f).normalized;
		Vector3 gravityDirection = new Vector3(0.0f, verticalSpeed, 0.0f);
		controller.Move(direction * speed * Time.deltaTime + gravityDirection * Time.deltaTime);
	}

	private void Action ()
	{
		if (Input.GetButtonDown("Attack"))
		{
			Debug.Log("Attack");
			Attack_Callback?.Invoke();
		}
	}
}
