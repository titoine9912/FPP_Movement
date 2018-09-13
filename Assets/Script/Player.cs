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
			if (Input.GetKeyDown(KeyCode.Space) && isGrounded==true)
			{
				//targetVelocity += Vector2.up * speed * 3;
				Jump();
			}
			rgBody.velocity = Vector2.Lerp(rgBody.velocity, targetVelocity, Time.deltaTime * force);
		}

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.tag=="Ground")
            {
                isGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if(collision.gameObject.tag=="Ground")
            {
                isGrounded = false;
            }
        }
    }
}