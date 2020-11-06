using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.RoboticBanana.MoonBounce {
    public class GameManager : MonoBehaviourPunCallbacks {

        public static GameManager Instance;

        public GameObject playerPrefab;

        public GameObject DummyBoi;

        // Start is called before the first frame update
        void Start () {
            Instance = this;

            if (PhotonNetwork.IsMasterClient) {
                PhotonNetwork.InstantiateRoomObject (DummyBoi.name, new Vector3 (0, 1002, -666), Quaternion.Euler (-35, 0, 0));

            }

            if (playerPrefab == null) {
                Debug.LogError ("<Color=Red><a>Missing player prefab</a></Color>");

            } else {
                Debug.Log ("We're instantiating localplayer");

                if (playerControl.LocalPlayerInstance == null) {
                    Debug.LogFormat ("We are instantiating local player from {0}", SceneManagerHelper.ActiveSceneName);
                    PhotonNetwork.Instantiate (playerPrefab.name, new Vector3 (-1, 995, -675), Quaternion.identity, 0);

                } else {
                    Debug.LogFormat ("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);

                }
            }
        }

        // Update is called once per frame
        void Update () {

        }

        void LoadGame () {
            if (!PhotonNetwork.IsMasterClient) {
                Debug.LogError ("Error loading master photon client");

            }

            Debug.LogFormat ("Photon loading level with playercount {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel ("GudgePlayground");

        }

        public override void OnPlayerEnteredRoom (Player other) {
            Debug.LogFormat ("Nickname entered room: {0}", other.NickName);

            if (PhotonNetwork.IsMasterClient) {
                // Debug.LogFormat ("Player entered room is Master Client: {0}", PhotonNetwork.IsMasterClient);

                // LoadGame ();
            }
        }

        public override void OnPlayerLeftRoom (Player otherPlayer) {
            Debug.LogFormat ("Nickname left room: {0}", PhotonNetwork.NickName);

            if (PhotonNetwork.IsMasterClient) {
                // Debug.LogFormat ("Player left rom and is master client: {0}", PhotonNetwork.IsMasterClient);

                // LoadGame ();
            }
        }

        public override void OnLeftRoom () {
            SceneManager.LoadScene (0);
        }

        public void LeaveRoom () {
            Cursor.lockState = CursorLockMode.None;

            PhotonNetwork.LeaveRoom ();

        }
    }
}