using UnityEngine;
using System.Collections;

public class MLG_MOVES : MonoBehaviour {

	public Texture Sonic;
	public float SonicMoveChance;
	public float Speed;
	public float Radius;
	public TrailRenderer Trail;
	public AudioSource Sound;
	Vector2 begin;
	Vector2 end;
	float lastTime;
	float nextTime;

	void recast() {
		Trail.enabled = false;
		lastTime = Time.time;
		GameObject camera = GameObject.Find("Main Camera");
		Vector2 cameraPos = new Vector2(camera.transform.position.x, camera.transform.position.y);
		//end = Random.insideUnitCircle * Radius + cameraPos;
		begin = new Vector2(Random.value - 0.5F, Random.value - 0.5F);
		begin.Normalize();
		begin *= Radius;
		end = -1 * begin;
		begin += cameraPos;
		end += cameraPos;		
		nextTime = Time.time + (Random.value) * SonicMoveChance * 2 + SonicMoveChance;
		Sound.Play();
	}
	

	// Use this for initialization
	void Start () {
		lastTime = Time.time;
		recast();
	}
	
	void Update() {
		if (Time.time > nextTime) {
			recast();
		}
		transform.position = Vector2.Lerp (begin, end, (Time.time - lastTime) * Speed);
		Trail.enabled = true;
	}
}
