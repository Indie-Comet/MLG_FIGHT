using UnityEngine;
using System.Collections;

public class MLG : MonoBehaviour {
	public int StartHp = 10;
	public int Hp;
	public GameObject Part;
	public GameObject Tex;
	public float Speed;
	public Texture Deog;
	public bool IsSnoopDog;
	
	public GUIStyle Style;
	
	void Start() {
		Hp = StartHp;
		Part.SetActive(false);
	}
	
	void OnGUI() {
		if (IsSnoopDog) {
			int h = Screen.height;
			h = h * 15 / 16;
			int w = Screen.width;
			int cnt = (10 * (Hp + (StartHp - 1) / 10)) / StartHp;
			//Debug.Log(cnt);
			for (int i = 0; i < cnt; i++) {
				GUI.DrawTexture(new Rect(9 * w / 10, (10 - i - 1) * h / 10 + h / 20, w / 11, h / 10), Deog);
			}
		} else {
			int h = Screen.height;
			h = h * 15 / 16;
			int w = Screen.width;
			int cnt = (10 * (Hp + (StartHp - 1) / 10)) / StartHp;
			//Debug.Log(cnt);
			for (int i = 0; i < cnt; i++) {
				GUI.DrawTexture(new Rect(w / 80, (10 - i - 1) * h / 10 + h / 20, w / 11, h / 10), Deog);
			}
		}
	}
	
	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "Hero" && other.gameObject != gameObject) {
			Vector3 tmp = (other.gameObject.transform.position + transform.position) / 2;
			tmp.z -= 5;
			Part.transform.position = tmp;			
			Hp -= 10;
			
			Tex.transform.rotation =  Quaternion.Lerp(Tex.transform.rotation, new Quaternion(0, 0, Random.value - 0.5F, Random.value), Time.time * Speed);
		}
		if (Hp <= 0) {
			Application.LoadLevel("MLG_ULTIMATE");
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

