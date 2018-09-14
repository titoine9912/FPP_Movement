using UnityEngine;

namespace Script
{
	public class Player : MonoBehaviour
	{
		private Rigidbody2D rgBody;
		private float speed = 10f;
		private bool isGrounded = false;
        private bool isOnWall = false;
		private float force = 5f;
		
		private int numberOfTouchedWall = 0;
		private int numberOfTouchedGround = 0;

		public Sprite sprite1;
		public Sprite sprite2;
		private SpriteRenderer spriteRenderer;
		
		public void Start()
		{
			rgBody = GetComponent<Rigidbody2D>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			
			Physics2D.gravity = new Vector3(0f,-40f,0f);

			rgBody.freezeRotation = true;

			if (spriteRenderer.sprite == null)
			{
				spriteRenderer.sprite = sprite1;
			}
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
			rgBody.velocity = new Vector2(rgBody.velocity.x, 25);
		}

        private void WallJump()
        {
            rgBody.velocity = new Vector2(-15, 25);
        }

		public void Update()
		{
			var targetVelocity = Vector2.zero;
			
			if (Input.GetKey(KeyCode.A))
			{
				targetVelocity += Vector2.left * speed;
			}
			if (Input.GetKey(KeyCode.D))
			{
				targetVelocity += Vector2.right * speed;
			}
			if (Input.GetKeyDown(KeyCode.Space) && (numberOfTouchedGround > 0 || numberOfTouchedWall > 0)) 
			{
                if(numberOfTouchedGround>0 || (numberOfTouchedWall>0 && numberOfTouchedGround>0))
                {
                    Jump();
                }
                else if(numberOfTouchedWall>0)
                {
                    WallJump();
                }
				//targetVelocity += Vector2.up * speed * 3;
				
			}
			rgBody.velocity = Vector2.Lerp(rgBody.velocity, targetVelocity, Time.deltaTime * force);

			if (Input.GetKeyDown(KeyCode.E))
			{
				ChangeSprite();
			}
			
			Debug.Log("Grounds : " + numberOfTouchedGround);
			Debug.Log("Walls : " + numberOfTouchedWall);
		}

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var contact = collision.contacts[0];                    
            if (contact.normal.y > 0)
            {
                numberOfTouchedGround++;
            }
            else if (Mathf.Abs(contact.normal.x) > 0)
            {
                numberOfTouchedWall++;
            }          
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            Vector3 closestPoint = collision.collider.bounds.ClosestPoint(transform.position);
            Vector3 direction = closestPoint - transform.position;
		    if (direction.y < 0 )
		    {
			    numberOfTouchedGround--;
                Debug.Log("touche pu plancher");
		    }
		    else if (Mathf.Abs(direction.x) > 0)
		    {
			    numberOfTouchedWall--;
                Debug.Log("touche pu le mur");
		    }
        }

		private void ChangeSprite()
		{
			if (spriteRenderer.sprite == sprite1)
			{
				spriteRenderer.sprite = sprite2;
			}
			else
			{
				spriteRenderer.sprite = sprite1;
			}
		}
    }
}