using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTableAppear : MonoBehaviour
{
    public static PlayerTableAppear Instance;
    public string SpawnPlayerTableCPName = "SpawnPlayerTableCP";
    public Transform SpawnPlayerTableCP;
    public List<Transform> CheckPoints; // Remove 'new' keyword to resolve CS0109 warning
    public float pointRequiredDistance = 2f; // Rename to avoid conflict

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        
        }
    }

    public virtual void LoadCheckPoint()
    {
        SpawnPlayerTableCP = GameObject.Find(SpawnPlayerTableCPName).transform;
        CheckPoints = new List<Transform>();
        foreach (Transform checkpoint in SpawnPlayerTableCP)
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
                    if (Vector3.Distance(point.position, checkpoint.position) < pointRequiredDistance)
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
