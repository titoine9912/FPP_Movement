using UnityEngine;

namespace Script
{
	public class Player : MonoBehaviour
	{
		private Rigidbody2D rgBody;
		private float speed = 5;
		
		public void Start()
		{
			rgBody = GetComponent<Rigidbody2D>();
		}

		public void FixedUpdate()
		{
			float moveHorizontal = Input.GetAxis("Horizontal");
			float moveVertical = Input.GetAxis("Vertical");
			
			Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
			
			rgBody.AddForce(movement * speed);
		}

		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				//rgBody.MovePosition(new Vector3(rgBody.position.x - 1 * speed, rgBody.position.y));
				rgBody.transform.Translate(new Vector3(rgBody.position.x - 1 * speed, rgBody.position.y));
			}
			
			if (Input.GetKeyDown(KeyCode.D))
			{
				//rgBody.MovePosition(new Vector3(rgBody.position.x + 1 * speed, rgBody.position.y));
				rgBody.transform.Translate(new Vector3(rgBody.position.x + 1 * speed, rgBody.position.y));
			}
		}
	}
}