using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//This functions code was modeled of the tutorial series found here: https://www.youtube.com/watch?v=hk6cUanSfXQ&ab_channel=Blackthornprod
public class LevelGeneration : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform[] startingPositions;
    public GameObject[] rooms; // index 0 - LR openings, 1 - LRB, 2 - LRT, 3 - LRTB
    public GameObject[] specialRooms;

    public float moveAmount;    //corresponds to how far level generation must move to go to the next designated room (distance between each rooms center point)

    public float minX;
    public float maxX;
    public float maxY;
    private bool stopGeneration;

    private float timeBtwRoom;
    public float startTimeBtwRoom = 15.00f;

    private int _startingPositionIndex;
    private int _direction;

    public LayerMask room;  //this will be used with our roomDetection collider to make sure it can only collide with rooms
    
    private void Start()
    {
        var position = transform.position;
        minX += position.x;
        maxX += position.x;
        maxY += position.y;
        GeneratePath();
        FillEmpty();
    }
    
    private void GeneratePath()
    {
        //Randomly choose the starting position of our path (note we will be picking one of the 4 bottom tiles and traversing upwards
        
        _startingPositionIndex = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[_startingPositionIndex].position;  
        CreateRoom(1);

        //randomly assign direction to 1,2,3,4,5
        // 1,2 = right
        // 3,4 = left
        // 5 = up
        _direction = Random.Range(1, 6);
        
        while (!stopGeneration)
        {
            Move();
        }
    }

    private void FillEmpty()
    {
        for (var x = minX; x <= maxX; x += moveAmount)
        {
            for (var y = startingPositions[_startingPositionIndex].position.y; y <= maxY; y += moveAmount)
            {
                var position = new Vector2(x, y);
                bool roomPresent = Physics2D.OverlapCircle(position, 1, room);
                if (!roomPresent)
                {
                    var randomRoom = specialRooms[Random.Range(0, specialRooms.Length)];
                    Instantiate(randomRoom, position, Quaternion.identity, transform.parent);
                }
            }

        }
    }

    private void Move()
    {
        //move depending on direction
        if (_direction == 1 || _direction == 2)
        {
            if (transform.position.x < maxX)
            {
                //Move to the right if level Generator is not already at furthest room to the right
                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;
                
                //We don't really care what kind of openings our room has as we move horizontally since every room has LR
                //so pick a random roomType and Instantiate
                int randRoomType = Random.Range(0, rooms.Length);
                CreateRoom(randRoomType);
                
                //When generating rooms we want to move in the same direction on each level (right or left)
                //before moving to the above level and repeating the process
                
                //Thus for right we want to reassign direction, but ensure that the directions will only ever be
                //right (1,2) or up(5)
                
                _direction = Random.Range(1, 6);
                if (_direction == 3)
                {
                    _direction = 2;
                }
                else if (_direction == 4)
                {
                    _direction = 5;
                }
            }
            else // We know level generation is at the right boundary so guarentee we move up by chagning direction
            {
                _direction = 5;
            }
        }
        else if (_direction == 3 || _direction == 4)
        {
            if (transform.position.x > minX)
            {
                //Move to the left w/ same logic as right (above)
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;
                
                //same logic on room type as above
                int randRoomType = Random.Range(0, rooms.Length);
                CreateRoom(randRoomType);
                
                //similar logic about generation in same direction as above
                _direction = Random.Range(3, 6);
            }
            else
            {
                _direction = 5;
            }

        }
        else if (_direction == 5)
        {
            if (transform.position.y < maxY)
            {
                //When levelGeneration moves vertically, need to make sure the room we are currently in has top openings
                //and the room above will have a bottom opening  We will use roomDetection and the RoomDestruction 
                //defined in Roomtype to ensure this

                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);

                //if the current room doesn't have an opening at the top,  destroy it then replace it with a room that does
                var currentRoomType = roomDetection.GetComponent<RoomType>().type;
                
                // Convert LR -> LRT
                if (currentRoomType == 0)
                {
                    roomDetection.GetComponent<RoomType>().RoomDestruction();
                    CreateRoom(2);
                } 
                // Convert LRB -> LRTB
                else if (currentRoomType == 1)
                {
                    roomDetection.GetComponent<RoomType>().RoomDestruction();
                    CreateRoom(3);
                }

                //Move Up only when we have not reached the top max of the level yet
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y + moveAmount);
                transform.position = newPos;

                int rand = Random.Range(2, 4);
                if (rand == 2)
                {
                    rand = 1;
                }
                
                CreateRoom(rand);

                //if we just went up can go in any direction freely after
                _direction = Random.Range(1, 6);
            }
            else //if we are at max height and try to move up, we should stop level generation
            {
                stopGeneration = true;
            }

        }

    }

    private void CreateRoom(int index)
    {
        Instantiate(rooms[index], transform.position, Quaternion.identity, transform.parent);
    }
}
