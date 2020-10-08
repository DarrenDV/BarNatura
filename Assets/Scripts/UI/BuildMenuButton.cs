using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuButton : MonoBehaviour
{
    [SerializeField] private BuildMenuItem buildMenuItem;
    [SerializeField] private Image buttonImage;
    private GameObject itemToBuild;

    private void Start()
    {
        buttonImage.sprite = buildMenuItem.sprite;
        itemToBuild = buildMenuItem.gameObject;
    }

    public void OnClick()
    {
        //Insert build code here dummy
    }


}
