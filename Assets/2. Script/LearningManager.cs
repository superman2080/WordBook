using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LearningManager : MonoBehaviour
{
    public Text correct;
    public Text placeHolder;
    public Text page;
    public Text result;
    public ScrollRect resultScroll;
    public InputField answer;
    public GameObject resultUI;
    public GameObject resultWord;
    public GameObject content;
    public AudioClip click;
    public AudioClip correctClip;
    public AudioClip wrongClip;
    public AudioSource audio;
    private int hintCnt;
    private int idx = 0;
    private List<Attr> hintList = new List<Attr>();
    private List<Attr> xmlList = new List<Attr>();
    private bool isUsedHint = false;
    private bool isFadePlaying = false;

    void Start()
    {
        xmlList = FileIO.Read("WordList_Resource");

        Debug.Log(Application.persistentDataPath);

        resultUI.SetActive(false);

        Attr temp;
        for (int i = 0; i < xmlList.Count; i++)                 //랜덤으로 섞기
        {
            var rand1 = Random.Range(0, xmlList.Count);
            var rand2 = Random.Range(0, xmlList.Count);

            temp = xmlList[rand1];
            xmlList[rand1] = xmlList[rand2];
            xmlList[rand2] = temp;
        }
    }
    void FixedUpdate()
    {
        if(idx < xmlList.Count)
            correct.text = xmlList[idx].kor;
        page.text = (idx + 1).ToString();
    }
    public void Hint()
    {
        audio.PlayOneShot(click);
        if (isUsedHint == true)
            return;
        hintCnt++;
        for (int i = 0; i < hintList.Count; i++)
        {
            if (hintList[i] == xmlList[idx])
                hintList.Remove(hintList[i]);
        }
        hintList.Add(xmlList[idx]);
        if (string.IsNullOrEmpty(answer.text))                      //정답칸이 비어있을 경우
        {
            placeHolder.text = xmlList[idx].eng.Substring(0, 2);
            isUsedHint = true;
            return;
        }
        else
        {
            for (int i = 0; i < answer.text.Length; i++)
            {
                if (answer.text[i] != xmlList[idx].eng[i])
                {
                    if (i + 2 < xmlList[idx].eng.Length)
                    {
                        placeHolder.text = xmlList[idx].eng.Substring(0, i + 2);
                        answer.text = string.Empty;
                    }
                    else
                    {
                        placeHolder.text = xmlList[idx].eng;
                        answer.text = string.Empty;
                    }
                    isUsedHint = true;
                    return;
                }
                else if(i == answer.text.Length - 1)
                {
                    if (i + 3 < xmlList[idx].eng.Length)
                    {
                        placeHolder.text = xmlList[idx].eng.Substring(0, i + 3);
                        answer.text = string.Empty;
                    }
                    else
                    {
                        placeHolder.text = xmlList[idx].eng;
                        answer.text = string.Empty;
                    }
                    isUsedHint = true;
                    return;
                }
            }
        }
    }
    public void Answer()
    {
        if(xmlList[idx].eng == answer.text)
        {
            audio.PlayOneShot(correctClip);
            answer.text = string.Empty;
            placeHolder.text = "Enter...";
            idx++;
            isUsedHint = false;
            if(!isFadePlaying)
            {
                answer.image.color = Color.green;
                StartCoroutine(FadeIn(answer.image, 0.3f));
            }
            if (idx == xmlList.Count)
                Result();
        }
        else
        {
            audio.PlayOneShot(wrongClip);
            if (!isFadePlaying)
            {
                answer.image.color = Color.red;
                StartCoroutine(FadeIn(answer.image, 0.3f));
            }
        }
    }

    public void Result()
    {
        resultUI.SetActive(true);
        result.text = "힌트 : " + hintCnt;
        resultScroll.content.sizeDelta = new Vector2(0, hintList.Count * 200);
        for (int i = 0; i < hintList.Count; i++)
        {
            GameObject temp = Instantiate(resultWord, new Vector2(0, 0), Quaternion.identity, content.transform);
            temp.GetComponent<ResultWord>().kor.text = hintList[i].kor;
            temp.GetComponent<ResultWord>().eng.text = hintList[i].eng;
            temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-200 * i) + 100);
        }
    }

    public void NextQuestion()                          //지금 문제를 맨 뒤로 넘김
    {
        placeHolder.text = "Enter...";
        isUsedHint = false;
        audio.PlayOneShot(click);
        if (idx >= xmlList.Count - 1)
        {
            return;
        }
        Attr temp = new Attr();
        for (int i = idx; i < xmlList.Count - 1; i++)
        {
            temp = xmlList[i];
            xmlList[i] = xmlList[i + 1];
            xmlList[i + 1] = temp;
        }
    }

    IEnumerator FadeIn(Image img, float time)           //하얀 색으로 시간동안 바꿔줌
    {
        Color c = img.color;
        Color first = c;
        float aniTime = 0;
        while(aniTime < 1f)
        {
            aniTime += Time.deltaTime / time;
            c.r = Mathf.Lerp(first.r, 1f, aniTime);
            c.g = Mathf.Lerp(first.g, 1f, aniTime);
            c.b = Mathf.Lerp(first.b, 1f, aniTime);
            img.color = c;
            yield return null;
        }
        isFadePlaying = false;
    }
}
