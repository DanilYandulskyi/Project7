using UnityEngine;
using System;

public class PointCounter : MonoBehaviour
{
    [SerializeField] private PointsMultiplier _pointsMultiplier;

    [SerializeField] private int _pointsPerCombo;
    [SerializeField] private int _points;

    public event Action<int> PointsAdded;

    public void AddPoints()
    {
        _points += _pointsMultiplier.GetPoints(_pointsPerCombo);

        _pointsMultiplier.AddPoints(_pointsMultiplier.GetPoints(_pointsPerCombo));

        PointsAdded?.Invoke(_points);
    }
}
