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

        for (int i = 0; i < CheckPoints.Count; i++)
        {
            Transform currentPoint = CheckPoints[i];

            // Check if the current point maintains the required distance from all previously added points
            bool isValid = true;
            foreach (Transform point in points)
            {
                if (Vector3.Distance(point.position, currentPoint.position) < requiredDistance)
                {
                    isValid = false;
                    break;
                }
            }

            if (isValid)
            {
                points.Add(currentPoint);
            }

            // Stop if we've collected enough points
            if (points.Count == pointCount)
            {
                break;
            }
        }

        if (points.Count < pointCount)
        {
            Debug.LogError("Could not find enough valid checkpoints with the required distance");
        }
    }

}
