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

    private int direction;    //integers representing the direction in which the next room will be placed
    public float moveAmount;    //corresponds to how far level generation must move to go to the next designated room (distance between each rooms center point)

    public float minX;
    public float maxX;
    public float maxY;
    private bool stopGeneratiion;

    private float timeBtwRoom;
    public float startTimeBtwRoom = 15.00f;

    private int roomsCount;

    public LayerMask room;  //this will be used with our roomDetection collider to make sure it can only collide with rooms
    void Start()
    {
        //Randomly choose the starting position of our path (note we will be picking one of the 4 bottom tiles and traversing upwards
        
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;  
        Instantiate(rooms[1], transform.position, Quaternion.identity);
        
        //randomly assign direction to 1,2,3,4,5, or 6
        direction = Random.Range(1, 6);
    }

    private void Move()
    {
        //move depending on direction
        if (direction == 1 || direction == 2)
        {
            if (transform.position.x < maxX)
            {
                //Move to the right if level Generator is not already at furthest room to the right
                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;
                
                //We don't really care what kind of openings our room has as we move horizontally since every room has LR
                //so pick a random roomType and Instantiate
                int randRoomType = Random.Range(0, rooms.Length);
                Instantiate(rooms[randRoomType], transform.position, Quaternion.identity);
                
                //When generating rooms we want to move in the same direction on each level (right or left)
                //before moving to the above level and repeating the process
                
                //Thus for right we want to reassign direction, but ensure that the directions will only ever be
                //right (1,2) or up(5)
                
                direction = Random.Range(1, 6);
                if (direction == 3)
                {
                    direction = 2;
                }
                else if (direction == 4)
                {
                    direction = 5;
                }
            }
            else // We know level generation is at the right boundary so guarentee we move up by chagning direction
            {
                direction = 5;
            }
        }
        else if (direction == 3 || direction == 4)
        {
            if (transform.position.x > minX)
            {
                //Move to the left w/ same logic as right (above)
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;
                
                //same logic on room type as above
                int randRoomType = Random.Range(0, rooms.Length);
                Instantiate(rooms[randRoomType], transform.position, Quaternion.identity);
                
                //similar logic about generation in same direction as above
                direction = Random.Range(3, 6);
            }
            else
            {
                direction = 5;
            }

        }
        else if (direction == 5)
        {
            if (transform.position.y < maxY)
            {
                //When levelGeneration moves vertically, need to make sure the room we are currently in has top openings
                //and the room above will have a bottom opening  We will use roomDetection and the RoomDestruction 
                //defined in Roomtype to ensure this

                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);

                //if the current room doesn't have an opening at the top,  destroy it then replace it with a room that does
                if (roomDetection.GetComponent<RoomType>().type != 2 &&
                    roomDetection.GetComponent<RoomType>().type != 3)
                {
                    roomDetection.GetComponent<RoomType>().RoomDestruction();

                    int randTopOpeningRoom = Random.Range(2, 4); //since 2 and 3 are our roomtypes with T openings
                    Instantiate(rooms[randTopOpeningRoom], transform.position, Quaternion.identity);
                }

                //Move Up only when we have not reached the top max of the level yet
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y + moveAmount);
                transform.position = newPos;

                int rand = Random.Range(2, 4);
                if (rand == 2)
                {
                    rand = 1;
                }
                Instantiate(rooms[rand], transform.position, Quaternion.identity);


                //if we just went up can go in any direction freely after
                direction = Random.Range(1, 6);
            }
            else //if we are at max height and try to move up, we should stop level generation
            {
                stopGeneratiion = true;
            }

        }

    }

    private void Update()
    {
        //limit on how quickly we make rooms mostly for initial testing
        if (timeBtwRoom <= 0 && stopGeneratiion == false)
        {
            Move();
            timeBtwRoom = startTimeBtwRoom;
        }
        else
        {
            timeBtwRoom -= Time.deltaTime;
        }
    }
}
