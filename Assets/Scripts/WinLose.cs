using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class WinLose : MonoBehaviour
{
    #region Variables

    public int tileCount;

    private bool canWin = true;
    private bool canLose = true;

    //Tile required variables
    //Nature
    public int requiredNatureTiles; //public can be removed, currently for testing purposes here
    [Range(0, 1)]
    [SerializeField] private float requiredNatureTilePercent = 0.05f;
    public int currentNatureTiles; //public can be removed, currently for testing purposes here

    //Toxic
    public int requiredToxicTiles; //public can be removed, currently for testing purposes here
    [Range(0, 1)]
    [SerializeField] private float requiredToxicTilePercent = 0.3f;
    public int currentToxicTiles; //public can be removed, currently for testing purposes here

    //Timer Variables
    [Tooltip("The time in seconds")]
    public float timeRemaining = 10;
    private bool timerIsRunning;
    private Text timeText = null;

    private GameObject endPopUpBack;
    private Text endPopUpTitle;
    private Text endPopUpDescription;

    [Header("Messages")]
    [SerializeField] private string winTitleText;
    [SerializeField] [TextArea] private string winDescriptionText;
    [SerializeField] private string loseTitleText;
    [SerializeField] [TextArea] private string loseDescriptionText;

    #endregion

    #region Default

    private void Awake()
    {
        endPopUpBack = GameObject.Find("EndPopUp");
        timeText = GameObject.Find("Timer").GetComponent<Text>();
        endPopUpTitle = GameObject.Find("EndPopUp/Border/Background/Panel/Title").GetComponent<Text>();
        endPopUpDescription = GameObject.Find("EndPopUp/Border/Background/Panel/DescriptionText").GetComponent<Text>();
    }

    void Start()
    {
        timerIsRunning = true;
        TileAmountCalculation();
        endPopUpBack.SetActive(false);
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
        tileCount = FindObjectOfType<Hexsphere>().TileCount;

        //Calculates the required amount of tiles needed to win or lose
        requiredNatureTiles = Mathf.RoundToInt(tileCount * requiredNatureTilePercent);
        requiredToxicTiles = Mathf.RoundToInt(tileCount * requiredToxicTilePercent);
    }

    //Ran from Basetilescript
    /// <summary>
    /// When a tile either is completely nature or completely toxic it runs this function to add it - 0 = nature, 1 = toxic
    /// </summary>
    public void AddTile(int tile)
    {
        //if Tile = 0, tile is nature -- If tile = 1, tile is toxic
        if (tile == 0)
        {
            currentNatureTiles++;

            //Checks the win when a tile is added, this way it doens't need to run every frame
            CheckTileWin();
        }
        if (tile == 1)
        {
            currentToxicTiles++;

            CheckTileLose();
        }
    }

    /// <summary>
    /// Call this function when a tile isn't completely nature or toxic anymore - 0 = nature, 1 = toxic
    /// </summary>
    public void RemoveTile(int tile)
    {
        //if Tile = 0, tile is nature -- If tile = 1, tile is toxic
        if (tile == 0) currentNatureTiles--;

        if (tile == 1) currentToxicTiles--;
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

    /// <summary>
    /// Run this if the player lost.
    /// </summary>
    void Lost()
    {
        if (canLose)
        {
            ShowEndPopup(loseTitleText, loseDescriptionText);
            canWin = false;
        }
    }

    /// <summary>
    /// Check if the player loses due to a time loss.
    /// </summary>
    void CheckTime() // this function can be removed if time losses are removed. 
    {
        if (!timerIsRunning) Lost();
    }

    /// <summary>
    /// Checks the current population in the game, if it is 0 or less, the player loses.
    /// </summary>
    /// <param name="pop"></param>
    public void CheckPopulation(int pop)
    {
        if(pop <= 0) Lost();
    }

    #endregion

    #region Winning

    /// <summary>
    /// Run this if the player won. 
    /// </summary>
    void Won()
    {
        if (canWin)
        {
            ShowEndPopup(winTitleText, winDescriptionText);
            canLose = false;
        }
    }

    #endregion

    #region PopUps

    private void ShowEndPopup(string title, string message)
    {
        endPopUpTitle.text = title;
        endPopUpDescription.text = message;

        endPopUpBack.SetActive(true);
    }

    public void PlayAgain()
    {
        //Reloads scene to play again
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Utils.QuitGame();
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
    #endregion // this entire section can be removed if the player won't lose to time anymore.
}
