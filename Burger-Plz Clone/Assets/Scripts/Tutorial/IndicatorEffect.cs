using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorEffect : MonoBehaviour
{
    Vector3 originPos;
    float amplitude = 0.5f; // Adjust this to control the oscillation height
    float frequency = 2.0f; // Adjust this to control the oscillation speed

    void Start()
    {
        originPos = transform.position;
    }

    void Update()
    {
        // Calculate the vertical offset using PingPong
        float yOffset = amplitude * Mathf.PingPong(Time.time * frequency, 1.0f);

        // Update the position
        transform.position = originPos + new Vector3(0, yOffset, 0);
    }
}
