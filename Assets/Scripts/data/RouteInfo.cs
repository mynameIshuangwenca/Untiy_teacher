using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteInfo 
{
    private int userId;
    private string  sceneId;
    private string start;
    private string middle;
    private string destination;
    private string route;
    private string routeOrder;
    private string stepNum;
    private string duration;
    private string fieldOne;
    private string fieldTwo;
    private string middleStep;

    public RouteInfo(int userId, string sceneId, string start, string middle, string destination, string route, string routeOrder)
    {
       
        this.UserId = userId;
        this.SceneId = sceneId;
        this.Start = start;
        this.Middle = middle;
        this.Destination = destination;
        this.Route = route;
        this.RouteOrder = routeOrder;
       
    }

    public RouteInfo(int userId, string sceneId, string start, string middle, string destination, string route, string routeOrder, string middleStep) : this(userId, sceneId, start, middle, destination, route, routeOrder)
    {
        this.middleStep = middleStep;
    }

    public int UserId { get => userId; set => userId = value; }
    public string SceneId { get => sceneId; set => sceneId = value; }
    public string Start { get => start; set => start = value; }
    public string Middle { get => middle; set => middle = value; }
    public string Destination { get => destination; set => destination = value; }
    public string Route { get => route; set => route = value; }
    public string RouteOrder { get => routeOrder; set => routeOrder = value; }
    public string StepNum { get => stepNum; set => stepNum = value; }
    public string Duration { get => duration; set => duration = value; }
    public string FieldOne { get => fieldOne; set => fieldOne = value; }
    public string FieldTwo { get => fieldTwo; set => fieldTwo = value; }
    public string MiddleStep { get => middleStep; set => middleStep = value; }

    public string [] GetStr()
    {
        return new string[] {string.Format("{0}", UserId), SceneId , Start , Middle, Destination, Route , RouteOrder, stepNum, duration, FieldOne , FieldTwo, middleStep };
    }
}
