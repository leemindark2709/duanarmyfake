using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTable : MonoBehaviour
{
    public Image image;
    private void Start()
    {
        image = GetComponent<Image>();
    }

}
