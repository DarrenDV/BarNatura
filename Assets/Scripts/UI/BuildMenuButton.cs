using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuButton : MonoBehaviour
{
    [SerializeField] private BuildMenuItem buildMenuItem;
    [SerializeField] private Image buttonImage;
    private GameObject itemToBuild;
    private GameManager gameManager;

    private void Start()
    {
        buttonImage.sprite = buildMenuItem.sprite;
        itemToBuild = buildMenuItem.itemToBuild;
    }

    public void OnClick()
    {
        if (gameManager == null) gameManager = FindObjectOfType<GameManager>();
        gameManager.objectToBuild = itemToBuild;
    }


}
