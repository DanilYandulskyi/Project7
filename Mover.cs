using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    private const float DistanceToStop = 0.001f;

    [SerializeField] private float _speed;

    public void StartMovingToZeroX(IMoveable moveable)
    {
        StartCoroutine(MoveToZeroX(moveable));
    }

    private IEnumerator MoveToZeroX(IMoveable moveable)
    {
        if (transform.localPosition.x != 0)
        {
            Vector3 target = transform.localPosition;

            target.x = 0;

            while (Vector3.SqrMagnitude(target - transform.localPosition) >= DistanceToStop)
            {
                Vector3 direction = target - transform.localPosition;

                Vector3 offset = direction.normalized * (_speed * Time.deltaTime);
                transform.localPosition += offset;

                yield return null;
            }
        }

        moveable.FinishMovement();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
