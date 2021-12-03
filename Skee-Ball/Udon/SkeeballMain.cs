
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace Pyralix.SkeeBall
{
    public class SkeeballMain : UdonSharpBehaviour
    {
        public GameObject Lights;
        public GameObject BallBlocker;
        public GameObject PowerButton;
        public GameObject ThrowCountText;
        public GameObject ScoreText;
        public GameObject HighScoreText;
        public GameObject HighScoreNameText;
        public GameObject OwnerText;
        public AudioSource Speaker;
        public AudioClip HighScoreClip;
        [SerializeField] private Transform BallStorage;
        private Ball[] balls;
        private GameObject[] ballGameObjects;
        [SerializeField] private StartResetButton StartResetButton;
        [UdonSynced] public int HighScore;
        [UdonSynced] public string HighScoreName;
        [UdonSynced] public string OwnerName;
        [UdonSynced] public int Score;
        [UdonSynced] public int ThrowCount;
        [UdonSynced] public bool GameOver;
        [UdonSynced] public bool GameActive;
        private int blockCount;

        [SerializeField] private TextAsset VersionFile;
        private string version;

        private void Start()
        {
            version = VersionFile.text;

            balls = BallStorage.GetComponentsInChildren<Ball>();

            int ballCount = balls.Length;

            ballGameObjects = new GameObject[ballCount];

            for (int i = 0; i < ballCount; i++)
            {
                ballGameObjects[i] = balls[i].gameObject;
            }

            OwnerName = $"Skee-Ball {version} by Pyralix";
            OwnerText.GetComponent<Text>().text = $"{OwnerName}";
            RequestSerialization();
        }

        private void TogglePower()
        {
            if (Networking.IsOwner(gameObject))
            {
                if (!GameOver && !GameActive) //Starting a new game
                {
                    GameOver = false;
                    GameActive = true;
                    blockCount = 0;
                    ResetScore();
                    RequestSerialization();
                    //Lights start in the inactive state, the ballblocker starts in the active state
                    PowerLightsOn();
                    BallBlockerOff();
                    ResetThrowCount();
                    _ResetBlockerCount();
                }
                else if (!GameOver && GameActive) //Ending a game in progress
                {
                    GameOver = false;
                    GameActive = false;
                    RequestSerialization();
                    //Lights start in the inactive state, the ballblocker starts in the active state
                    PowerLightsOff();
                    BallBlockerOn();
                    ResetThrowCount();
                    _ResetBlockerCount();
                }
                else if (GameOver && !GameActive) //A game was finished either by timeout or all the balls were thrown
                {
                    GameOver = false;
                    GameActive = false;
                    if (Score > HighScore)
                    {
                        HighScore = Score;
                        HighScoreName = OwnerName;
                        HighScoreText.GetComponent<Text>().text = $"Highscore: {HighScore}";
                        HighScoreNameText.GetComponent<Text>().text = $"{HighScoreName}";
                        Speaker.PlayOneShot(HighScoreClip, 1.0f);
                    }
                    RequestSerialization();
                    //Lights start in the inactive state, the ballblocker starts in the active state
                    SendCustomNetworkEvent(NetworkEventTarget.All, "PowerLightsOff");
                    StartResetButton._TurnOffButtonLight();
                    BallBlockerOn();
                    ResetThrowCount();
                    _ResetBlockerCount();
                }
                else if (GameOver && GameActive)//The game cannot be active and over at the same time, reset everything because something strange happened
                {
                    //TODO: Reset all the things
                    Debug.Log("Somehow the game is over and still active... You're drunk, go home.");
                }
            }
        }

        public void _ScorePoints(int points)
        {
            if (GameOver || !GameActive) return;

            if (Networking.IsOwner(gameObject))
            {
                Score += points;
                ThrowCount++;
                if (ThrowCount >= 9) //Threw all balls TODO: Also a countdown timer
                {
                    GameOver = true;
                    GameActive = false;
                    TogglePower();
                }
                RequestSerialization();
            }

            ThrowCountText.GetComponent<Text>().text = $"Ball: {ThrowCount}";
            ScoreText.GetComponent<Text>().text = $"{Score}";
        }

        /// <summary>
        /// RPC Target
        /// </summary>
        public void PowerLightsOff()
        {
            Lights.SetActive(false);
        }

        /// <summary>
        /// RPC Target
        /// </summary>
        public void PowerLightsOn()
        {
            Lights.SetActive(true);
        }

        private void ResetThrowCount()
        {
            ThrowCount = 0;
            RequestSerialization();
            ThrowCountText.GetComponent<Text>().text = $"Ball: {ThrowCount}";

        }

        private void ResetScore()
        {
            Score = 0;
            RequestSerialization();
            ScoreText.GetComponent<Text>().text = $"{Score}";
        }

        //Interop Methods
        public void _SetGameOwnerAndTogglePower(VRCPlayerApi player)
        {
            OwnerName = player.displayName;
            RequestSerialization();
            Networking.SetOwner(player, gameObject);
            Networking.SetOwner(player, BallBlocker);
            Networking.SetOwner(player, Lights);
            Networking.SetOwner(player, PowerButton);
            Networking.SetOwner(player, ThrowCountText);
            Networking.SetOwner(player, ScoreText);
            Networking.SetOwner(player, OwnerText);
            Networking.SetOwner(player, HighScoreText);
            foreach(GameObject go in ballGameObjects)
            {
                Networking.SetOwner(player, go);
            }
            OwnerText.GetComponent<Text>().text = $"Player: {OwnerName}";
            TogglePower();
        }

        public void _SetGameOwner(VRCPlayerApi player)
        {
            Networking.SetOwner(player, gameObject);
            Networking.SetOwner(player, BallBlocker);
            Networking.SetOwner(player, Lights);
            Networking.SetOwner(player, PowerButton);
            Networking.SetOwner(player, ThrowCountText);
            Networking.SetOwner(player, ScoreText);
            Networking.SetOwner(player, OwnerText);
            Networking.SetOwner(player, HighScoreText);
            foreach (GameObject go in ballGameObjects)
            {
                Networking.SetOwner(player, go);
            }
        }

        public void _Reset(VRCPlayerApi player)
        {
            _SetGameOwner(player);

            if (Networking.IsOwner(gameObject))
            {
                BallBlockerOn();
                ResetAllBalls();
                OwnerName = $"Skee-Ball {version} by Pyralix";
                GameOver = false;
                GameActive = false;
                blockCount = 0;
                OwnerText.GetComponent<Text>().text = $"{OwnerName}";
                ResetThrowCount();
                ResetScore();
                PowerLightsOff();
                //ResetBalls();
                RequestSerialization();
            }
        }

        private void ResetAllBalls()
        {
            foreach (Ball ball in balls)
            {
                ball._Now();
            }
        }

        public override void OnPlayerJoined(VRCPlayerApi api)
        {
            RequestSerialization();
            if (Networking.IsOwner(gameObject))
            {
                if (Lights.activeSelf)
                {
                    SendCustomNetworkEvent(NetworkEventTarget.All, "PowerLightsOn");
                }
                else
                {
                    SendCustomNetworkEvent(NetworkEventTarget.All, "PowerLightsOff");
                }
            }
            OwnerText.GetComponent<Text>().text = $"{OwnerName}";
            ThrowCountText.GetComponent<Text>().text = $"Ball: {ThrowCount}";
            ScoreText.GetComponent<Text>().text = $"{Score}";
            HighScoreText.GetComponent<Text>().text = $"Highscore: {HighScore}";
            HighScoreNameText.GetComponent<Text>().text = $"{HighScoreName}";
        }

        public override void OnDeserialization()
        {
            OwnerText.GetComponent<Text>().text = $"{OwnerName}";
            ThrowCountText.GetComponent<Text>().text = $"Ball: {ThrowCount}";
            ScoreText.GetComponent<Text>().text = $"{Score}";
            HighScoreText.GetComponent<Text>().text = $"Highscore: {HighScore}";
            HighScoreNameText.GetComponent<Text>().text = $"{HighScoreName}";
            Lights.SetActive(GameActive);
        }

        //BallBlocker
        private void BallBlockerOn()
        {
            BallBlocker.SetActive(true);
        }

        private void BallBlockerOff()
        {
            BallBlocker.SetActive(false);
        }

        public void _IncrementBlockCount()
        {
            if (Networking.IsOwner(gameObject))
            {
                blockCount++;
                if (blockCount >= 9)
                {
                    BallBlockerOn();
                    _ResetBlockerCount();
                }
            }
        }

        public void _ResetBlockerCount()
        {
            blockCount = 0;
        }
    }
}
