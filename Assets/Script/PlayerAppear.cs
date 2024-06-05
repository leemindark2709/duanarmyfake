using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppear : MonoBehaviour
{
    public static PlayerAppear Instance;

    public Transform SpawnPlayerCP;
    public List<Transform> CheckPoints;
    public Transform point1;
    public Transform point2;
    public float requiredDistance = 2f;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public virtual void LoadCheckPoint()
    {
        SpawnPlayerCP = GameObject.Find("SpawnPlayerCP").transform;
        CheckPoints = new List<Transform>();
        foreach (Transform checkpoint in SpawnPlayerCP)
        {
            CheckPoints.Add(checkpoint);
        }
    }

    public virtual void GetTwoPoints(float requiredDistance, out Transform point1, out Transform point2)
    {
        point1 = null;
        point2 = null;

        if (this.CheckPoints.Count < 2)
        {
            Debug.LogError("Not enough checkpoints in the list");
            return;
        }

        point1 = CheckPoints[Random.Range(0, CheckPoints.Count)];

        List<Transform> validPoints = new List<Transform>();
        foreach (Transform checkpoint in CheckPoints)
        {
            if (checkpoint != point1 && Vector3.Distance(point1.position, checkpoint.position) >= requiredDistance)
            {
                validPoints.Add(checkpoint);
            }
        }

        if (validPoints.Count > 0)
        {
            point2 = validPoints[Random.Range(0, validPoints.Count)];
        }
        else
        {
            Debug.LogError("No valid second checkpoint found with the required distance");
        }
    }
}