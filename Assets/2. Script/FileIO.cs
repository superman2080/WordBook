using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.IO;
using UnityEditor;

public class Attr
{
    public string kor;
    public string eng;
}

public sealed class FileIO
{
    //void SaveOverlapXml(List<Attr> wordList, string filePath)
    //{
    //    TextAsset textAsset = (TextAsset)Resources.Load("");
    //    XmlDocument xmlDoc = new XmlDocument();
    //    xmlDoc.LoadXml(textAsset.text);

    //    XmlNodeList nodes = xmlDoc.SelectNodes("CharacterInfo/Character");
    //    foreach (var i in wordList)
    //    {
            
    //    }
    //    XmlNode character = nodes[0];

    //    character.SelectSingleNode("Name").InnerText = "wergia";
    //    character.SelectSingleNode("Level").InnerText = "5";
    //    character.SelectSingleNode("Experience").InnerText = "180";

    //    xmlDoc.Save(filePath);

    //    xmlDoc.Save("./Assets/Resources/Character.xml");
    //}

    //public static void WriteOverlapFile(List<Attr> wordList, string fileName)
    //{
    //    TextAsset textAsset = (TextAsset)Resources.Load(fileName);
    //    XmlDocument document = new XmlDocument();
    //    document.LoadXml(textAsset.text);

    //    XmlNodeList nodes = document.SelectNodes("WordList");
    //    XmlNode xmlNode = nodes[0];
    //    foreach(Attr attr in wordList)
    //    {
    //        xmlNode.SelectNodes
    //    }

    //    document.Save("./Assets/Resources/" + fileName + ".xml");
    //}

    public static void Write(List<Attr> wordList, string filePath)
    {

        XmlDocument document = new XmlDocument();
        XmlElement wordListElement = document.CreateElement("WordList");
        document.AppendChild(wordListElement);

        foreach(Attr attr in wordList)
        {
            XmlElement wordElement = document.CreateElement("Word");
            wordElement.SetAttribute("Kor", attr.kor.ToString());
            wordElement.SetAttribute("Eng", attr.eng.ToString());
            wordListElement.AppendChild(wordElement);
        }
        document.Save(Application.persistentDataPath + "/WordList_Resource.xml");
    }

    public static List<Attr> Read(string fileName)
    {
        //TextAsset textAsset = (TextAsset)Resources.Load(fileName);
        //string filePath = textAsset.text;
        XmlDocument document = new XmlDocument();
        document.Load(Application.persistentDataPath + "/WordList_Resource.xml");
        XmlElement wordListElement = document["WordList"];

        List<Attr> wordList = new List<Attr>();

        foreach (XmlElement wordElement in wordListElement.ChildNodes)
        {
            Attr attr = new Attr();
            attr.kor = wordElement.GetAttribute("Kor");
            attr.eng = wordElement.GetAttribute("Eng");
            wordList.Add(attr);
        }
        return wordList;
    }
}