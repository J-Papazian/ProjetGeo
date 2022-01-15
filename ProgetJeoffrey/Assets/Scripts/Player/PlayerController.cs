using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController controller = null;
    [SerializeField] float speed = 5.0f;
    [SerializeField] float jumpHeight = 2.0f;
    [SerializeField] float gravity = -9.0f;

    private Vector3 velocity;
    private float distanceToGround = 1.0f;

    internal System.Action Attack_Callback;

	private void Start()
	{
        Collider collider = GetComponent<Collider>();
        distanceToGround = collider.bounds.extents.y;
    }

	private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        //float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x;// + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

		if (Input.GetButtonDown("Jump") && IsGrounded())
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Attack"))
            Attack_Callback?.Invoke();
	}

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
    }
}
