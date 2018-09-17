using System.Collections;
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
			NormalScale();
		}

		public void Update()
		{
			spriteRenderer.transform.position = new Vector2(player.transform.position.x - 2, player.transform.position.y + 2);
			ChangeSprite();
		}

		private void ChangeSprite()
		{
			if (player.GetComponent<SpriteRenderer>().sprite == sprite1)
			{
				spriteRenderer.sprite = sprite2;
			}
			else
			{
				spriteRenderer.sprite = sprite1;
			}

			//StartCoroutine(ChangeSpriteCoroutine());
		}

		/*private IEnumerator ChangeSpriteCoroutine()
		{
			ScaleSprite();
			yield return new WaitForSeconds(Player.speed);
			NormalScale();
		}

		private void ScaleSprite()
		{
			spriteRenderer.transform.position = player.transform.position;
			spriteRenderer.transform.localScale = player.transform.localScale;
		}*/

		private void NormalScale()
		{
			spriteRenderer.transform.localScale = new Vector2(0.5f, 0.5f);
		}
	}
}