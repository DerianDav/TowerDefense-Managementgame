using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{

    float baseMoveSpeed = 100f;
    // xSpeed and ySpeed are a ratio based on direction it should move
    //xSpeed + ySpeed = 1;
    float xSpeed = 0f;
    float ySpeed = 0f;

    //Map Boundary: Destroy this object once it passes these in position
    float xBound = 200;
    float yBound = 200;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        Vector2 position = this.transform.position;
        position.x = position.x + baseMoveSpeed * xSpeed * Time.deltaTime;
        position.y = position.y + baseMoveSpeed * ySpeed * Time.deltaTime;

        this.transform.position = position;

        if (position.x > xBound || position.x < -xBound)
            Destroy(this);
        if(position.y > yBound || position.y < -yBound)
            Destroy (this);

    }

    public float getBaseSpeed() { return baseMoveSpeed; }

    public void setXSpeed(float speed) 
    { 
        xSpeed = speed; 
    
    }
    public void setYSpeed(float speed) 
    { 
        ySpeed = speed; 
    }
}
