using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class Word : MonoBehaviour
{
    public InputField kor;
    public InputField eng;
    public GameObject buttonManager;

    public Word(string kor, string eng, GameObject buttonManager)
    {
        this.kor.text = kor;
        this.eng.text = eng;
        buttonManager = GameObject.Find("ButtonManager");
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonManager = GameObject.Find("ButtonManager");
    }
    public void DeleteWord()
    {
        buttonManager.GetComponent<ButtonManager>().DeleteWord(gameObject);
        Destroy(gameObject);
    }
}
