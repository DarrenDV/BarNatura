using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinLose : MonoBehaviour
{
    public int tileCount;

    private bool canWin = true;
    private bool canLose = true;

    //Tile required variables
    //Nature
    public int requiredNatureTiles; //public can be removed, currently for testing purposes here
    [Range (0, 1)]
    [SerializeField] private float requiredNatureTilePercent = 0.05f;
    public int currentNatureTiles; //public can be removed, currently for testing purposes here

    //Toxic
    public int requiredToxicTiles; //public can be removed, currently for testing purposes here
    [Range(0, 1)]
    [SerializeField] private float requiredToxicTilePercent = 0.3f;
    public int currentToxicTiles; //public can be removed, currently for testing purposes here

    private GameObject WinPopUp = null;
    private GameObject LosePopUp = null;

    //Timer Variables
    [Tooltip("The time in seconds")]
    public float timeRemaining = 10;
    private bool timerIsRunning;
    private Text timeText = null;

    //Population related variables
    [SerializeField] private bool populationDeathCanTrigger;
    [SerializeField] private int popRequiredForTrigger = 10;

    #region Default

    private void Awake()
    {
        WinPopUp = GameObject.Find("WinPopUp");
        LosePopUp = GameObject.Find("LosePopUp");
        timeText = GameObject.Find("Timer").GetComponent<Text>();
    }

    void Start()
    {
        timerIsRunning = true;
        TileAmountCalculation();
        WinPopUp.SetActive(false);
        LosePopUp.SetActive(false);
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
        timeText.text = $"{minutes:00}:{seconds:00}";
    }
    #endregion
}
