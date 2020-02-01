using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    //[Range(0, 10)]
   //private int distanceByTurn = 1;

    private float moveDuration = 0.5f;

    [HideInInspector]
    public Vector2Int CurrentTile;

    public IEnumerator Action()
    {
        Vector3 prePos = transform.position;
        Vector3 nextPos = prePos + Vector3.right;

        float pourcent = 0;

        while (pourcent < 1)
        {
            pourcent += Time.deltaTime / moveDuration;

            transform.position = Vector3.LerpUnclamped(prePos, nextPos, Easing.EaseInOut(pourcent, EasingType.Back));

            yield return null;
        }
    }

}
