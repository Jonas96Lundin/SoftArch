using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Kodad av: Johan Melkersson
/// </summary>
public class ParallaxBackground : MonoBehaviour
{
	[SerializeField]
	private Vector2 parallaxEffect;
	[SerializeField]
	private bool infiniteHorizontal;
	[SerializeField]
	private bool infiniteVertical;
	[SerializeField]
	private bool object3D;

	private Transform cameraTransform;
	private Vector3 lastCameraPos;

	private float textureSizeX;
	private float textureSizeY;

	public static Transform backgrondObject;

	void Start()
	{
		cameraTransform = Camera.main.transform;
		Camera camera = Camera.main;
		lastCameraPos = cameraTransform.position;

		////Canvas
		//if (object3D)
		//{
		//	RectTransform rectTransform = GetComponent<RectTransform>();
		//	textureSizeX = rectTransform.localScale.x / 4;
		//	textureSizeY = rectTransform.localScale.y / 4;

		//}
		//else
		//{
		//	Sprite sprite = GetComponent<SpriteRenderer>().sprite;
		//	Texture2D texture = sprite.texture;
		//	textureSizeX = texture.width;
		//	textureSizeY = texture.height;
		//}

		//Regular and Canvas
		if (object3D)
		{
			Transform rectTransform = GetComponent<Transform>();
			textureSizeX = rectTransform.localScale.x / 4;
			textureSizeY = rectTransform.localScale.y / 4;

		}
		else
		{
			Sprite sprite = GetComponent<SpriteRenderer>().sprite;
			Texture2D texture = sprite.texture;
			textureSizeX = (texture.width / sprite.pixelsPerUnit) * transform.localScale.x;
			textureSizeY = (texture.height / sprite.pixelsPerUnit) * transform.localScale.y;
		}

	}

	void LateUpdate()
	{
		Vector3 deltaMovement = lastCameraPos - cameraTransform.position;
		transform.position += new Vector3(deltaMovement.x * parallaxEffect.x, deltaMovement.y * parallaxEffect.y, 0);
		lastCameraPos = cameraTransform.position;

		////Canvas
		//if (infiniteHorizontal)
		//{
		//	if (Mathf.Abs(transform.localPosition.x) >= textureSizeX)
		//	{
		//		float offsetPositionX = transform.localPosition.x % textureSizeX;
		//		transform.localPosition = new Vector3(offsetPositionX, transform.localPosition.y);
		//	}
		//}
		//if (infiniteVertical)
		//{
		//	if (Mathf.Abs(transform.localPosition.y) >= textureSizeY)
		//	{
		//		float offsetPositionY = transform.localPosition.y % textureSizeY;
		//		transform.localPosition = new Vector3(transform.localPosition.x, offsetPositionY);
		//	}
		//}

		//Regular
		if (infiniteHorizontal)
		{
			if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureSizeX)
			{
				float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureSizeX;
				transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y, transform.position.z);
			}
		}
		if (infiniteVertical)
		{
			if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureSizeY)
			{
				float offsetPositionY = (cameraTransform.position.y - transform.position.y) % textureSizeY;
				transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offsetPositionY, transform.position.z);
			}
		}
	}
}

