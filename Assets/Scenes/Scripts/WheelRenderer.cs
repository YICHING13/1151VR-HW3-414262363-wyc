
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasRenderer))]
public class WheelRenderer : MaskableGraphic
{
    [Range(2, 12)]
    public int segmentCount = 6;

    public int smoothness = 60;

    [Header("Option Labels")]
    public TMP_FontAsset labelFont;
    public float labelFontSize = 36f;

    private readonly List<TextMeshProUGUI> labels =
        new List<TextMeshProUGUI>();

    private Color[] colors =
    {
        new Color(0.85f, 0.70f, 1f),
        new Color(1f, 0.75f, 0.75f),
        new Color(1f, 0.90f, 0.60f),
        new Color(0.65f, 0.85f, 1f),
        new Color(0.70f, 1f, 0.80f),
        new Color(1f, 0.75f, 0.90f)
    };

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (segmentCount < 2) return;

        float radius = Mathf.Min(
            rectTransform.rect.width,
            rectTransform.rect.height
        ) / 2f;

        float anglePerSegment = 360f / segmentCount;

        for (int i = 0; i < segmentCount; i++)
        {
            Color c = colors[i % colors.Length];

            for (int j = 0; j < smoothness; j++)
            {
                float a1 = (i * anglePerSegment +
                    j * anglePerSegment / smoothness)
                    * Mathf.Deg2Rad;

                float a2 = (i * anglePerSegment +
                    (j + 1) * anglePerSegment / smoothness)
                    * Mathf.Deg2Rad;

                int index = vh.currentVertCount;

                vh.AddVert(Vector3.zero, c, Vector2.zero);

                vh.AddVert(
                    new Vector3(
                        Mathf.Sin(a1) * radius,
                        Mathf.Cos(a1) * radius, 0),
                    c, Vector2.zero
                );

                vh.AddVert(
                    new Vector3(
                        Mathf.Sin(a2) * radius,
                        Mathf.Cos(a2) * radius, 0),
                    c, Vector2.zero
                );

                vh.AddTriangle(index, index + 1, index + 2);
            }
        }
    }

    public void SetSegments(int count)
    {
        segmentCount = Mathf.Clamp(count, 2, 12);
        SetVerticesDirty();
    }

    public void SetOptionNames(List<string> names)
    {
        ClearLabels();

        if (names == null || names.Count == 0)
            return;

        SetSegments(names.Count);

        float radius = Mathf.Min(
            rectTransform.rect.width,
            rectTransform.rect.height
        ) / 2f;

        float anglePerSegment = 360f / segmentCount;

        for (int i = 0; i < names.Count; i++)
        {
            GameObject obj = new GameObject(
                "OptionLabel_" + (i + 1),
                typeof(RectTransform)
            );

            obj.transform.SetParent(transform, false);

            TextMeshProUGUI label =
                obj.AddComponent<TextMeshProUGUI>();

            if (labelFont != null)
                label.font = labelFont;

            label.text = names[i];
            label.fontSize = labelFontSize;
            label.color = Color.black;
            label.alignment = TextAlignmentOptions.Center;

            label.enableAutoSizing = true;
            label.fontSizeMin = 12;
            label.fontSizeMax = labelFontSize;
            label.raycastTarget = false;

            // 依扇形大小調整文字框
            float angleDeg =
                (i + 0.5f) * anglePerSegment;

            float angleRad = angleDeg * Mathf.Deg2Rad;

            float distance = radius * 0.62f;

            RectTransform rt = label.rectTransform;

            float width;

        if (segmentCount == 2)
        {
        width = radius * 0.85f;
        }
        else
        {
        width = Mathf.Min(
            radius * 0.85f,
            2f * distance *
            Mathf.Tan(anglePerSegment * Mathf.Deg2Rad / 2f)
            * 0.8f
        );
}

            rt.sizeDelta = new Vector2(width, 65f);

            rt.anchoredPosition = new Vector2(
                Mathf.Sin(angleRad) * distance,
                Mathf.Cos(angleRad) * distance
            );

            // 文字沿著扇形方向旋轉
            rt.localRotation = Quaternion.Euler(
                0, 0, -angleDeg
            );

            labels.Add(label);
        }
    }

    private void ClearLabels()
    {
        foreach (TextMeshProUGUI label in labels)
        {
            if (label != null)
            {
                label.gameObject.SetActive(false);
                Destroy(label.gameObject);
            }
        }

        labels.Clear();
    }
}

