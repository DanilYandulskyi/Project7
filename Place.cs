using UnityEngine;
using System;
using System.Collections;

public class Place : MonoBehaviour
{
    [SerializeField] private Good _good;
    [SerializeField] private bool _isFull;

    public event Action<Place> RemovedGood;

    public string GoodName => _good.Name;

    public bool IsFull => _isFull;
    public bool HasGood => _good != null;

    private void Awake()
    {
        if (_good != null)
        {
            SetGood(_good);
            _good.MovedToAnotherShelf += RemoveGood;
        }
    }

    public Good GetGood()
    {
        return _good;
    }

    public void SetGood(Good good)
    {
        if (_good == null || good == _good)
        {
            _isFull = true;
            _good = good;
            _good.transform.position = transform.position;
            _good.SetPosition();
        }
    }

    public void RemoveGood()
    {
        if (_good != null)
        {
            _good.MovedToAnotherShelf -= RemoveGood;
            _isFull = false;
            _good = null;
            RemovedGood?.Invoke(this);
        }
    }

    private void OnDestroy()
    {
        if (_good != null)
        {
            _good.MovedToAnotherShelf -= RemoveGood;
        }
    }
}

