using System.Collections;
using System.Collections.Generic;
using com.RoboticBanana.MoonBounce;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (DamageableEntity))]
public class playerControl : MonoBehaviourPunCallbacks, IPunObservable {

	public static GameObject LocalPlayerInstance;

	public Camera ourCamera;
	public Rigidbody ourRigidbody;
	public CapsuleCollider ourCollider;

	public Text playerNicknameText;

	public float MinimumY;
	public float MaximumY;

	float currentUpDownLookRotation = 0f;

	public float jumpForce = 3f;

	public float lookSensitivity;

	public float movementSpeed;
	public float sprintModifier;
	public Vector3 maxSpeed;

	private Vector3 movementThisFrame;

	public Weapon currentWeapon;
	public WeaponInventory ourWeaponInventory;

	public GravityAffectedPlayer ourGravityPlayer;

	public DamageableEntity ourDamageableEntity;

	private bool cursorLockstate = true;

	void Awake () {
		if (photonView.IsMine) {
			playerControl.LocalPlayerInstance = this.gameObject;
			ourDamageableEntity = GetComponent<DamageableEntity> ();

		}

		playerNicknameText.text = photonView.Owner.NickName;

		DontDestroyOnLoad (this.gameObject);

	}

	// Use this for initialization
	void Start () {
		if (!photonView.IsMine) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	// Update is called once per frame
	void Update () {
		if (!photonView.IsMine) {
			ourCamera.enabled = false;
			ourCamera.GetComponent<AudioListener> ().enabled = false;
			ourRigidbody.isKinematic = true;

			if (PhotonNetwork.IsConnected == true) return; //during development, we may want to test this prefab without being connected. In a dummy scene for example, just to create and validate code that is not related to networking features

		}

		if (Input.GetKey (KeyCode.Q)) Application.Quit ();

		if (Input.GetKey (KeyCode.Escape)) {
			cursorLockstate = !cursorLockstate;
		}

		if (Input.GetKey (KeyCode.P) || cursorLockstate != true) {
			Cursor.lockState = CursorLockMode.None;
			cursorLockstate = false;
			Cursor.visible = true;

		} else if (cursorLockstate == true) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

		}

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			currentWeapon = ourWeaponInventory.SwapWeapon (0);
		}

		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			currentWeapon = ourWeaponInventory.SwapWeapon (1);
		}

		if (Input.GetMouseButton (0)) {
			TryFireWeapon ();
		}

		movementThisFrame = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));

		Vector2 mouseLook = new Vector2 (Input.GetAxis ("Mouse X"), -Input.GetAxis ("Mouse Y"));
		currentUpDownLookRotation += (mouseLook.y * lookSensitivity);
		currentUpDownLookRotation = Mathf.Clamp (currentUpDownLookRotation, MinimumY, MaximumY);

		transform.RotateAround (ourCamera.transform.parent.position, this.transform.up, mouseLook.x * lookSensitivity);

		ourCamera.transform.localRotation = Quaternion.Euler (currentUpDownLookRotation, ourCamera.transform.localRotation.eulerAngles.y, ourCamera.transform.localRotation.eulerAngles.z);

		if (Input.GetKeyDown (KeyCode.Space) && isGrounded ()) {

			ourRigidbody.AddForce (this.transform.up * jumpForce, ForceMode.VelocityChange);

		}

		if (ourDamageableEntity.health <= 0) {
			GameManager.Instance.LeaveRoom ();

		}
	}

	public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
		if (stream.IsWriting) {
			//this is our character since it's in writing state
			// stream.SendNext()

			stream.SendNext (ourWeaponInventory.activeWeaponIndex);
		} else {
			//this is a network player because it's in Read, not write
			//stream.ReceiveNext();

			ourWeaponInventory.SwapWeapon((int) stream.ReceiveNext ());
		}
	}

	void FixedUpdate () {
		if (!photonView.IsMine) return;
		Vector3 oldVel = transform.InverseTransformDirection (ourRigidbody.velocity);

		Vector3 newVel = new Vector3 (movementThisFrame.x, 0, movementThisFrame.z);

		if (newVel == Vector3.zero) {
			newVel = oldVel;
			newVel.x /= -4;
			newVel.y = 0;
			newVel.z /= -4;
		} else if (newVel.x == 0) {
			newVel.x = oldVel.x / -4;
		} else if (newVel.z == 0) {
			newVel.z = oldVel.z / -4;
		}

		ourRigidbody.AddForce (transform.TransformDirection (newVel), ForceMode.VelocityChange);

		newVel = transform.InverseTransformDirection (ourRigidbody.velocity);
		if (newVel.x > 15) newVel.x = 15;
		if (newVel.x < -15) newVel.x = -15;
		if (newVel.z > 15) newVel.z = 15;
		if (newVel.z < -15) newVel.z = -15;
		ourRigidbody.velocity = transform.TransformDirection (newVel);

		ourRigidbody.angularVelocity = Vector3.zero;

		if (ourGravityPlayer != null && ourGravityPlayer.planetCollider != null) {
			ourRigidbody.AddForce ((ourGravityPlayer.planetCollider.transform.position - transform.position).normalized * 9.81f, ForceMode.Acceleration);

		}

		Debug.DrawLine (transform.position, transform.position + ourRigidbody.velocity, Color.blue, 0.1f);
	}

	bool isGrounded () {

		if (ourGravityPlayer.planetCollider == null) return false;

		RaycastHit hit;

		Physics.Raycast (transform.position, ourGravityPlayer.planetCollider.transform.position - transform.position, out hit, 2f);

		return hit.collider;

	}

	private void TryFireWeapon () {
		if (ourWeaponInventory.weaponList[ourWeaponInventory.activeWeaponIndex].PullTheoreticalTrigger ()) {
			photonView.RPC ("FireWeapon", RpcTarget.All);

		}
	}

	[PunRPC]
	public void FireWeapon () {
		ourWeaponInventory.weaponList[ourWeaponInventory.activeWeaponIndex].FireWeapon ();
	}
}