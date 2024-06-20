using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppear : MonoBehaviour
{
    public static PlayerAppear Instance;
    public string SpawnPlayerCPName = "SpawnPlayerCP";
    public Transform SpawnPlayerCP;
    public List<Transform> CheckPoints;
    public float requiredDistance = 2f;

    private void Awake()
    {
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    public virtual void LoadCheckPoint() 
    {
        SpawnPlayerCP = GameObject.Find(SpawnPlayerCPName).transform;
        CheckPoints = new List<Transform>();
        foreach (Transform checkpoint in SpawnPlayerCP)
        {
            CheckPoints.Add(checkpoint);
        }
    }

    public virtual void GetPoints(int pointCount, float requiredDistance, out List<Transform> points)
    {
        points = new List<Transform>();

        if (this.CheckPoints.Count < pointCount)
        {
            Debug.LogError("Not enough checkpoints in the list");
            return;
        }

        List<Transform> remainingCheckPoints = new List<Transform>(CheckPoints);
        Transform currentPoint = remainingCheckPoints[Random.Range(0, remainingCheckPoints.Count)];
        points.Add(currentPoint);
        remainingCheckPoints.Remove(currentPoint);

        while (points.Count < pointCount)
        {
            List<Transform> validPoints = new List<Transform>();
            foreach (Transform checkpoint in remainingCheckPoints)
            {
                bool isValid = true;
                foreach (Transform point in points)
                {
                    if (Vector3.Distance(point.position, checkpoint.position) < requiredDistance)
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid)
                {
                    validPoints.Add(checkpoint);
                }
            }

            if (validPoints.Count > 0)
            {
                currentPoint = validPoints[Random.Range(0, validPoints.Count)];
                points.Add(currentPoint);
                remainingCheckPoints.Remove(currentPoint);
            }
            else
            {
                Debug.LogError("No valid checkpoint found with the required distance");
                break;
            }
        }

        if (points.Count < pointCount)
        {
            Debug.LogError("Could not find enough valid checkpoints with the required distance");
        }
    }
}
