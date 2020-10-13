using UnityEngine;

public class CopyHexPlanet : MonoBehaviour
{
    [SerializeField] private GameObject HexagonPrefab;
    [SerializeField] private Transform Hexsphere;

    void Start()
    {
        var tiles = Hexsphere.GetComponentsInChildren<Tile>();

        foreach (var tile in tiles)
        {
            var hexagon = Instantiate(HexagonPrefab, transform);

            hexagon.transform.localPosition = tile.transform.localPosition;
            hexagon.transform.localRotation = tile.transform.localRotation;
        }
    }
}
