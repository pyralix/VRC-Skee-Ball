using System;
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

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
    public GameObject Ball1;
    public GameObject Ball2;
    public GameObject Ball3;
    public GameObject Ball4;
    public GameObject Ball5;
    public GameObject Ball6;
    public GameObject Ball7;
    public GameObject Ball8;
    public GameObject Ball9;
    public GameObject Ball10;
    public AudioSource Speaker;
    public AudioClip HighScoreClip;
    [SerializeField] private Ball01Reset Ball1Reset;
    [SerializeField] private Ball2Reset Ball2Reset;
    [SerializeField] private Ball3Reset Ball3Reset;
    [SerializeField] private Ball4Reset Ball4Reset;
    [SerializeField] private Ball05Reset Ball5Reset;
    [SerializeField] private Ball6Reset Ball6Reset;
    [SerializeField] private Ball7Reset Ball7Reset;
    [SerializeField] private Ball8Reset Ball8Reset;
    [SerializeField] private Ball9Reset Ball9Reset;
    [SerializeField] private Ball10Reset Ball10Reset;
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
                ResetBlockerCount();
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
                ResetBlockerCount();
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
                StartResetButton.TurnOffButtonLight();
                BallBlockerOn();
                ResetThrowCount();
                ResetBlockerCount();
            }
            else if(GameOver && GameActive)//The game cannot be active and over at the same time, reset everything because something strange happened
            {
                //TODO: Reset all the things
                Debug.Log("Somehow the game is over and still active... You're drunk, go home.");
            }
        }
    }
    public void ScorePoints(int points)
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
    public void PowerLightsOff()
    {
        Lights.SetActive(false);
    }
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
    public void SetGameOwnerAndTogglePower(VRCPlayerApi player)
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
        Networking.SetOwner(player, Ball1);
        Networking.SetOwner(player, Ball2);
        Networking.SetOwner(player, Ball3);
        Networking.SetOwner(player, Ball4);
        Networking.SetOwner(player, Ball5);
        Networking.SetOwner(player, Ball6);
        Networking.SetOwner(player, Ball7);
        Networking.SetOwner(player, Ball8);
        Networking.SetOwner(player, Ball9);
        Networking.SetOwner(player, Ball10);
        OwnerText.GetComponent<Text>().text = $"Player: {OwnerName}";
        TogglePower();
    }

    public void SetGameOwner(VRCPlayerApi player)
    {
        Networking.SetOwner(player, gameObject);
        Networking.SetOwner(player, BallBlocker);
        Networking.SetOwner(player, Lights);
        Networking.SetOwner(player, PowerButton);
        Networking.SetOwner(player, ThrowCountText);
        Networking.SetOwner(player, ScoreText);
        Networking.SetOwner(player, OwnerText);
        Networking.SetOwner(player, HighScoreText);
        Networking.SetOwner(player, Ball1);
        Networking.SetOwner(player, Ball2);
        Networking.SetOwner(player, Ball3);
        Networking.SetOwner(player, Ball4);
        Networking.SetOwner(player, Ball5);
        Networking.SetOwner(player, Ball6);
        Networking.SetOwner(player, Ball7);
        Networking.SetOwner(player, Ball8);
        Networking.SetOwner(player, Ball9);
        Networking.SetOwner(player, Ball10);
    }

    public void Reset(VRCPlayerApi player)
    {
        SetGameOwner(player);

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
        Ball1Reset.Now();
        Ball2Reset.Now();
        Ball3Reset.Now();
        Ball4Reset.Now();
        Ball5Reset.Now();
        Ball6Reset.Now();
        Ball7Reset.Now();
        Ball8Reset.Now();
        Ball9Reset.Now();
        Ball10Reset.Now();
    }
    //Events
    public void Start()
    {
        version = VersionFile.text;

        OwnerName = $"Skee-Ball {version} by Pyralix";
        OwnerText.GetComponent<Text>().text = $"{OwnerName}";
        RequestSerialization();
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

    public void IncrementBlockCount()
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

    public void ResetBlockerCount()
    {
        blockCount = 0;
    }
}