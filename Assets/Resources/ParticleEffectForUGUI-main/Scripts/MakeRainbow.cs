using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeRainbow : MonoBehaviour
{
    public float speed = 1f; // ������ ���� ��ȭ �ӵ�
    private float hue = 0f; // ���� ���� hue ��

    private void Update()
    {
        // �ð��� ���� hue ���� ��ȭ��Ŵ
        hue += Time.unscaledDeltaTime * speed;
        // hue ���� 0~1 ���� ���� ����
        if (hue > 1f)
        {
            hue -= 1f;
        }

        // ���� hue ���� HSV �������� ��ȯ�Ͽ� �̹����� ������ ����
        Color color = Color.HSVToRGB(hue, 1f, 1f);
        GetComponent<Image>().color = color;
    }
}
