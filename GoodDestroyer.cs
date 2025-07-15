using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class GoodDestroyer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    public event Action DestroyingFinished;

    public void DestroyGoods(List<Good> goods)
    {
        foreach (var good in goods)
        {
            good.MoveToCentre();
        }

        StartCoroutine(Destroy(goods));
    }

    private IEnumerator Destroy(List<Good> goods)
    {
        yield return new WaitUntil(() => AllGoodsMoved(goods));

        foreach (var good in goods)
        {
            Destroy(good.gameObject);
        }

        _particleSystem.Play();

        DestroyingFinished?.Invoke();
    }

    private bool AllGoodsMoved(List<Good> goods)
    {
        foreach (var good in goods)
        {
            if (good.MovementFinished == false)
                return false;
        }

        return true;
    }
}
