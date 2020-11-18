
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

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

        public GameObject lobbyUI;

        public GameObject playerList;

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

            // progressLabel.SetActive (true);
            controlPanel.SetActive (false);
            lobbyUI.SetActive(true);

            if (PhotonNetwork.IsConnected) {
                PhotonNetwork.JoinRandomRoom ();

            } else {
                isConnecting = PhotonNetwork.ConnectUsingSettings ();
                PhotonNetwork.GameVersion = gameVersion;

            }

        }

        public void Disconnect()
        {
            PhotonNetwork.LeaveRoom();
            
            progressLabel.SetActive (false);
            controlPanel.SetActive (true);
            lobbyUI.SetActive(false);
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster () {
            if (isConnecting) {
                Debug.Log ("ONCONNECTEDTOMASTER has been called");
                // PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected (DisconnectCause cause) {
            progressLabel.SetActive (false);
            controlPanel.SetActive (true);
            lobbyUI.SetActive(false);
            
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.InRoom)
            {
                var hostButton = GameObject.Find("PlayerLobby/Canvas/HostStartButton");
                hostButton.SetActive(false);
            } else {
                var playerButton = GameObject.Find("PlayerLobby/Canvas/PlayerLeaveButton");
                playerButton.SetActive(false);
            }

            isConnecting = false;

            Debug.LogWarningFormat ("Disconnected was called with reason {0}", cause);

        }

        public override void OnJoinRandomFailed (short returnCode, string message) {
            Debug.Log ("Joined random failed, creating room");

            PhotonNetwork.CreateRoom (PhotonNetwork.LocalPlayer.NickName + "'s Lobby", new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom () {

            Debug.Log ("Joined room called, now we're in a room");

            Debug.LogFormat ("Joined room with id {0}", PhotonNetwork.CurrentRoom.Name);
            Debug.LogFormat("Player List Count: {0}", PhotonNetwork.PlayerList.Length);
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.InRoom)
            {
                var hostButton = GameObject.Find("PlayerLobby/Canvas/HostStartButton");
                if (!hostButton.activeSelf)
                {
                    hostButton.SetActive(true);
                    JoinTeam();
                }
            } else
            {
                PhotonNetwork.CurrentRoom.AddPlayer(PhotonNetwork.LocalPlayer);
                var playerButton = GameObject.Find("PlayerLobby/Canvas/PlayerLeaveButton");
                if (!playerButton.activeSelf)
                {
                    playerButton.SetActive(true);
                    JoinTeam();
                }
            }
        }

        #endregion

        public void StartGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("GudgePlayground");
            }

        }
        


        private void JoinTeam()
        {
            var playerList = this.playerList.GetComponent<PlayerListController>().playersList;
            var alliesPanel = GameObject.Find("AlliesPanel");
            var axisPanel = GameObject.Find("AxisPanel");


            foreach (var player in playerList)
            {
                Debug.LogFormat("Player Name Start: {0}", player.transform.Find("PlayerName").GetComponent<Text>().text);

                if (player.transform.Find("PlayerName").GetComponent<Text>().text.Length == 0)
                {
                    Debug.LogFormat("Player Name: {0}", PhotonNetwork.LocalPlayer.NickName);
                    player.transform.Find("PlayerName").GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName;
                    GameObject obj = Instantiate(player);
                    obj.GetComponent<RectTransform>().anchoredPosition =
                        alliesPanel.transform.Find("Player Panel Box").transform.position;

                    obj.transform.SetParent(alliesPanel.transform);
                    Debug.LogFormat("Set PlayerName: {0}", player.transform.Find("PlayerName").GetComponent<Text>().text);
                    break;
                }
            }
        }
    }
}