using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A place to put anything to be used across scenes 
/// This can be accessed in other scripts by using Globals.variableName
/// </summary>
public class Globals 
{
    public static int difficulty = 1; //1 for easy, 2 for medium, 3 for hard
    public enum Maps //defines an enum so map can be carried across from menu, may or may not be useful
    {
        ApeGarden, MapOne, MapTwo
    }
   public static Maps curMap = Maps.ApeGarden; //defines current map

}
