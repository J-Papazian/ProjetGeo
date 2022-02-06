using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3DController : MonoBehaviour
{
	[SerializeField] float speed = 5.0f;
	[SerializeField] float maxHorizontalSpeed = 7.0f;
	[SerializeField] float maxVerticalSpeed = 7.0f;
	[SerializeField] float gravity = 9.81f;
	[SerializeField] float jumpDistance = 3.0f;
	[SerializeField] LayerMask ground;

	private Vector3 inputs = Vector3.zero;
	private bool grounded = false;
	private bool jump = false;
    private float jumpTarget = 0.0f;
    private Coroutine jumpCoroutine;
    private Vector3 checkGround = Vector3.zero;
    private float checkGroundOffsetX = 0.0f;
    private float checkGroundOffsetY = 0.0f;

    //private CharacterController controller = null;
    private Rigidbody rigidbody = null;
	private Collider collider = null;
	private Animator animator = null;

	internal System.Action Attack_Callback;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		//controller = GetComponent<CharacterController>();
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<Collider>();

        jumpTarget = jumpDistance;
        checkGroundOffsetX = collider.bounds.extents.x - (collider.bounds.extents.x * 5.0f / 100.0f);
        checkGroundOffsetY = collider.bounds.extents.y + (collider.bounds.extents.y / 100.0f);
        checkGround = new Vector3(checkGroundOffsetX, checkGroundOffsetY, collider.bounds.extents.z);
    }

	private void Update()
	{
		Move();
		Action();
	}

    private void FixedUpdate() 
	{
        if (inputs.x > maxHorizontalSpeed)
            inputs.x = maxHorizontalSpeed;
        //if (Mathf.Abs(inputs.y) > maxVerticalSpeed)

		rigidbody.MovePosition(rigidbody.position + inputs * speed * Time.fixedDeltaTime);
    }

    private void Move()
	{
        #region RigidBody
        grounded = Physics.CheckBox(transform.position, checkGround, Quaternion.identity, ground, QueryTriggerInteraction.Ignore);

        inputs.x = 0.0f;

        inputs.x = Input.GetAxisRaw("Horizontal");
        if (inputs.x > 0.01f)
            transform.rotation = Quaternion.Euler(Vector3.up * 180);
        else if (inputs.x < -0.01f)
            transform.rotation = Quaternion.Euler(Vector3.zero);

        if (Input.GetButtonDown("Jump") && grounded) 
        {
            //rigidbody.AddForce(Vector3.up * Mathf.Sqrt(jumpTakeOffSpeed * gravity), ForceMode.VelocityChange);
            jumpCoroutine = StartCoroutine(Jump());
        }
        else if (Input.GetButtonUp("Jump")) 
        { 
            if (inputs.y > 0)
                jumpTarget = jumpTarget * 0.5f;
        }

        if (grounded && jump) 
            StartCoroutine(CheckFramTouchGround());

        #endregion

        #region CharacterController
        // move
        //float horizontal = Input.GetAxisRaw("Horizontal");
        //if (horizontal > 0.01f)
        //	transform.rotation = Quaternion.Euler(Vector3.up * 180);
        //else if (horizontal < -0.01f)
        //	transform.rotation = Quaternion.Euler(Vector3.zero);

        //      // grounded
        //      RaycastHit hit;
        //      Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * (controller.height + 0.5f) / 2.0f, Color.red);
        //      if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1 * (controller.height + 0.5f) / 2.0f))
        //          grounded = true;
        //      else
        //          grounded = false;

        //      // gravity
        //      if (controller.isGrounded)
        //          verticalSpeed = 0.0f;
        //      else if (!grounded)
        //          verticalSpeed -= gravity * Time.deltaTime;

        //      // jump
        //      if (Input.GetButtonDown("Jump") && grounded) {
        //          verticalSpeed = jumpTakeOffSpeed;
        //          grounded = false;
        //      } else if (Input.GetButtonUp("Jump")) {
        //          if (verticalSpeed > 0)
        //              verticalSpeed = verticalSpeed * 0.5f;
        //      }

        ////if (controller.isGrounded && Input.GetButton("Jump")) {
        ////	moveDirection.y = jumpSpeed;
        ////}
        ////moveDirection.y -= gravity * Time.deltaTime;
        ////controller.Move(moveDirection * Time.deltaTime);

        //// compute
        //Vector3 direction = new Vector3(horizontal, 0.0f, 0.0f).normalized;
        //Vector3 gravityDirection = new Vector3(0.0f, verticalSpeed, 0.0f);
        //controller.Move(direction * speed * Time.deltaTime + gravityDirection * Time.deltaTime);
        #endregion
    }

    private IEnumerator Jump()
    {
        StartCoroutine(JumpToTrue());
        float time = 0.05f;
        while (inputs.y < jumpTarget - 0.1f) 
        {
            inputs.y = Mathf.Lerp(inputs.y, jumpDistance, time);
            yield return null;
        }

        while (inputs.y > 0.1f) 
        {
            inputs.y = Mathf.Lerp(inputs.y, 0, time);
            yield return null;
        }
    }

    private IEnumerator JumpToTrue() 
    {
        yield return new WaitForSeconds(0.1f);
        jump = true;
    }

    private IEnumerator CheckFramTouchGround() 
    {
        yield return new WaitForEndOfFrame();
        if (grounded && jump) 
        {
            if (inputs.y > 0 || inputs.y < 0) 
            {
                jump = false;
                jumpTarget = jumpDistance;
                inputs.y = 0.0f;
                StopCoroutine(jumpCoroutine);
            }
        }
    }

    private void Action()
	{
		if (Input.GetButtonDown("Attack"))
		{
			Debug.Log("Attack");
			Attack_Callback?.Invoke();
		}
	}
}
