using UnityEngine;
using UnityEngine.UI;

public class TeamWinManager : MonoBehaviour
{
    public static TeamWinManager instance;
    public GameObject Team1Win;
    public GameObject Team2Win;

    private void Awake()
    {
        // Ensure there is only one instance of TeamWinManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Optionally, make this persistent across scenes
        // DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Team1Win = transform.Find("Canvas/Panel/Team1Win").gameObject;
        Team2Win = transform.Find("Canvas/Panel/Team2Win").gameObject;

        if (Team1Win == null || Team2Win == null)
        {
            Debug.LogError("One or both of the team win texts could not be found. Please check your hierarchy and ensure the paths are correct.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // You can add logic here to check for team win conditions and update the texts accordingly
    }
}
