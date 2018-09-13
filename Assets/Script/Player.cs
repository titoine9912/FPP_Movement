using UnityEngine;

namespace Script
{
	public class Player : MonoBehaviour
	{
		private Rigidbody2D rgBody;
		//private GameObject ground;
		private float speed = 10f;
		private bool isGrounded = true;
		private float force = 5f;
		private int xMov;
		
		public void Start()
		{
			rgBody = GetComponent<Rigidbody2D>();
			Physics2D.gravity = new Vector3(0f,-40f,0f);

			//ground = GameObject.FindGameObjectWithTag("Ground").GetComponent<GameObject>();

		}

		public void FixedUpdate()
		{
			/*float moveHorizontal = Input.GetAxis("Horizontal");
			float moveVertical = Input.GetAxis("Vertical");
			
			Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
			
			rgBody.AddForce(movement * speed);*/
		}

		private void Jump()
		{
			//if (rgBody.IsTouching(ground.GetComponent<Collider2D>()))
				rgBody.velocity = new Vector2(rgBody.velocity.x, 25);
			//rgBody.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
			//Vector2 dForce = (Vector2.up * this.force * 5) / Time.fixedDeltaTime;
			//rgBody.AddForce(dForce);
			
			//var characterVelocity = new Vector2 (xMov * speed, rgBody.velocity.y); // where y is gravity
            //rgBody.velocity = characterVelocity;
		}

		public void Update()
		{
			var targetVelocity = Vector2.zero;
			xMov = 0;
			
			if (Input.GetKey(KeyCode.A))
			{
				targetVelocity += Vector2.left * speed;
				xMov = -1;
			}
			if (Input.GetKey(KeyCode.D))
			{
				targetVelocity += Vector2.right * speed;
				xMov = 1;
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				//targetVelocity += Vector2.up * speed * 3;
				Jump();
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