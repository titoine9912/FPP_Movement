using UnityEngine;

namespace Script
{
	public class Player : MonoBehaviour
	{
		private Rigidbody2D rgBody;
		private float speed = 5f;
		private bool isGrounded = true;
		
		public void Start()
		{
			rgBody = GetComponent<Rigidbody2D>();
		}

		public void FixedUpdate()
		{
			/*float moveHorizontal = Input.GetAxis("Horizontal");
			float moveVertical = Input.GetAxis("Vertical");
			
			Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
			
			rgBody.AddForce(movement * speed);*/
		}

		public void Update()
		{
			var targetVelocity = Vector2.zero;
			var force = 5f;
			
			if (Input.GetKey(KeyCode.A))
			{
				targetVelocity += Vector2.left * speed;
			}
			if (Input.GetKey(KeyCode.D))
			{
				targetVelocity += Vector2.right * speed;
			}
			if (Input.GetKey(KeyCode.Space))
			{
				targetVelocity += Vector2.up * speed * 3;
			}
			rgBody.velocity = Vector2.Lerp(rgBody.velocity, targetVelocity, Time.deltaTime * force);

			/*if (targetVelocity.sqrMagnitude > 0)
				rgBody.velocity = Vector2.Lerp(rgBody.velocity, targetVelocity, Time.deltaTime * force);
			else
				rgBody.velocity = Vector2.zero;*/
				//rgBody.velocity = Vector2.Lerp(rgBody.velocity, targetVelocity, Time.deltaTime);
				

			/*
			if (Input.GetKey(KeyCode.A))
			{
				//rgBody.MovePosition(new Vector3(rgBody.position.x - 1 * speed, rgBody.position.y));
				//transform.Translate(speed * Time.deltaTime, 0f, 0f);
				rgBody.velocity = new Vector2(-speed, 0);
			}
			if (Input.GetKey(KeyCode.D))
			{
				//rgBody.MovePosition(new Vector3(rgBody.position.x + 1 * speed, rgBody.position.y));
				rgBody.velocity = new Vector2(speed, 0);
			}
			if (Input.GetKey(KeyCode.Space))
			{
				rgBody.AddForce(new Vector2(0, 1), ForceMode2D.Impulse);
				//rgBody.velocity = new Vector2(0.0f);
			}
			if (Input.GetKey(KeyCode.None))
			{
				rgBody.velocity = new Vector2(0, 0);
			}
			*/
		}
	}
}