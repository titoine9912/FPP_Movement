using UnityEngine;
using UnityEngine.UI;

namespace Script
{
	public class Player : MonoBehaviour
	{
		private Rigidbody2D rgBody;
		private float speed = 1;
		
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
	}
}