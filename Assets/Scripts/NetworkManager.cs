using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace com.RoboticBanana.MoonBounce {
    public class NetworkManager : MonoBehaviourPunCallbacks {
        #region Private Serializable Fields
        [SerializeField]
        private byte maxPlayersPerRoom = 6;

        #endregion

        #region Private Fields
        /// game version is used to divide players between varying versions
        string gameVersion = "2";
        bool isConnecting;
        #endregion

        public GameObject controlPanel;

        public GameObject progressLabel;

        #region MonoBehaviour CallBacks
        void Awake () {

        }

        void Start () {
            // Connect ();
            progressLabel.SetActive (false);
            controlPanel.SetActive (true);
        }
        #endregion

        #region Public Methods
        public void Connect () {

            progressLabel.SetActive (true);
            controlPanel.SetActive (false);

            if (PhotonNetwork.IsConnected) {
                PhotonNetwork.JoinRandomRoom ();

            } else {
                isConnecting = PhotonNetwork.ConnectUsingSettings ();
                PhotonNetwork.GameVersion = gameVersion;

            }

        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster () {
            if (isConnecting) {
                Debug.Log ("ONCONNECTEDTOMASTER has been called");
                PhotonNetwork.JoinRandomRoom ();

            }
        }

        public override void OnDisconnected (DisconnectCause cause) {
            progressLabel.SetActive (false);
            controlPanel.SetActive (true);

            isConnecting = false;

            Debug.LogWarningFormat ("Disconnected was called with reason {0}", cause);

        }

        public override void OnJoinRandomFailed (short returnCode, string message) {
            Debug.Log ("Joined random failed, creating room");

            PhotonNetwork.CreateRoom (null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom () {

            Debug.Log ("Joined room called, now we're in a room");

            Debug.LogFormat ("Joined room with id {0}", PhotonNetwork.CurrentRoom.Name);

            PhotonNetwork.LoadLevel ("GudgePlayground");

        }

        #endregion

    }
}