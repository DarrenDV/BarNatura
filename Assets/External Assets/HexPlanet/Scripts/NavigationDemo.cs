using UnityEngine;

public class NavigationDemo : MonoBehaviour
{
    public MobileUnit Unit;

    public bool selected = false;
    
	// Use this for initialization
	void Start ()
    {
        Tile.OnTileClickedAction += OnTileClicked;
	}

    private void OnTileClicked(Tile tile)
    {
        if(!Unit.moving && selected)
        {
            if(Hexsphere.planetInstances[0].navManager.findPath(Unit.currentTile, tile, out var path))
            {
                Unit.moveOnPath(path);
            }
        }
    }
}
