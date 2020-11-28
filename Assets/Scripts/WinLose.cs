using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLose : MonoBehaviour
{

    public int tileCount;

    //Tile required variables
    //Nature
    public int requiredNatureTiles;
    [Range (0, 1)]
    public float requiredNatureTilePercent;
    public int currentNatureTiles;

    //Toxic
    public int requiredToxicTiles;
    [Range(0, 1)]
    public float requiredToxicTilePercent;
    public int currentToxicTiles;

    public GameObject WinPopUp;

    // Start is called before the first frame update
    void Start()
    { 
        TileAmountCalculation();
    
    }

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
            CheckWin();
        }
        if (toxicTile)
        {
            currentToxicTiles++;
        }
    }

    void CheckWin()
    {
        if(currentNatureTiles >= requiredNatureTiles)
        {
            WinPopUp.SetActive(true);
        }
    }

    #region WinPopUpButtons

    public void PlayAgain()
    {
        //Reloads scene to play again
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
         #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
         #endif
    }

    #endregion
}
