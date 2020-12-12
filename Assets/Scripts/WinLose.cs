using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class WinLose : MonoBehaviour
{
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

    //Population related variables
    [SerializeField] private bool populationDeathCanTrigger;
    [SerializeField] private int popRequiredForTrigger = 10;

    //Popup
    [SerializeField] private GameObject endPopUpPanel;
    [SerializeField] private Text endPopUpTitle;
    [SerializeField] private Text endPopUpDescription;

    [Header("Messages")]
    [SerializeField] private string winTitleText;
    [SerializeField] [TextArea] private string winDescriptionText;
    [SerializeField] private string loseTitleText;
    [SerializeField] [TextArea] private string loseDescriptionText;

    #region Default

    private void Awake()
    {
        endPopUpPanel = GameObject.Find("EndPopUp/Panel");
    }

    void Start()
    {
        timeText = GameObject.Find("Timer").GetComponent<Text>();
        
        endPopUpTitle = GameObject.Find("EndPopUp/Panel/Title").GetComponent<Text>();
        endPopUpDescription = GameObject.Find("EndPopUp/Panel/DescriptionText").GetComponent<Text>();
        timerIsRunning = true;
        TileAmountCalculation();
        endPopUpPanel.SetActive(false);
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
            ShowEndPopup(loseTitleText, loseDescriptionText);
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
            ShowEndPopup(winTitleText, winDescriptionText);
            canLose = false;
        }
    }

    #endregion

    private void ShowEndPopup(string title, string message)
    {
        endPopUpTitle.text = title;
        endPopUpDescription.text = message;

        endPopUpPanel.SetActive(true);
    }

    #region PopUpButtons

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
    #endregion
}
