using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControll : MonoBehaviour {

	public float walkSpeed = 10f;
	public float lookSensitivity = 3f;
	public Camera cameraThis;
	public float lookUpDownSensitivity = 0.5f;
	public float runSpeed = 2f;

	[SerializeField]
	[Header("Private Fields")]
	private Rigidbody rb;
	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private Vector3 cameraRotation = Vector3.zero;
	private bool rotateBool = true;
	private bool running = false;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//FROM UNITY DOCUMENTATION
		//float translation = Input.GetAxis("Vertical") * speed;
		float zAxis = Input.GetAxisRaw("Vertical");
		float xAxis = Input.GetAxisRaw ("Horizontal");
		//transform.Translate (xAxis, 0, yAxis);
		//rb.AddForce(xAxis, 0, zAxis, ForceMode.Force);
		if (Input.GetKeyDown (KeyCode.LeftShift) && Input.GetAxisRaw ("Vertical") > 0) {
			running = true;
			Debug.Log ("UE");
		} 
		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			running = false;
			Debug.Log ("EU");
		}

		Vector3 _movHorizontal = transform.right * xAxis;
		Vector3 _movVertical = transform.forward * zAxis;
		//final mov vector
		if (running) {
			Vector3 _velocity = (_movHorizontal + _movVertical).normalized * runSpeed;
			velocity = _velocity;
		} else {
			Vector3 _velocity = (_movHorizontal + _movVertical).normalized * walkSpeed;
			velocity = _velocity;
		}

		//apply vector
		PerformMovement();

		//calculate rotation as 3d vector
		float yRot = Input.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3 (0, yRot, 0) * lookSensitivity;
		rotation = _rotation;
		Rotate ();

		//calculate camera rotation
		float xRot = Input.GetAxisRaw("Mouse Y");

		Vector3 _cameraRotation = new Vector3 (xRot, 0, 0) * lookSensitivity * lookUpDownSensitivity;
		cameraRotation = _cameraRotation;
		CameraRotation ();
	}

	void PerformMovement(){
		if (velocity != Vector3.zero) {
			rb.MovePosition (rb.position + velocity * Time.fixedDeltaTime);
		}
	}

	void Rotate(){
		rb.MoveRotation (rb.rotation * Quaternion.Euler(rotation));
	}

	void CameraRotation(){
		//Debug.Log (cameraThis.transform.rotation.eulerAngles.x);
		//Debug.Log (-cameraRotation.x);

		if (cameraThis.transform.rotation.eulerAngles.x > 20 && cameraThis.transform.rotation.eulerAngles.x < 180) {
			if (-cameraRotation.x < 0) {
				rotateBool = true;
				cameraThis.transform.Rotate (-cameraRotation);
			}
		}

		if (cameraThis.transform.rotation.eulerAngles.x < 340 && cameraThis.transform.rotation.eulerAngles.x > 180) {
			if (-cameraRotation.x > 0) {
				rotateBool = true;
				cameraThis.transform.Rotate (-cameraRotation);
			}
		}

		if (rotateBool == true) {
			if (cameraThis.transform.rotation.eulerAngles.x >= 0 && cameraThis.transform.rotation.eulerAngles.x <= 20 || cameraThis.transform.rotation.eulerAngles.x >= 340 && cameraThis.transform.rotation.eulerAngles.x <= 360) {
				cameraThis.transform.Rotate (-cameraRotation);
			} else {
				if (cameraThis.transform.rotation.eulerAngles.x > 20 && cameraThis.transform.rotation.eulerAngles.x < 340) {
					rotateBool = false;
				}
			}

		}

	}
		

}
