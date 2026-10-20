
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class WheelManager : MonoBehaviour
{
    [Header("Input")]
    public TMP_InputField optionCountInput;
    public Button generateButton;

    [Header("Option Fields")]
    public GameObject optionInputPrefab;
    public Transform optionsContainer;

    [Header("Wheel")]
    public WheelRenderer wheelRenderer;

    private List<TMP_InputField> optionInputs =
        new List<TMP_InputField>();

    public List<string> optionNames =
        new List<string>();

    public void GenerateOptions()
    {
        int count;

        if (!int.TryParse(optionCountInput.text, out count))
        {
            Debug.LogWarning("Please enter a number.");
            return;
        }

        count = Mathf.Clamp(count, 2, 12);

        // 清除舊選項
        foreach (Transform child in optionsContainer)
        {
            Destroy(child.gameObject);
        }

        optionInputs.Clear();
        optionNames.Clear();

        // 產生新的選項輸入框
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(
                optionInputPrefab,
                optionsContainer
            );

            TMP_InputField input =
                obj.GetComponent<TMP_InputField>();

            if (input == null)
            {
                Debug.LogError(
                    "Option prefab needs TMP_InputField!"
                );
                continue;
            }

            input.text = "";

            if (input.placeholder is TMP_Text placeholder)
            {
                placeholder.text = "Option " + (i + 1);
            }

            optionInputs.Add(input);
        }

        // 更新輪盤格數
        if (wheelRenderer != null)
        {
            wheelRenderer.SetSegments(count);
        }

        Debug.Log("Generated " + count + " options!");
    }

    public void SaveOptions()
    {
        optionNames.Clear();

        for (int i = 0; i < optionInputs.Count; i++)
        {
            string name = optionInputs[i].text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                name = "Option " + (i + 1);
            }

            optionNames.Add(name);
        }
        if (wheelRenderer != null) {
            wheelRenderer.SetOptionNames(optionNames);
        }
    }
}

