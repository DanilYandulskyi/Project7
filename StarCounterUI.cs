using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StarCounterUI : MonoBehaviour
{
    private const string StarAddingTrigger = "StarAdding";

    [SerializeField] private Image _progressBar;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Animator _animator;

    private List<int> _sortedThresholds;
    private IReadOnlyDictionary<int, int> _pointsToStars;

    private Coroutine _progressCoroutine;

    public void Initialize(IReadOnlyDictionary<int, int> pointsToStars)
    {
        _pointsToStars = pointsToStars;
        _sortedThresholds = _pointsToStars.Keys.ToList();
        _sortedThresholds.Sort();
    }

    public void UpdateProgressBar(int currentPoints, int currentStars, int starsForText)
    {
        int currentThreshold = 0;

        foreach (var thresholdToStar in _pointsToStars)
        {
            if (thresholdToStar.Value == currentStars)
            {
                currentThreshold = thresholdToStar.Key;
                break;
            }
        }

        int nextThreshold = _sortedThresholds.FirstOrDefault(t => t > currentThreshold);

        float targetFillAmount;

        if (nextThreshold == 0)
        {
            targetFillAmount = 1f;
        }
        else
        {
            float progress = Mathf.InverseLerp(currentThreshold, nextThreshold, currentPoints);
            targetFillAmount = Mathf.Clamp01(progress);
        }

        if (_progressCoroutine != null)
        {
            StopCoroutine(_progressCoroutine);
        }

        _progressCoroutine = StartCoroutine(AnimateProgressBar(targetFillAmount, starsForText));
    }

    public void AnimateStarsChanged()
    {
        _animator.SetTrigger(StarAddingTrigger);
    }

    private void UpdateText(int starsCount)
    {
        _text.text = $"{starsCount} - stars";
    }

    private IEnumerator AnimateProgressBar(float targetFill, int starsForText)
    {
        float startFill = _progressBar.fillAmount;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _progressBar.fillAmount = Mathf.Lerp(startFill, targetFill, elapsed / duration);

            yield return null;
        }

        _progressBar.fillAmount = targetFill;

        if (_progressBar.fillAmount == 1)
        {
            AnimateStarsChanged();
            UpdateText(starsForText);
            _progressBar.fillAmount = 0;
        }
    }
}
