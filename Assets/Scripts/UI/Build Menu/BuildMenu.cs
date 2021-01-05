using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenu : MonoBehaviour
{
    public static BuildMenu Instance;

    private List<Button> buttons = new List<Button>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("BuildMenu instance already set!");
        }
    }

    private void Start()
    {
        buttons = GetComponentsInChildren<Button>().ToList();
    }

    /// <summary>
    /// Disable all build buttons.
    /// </summary>
    public void DisableButtons()
    {
        foreach(var button in buttons)
        {
            button.enabled = false;
        }
    }

    /// <summary>
    /// Enable all build buttons.
    /// </summary>
    public void EnableButtons()
    {
        foreach (var button in buttons)
        {
            button.enabled = true;
        }
    }
}
