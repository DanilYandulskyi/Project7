using UnityEngine;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
    [SerializeField] private List<Shelf> _shelfs;
    [SerializeField] private List<Good> _goods;
    [SerializeField] private PointCounter _pointCounter;

    [SerializeField] private LayerMask _goodLayerMask;
    [SerializeField] private float _scanRadius = 5000f;
    [SerializeField] private Vector3 _scanCenter = Vector3.zero;

    [SerializeField] private StarCounter _starCounter; 

    private void OnEnable()
    {
        foreach (var good in _goods)
        {
            good.Initialize();
        }

        foreach (var shelf in _shelfs)
        {
            if (shelf.TryGetComponent(out GoodDestroyer goodDestroyer))
            {
                goodDestroyer.DestroyingFinished += _pointCounter.AddPoints;
                goodDestroyer.DestroyingFinished += ScanForGoods;
            }

            shelf.Initialize();
        }
    }

    private void OnDisable()
    {
        foreach (var shelf in _shelfs)
        {
            if (shelf != null)
            {
                if (shelf.TryGetComponent(out GoodDestroyer goodDestroyer))
                {
                    goodDestroyer.DestroyingFinished -= _pointCounter.AddPoints;
                    goodDestroyer.DestroyingFinished -= ScanForGoods;
                }
            }
        }
    }

    private void ScanForGoods()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(_scanCenter, _scanRadius, _goodLayerMask);

        if (hitColliders.Length == 0)
        {
            Debug.Log("GG");
        }

        CheckThreeStars();
    }

    private void CheckThreeStars()
    {
        if (_starCounter.CurrentStars >= 3)
        {
            Debug.Log("Horosh");
        }
    }
}