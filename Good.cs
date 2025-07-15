using UnityEngine;
using System;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Mover))]
public class Good : MonoBehaviour, IMoveable
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private string _name = "Good";

    private Vector3 _offset;
    private Mover _mover;
    private BoxCollider2D _collider;
    private Camera _camera;
    private Vector3 _currentShelfPosition;

    public event Action MovedToAnotherShelf;

    public bool MovementFinished { get; private set; }
    public string Name => _name;

    public void Initialize()
    {
        _camera = Camera.main;

        _mover = GetComponent<Mover>();
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        _offset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + _offset;
    }

    private void OnMouseUp()
    {
        Collider2D[] collider1 = Physics2D.OverlapBoxAll(transform.position, Vector2.one, 0);

        foreach (var collider in collider1)
        {
            if (collider.gameObject.TryGetComponent(out Shelf shelf))
            {
                Place place;

                if (shelf.TryAdd(this, out place))
                {
                    MovedToAnotherShelf?.Invoke();
                    _currentShelfPosition = transform.position;
                    MovedToAnotherShelf += place.RemoveGood;

                    return;
                }
            }
        }

        transform.position = _currentShelfPosition;
    }

    public void FinishMovement()
    {
        MovementFinished = true;
    }

    public void SetPosition()
    {
        _currentShelfPosition = transform.position;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = _camera.nearClipPlane;
        return _camera.ScreenToWorldPoint(mouseScreenPosition);
    }

    public void Enable()
    {
        Color color = _spriteRenderer.color;
        _collider.enabled = true;

        color.a = 1;

        _spriteRenderer.color = color;
    }

    public void MoveToCentre()
    {
        _mover.StartMovingToZeroX(this);
    }

    public void Disable()
    {
        Color color = _spriteRenderer.color;
        _collider.enabled = false;

        color.a = 0.5f;

        _spriteRenderer.color = color;
    }

    private void OnDestroy()
    {
        MovedToAnotherShelf?.Invoke();
    }
}

public interface IMoveable
{
    void FinishMovement();
}