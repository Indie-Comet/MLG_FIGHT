using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour {
	public float MaxSpeed;
	public float JumpForce;
	public string HorizontalAxis;
	public string VerticalAxis;
	public string JumpButton;
	
	public bool grounded = false;
	public Transform GroundCheck;
	public float GroundRadius;
	public LayerMask WhatIsGround;

	void Start () {
	
	}
	
	void FixedUpdate () {
		grounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, WhatIsGround);
	
		float move = Input.GetAxis (HorizontalAxis);
		
		rigidbody2D.velocity = new Vector2(move * MaxSpeed, rigidbody2D.velocity.y );
	}
	
	void Update() {
		if (grounded && Input.GetButtonDown(JumpButton)) {
			rigidbody2D.AddForce(new Vector2(0, JumpForce));
		}
	}
}
