﻿using System.Collections.Generic;
using System;

//解析类，要添加[System.Serializable]可序列化特性
[Serializable]
public class LevelCfg
{

    public String Id;
    public string title;
    public string mapSprite;// 地图的图片
    public string iconSprite;
    public string classType;
    public string spriteType;

}



[Serializable]
public class LevelList
{

    //此处定义的名称与json文件中的表头一致
    public List<LevelCfg> LevelCfgTable;

}
