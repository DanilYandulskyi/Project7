using UnityEngine;
using System.Collections;

public class PointsMultiplier : MonoBehaviour
{
    [SerializeField] private float _points = 0;
    [SerializeField] private int _multiplierIncrementThreshold = 10;
    [SerializeField] private float _pointsDecrementRate = 1f;
    [SerializeField] private float _addingPointsDecrementRate = 0.2f;

    private int _multiplier = 1;

    private PointsMultiplierUI _ui;

    private void Start()
    {
        _ui = GetComponent<PointsMultiplierUI>();
        StartCoroutine(DecreasePointsContinuously());
    }

    private void Update()
    {
        CheckMultiplier();
    }

    public void AddPoints(int points)
    {
        _points += points * _multiplier;

        CheckMultiplier();
        _ui.IncreaseBar(_points, _multiplierIncrementThreshold, _multiplier);
    }

    private void CheckMultiplier()
    {
        if ((_points / _multiplierIncrementThreshold) >= 1)
        {
            _multiplier++;
            _multiplierIncrementThreshold += 10;
            _pointsDecrementRate += _addingPointsDecrementRate;
        }
    }

    private IEnumerator DecreasePointsContinuously()
    {
        while (true)
        {
            if (_points > 0)
            {
                _points -= (_pointsDecrementRate * Time.deltaTime);
                _points = Mathf.Max(_points, 0);
            }
            else
                _multiplier = 1;

            _ui.DecreaseBar(_points, _multiplierIncrementThreshold, _multiplier);

            yield return null;
        }
    }

    public int GetPoints(int points)
    {
        return _multiplier * points;
    }
}