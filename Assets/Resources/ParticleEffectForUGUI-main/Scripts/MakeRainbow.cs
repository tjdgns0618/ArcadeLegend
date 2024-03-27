using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeRainbow : MonoBehaviour
{
    public float speed = 1f; // 무지개 색상 변화 속도
    private float hue = 0f; // 현재 색상 hue 값

    private void Update()
    {
        // 시간에 따라 hue 값을 변화시킴
        hue += Time.unscaledDeltaTime * speed;
        // hue 값을 0~1 범위 내로 유지
        if (hue > 1f)
        {
            hue -= 1f;
        }

        // 현재 hue 값을 HSV 색상으로 변환하여 이미지의 색상을 변경
        Color color = Color.HSVToRGB(hue, 1f, 1f);
        GetComponent<Image>().color = color;
    }
}
