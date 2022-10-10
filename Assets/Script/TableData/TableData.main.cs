using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TableData : MonoBehaviour
{
    public class MainData
    {
        public string Script;
    }

    Dictionary<string, Dictionary<string, List<MainData>>> mainDataDic = new Dictionary<string, Dictionary<string, List<MainData>>>();

    void MainDataInit()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("main");

        for (int i = 0; i < data.Count; i++)
        {
            MainData mainData = new MainData();

            string Scene = data[i]["Scene"].ToString();
            string Speaker = data[i]["Speaker"].ToString();

            mainData.Script = data[i]["Script"].ToString();

            if (!mainDataDic.ContainsKey(Scene))
            {
                mainDataDic.Add(Scene, new Dictionary<string, List<MainData>>());
                if (!mainDataDic[Scene].ContainsKey(Speaker))
                {
                    mainDataDic[Scene].Add(Speaker, new List<MainData>());
                    mainDataDic[Scene][Speaker].Add(mainData);
                }
                else
                {
                    mainDataDic[Scene][Speaker].Add(mainData);
                }
            }
            else
            {
                if (!mainDataDic[Scene].ContainsKey(Speaker))
                {
                    mainDataDic[Scene].Add(Speaker, new List<MainData>());
                    mainDataDic[Scene][Speaker].Add(mainData);
                }
                else
                {
                    mainDataDic[Scene][Speaker].Add(mainData);
                }
            }
        }
    }

    public Dictionary<string, Dictionary<string, List<MainData>>> GetMainDataDic()
    {
        return mainDataDic;
    }

    public string GetScript(string Scene, string Speaker)
    {
        string str = "";
        str = mainDataDic[Scene][Speaker][0].Script;
        return str;
    }

    public List<MainData> GetScriptList(string scene, string speaker)
    {
        List<MainData> mainDatas = new List<MainData>();
        mainDatas = mainDataDic[scene][speaker];
        return mainDatas;
    }
}
