using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class defines the types of rooms that can be spawned as well as 
public class RoomType : MonoBehaviour
{
    public int type;
    
    //This function will be used to replace rooms that were previously placed along the path, but result in blockages to
    //new rooms
    public void RoomDestruction()
    {
        Destroy(gameObject);
    }
}
