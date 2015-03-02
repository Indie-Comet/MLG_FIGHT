using UnityEngine;
using System.Collections;

public class MLGHero : MonoBehaviour {
	public int StartHp = 10;
	public int Hp;
	public GameObject Part;
	public GameObject WinPart;
	public GameObject Tex;
	public float Speed;
	public Texture Deog;
	bool IsWin = false;
	bool IsWinningPart = false;
	bool IsGlasses = false;

	public GameObject Opponent;
	public GameObject Glasses;

	public Rect HpBarPos;
	public int HpInDoges;

	
	void Start() {
		Hp = StartHp;
		Part.SetActive(false);
		WinPart.SetActive(false);
	}


	void OnGUI() {
		int h = Screen.height;
		int w = Screen.width;
		int cnt = (HpInDoges * (Hp + (StartHp - 1) / HpInDoges)) / StartHp;
		for (int i = 0; i < cnt; i++) {
			GUI.DrawTexture(new Rect(
				w * HpBarPos.x,
				HpBarPos.y * h + (HpInDoges - i - 1) * h * HpBarPos.height,
				w * HpBarPos.width, 
				h * HpBarPos.height
			), Deog);
		}
	}

	Vector2 StartPosition;
	Vector2 GlassesStartPosition;
	float WinningTime;
	public GameObject glassPosition;
	float explosionStartTime;

	void Update() {
		if (IsWin) {
			GameObject cam = GameObject.Find("Main Camera");
			Vector2 cameraPos = new Vector2(cam.transform.position.x, cam.transform.position.y);
			transform.position = Vector2.Lerp(StartPosition, cameraPos, Time.time - WinningTime);
			Tex.transform.rotation = new Quaternion(0, 0, 0, 0);
		}
		if (IsWinningPart && (WinningTime + 1 <= Time.time)) {
			WinningExplosion ();
			IsWinningPart = false;
			explosionStartTime = Time.time;
		}
		if (IsGlasses) {
			Glasses.transform.position = Vector2.Lerp(
				GlassesStartPosition, 
				new Vector2(
					glassPosition.transform.position.x, 
					glassPosition.transform.position.y
				), 
				(Time.time - explosionStartTime) * 0.7F
			);
		}
	}

	void doLose() {
		Opponent.GetComponent<MLGHero>().doWin();
		Destroy(gameObject);
	}

	void WinningExplosion() {
		GameObject cam = GameObject.Find("Main Camera");
		GlassesStartPosition = new Vector2 (cam.transform.position.x, cam.transform.position.y + 7);
		Glasses.transform.position = GlassesStartPosition;
		IsGlasses = true;
		WinPart.SetActive(true);
	}

	void doWin() {
		IsWinningPart = true;
		StartPosition = new Vector2 (transform.position.x, transform.position.y);
		WinningTime = Time.time;
		IsWin = true;
	}
	
	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "Deadly shit" && other.gameObject != gameObject) {
			doLose();
		}
		if (other.gameObject.tag == "Hero" && other.gameObject != gameObject) {
			Vector3 tmp = (other.gameObject.transform.position + transform.position) / 2;
			tmp.z -= 5;
			Part.transform.position = tmp;			
			Hp -= 10;
			
			Tex.transform.rotation =  Quaternion.Lerp(Tex.transform.rotation, new Quaternion(0, 0, Random.value - 0.5F, Random.value), Time.time * Speed);
		}
		if (Hp <= 0) {
			Part.SetActive(false);
			doLose();
			//Application.LoadLevel("MLG_ULTIMATE");
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Hero" && other.gameObject != gameObject) {
			Part.SetActive(true);
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Hero" && other.gameObject != gameObject) {
			Part.SetActive(false);
			Tex.transform.rotation = new Quaternion(0, 0, 0, 0);
		}
	}
}

