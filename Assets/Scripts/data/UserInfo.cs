using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo
{
   
  
    private string name;
    private int age;
    private string gender;
   
    private string password;

    public UserInfo()
    {
    }

    public UserInfo( string name, int age, string gender)
    {
       
        this.Name = name;
        this.Age = age;
        this.Gender = gender;
       
    }

    public UserInfo( string name, int age, string gender, string password) : this( name, age, gender)
    {
        this.Password = password;
    }

  
    public string Name { get => name; set => name = value; }
    public int Age { get => age; set => age = value; }
    public string Gender { get => gender; set => gender = value; }
    public string Password { get => password; set => password = value; }
}
