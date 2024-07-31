using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOK : MonoBehaviour
{
    public static ButtonOK instance;
    public PickPlayer pickPlayer;
    public GameObject pickScene;
    public GameObject ReadyGo;
    [SerializeField] private GameObject ReadyUI;
    [SerializeField] private GameObject GoUI;
    [SerializeField] private AudioManager audioManager;

    private void Awake()
    {
        instance = this;

        // Ensure AudioManager is assigned
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ReadyGo = GameObject.Find("ReadyGo");
        if (ReadyGo != null)
        {
            Transform canvasTransform = ReadyGo.transform.Find("Canvas");
            if (canvasTransform != null)
            {
                ReadyUI = canvasTransform.Find("ready")?.gameObject;
                GoUI = canvasTransform.Find("go")?.gameObject;

                if (ReadyUI != null)
                {
                    ReadyUI.SetActive(false);
                }
                else
                {
                    Debug.LogError("ReadyUI not found under Canvas!");
                }

                if (GoUI != null)
                {
                    GoUI.SetActive(false);
                }
                else
                {
                    Debug.LogError("GoUI not found under Canvas!");
                }
            }
            else
            {
                Debug.LogError("Canvas not found under ReadyGo!");
            }
        }
        else
        {
            Debug.LogError("ReadyGo not found in the scene!");
        }

        pickScene = GameObject.Find("PickScene");
        if (pickScene == null)
        {
            Debug.LogError("PickScene not found in the scene!");
        }

        pickPlayer = FindObjectOfType<PickPlayer>();
        if (pickPlayer == null)
        {
            Debug.LogError("PickPlayer not found in the scene!");
        }
    }

    // Method to handle button click event
    public void OnButtonClick()
    {
       
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.ButtonClick);
        }

        if (pickPlayer != null&& pickPlayer.ActivePlayer.GetComponent<Player>().player!=null)
        {
            pickPlayer.ListPlayerInGames.Add(pickPlayer.ActivePlayer.GetComponent<Player>().player);
            pickPlayer.NextPlayer();

            if (pickPlayer.ListPlayerInGames.Count == GameManager.instance.playerCount)
            {
                 //SpawnMap();
                ReadyGo.SetActive(true);
                StartCoroutine(ShowReadyGoSequence());

            }
        }
       
        
    }


    private IEnumerator ShowReadyGoSequence()
    {
        // Tìm tất cả các đối tượng có tên "pause"
        GameObject[] pauseObjects = GameObject.FindGameObjectsWithTag("Pause");

        // Tắt tất cả các đối tượng "pause"
        foreach (GameObject pauseObject in pauseObjects)
        {
            pauseObject.SetActive(false);
        }

        if (pickScene != null)
        {
            setfalsePickScene();
        }

        if (ReadyUI != null && GoUI != null)
        {
            // Hiển thị ReadyUI trong 1 giây
            ReadyUI.SetActive(true);
            audioManager.PlaySFX(audioManager.Ready);
            yield return new WaitForSeconds(1f);
            ReadyUI.SetActive(false);

            // Hiển thị GoUI trong 2 giây
            GoUI.SetActive(true);
            audioManager.PlaySFX(audioManager.Go);
            yield return new WaitForSeconds(2f);
            GoUI.SetActive(false);
        }
        else
        {
            Debug.LogError("ReadyUI hoặc GoUI không được gán đúng!");
        }

        SetTruePickScene();
        pickScene.SetActive(false);

        // Bật lại tất cả các đối tượng "pause"
        foreach (GameObject pauseObject in pauseObjects)
        {
            pauseObject.SetActive(true);
        }
    }


    protected virtual void setfalsePickScene()
    {
        Transform pickBackground = pickScene.transform.Find("PickBackGround");
        if (pickBackground != null)
        {
            pickBackground.gameObject.SetActive(false);
        }

        Transform canvasTransform = pickScene.transform.Find("Canvas");
        if (canvasTransform != null)
        {
            foreach (Transform child in canvasTransform)
            {
                if (child.gameObject.name == "ButtonOK")
                {
                   child.GetComponent<Image>().enabled = false;
                   
             
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
    protected virtual void SetTruePickScene()
    {
        Transform pickBackground = pickScene.transform.Find("PickBackGround");
        if (pickBackground != null)
        {
            pickBackground.gameObject.SetActive(true);
        }

        Transform canvasTransform = pickScene.transform.Find("Canvas");
        if (canvasTransform != null)
        {
            foreach (Transform child in canvasTransform)
            {
                if (child.gameObject.name == "ButtonOK")
                {
                    child.GetComponent<Image>().enabled = true;
                }
                else
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }

}
