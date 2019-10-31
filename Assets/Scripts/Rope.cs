using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rope : MonoBehaviour {

    Transform[] PositionList;
    int CurrentPosition = -1;

	void Start () {
        PositionList = GetComponentsInChildren<Transform>();
	}
	
    public Vector3 GetNextPositionUp(Vector3 playerPosition)
    {
        if (Vector3.Distance(playerPosition, PositionList[CurrentPosition].position) < 0.05f)
        {
            CurrentPosition++;

            if (CurrentPosition >= PositionList.Length)
            {
                return Vector3.zero;
            }
        }

        return PositionList[CurrentPosition].position;
    }

    public bool IsCatched()
    {
        return CurrentPosition != -1;
    }

    public void ReleaseRope()
    {
        CurrentPosition = -1;
    }

    public void CatchRope(Vector3 playerPosition)
    {
        int index = 0;
        Vector3 nextPosition = new Vector3(0,0,0);

        foreach (Transform ropePosition in PositionList)
        {
            float bestDistance = Vector3.Distance(playerPosition, nextPosition);
            float currentPointDistance = Vector3.Distance(playerPosition, ropePosition.position);

            if (bestDistance > currentPointDistance)
            {
                nextPosition = ropePosition.position;
                CurrentPosition = index;
            }           
            index++;
        }

        if (CurrentPosition == 0) 
        {
            CurrentPosition++;
        }
        else if (CurrentPosition + 1 < PositionList.Length)
        {
            float distanceNext = Vector3.Distance(playerPosition, PositionList[CurrentPosition + 1].position);
            float distancePrevious = Vector3.Distance(playerPosition, PositionList[CurrentPosition - 1].position);

            if (distanceNext < distancePrevious)
            {
                CurrentPosition++;
            }
        }
    }
}
