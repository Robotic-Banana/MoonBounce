using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace com.RoboticBanana.MoonBounce {
    public class GameManager : MonoBehaviourPunCallbacks {

        public static GameManager Instance;

        public GameObject playerPrefab;

        public GameObject DummyBoi;

        public Transform[] RespawnPoints;

        public GameObject respawnPanel;
        public Text respawnText;

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

                    Transform spawnLocation = PickRespawnPoint();
                    PhotonNetwork.Instantiate (playerPrefab.name, spawnLocation.transform.position, Quaternion.identity, 0);

                } else {
                    Debug.LogFormat ("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);

                }
            }
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
        }

        public override void OnPlayerLeftRoom (Player otherPlayer) {
            Debug.LogFormat ("Nickname left room: {0}", PhotonNetwork.NickName);
        }

        public override void OnLeftRoom () {
            SceneManager.LoadScene (0);
        }

        public void LeaveRoom () {
            Cursor.lockState = CursorLockMode.None;
            PhotonNetwork.LeaveRoom ();
        }

        public Transform PickRespawnPoint () {
            int chosenPoint = Random.Range (0, RespawnPoints.Length);

            return RespawnPoints[chosenPoint];

        }

        public void UpdateRespawnGraphic (int number) {
            if (number > 0) {
                respawnPanel.SetActive (true);

            } else {
                respawnPanel.SetActive (false);

            }

            respawnText.text = (number).ToString ();

        }
    }
}