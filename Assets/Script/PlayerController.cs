using UnityEngine;
using System.Collections;

namespace Script
{
	public class PlayerController:MonoBehaviour
	{
        private float playerxValue;
        private Rigidbody2D playerBody;
        public float playerSpeed;


        private void Start()
        {
            playerBody = GetComponent<Rigidbody2D>();
            playerBody.velocity = new Vector2(playerSpeed, 0);
        }

        private void Update()
        {
            
        }
    }
}