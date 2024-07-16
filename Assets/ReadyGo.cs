//using UnityEngine;
//using System.Collections; // Ensure this namespace is included for IEnumerator

//public class ReadyGo : MonoBehaviour
//{
//    [SerializeField] private GameObject ReadyUI;
//    [SerializeField] private GameObject GoUI;
//    [SerializeField] private AudioManager audioManager; // Serialized field for AudioManager

//    private void Awake()
//    {
//        // Assign AudioManager component if not already assigned in the inspector
//        if (audioManager == null)
//        {
//            GameObject audioManagerObj = GameObject.Find("AudioManager");
//            if (audioManagerObj != null)
//            {
//                audioManager = audioManagerObj.GetComponent<AudioManager>();
//            }
//            else
//            {
//                Debug.LogError("AudioManager not found in the scene.");
//            }
//        }
//    }

//    private void Start()
//    { 
//            ReadyUI = transform.Find("Canvas").Find("ready").gameObject;
//            GoUI = transform.Find("Canvas").Find("go").gameObject;

//        ReadyUI.SetActive(false);
//        GoUI.SetActive(false);
//        // Start the coroutine to handle the UI display
//        if (ReadyUI != null && GoUI != null)
//        {
//            StartCoroutine(DisplayReadyAndGoUI());
//        }
       
//    }

//    private IEnumerator DisplayReadyAndGoUI()
//    {
//        // Show ReadyUI and play sound if necessary
//        ReadyUI.SetActive(true);
        
//        // Wait for 1 second
//        yield return new WaitForSeconds(1f);

//        // Hide ReadyUI
//        ReadyUI.SetActive(false);   
//        GoUI.SetActive(true);
//        yield return new WaitForSeconds(2f);
//        GoUI.SetActive(false);
//    }
//}
