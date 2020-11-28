﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinLose : MonoBehaviour
{

    public int tileCount;

    bool canWin = true;
    bool canLose = true;

    //Tile required variables
    //Nature
    public int requiredNatureTiles; //public can be removed, currently for testing purposes here
    [Range (0, 1)]
    [SerializeField] float requiredNatureTilePercent;
    public int currentNatureTiles; //public can be removed, currently for testing purposes here

    //Toxic
    public int requiredToxicTiles; //public can be removed, currently for testing purposes here
    [Range(0, 1)]
    [SerializeField] float requiredToxicTilePercent;
    public int currentToxicTiles; //public can be removed, currently for testing purposes here

    [SerializeField] GameObject WinPopUp;
    [SerializeField] GameObject LosePopUp;

    //Timer Variables
    [Tooltip("The time in seconds")]
    public float timeRemaining = 10;
    bool timerIsRunning = false;
    [SerializeField] Text timeText;

    //Population related variables
    [SerializeField] bool populationDeathCanTrigger = false;
    [SerializeField] int popRequiredForTrigger;

    #region Default
    void Start()
    {
        timerIsRunning = true;
        TileAmountCalculation();
    }

    void Update()
    {
        CalcTime();
        CheckTime();
    }
    #endregion

    #region Tiles
    void TileAmountCalculation()
    {
        //Gets the amount of tiles from the planet which generates them
        tileCount = GameObject.Find("Planet").GetComponent<Hexsphere>().TileCount;

        //Calculates the required amount of tiles needed to win or lose
        requiredNatureTiles = Mathf.RoundToInt(tileCount * requiredNatureTilePercent);
        requiredToxicTiles = Mathf.RoundToInt(tileCount * requiredToxicTilePercent);
    }

    //Ran from Basetilescript, when a tile either is completely nature or completely toxic it runs this function to add it
    public void AddTile(bool natureTile, bool toxicTile)
    {
        if (natureTile)
        {
            currentNatureTiles++;

            //Checks the win when a tile is added, this way it doens't need to run every frame
            CheckTileWin();
        }
        if (toxicTile)
        {
            currentToxicTiles++;

            CheckTileLose();
        }
    }

    //Checks for both types of tiles if they are equal or greater than the required amount for the win or loss
    void CheckTileWin()
    {
        if(currentNatureTiles >= requiredNatureTiles) Won();
    }

    void CheckTileLose()
    {
        if (currentToxicTiles >= requiredToxicTiles) Lost();
    }

    #endregion

    #region Losing

    void Lost()
    {
        if (canLose)
        {
            LosePopUp.SetActive(true);
            canWin = false;
        }
    }

    void CheckTime()
    {
        if (!timerIsRunning) Lost();
    }

    public void CheckPopulation(int pop)
    {

        if (pop > popRequiredForTrigger) populationDeathCanTrigger = true;

        if (populationDeathCanTrigger)
        {
            if(pop <= 0)
            {
                Lost();
            }
        }
    }
    #endregion

    #region Winning

    void Won()
    {
        if (canWin)
        {
            WinPopUp.SetActive(true);
            canLose = false;
        }
    }

    #endregion

    #region PopUpButtons

    public void PlayAgain()
    {
        //Reloads scene to play again
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        //Quits the game if it is an application
        Application.Quit();

        //Quits the playing state of the editor if it is in-editor
         #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
         #endif
    }

    #endregion

    #region Time
    void CalcTime()
    {
        //Checks if the timer is running and removes a second every second
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        //Rounds and calculates the remaining time to minutes and seconds
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        //Displays the seconds and minuts on the Text UI
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    #endregion
}
