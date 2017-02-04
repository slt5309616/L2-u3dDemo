using UnityEngine;
using System.Collections;
using System.Xml;
using System;
using LitJson;
using System.IO;
using Assets.Scripts;
using Mono.Data.Sqlite;
using System.Collections.Generic;
public class U3dDemo : MonoBehaviour {
    private Action<int,int> printAdd = (a,b)=> print(a+b);
    private UILabel lbl;
    private UILabel lblTime;
    private UILabel lblJson;
    private UILabel lblHTTP;
    private UILabel lblSQL;
    private string ABpath ;
    private string jsonPath;
    private string urlHttp;
    private string dbName;
    private string xmlPath;
    private SQLUtility sqlUtl;
    struct Person {
        public string name;
        public string sex;
        public string age;
        public string hobby;
    }
	// Use this for initialization
    void Start() {

        lbl = GameObject.Find("Content").GetComponent<UILabel>();
        lblTime = GameObject.Find("lblTime").GetComponent<UILabel>();
        lblJson = GameObject.Find("lblJson").GetComponent<UILabel>();
        lblHTTP = GameObject.Find("lblHTTP").GetComponent<UILabel>();
        lblSQL = GameObject.Find("lblSQL").GetComponent<UILabel>();
        //lblLoadInfo = GameObject.Find("lblLoadInfo").GetComponent<UILabel>();
        EventDelegate.Add(GameObject.Find("LoadABBtn").GetComponent<UIButton>().onClick,OnLoadAbBtnClick);

        StartCoroutine(StartCountDown());
        lbl.text += "\n";
        //xml&coroutine
        xmlPath =  "Data/TestXML";
        StartCoroutine(LoadXML(xmlPath));
        //delegate
        printAdd.Invoke(1, 2);
        
        //ABpath = "file://" + Application.dataPath + "/StreamingAssets/" + "Unit_121001_Test.assetbundle";
        //ABpath = "file://" + Application.dataPath + "/StreamingAssets/" + "unit_121001-models.manifest";
        //ABpath = "file://" + Application.dataPath + "/StreamingAssets/" + "StreamingAssets"; 
        ABpath = "file://" + Application.dataPath + "/StreamingAssets/"; 

        //json
        jsonPath ="Data/u3dDemoL2Json";
        urlHttp = "http://www.haosimple.com/";
        LoadJson(jsonPath);
        StartCoroutine(GetHTTP(urlHttp));
        //database
        dbName = "u3dDemo.db";
        CreateDB(dbName);
    }

    IEnumerator LoadXML(string path) {
        XmlDocument xmlFile = new XmlDocument();
        yield return new WaitForSeconds(1.0f);
        var data = Resources.Load(path).ToString();
        xmlFile.LoadXml(data);

        XmlNodeList nodeList = xmlFile.SelectSingleNode("Message").ChildNodes;
        var itr = nodeList.GetEnumerator();
        while (itr.MoveNext()) {
            var node = (XmlElement)itr.Current;
            var innerItr = node.ChildNodes.GetEnumerator();
            while (innerItr.MoveNext()) {
                var innerNode = (XmlElement)innerItr.Current;
                lbl.text +=innerNode.GetAttribute("name") + ":" + innerNode.InnerText+"\n";
            }
        }
    }
    IEnumerator StartCountDown() {
        lblTime.text = "3";
        yield return new WaitForSeconds(1.0f);
        lblTime.text = "2";
        yield return new WaitForSeconds(1.0f);
        lblTime.text = "1";
        yield return new WaitForSeconds(1.0f);
        lblTime.text = "0";
    }
    private void LoadJson(string path) {
        Person person = new Person();
        person.name = "ovuvuevuevue enyetuenwuevue ugbemugbem osas";
        person.age = "25";
        person.sex = "male";
        person.hobby = "unknown";
        string strData1 = JsonMapper.ToJson(person);

        JsonData data2 = new JsonData();
        data2["name"] = "Terminator";
        data2["age"] = "unknown";
        data2["sex"] = "unknown";
        data2["hobby"] = "Killing Spree";
        string strData2 =data2.ToJson();

        string strData3 = Resources.Load(path).ToString();

        var myData1 = JsonMapper.ToObject<Person>(strData1);
        var myData2 = JsonMapper.ToObject(strData2);
        var myData3 = JsonMapper.ToObject<Person>(strData3);

        lblJson.text += "[FF0000FF]" + myData1.name + "[-]\n" + "age:" + myData1.age + "\n" + "sex:" + myData1.sex + "\n" + "hobby:" + myData1.hobby + "\n\n";
        lblJson.text += "[FF0000FF]" + myData2["name"] + "[-]\n" + "age:" + myData2["age"] + "\n" + "sex:" + myData2["sex"] + "\n" + "hobby:" + myData2["hobby"] + "\n\n";
        lblJson.text += "[FF0000FF]" + myData3.name + "[-]\n" + "age:" + myData3.age + "\n" + "sex:" + myData3.sex + "\n" + "hobby:" + myData3.hobby;
    }
    void OnLoadAbBtnClick() {
        StartCoroutine(LoadAB(ABpath));
    }
    IEnumerator LoadAB(string path) {


        WWW www1 = new WWW(path + "StreamingAssets");
        yield return www1;
        
        var abmanifast = www1.assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        var depends = abmanifast.GetAllDependencies("unit_121001-prefab");
        AssetBundle[] abArray= new AssetBundle[depends.Length];
        for (int i = 0; i < depends.Length;i++ ) {
            var tempWWW = new WWW(path + depends[i]);
            abArray[i]=tempWWW.assetBundle;
        }

        WWW www = new WWW(path + "unit_121001-prefab");
        yield return www;
        var obj = www.assetBundle.LoadAsset("Unit_121001_Test");
        yield return obj = Instantiate(obj);
        
        www.assetBundle.Unload(false);
        for (int i = 0; i < abArray.Length; i++) {
            abArray[i].Unload(false);
        }
    }
    IEnumerator GetHTTP(string url) {
        WWW www = new WWW (url);
        yield return www;
        if (www.error != null){
            lblHTTP.text="error is :" + www.error;
        }else{
            lblHTTP.text= "request ok : " + www.text.Remove(100,www.text.Length-101);
        }
    }
    private void CreateDB(string name) {
        sqlUtl = new SQLUtility();
        string connectingString = "URI=file:" + Application.dataPath + "/StreamingAssets/" + name;
        sqlUtl.OpenDataBase(connectingString);
        if (!sqlUtl.IsTableExist("Character")) {
            sqlUtl.CreateTable("Character", new string[] { "name", "age", "sex", "hobby" }, new string[] { "TEXT", "TEXT", "TEXT", "TEXT" });
            sqlUtl.InsertValues("Character", new string[] { "LichKing", "一万多年", "male", "Killing" });
            sqlUtl.InsertValues("Character", new string[] { "Arthas", "30", "male", "unknown" });
        }

        SqliteDataReader reader = sqlUtl.ReadFullTable("Character");
        while (reader.Read()){
            lblSQL.text += "[1300FFFF]" + reader.GetString(reader.GetOrdinal("name"))+"[-]"+"\n";
            lblSQL.text += "Age:" + reader.GetString(reader.GetOrdinal("age")) + "\n";
            lblSQL.text += "Sex:" + reader.GetString(reader.GetOrdinal("sex")) + "\n";
            lblSQL.text += "Hobby:" + reader.GetString(reader.GetOrdinal("hobby")) + "\n";
        }
        reader.Close();
        sqlUtl.CloseConnection();
    }
}
