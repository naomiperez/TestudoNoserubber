using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class lets you create a path of gameobjects for the enemy to travel along. You do this
 * by creating a series of points (referred to as vertices/vertex) for the enemy to move to.
 * You can add new vertex's to the path in the editor window. You do this by creating a new
 * 3d cube. You will need to then remove the collider on the object, and click the checkmark
 * next to mesh renderer to off so that no image is displayed. This is kind of ugly but allows
 * you to click and drag the object in 2d space (empty game objects cant be clicked on in the 2d editor),
 * that being said if you dont care you can create a empty object and it will work all the same you just have
 * to go into 3d and grab it that way. If you click and drag the enemy holder, the entire setup you create
 * will move with you, so you dont have to change where each vertex is relative to another everytime.
 * When you create a new vertex you will then go onto the EnemyPathing script on the enemy you're looking to change
 * and increase the size of the vertices list. You will then drag the new vertex to the empty slot in the list.
 * You can change the order the vertex's are visited in by changing the order of vertex's you see in the list in the editor.
 * The vertex at the top is at list index 0 and will visited first, the second be list index 1 and visited second and so on.
 * Once the enemy has gotten to the last vertex in the list, it will then start moving towards the first vertex in the list,
 * creating a loop. 
 * Vertex travel time is the amount of time it takes for the enemy to move between each vertex. So the default value is 4,
 * meaning to get from vertex A to vertex B it will take 4 seconds.
 * The firstTargetVertex is more of a luxury device than anything. Basically, if you have two vertex's and the enemy in the middle
 * it will go to vertex 0 and then back to vertex 1 on initial startup. If the user can see the enemy on startup it might
 * look a bit funny so i added this so you can set firstTargetVertex to 1 so that the enemy will
 * target vertex 1 first and it will look more smooth. 99% of the time you can leave this at 0
 */
public class EnemyPathing : MonoBehaviour
{
    public List<GameObject> vertices = new List<GameObject>();
    public int vertexTravelTime = 4;
    public int firstTargetVertex = 0;

    private int counter;
    private bool ready = true;
    // Update is called once per frame
    void Start()
    {
        counter = firstTargetVertex;
    }

    void Update()
    {
        // wait until we're ready for the next vertex before calling again
        // pass in the next vertex in the list to start moving towards
        // this will create the effect of moving to all the vertexes in the list smoothly
        // allowing you to create custom paths for the enemy
        if (ready) { StartCoroutine(moveEnemy(vertices[counter % vertices.Count])); }
        
    }

    // starting from the current location, slowly move over to given vertex
    private IEnumerator moveEnemy(GameObject target)
    {
        ready = false;
        float delta = 0;
        Vector3 startPos = this.transform.position;
        while (delta < vertexTravelTime)
        {
            delta += Time.deltaTime;
            this.transform.position = Vector3.Lerp(startPos, target.transform.position, delta / vertexTravelTime);
            yield return null;
        }
        counter++;
        ready = true;
    }
}
