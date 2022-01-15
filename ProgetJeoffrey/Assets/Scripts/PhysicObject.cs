using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicObject : MonoBehaviour
{
	[SerializeField] float minGroundYNormal = 0.65f;
    [SerializeField] float gravityModifier = 1.0f;

	protected Vector2 targetVelocity = Vector2.zero;

	protected bool grounded = false;
	protected Vector2 groundNormal = Vector2.zero;
    protected Rigidbody2D rb2D = null;
    protected Vector2 velocity = Vector2.zero;
	protected ContactFilter2D contactFilter;
	protected RaycastHit2D[] hitbuffer = new RaycastHit2D[16];
	protected List<RaycastHit2D> hitbufferList = new List<RaycastHit2D>(16);

	protected const float minMoveDistance = 0.001f;
	protected const float shellRadius = 0.01f;

	#region Unity functions

	private void OnEnable()
	{
		rb2D = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		contactFilter.useTriggers = false;
		contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
		contactFilter.useLayerMask = true;
	}

	private void Update()
	{
		targetVelocity = Vector2.zero;
		ComputeVelocity();
	}

	private void FixedUpdate()
	{
		grounded = false;
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
		velocity.x = targetVelocity.x;

        Vector2 deltaPosition = velocity * Time.deltaTime;
		Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
		Vector2 move = moveAlongGround * deltaPosition.x;
		Movement(move, false);
        move = Vector2.up * deltaPosition.y;
        Movement(move, true);
	}

	#endregion

	protected virtual void ComputeVelocity() {	}

    private void Movement (Vector2 move, bool yMovement)
	{
		float distance = move.magnitude;

		if (distance > minMoveDistance)
		{
			int count = rb2D.Cast(move, contactFilter, hitbuffer, distance + shellRadius);
			hitbufferList.Clear();
			for (int i = 0; i < count; ++i)
				hitbufferList.Add(hitbuffer[i]);

			for (int i = 0; i < hitbufferList.Count; ++i)
			{
				Vector2 currentNormal = hitbufferList[i].normal;
				if (currentNormal.y > minGroundYNormal)
				{
					grounded = true;
					if (yMovement)
					{
						groundNormal = currentNormal;
						currentNormal.x = 0;
					}
				}

				float projection = Vector2.Dot(velocity, currentNormal);
				if (projection < 0)
					velocity = velocity - projection * currentNormal;

				float modifiedDistance = hitbufferList[i].distance - shellRadius;
				distance = modifiedDistance < distance ? modifiedDistance : distance;
			}
		}

		rb2D.position = rb2D.position + move.normalized * distance;
	}
}
