using UnityEngine;

namespace Script
{
	public class Player2 : MonoBehaviour
	{
		public Transform player;
		public Sprite sprite1;
		public Sprite sprite2;
		private SpriteRenderer spriteRenderer;

		private Vector2 position;

		public Player2(Vector2 position)
		{
			this.position = position;
		}
		
		private void Start()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
			
			if (spriteRenderer.sprite == null)
			{
				spriteRenderer.sprite = sprite2;
			}

			spriteRenderer.transform.position = position;
			spriteRenderer.transform.localScale = new Vector2(0.5f, 0.5f);
		}

		public void Update()
		{
			spriteRenderer.transform.position = new Vector2(player.transform.position.x - 2, player.transform.position.y + 2);
			ChangeSprite();
		}

		public void ChangeSprite()
		{
			if (player.GetComponent<SpriteRenderer>().sprite == sprite1)
			{
				spriteRenderer.sprite = sprite2;
			}
			else
			{
				spriteRenderer.sprite = sprite1;
			}
			
			/*if (spriteRenderer.sprite == sprite2)
			{
				spriteRenderer.sprite = sprite1;
			}
			else
			{
				spriteRenderer.sprite = sprite2;
			}*/
		}
	}
}