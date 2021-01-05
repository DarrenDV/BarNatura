using UnityEngine;

/// <summary>
/// A data container for buildable structures.
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Build Menu Item", order = 1)]
public class BuildMenuItem : ScriptableObject
{
    public BuildObject buildItem;
    public Mesh buildingItemMesh;
    public int numberOfMeshes = 1;
    public float meshScale = 1;
}
