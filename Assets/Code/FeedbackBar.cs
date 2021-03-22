//------------------------------------------------------------------------------
//
// File Name:	FeedbackBar.cs
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
using UnityEngine;
using UnityEngine.UI;

public class FeedbackBar : MonoBehaviour
{
    // The time it takes the bar to change to the new current amount
    public float BarTransitionTime = 0.05f;
    // The amount of time separating the two transitions. Can be set to 0 for no delay.
    public float HighlightDelayTime = 0.25f;
    // The time it takes the highlight bar to shrink to the new current amount.
    public float HighlightTransitionTime = 0.05f;

    // Colors for UI
    public Color BackColor = new Color(0.8f, 0.8f, 0.85f, 1f);
    public Color HighlightColor = new Color(1f, 1f, 1f, 1f);
    public Color BarColor = new Color(0f, 1f, 0f, 1f);

    // Max bar value
    public float MaxValue = 100.0f;

    // Private data
    private float currentValue = 0.0f;
    private float targetPercent = 1.0f;
    private float timer = 0.0f;
    private float barSizePrevious = 0.0f;
    private float highlightSizePrevious = 0.0f;
    private bool valueIncreasing = false;
    private GameObject highlightSprite = null;
    private GameObject barSprite = null;
    private Vector3 originalSize = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        highlightSprite = transform.Find("Highlight").gameObject;
        barSprite = transform.Find("Bar").gameObject;
        if (barSprite != null)
        {
            originalSize = barSprite.transform.localScale;
        }
        currentValue = MaxValue;

        if (highlightSprite != null && barSprite != null)
        {
            GetComponent<Image>().color = BackColor;
            barSprite.GetComponent<Image>().color = BarColor;
            highlightSprite.GetComponent<Image>().color = HighlightColor;
        }
        else
        {
            Debug.Log("ERROR: A FeedbackBar has missing or incorrectly " +
                "named children objects.There should two children with the names " +
                "of 'Highlight' and 'Bar'");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Skip scaling if new value has not been set
        if (targetPercent >= 1.0f)
            return;

        // Lerp bar scale
        float barScaleX = Mathf.Lerp(barSizePrevious, targetPercent * originalSize.x, timer / BarTransitionTime);
        barSprite.transform.localScale =
            new Vector3(barScaleX, originalSize.y, originalSize.z);


        float highlightScaleX;
        // If value is decreasing
        if (!valueIncreasing)
        {
            // If delay is up
            if(timer >= HighlightDelayTime)
            {
                // Lerp highlight scale
                highlightScaleX = Mathf.Lerp(highlightSizePrevious, targetPercent * originalSize.x,
                    (timer - HighlightDelayTime) / HighlightTransitionTime);
            }
            // If delay is still occurring
            else
            {
                // Keep previous value
                highlightScaleX = barSizePrevious;
            }
        }
        // If value is increasing
        else
        {
            // Set highlight scale
            highlightScaleX = targetPercent * originalSize.x;
        }

        highlightSprite.transform.localScale =
                new Vector3(highlightScaleX, originalSize.y, originalSize.z);

        timer += Time.deltaTime;
    }

    public void SetValue(float newValue)
    {
        newValue = Mathf.Clamp(newValue, 0, MaxValue);
        targetPercent = newValue / MaxValue;
        // If the bar was moving before, then just cancel the old action
        // because the new action will override it.
        timer = 0.0f;
        barSizePrevious = barSprite.transform.localScale.x;

        // If the bar level increases, then no delay is needed for the highlight
        if (newValue > currentValue)
        {
            valueIncreasing = true;
        }
        highlightSizePrevious = highlightSprite.transform.localScale.x;

        // Update the current value
        currentValue = newValue;
    }

    public void SetMax(float maxValue_)
    {
        MaxValue = maxValue_;
        currentValue = Mathf.Clamp(currentValue, 0, MaxValue);
    }
}
