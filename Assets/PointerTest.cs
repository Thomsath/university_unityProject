using UnityEngine;
using VRTK;
	
	public class PointerTest : VRTK_InteractableObject
	{
		public GameObject empty;
		private GameObject a;
		private string text = "Test";
		private GameObject Temptext;

		public override void StartUsing(VRTK_InteractUse usingObject)
		{
		
		base.StartUsing(usingObject);
		foreach (TextMesh m in gameObject.GetComponentsInChildren<TextMesh>()) {
			foreach(Renderer r in m.GetComponentsInChildren<Renderer>()){
				r.enabled = false;
			}
		}
		Debug.Log ("Start");
		}

		public override void StopUsing(VRTK_InteractUse usingObject)
		{
		base.StopUsing(usingObject);

		foreach (TextMesh m in gameObject.GetComponentsInChildren<TextMesh>()) {
			foreach(Renderer r in m.GetComponentsInChildren<Renderer>()){
				r.enabled = true;
			}
		}
		Debug.Log ("Stop");
		}

		protected void Start()
		{
		
		GameObject Temptext = Instantiate(empty, new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y + 1f, gameObject.transform.position.z), Quaternion.identity) as GameObject;
		Temptext.GetComponent<TextMesh>().text = text;
		Temptext.GetComponent<TextMesh>().color = Color.black;
		Temptext.transform.parent = gameObject.transform;



		}

		protected override void Update()
		{
		base.Update();
		}

	}
