
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WheelSpinner : MonoBehaviour
{
    [Header("Wheel")]
    public RectTransform wheel;
    public Button spinButton;

    [Header("Spin Settings")]
    public float spinDuration = 5f;
    public int minRotations = 5;
    public int maxRotations = 8;

    [Header("Sound")]
    public AudioSource spinSound;

    private bool isSpinning = false;

    public void Spin()
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinRoutine());
        }
    }

    IEnumerator SpinRoutine()
    {
        isSpinning = true;
        spinButton.interactable = false;

        // 開始播放音效
        if (spinSound != null)
        {
            spinSound.Stop();
            spinSound.Play();
        }

        float startAngle = wheel.localEulerAngles.z;

        float rotations = Random.Range(
            minRotations, maxRotations + 1
        );

        float targetAngle = startAngle
            - rotations * 360f
            - Random.Range(0f, 360f);

        float elapsed = 0f;

        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsed / spinDuration
            );

            // Ease Out Cubic：逐漸減速
            float eased = 1f - Mathf.Pow(1f - t, 3f);

            float angle = Mathf.LerpUnclamped(
                startAngle, targetAngle, eased
            );

            wheel.localRotation =
                Quaternion.Euler(0, 0, angle);

            yield return null;
        }

        wheel.localRotation =
            Quaternion.Euler(0, 0, targetAngle);

        // 輪盤停止，音效也停止
        if (spinSound != null)
        {
            spinSound.Stop();
        }

        spinButton.interactable = true;
        isSpinning = false;
    }
}
