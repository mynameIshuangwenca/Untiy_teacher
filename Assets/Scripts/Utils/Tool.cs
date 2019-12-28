
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tool : MonoSingleton<Tool>
{

    //加载scene
    public void LoadScene(string nextSceneName)
    {
        // 获取本页面的名字  作为返回页面跳转
        Scene scene = SceneManager.GetActiveScene();
        PlayerPrefs.SetString("BeforeSence", scene.name);
        StartCoroutine(Loadscene(nextSceneName));
    }

    IEnumerator Loadscene(string nextSceneName)
    {
       
       
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);
        yield return new WaitForEndOfFrame();
        op.allowSceneActivation = true;


    }

   /// <summary>
   ///  返回上一个页面
   /// </summary>
    public void ReturnScene()
    {
        string sceneName;
       if ((sceneName = PlayerPrefs.GetString("BeforeSence", null)) == null)
        {
            Debug.Log("没有上一个sene");
            return ;

        }
        LoadScene(sceneName);
    }

    /// <summary>
    /// 重新加载
    /// </summary>
    public  void Reload()
    {
        Debug.Log("reload");
        Scene scene = SceneManager.GetActiveScene();
        LoadScene(scene.name);
    }
    // 加载图片

    IEnumerator GetText(string name, Image image)
    {
        string path =
#if UNITY_ANDROID && !UNITY_EDITOR
		Application.streamingAssetsPath; //安卓的Application.streamingAssetsPath已默认有"file://"
#elif UNITY_IOS && !UNITY_EDITOR
		"file://" + Application.streamingAssetsPath;
#elif UNITY_STANDLONE_WIN || UNITY_EDITOR
        "file://" + Application.streamingAssetsPath;
#else
		string.Empty;
#endif    

         path =path + name;
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            }
        }
    }



    public void getSprite(string name, Image image)
    {
        StartCoroutine(GetText(name, image));
    }



    /// <summary>
    /// 得到配置文件
    /// </summary>
    /// <returns></returns>
    public LevelList getConfig()
    {

        TextAsset jsonText = Resources.Load<TextAsset>("LevelConfigTable");
        LevelList levelCfgList = JsonUtility.FromJson<LevelList>(jsonText.text);

        if (levelCfgList == null)
        {
            Debug.Log("没有配置存储不存在");
            return null;
        }

        return levelCfgList;
    }

    /// <summary>
    /// 通过Id 找到LevelCfg
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public LevelCfg GetLevelCfg(string Id)
    {

        LevelList levelCfgList = getConfig();
        if (levelCfgList == null)
        {
            return null;
        }

        else
        {
            foreach (LevelCfg cfg in levelCfgList.LevelCfgTable)
            {
                if (Id == cfg.Id)
                {
                    return cfg;
                }
            }
        }
        Debug.Log("没有此编号");
        return null;
    }



    // 
    public int DirtFindByName(string name)
    {
        for (int i= 0;i<Util.dirtion.Count;i++)
        {
            if (name == Util.dirtion[i].ObjName)
                return i;
        }
        Debug.Log("没有找到方向");
        return 0;
    }



    public void GetIJByName(string name,out int i,out int j)
    {
        //如果在拆分时不需要包含空字符串，则可以使用 StringSplitOptions.RemoveEmptyEntries 选项，例如在上例中将 StringSplitOptions.None 更改成 StringSplitOptions.RemoveEmptyEntries
        string[] result = name.Split('_' );
        int length = result.Length;
        i = int.Parse(result[length-2]);
        j= int.Parse(result[length - 1]);

    }

    public  string IJToName(int i,int j)
    {
        return "Img_Map"  +"_" + i + "_" + j;
    }

    /// <summary>
    /// 通过名字获取周围MapPos的名字
    /// 注意：只负责得到名字不管是不是越界
    /// </summary>
    /// <param name="name">传入的名字</param>
    /// <param name="index">类型 0-3 ：上下左右</param>
    /// <returns>得到的mapPos 名字</returns>
    public string AroundName(string name,int index)
    {
        int i, j;
        GetIJByName(name, out i, out j);
        if (index==0)
        {
            i += 1;
        }
        else if (index==1)
        {
            i--;
        }
        else if(index==2)
        {
            j++;
        }
        else
        {
            j--;
        }
        
        return IJToName(i, j);

    }


    public int TurnLeft(int dirt)
    {
        int[] turn = {2,3,1,0};
        return turn[dirt];


    }

    public int TurnRight(int dirt)
    {
        int[] turn = {3,2,0,1};
        return turn[dirt];
       
    }

   
    public  void Fade (Image img ,bool flag)
    {
      Color  color = img.color;
        if (flag)
        {
            img.color = new Color(color.r, color.g, color.b, 1f);
        }
        else
        {
            img.color = new Color(color.r, color.g, color.b, 0.5f);
        }

       
    }


    // 深度复制  序列号
    public static T Clone<T>(T RealObject)
    {
        using (Stream stream = new MemoryStream())
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, RealObject);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)serializer.Deserialize(stream);
        }
    }
    public static T DeepCopyByReflect<T>(T obj)
    {
        //如果是字符串或值类型则直接返回
        if (obj is string || obj.GetType().IsValueType) return obj;
        object retval = Activator.CreateInstance(obj.GetType());
        FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        foreach (FieldInfo field in fields)
        {
            try { field.SetValue(retval, DeepCopyByReflect(field.GetValue(obj))); }
            catch { }
        }
        return (T)retval;
    }
}
