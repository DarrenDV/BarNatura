﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Build Menu Item", order = 1)]
public class BuildMenuItem : ScriptableObject
{
    public Sprite sprite;
    public GameObject itemToBuild;
}