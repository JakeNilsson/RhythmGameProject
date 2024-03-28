using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncColor : AudioSyncer
{
    public Color[] beatColors;
    public Color restColor;
    private int colorIndex;
    private SpriteRenderer block;

    void Start()
    {
        block = GetComponent<SpriteRenderer>();
        colorIndex = beatColors.Length - 1; // Start at the last color in the array
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (m_isBeat) return;
        block.color = Color.Lerp(block.color, restColor, restSmoothTime * Time.deltaTime);
    }

    private IEnumerator MoveToColor(Color target)
    {
        Color current = block.color;
        Color initial = current;
        float timer = 0;

        while (current != target)
        {
            current = Color.Lerp(initial, target, timer / timeToBeat);
            timer += Time.deltaTime;
            block.color = current;

            yield return null;
        }
        m_isBeat = false;
    }

    public override void OnBeat()
    {
        base.OnBeat();
        if (colorIndex < 0)
        {
            colorIndex = beatColors.Length - 1; // Reset to the last color
        }

        Color c = beatColors[colorIndex];
        colorIndex--;

        StopCoroutine("MoveToColor");
        StartCoroutine("MoveToColor", c);
    }
}
