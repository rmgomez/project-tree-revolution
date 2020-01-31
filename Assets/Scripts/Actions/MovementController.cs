using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector2Int CurrentTile;
    
    void GetTilePosition(){}

    public void Move(int actionsLeft)
    {
        StartCoroutine(MoveCR(actionsLeft));
    }

    private IEnumerator MoveCR(int actionsLeft)
    {
        //Are we having different movement types or only straight?
        
        for (int i = 0; i < actionsLeft; i++)
        {
            //Do Movement
            //Wait for Movement to complete
        }

        yield return null;

    }
}
