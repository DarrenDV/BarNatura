using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuButton : MonoBehaviour
{
    [SerializeField] private BuildMenuItem buildMenuItem;
    [SerializeField] private Image buttonImage;
    private GameManager gameManager;

    private void Start()
    {
        buttonImage.sprite = buildMenuItem.sprite;
    }

    public void OnClick()
    {
        if (gameManager == null) gameManager = FindObjectOfType<GameManager>();
        gameManager.ChangeBuildObject(buildMenuItem.buildItem.gameObject, buildMenuItem.buildItemPreview.gameObject);
    }


}
