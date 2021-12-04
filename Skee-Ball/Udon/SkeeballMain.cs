
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace Pyralix.SkeeBall
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class SkeeballMain : UdonSharpBehaviour
    {
        [SerializeField] private GameObject Lights;
        [SerializeField] private GameObject BallBlocker;
        [SerializeField] private GameObject PowerButton;
        [SerializeField] private GameObject ThrowCountText;
        [SerializeField] private GameObject ScoreText;
        [SerializeField] private GameObject HighScoreText;
        [SerializeField] private GameObject HighScoreNameText;
        [SerializeField] private GameObject OwnerText;
        [SerializeField] private AudioSource Speaker;
        [SerializeField] private AudioClip HighScoreClip;
        [SerializeField] private Transform BallStorage;
        private Ball[] balls;
        private GameObject[] ballGameObjects;
        [SerializeField] private StartResetButton StartResetButton;
        [UdonSynced] private int highScore;
        [UdonSynced] private string highScoreName;
        [UdonSynced] private string ownerName;
        [UdonSynced] private int score;
        [UdonSynced] private int throwCount;
        [UdonSynced] private bool gameOver;
        [UdonSynced] private bool gameActive;
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

            ownerName = $"Skee-Ball {version} by Pyralix";
            OwnerText.GetComponent<Text>().text = $"{ownerName}";
            RequestSerialization();
        }

        private void TogglePower()
        {
            if (Networking.IsOwner(gameObject))
            {
                if (!gameOver && !gameActive) //Starting a new game
                {
                    gameOver = false;
                    gameActive = true;
                    blockCount = 0;
                    ResetScore();
                    RequestSerialization();
                    //Lights start in the inactive state, the ballblocker starts in the active state
                    PowerLightsOn();
                    BallBlockerOff();
                    ResetThrowCount();
                    ResetBlockerCount();
                }
                else if (!gameOver && gameActive) //Ending a game in progress
                {
                    gameOver = false;
                    gameActive = false;
                    RequestSerialization();
                    //Lights start in the inactive state, the ballblocker starts in the active state
                    PowerLightsOff();
                    BallBlockerOn();
                    ResetThrowCount();
                    ResetBlockerCount();
                }
                else if (gameOver && !gameActive) //A game was finished either by timeout or all the balls were thrown
                {
                    gameOver = false;
                    gameActive = false;
                    if (score > highScore)
                    {
                        highScore = score;
                        highScoreName = ownerName;
                        HighScoreText.GetComponent<Text>().text = $"Highscore: {highScore}";
                        HighScoreNameText.GetComponent<Text>().text = $"{highScoreName}";
                        Speaker.PlayOneShot(HighScoreClip, 1.0f);
                    }
                    RequestSerialization();
                    //Lights start in the inactive state, the ballblocker starts in the active state
                    SendCustomNetworkEvent(NetworkEventTarget.All, "PowerLightsOff");
                    StartResetButton._TurnOffButtonLight();
                    BallBlockerOn();
                    ResetThrowCount();
                    ResetBlockerCount();
                }
                else if (gameOver && gameActive)//The game cannot be active and over at the same time, reset everything because something strange happened
                {
                    //TODO: Reset all the things
                    Debug.Log("Somehow the game is over and still active... You're drunk, go home.");
                }
            }
        }

        public void _ScorePoints(int points)
        {
            if (gameOver || !gameActive) return;

            if (Networking.IsOwner(gameObject))
            {
                score += points;
                throwCount++;
                if (throwCount >= 9) //Threw all balls TODO: Also a countdown timer
                {
                    gameOver = true;
                    gameActive = false;
                    TogglePower();
                }
                RequestSerialization();
            }

            ThrowCountText.GetComponent<Text>().text = $"Ball: {throwCount}";
            ScoreText.GetComponent<Text>().text = $"{score}";
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
            throwCount = 0;
            RequestSerialization();
            ThrowCountText.GetComponent<Text>().text = $"Ball: {throwCount}";

        }

        private void ResetScore()
        {
            score = 0;
            RequestSerialization();
            ScoreText.GetComponent<Text>().text = $"{score}";
        }

        //Interop Methods
        public void _SetGameOwnerAndTogglePower(VRCPlayerApi player)
        {
            ownerName = player.displayName;
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
            OwnerText.GetComponent<Text>().text = $"Player: {ownerName}";
            TogglePower();
        }

        private void SetGameOwner(VRCPlayerApi player)
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
            SetGameOwner(player);

            if (Networking.IsOwner(gameObject))
            {
                BallBlockerOn();
                ResetAllBalls();
                ownerName = $"Skee-Ball {version} by Pyralix";
                gameOver = false;
                gameActive = false;
                blockCount = 0;
                OwnerText.GetComponent<Text>().text = $"{ownerName}";
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
                ball._ResetBall();
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
            OwnerText.GetComponent<Text>().text = $"{ownerName}";
            ThrowCountText.GetComponent<Text>().text = $"Ball: {throwCount}";
            ScoreText.GetComponent<Text>().text = $"{score}";
            HighScoreText.GetComponent<Text>().text = $"Highscore: {highScore}";
            HighScoreNameText.GetComponent<Text>().text = $"{highScoreName}";
        }

        public override void OnDeserialization()
        {
            OwnerText.GetComponent<Text>().text = $"{ownerName}";
            ThrowCountText.GetComponent<Text>().text = $"Ball: {throwCount}";
            ScoreText.GetComponent<Text>().text = $"{score}";
            HighScoreText.GetComponent<Text>().text = $"Highscore: {highScore}";
            HighScoreNameText.GetComponent<Text>().text = $"{highScoreName}";
            Lights.SetActive(gameActive);
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
                    ResetBlockerCount();
                }
            }
        }

        private void ResetBlockerCount()
        {
            blockCount = 0;
        }
    }
}
