
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject setupPanel;
    public GameObject wheelPanel;

    void Start()
    {
        setupPanel.SetActive(true);
        wheelPanel.SetActive(false);
    }

    public void ShowWheel()
    {
        setupPanel.SetActive(false);
        wheelPanel.SetActive(true);
    }

    public void ShowSetup()
    {
        wheelPanel.SetActive(false);
        setupPanel.SetActive(true);
    }
}
