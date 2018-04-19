using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastCollisionChecks : MonoBehaviour {

	private struct RaycastOrigins
	{
		public Vector2 botLeft, botRight, topLeft, topRight;
	}

	public LayerMask collisionMask;
	private BoxCollider2D bc;
	private RaycastOrigins raycastOrigins;

	private float rayLengthVertical = 0.1f;
	private float rayLengthHorizontal = 0.1f;
	private const float dstBetweenRays = 0.15f;

	private int horizontalRayCount;
	private int verticalRayCount;
	private float horizontalRaySpacing;
	private float verticalRaySpacing;

	public bool up, down, left, right;
	
	void Start () {
		bc = GetComponent<BoxCollider2D>();
	}
	
	void Update (){
		UpdateRaycastOrigins();
		CalculateRaySpacing();
		UpdateCollisions();
	}

	private void UpdateRaycastOrigins(){
		Bounds bounds = bc.bounds;

		raycastOrigins.botLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.botRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	public void CalculateRaySpacing() {
		Bounds bounds = bc.bounds;

		float boundsWidth = bounds.size.x;
		float boundsHeight = bounds.size.y;

		horizontalRayCount = Mathf.RoundToInt (boundsHeight / dstBetweenRays);
		verticalRayCount = Mathf.RoundToInt (boundsWidth / dstBetweenRays);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x  / (verticalRayCount - 1);
	}

	private void UpdateCollisions(){

		left = LeftCollision();
		right = RightCollision();
		up = UpCollision();
		down = DownCollision();
	}

	bool UpCollision(){
		bool collision = false;

		for (int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigin = raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i);
			
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLengthVertical, collisionMask);
			Debug.DrawRay(rayOrigin, Vector2.up * rayLengthVertical, Color.red);

			if (hit) {
				collision = true;
			}
		}

		if (!collision) {
			collision = false;
		}

		return collision;
	}

	bool DownCollision() {
		bool collision = false;

		for (int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigin = raycastOrigins.botLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i);

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLengthVertical, collisionMask);
			Debug.DrawRay(rayOrigin, Vector2.down * rayLengthVertical, Color.red);

			if (hit) {
				collision = true;
			}
		}

		if (!collision) {
			collision = false;
		}

		return collision;
	}

	bool LeftCollision(){
		bool collision = false;

		for (int i = 0; i < horizontalRayCount; i++) {
			Vector2 rayOrigin = raycastOrigins.botLeft;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.left, rayLengthHorizontal, collisionMask);
			Debug.DrawRay(rayOrigin, Vector2.left * rayLengthVertical, Color.red);

			if (hit) {
				collision = true;
			}
		}

		if (!collision) {
			collision = false;
		}

		return collision;
	}

	bool RightCollision() {
		bool collision = false;

		for (int i = 0; i < horizontalRayCount; i++) {
			Vector2 rayOrigin = raycastOrigins.botRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, rayLengthHorizontal, collisionMask);
			Debug.DrawRay(rayOrigin, Vector2.right * rayLengthVertical, Color.red);

			if (hit) {
				collision = true;
			}
		}

		if (!collision) {
			collision = false;
		}

		return collision;
	}
}
