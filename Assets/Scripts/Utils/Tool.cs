
using System;
using System.Collections;
using System.Collections.Generic;
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
        i = int.Parse(result[-2]);
        j= int.Parse(result[-1]);

    }

    public  string IJToName(int i,int j)
    {
        return "Img_Map"  +"_" + i + "_" + j;
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

}
