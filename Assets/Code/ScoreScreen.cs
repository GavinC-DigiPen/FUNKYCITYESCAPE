//------------------------------------------------------------------------------
//
// File Name:	ScoreScreen.cs
// Author(s):	Jeremy Kings (j.kings) - Unity Project
//              Nathan Mueller - original Zero Engine project
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string dist = string.Format("{0,4:F1}", PlayerSaveData.DistanceRun);
        GetComponent<TextMeshProUGUI>().text = "You Ran " + dist.ToString() + " Meters!";
    }
}
