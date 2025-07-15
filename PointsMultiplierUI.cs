using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PointsMultiplierUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;

    private void Awake()
    {
        _text.text = "X1";
    }

    public void IncreaseBar(float currentPoints, float maxPoints, int multiplier)
    {
        StopAllCoroutines();

        _image.fillAmount = currentPoints / maxPoints;

        _text.text = "X" + multiplier;
    }

    public void DecreaseBar(float currentPoints, float maxPoints, int multiplier)
    {
        StartCoroutine(FillBar(currentPoints / maxPoints));

        _text.text = "X" + multiplier;
    }

    private IEnumerator FillBar(float targetFillAmount)
    {
        float startFillAmount = _image.fillAmount;
        float elapsedTime = 0f;
        float duration = 1f; 

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _image.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / duration);

            yield return null; 
        }

        _image.fillAmount = targetFillAmount;
    }
}
