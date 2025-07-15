using UnityEngine;
using System.Collections.Generic;

public class StarCounter : MonoBehaviour
{
    [SerializeField] private PointCounter _pointCounter;
    [SerializeField] private StarCounterUI _starCounterUI;
    [SerializeField] private int _currentStars = 0;

    private Dictionary<int, int> _pointsToStars = new Dictionary<int, int>()
    {
        { 12, 3 },
        { 8, 2 },
        { 4, 1 }
    };

    public int CurrentStars => _currentStars;
    public IReadOnlyDictionary<int, int> PointsToStars => _pointsToStars;

    private void Awake()
    {
        _starCounterUI.Initialize(_pointsToStars);
        _pointCounter.PointsAdded += OnPointsAdded;
    }

    private void OnPointsAdded(int points)
    {
        _starCounterUI.UpdateProgressBar(points, _currentStars, CalculateStars(points));
        UpdateStars(points);
    }
    
    private void UpdateStars(int points)
    {
        int newStars = CalculateStars(points);

        if (newStars == _currentStars)
            return;

        _currentStars = newStars;
    }

    private int CalculateStars(int points)
    {
        foreach (var threshold in GetSortedThresholds())
        {
            if (points >= threshold)
                return _pointsToStars[threshold];
        }

        return 0;
    }

    private List<int> GetSortedThresholds()
    {
        var thresholds = new List<int>(_pointsToStars.Keys);
        thresholds.Sort((min, max) => max.CompareTo(min));

        return thresholds;
    }
}