using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ParallaxLayer : PooledObject {

    public float speed = 1f;
    public GameObject prefab;
    public float yOffset = -1.5f;

    private Vector3 bounds;
    private List<GameObject> objectsInLayer = new List<GameObject>();
    private Vector3 leftBounds;
    private int spriteAmount = 2;

    public void Start()
    {
        bounds = prefab.GetComponent<SpriteRenderer>().sprite.bounds.min; // where the leftmost coordinate of the sprite is invert for rightmost coordinate if the pivot is in the middle.
        leftBounds = new Vector3(bounds.x,0,0);
        for (int i = 0; i < spriteAmount; i++)
        {
            //create needed components
            GameObject newSpriteObject = Instantiate(prefab);

            Vector3 newPos = new Vector3((-bounds.x * 2) * i, yOffset , 0); //generates the right position to instantiate the new sprite at.
            newSpriteObject.transform.position = newPos;
            newSpriteObject.transform.parent = this.gameObject.transform;
            objectsInLayer.Add(newSpriteObject);
        }
    }

    public void MoveLayer()
    {
        for (int i = 0; i < objectsInLayer.Count; i++)
        {
            if(objectsInLayer[1].transform.position.x <= 0)
            {
                objectsInLayer[0].transform.position = objectsInLayer[1].transform.position + -leftBounds * 2; 
                GameObject holdObject = objectsInLayer[0];
                objectsInLayer.RemoveAt(0);
                objectsInLayer.Add(holdObject); // puts moved object to the back of the list as it is in the world.
            }
            objectsInLayer[i].transform.Translate(Vector2.left * (Time.deltaTime * speed));
        }
    }

    public void Update()
    {
        MoveLayer();
    }
}
