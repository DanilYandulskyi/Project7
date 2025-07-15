using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(GoodDestroyer))]
public class Shelf : MonoBehaviour
{
    [SerializeField] private List<Place> _frontPlaces = new List<Place>();
    [SerializeField] private List<Place> _backPlaces = new List<Place>();

    [SerializeField] private GoodDestroyer _goodDestroyer;

    private Dictionary<Place, Good> _backGoodsDictionary = new Dictionary<Place, Good>();

    private bool _isSwitchingLines = false;

    public void Initialize()
    {
        SetGoodsToPlaces(_backPlaces, _backGoodsDictionary);

        DisableBackGoods();

        _goodDestroyer = GetComponent<GoodDestroyer>();

        foreach (var place in _frontPlaces)
        {
            place.RemovedGood += OnFrontPlaceRemovedGood;
            _goodDestroyer.DestroyingFinished += place.RemoveGood;
        }

        _goodDestroyer.DestroyingFinished += SwitchLines;
    }

    private void SetGoodsToPlaces(List<Place> places, Dictionary<Place, Good> goodsDictionary)
    {
        foreach (var place in places)
        {
            if (place.HasGood == true)
            {
                Good good = place.GetGood();

                if (goodsDictionary.ContainsKey(place) == false)
                {
                    goodsDictionary.Add(place, good);
                }
            }
        }
    }

    private void DisableBackGoods()
    {
        foreach (var place in _backPlaces)
        {
            if (place.HasGood == true)
            {
                place.GetGood().Disable();
            }
        }
    }

    public bool TryAdd(Good good, out Place place)
    {
        if (TryGetPlace(out place) == true && IsGoodOnFrontLine(good) == false)
        {
            good.transform.parent = transform;
            place.SetGood(good);

            if (IsFrontLineFull() == true)
            {
                HandleFullFrontLine();
            }

            return true;
        }

        place = null;
        return false;
    }

    private bool IsGoodOnFrontLine(Good good)
    {
        foreach (var place in _frontPlaces)
        {
            if (place.HasGood == true && place.GetGood() == good)
                return true;
        }

        return false;
    }

    private bool IsFrontLineFull()
    {
        foreach (var place in _frontPlaces)
        {
            if (place.IsFull == false)
            {
                return false;
            }
        }

        return true;
    }

    private void HandleFullFrontLine()
    {
        if (_frontPlaces.Count == 0)
            return;

        Good firstGood = _frontPlaces[0].HasGood == true ? _frontPlaces[0].GetGood() : null;

        if (firstGood == null)
            return;

        string goodName = firstGood.Name;

        foreach (var place in _frontPlaces)
        {
            if (place.HasGood == false || place.GetGood().Name != goodName)
            {
                return;
            }
        }

        List<Good> goods = new List<Good>();

        foreach (var place in _frontPlaces)
        {
            goods.Add(place.GetGood());
        }

        _goodDestroyer.DestroyGoods(goods);
    }

    private void SwitchLines()
    {
        if (_isSwitchingLines == true)
            return;

        _isSwitchingLines = true;

        for (int i = 0; i < _backPlaces.Count; i++)
        {
            Place backPlace = _backPlaces[i];

            if (_backGoodsDictionary.ContainsKey(backPlace) == true)
            {
                Good backGood = _backGoodsDictionary[backPlace];
                backGood.Enable();
                backPlace.RemoveGood();

                if (TryAdd(backGood, out Place frontPlace) == true)
                {
                    backGood.MovedToAnotherShelf += frontPlace.RemoveGood;
                }
            }
        }

        _backGoodsDictionary.Clear();
        _isSwitchingLines = false;
    }

    private bool TryGetPlace(out Place place)
    {
        foreach (var placeCandidate in _frontPlaces)
        {
            if (placeCandidate.IsFull == false)
            {
                place = placeCandidate;
                return true;
            }
        }

        place = null;
        return false;
    }

    private void OnFrontPlaceRemovedGood(Place place)
    {
        if (_isSwitchingLines == true)
            return;

        if (AreAllFrontPlacesEmpty())
        {
            SwitchLines();
        }
    }

    private bool AreAllFrontPlacesEmpty()
    {
        foreach (var place in _frontPlaces)
        {
            if (place.HasGood == true)
            {
                return false;
            }
        }

        return true;
    }

    private void OnDestroy()
    {
        foreach (var place in _frontPlaces)
        {
            place.RemovedGood -= OnFrontPlaceRemovedGood;
            _goodDestroyer.DestroyingFinished -= place.RemoveGood;
        }

        _goodDestroyer.DestroyingFinished -= SwitchLines;
    }
}