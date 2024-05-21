using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{

    private float speed = 8.0f;
    private bool _isEnemyLaser = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }


    void MoveUp()
    {
    
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if(transform.position.y > 9f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }
    void MoveDown()
    {
    
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if(transform.position.y < -9f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }
    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }
    
}