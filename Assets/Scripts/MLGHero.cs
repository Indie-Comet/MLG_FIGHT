using UnityEngine;
using System.Collections;

public class MLGHero : MonoBehaviour {
	public ParticleSystem[] Parts;
	public GameObject WinPart;
	public GameObject Tex;
	public float RotationSpeed;
	bool isFighting = false;
	Vector3 partsPoint;

	public GameObject Opponent;
	public Transform GlassesPosition;
	public GameObject Glasses;

	public int StartHp = 10;
	public int Hp;
	public Rect HpBarPos;
	public Texture Deog;
	public int HpInDoges;
	public AudioSource Wow;
	public AudioSource Boom;
	public Animator Explosion;

	void Start() {
		Hp = StartHp;
		WinPart.SetActive(false);
	}

	void DoFight() {
		Hp -= 10;
		Tex.transform.rotation =  Quaternion.Lerp(Tex.transform.rotation, new Quaternion(0, 0, Random.value - 0.5F, Random.value), Time.time * RotationSpeed);
		foreach (var i in Parts) {
			i.transform.position = partsPoint;
			if (Random.value < 0.5) i.Emit(1);
		}
	}
	
	void Update() {
		if (isFighting)
			DoFight();
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
	
	void doLose() {
		Opponent.GetComponent<MLGHero>().doWin();
		Destroy(gameObject);
	}

	void doWow() {
		Wow.Play();
	}

	public RuntimeAnimatorController ExplosionController;
	void doExplosion() {
		//Explosion.Play ("Explosion");
		Explosion.runtimeAnimatorController = ExplosionController;
		Boom.Play ();
	}

	IEnumerator PutGlasses() {
		float pos = 0;
		GameObject cam = GameObject.Find("Main Camera");
		Vector2 begin = new Vector2 (cam.transform.position.x, cam.transform.position.y + 7);
		Vector2 end = GlassesPosition.position;
		while (true) {
			Debug.DrawLine(begin, GlassesPosition.position, Color.red);
			Glasses.transform.position = Vector3.Lerp(begin, end, Mathf.Min(1, pos));
			float last_pos = pos;
			pos += Time.deltaTime;
			yield return null;
			if (pos >= 2 && last_pos <= 2) {
				doExplosion();
			}
			if (pos >= 1 && last_pos <= 1) {
				doWow();
			}
		}
	}

	IEnumerator WinningExplosion() {
		yield return new WaitForSeconds(1);
		StartCoroutine("PutGlasses");
		WinPart.SetActive(true);
		yield return new WaitForSeconds (2.55F);
		Application.LoadLevel("MLG_ULTIMATE");
	}
	
	IEnumerator SlowMove() {
		float pos = 0;
		Vector3 start = transform.position;
		GameObject cam = GameObject.Find("Main Camera");
		Vector3 end = cam.transform.position;
		end.z += 10;
		while (true) {
			transform.position = Vector3.Lerp(start, end, pos);
			yield return null;
			pos += Time.deltaTime;
		}
	}

	void doWin() {
		isFighting = false;
		Tex.transform.rotation = new Quaternion(0, 0, 0, 0);
		StartCoroutine("WinningExplosion");
		StartCoroutine("SlowMove");
	}
	
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Deadly shit" && other.gameObject != gameObject) {
			doLose();
		}
		if (other.gameObject.tag == "Hero" && other.gameObject != gameObject) {
			Vector3 tmp = (other.gameObject.transform.position + transform.position) / 2;
			tmp.z -= 5;
			partsPoint = tmp;
		}
		if (Hp <= 0) {
			doLose();
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Hero" && other.gameObject != gameObject) {
			isFighting = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Hero" && other.gameObject != gameObject) {
			Tex.transform.rotation = new Quaternion(0, 0, 0, 0);
			isFighting = false;
		}
	}
}

