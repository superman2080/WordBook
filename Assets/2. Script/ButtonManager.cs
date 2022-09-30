using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.IO;
public class ButtonManager : MonoBehaviour
{
    public GameObject content;
    public GameObject word;
    public ScrollRect scroll;
    public AudioSource audio;
    public AudioClip click;
    private List<GameObject> wordList = new List<GameObject>();

    private void Start()                    //시작할 때 xml파일에서 단어를 읽어오고 단어 생성
    {
        //Debug.Log(Application.persistentDataPath + "/Resources/WordList_Resource.xml");
        
        //string path = Application.persistentDataPath;
        //path = path.Substring(0, path.LastIndexOf('/'));
        //Debug.Log(Path.Combine(path, "Resources"));

        audio.volume = PlayerPrefs.GetFloat("Volume");

        List<Attr> xmlList = FileIO.Read("WordList_Resource");
        for (int i = 0; i < xmlList.Count; i++)
        {
            CreateWord(xmlList[i].kor, xmlList[i].eng);
        }
    }

    public void FixedUpdate()               //씬에 있는 단어들을 계속 워드리스트에 불러옴(RecallWord()), 스크롤뷰의 콘텐츠 사이즈 조정
    {
        scroll.content.sizeDelta = new Vector2(0, wordList.Count * 200);
        RecallWord();
    }
    public void CreateWord()                            //빈 단어 생성
    {
        if (wordList.Count == 0)             //리스트가 비었을때는 하나 생성하고 최상단에 위치
        {
            GameObject root = Instantiate(word, new Vector2(0, 0), Quaternion.identity, content.transform);
            root.transform.localPosition = new Vector2(0, 0);
            wordList.Add(root);
            return;
        }
        GameObject temp = Instantiate(word, new Vector2(wordList[wordList.Count - 1].transform.position.x, wordList[wordList.Count - 1].transform.position.y - 200), Quaternion.identity, content.transform);
        wordList.Add(temp);
    }

    public void CreateWord(string kor, string eng)       //단어 생성(로딩할 때)
    {
        audio.PlayOneShot(click);
        if (wordList.Count == 0)             //리스트가 비었을때는 하나 생성하고 최상단에 위치
        {
            GameObject root = Instantiate(word, new Vector2(0, 0), Quaternion.identity, content.transform);
            root.transform.localPosition = new Vector2(0, 0);
            root.GetComponent<Word>().kor.text = kor;
            root.GetComponent<Word>().eng.text = eng;
            wordList.Add(root);
            return;
        }
        GameObject temp = Instantiate(word, new Vector2(wordList[wordList.Count - 1].transform.position.x, wordList[wordList.Count - 1].transform.position.y - 200), Quaternion.identity, content.transform);
        temp.GetComponent<Word>().kor.text = kor;
        temp.GetComponent<Word>().eng.text = eng;
        wordList.Add(temp);
    }

    public void RecallWord()            //리스트를 초기화하고 씬에 있는 단어들을 다시 불러옴
    {
        wordList.Clear();               //리스트 초기화
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("WORD").Length; i++)              //씬에 있는 단어들 불러옴
        {
            wordList.Add(GameObject.FindGameObjectsWithTag("WORD")[i]);
        }
    }

    public void DeleteWord(GameObject w)                    //단어 삭제
    {
        audio.PlayOneShot(click);
        for (int i = 0; i < wordList.Count; i++)            //단어 리스트에서 삭제할 단어를 찾음
        {
            if (wordList[i].gameObject == w)
            {
                wordList.Remove(wordList[i]);               //찾을경우 삭제하고 브레이크
                break;
            }
        }
        for (int i = 0; i < wordList.Count; i++)            //단어 위치 재조정
        {
            wordList[i].transform.localPosition = new Vector2(0, i * -200);
        }
    }

    public void SaveWords()                                 //단어 저장
    {
        List<Attr> xmlList = new List<Attr>();              //xml에 저장할 리스트 생성
        for (int i = 0; i < wordList.Count; i++)
        {
            if (string.IsNullOrEmpty(wordList[i].GetComponent<Word>().kor.text) || string.IsNullOrEmpty(wordList[i].GetComponent<Word>().eng.text))         //영어나 한글 텍스트중 하나라도 비어있으면 삭제
                continue;
            Attr a = new Attr();
            a.kor = wordList[i].GetComponent<Word>().kor.text;
            a.eng = wordList[i].GetComponent<Word>().eng.text;
            xmlList.Add(a);
        }

        FileIO.Write(xmlList, Application.persistentDataPath + "/Resources/WordList_Resource.xml");       //저장
    }

    public void ShowWords()                 //워드 리스트에 있는 단어들을 콘솔에서 보여줌(디버그용)
    {
        for (int i = 0; i < wordList.Count; i++)
        {
            Debug.Log(wordList[i].GetComponent<Word>().kor.text + ", " + wordList[i].GetComponent<Word>().eng.text);
        }
    }
}