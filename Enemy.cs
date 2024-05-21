using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float speed = 4.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    // Start is called before the first frame update
    void Start()
    
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        // GameObject player = GameObject.Find("Player");
        // if (player != null)
        // {
        //     _player = player.GetComponent<Player>();
        // }

        // else
        // {
        //     Debug.LogError("Player GameObject not found.");
        // }
        _anim = GetComponent<Animator>();
    }
    // {   
    //     // transform.position = new Vector3(0, 8f, 0);
    //     _player = GameObject.Find("Player").GetComponent<Player>();
    // }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            laser[] lasers = enemyLaser.GetComponentsInChildren<laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }
    void CalculateMovement()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    // float verticalInput = Input.GetAxis("Vertical");
    // transform.position = new Vector3(0, 8f, 0);
    // transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (transform.position.y <= -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        // transform.position = new Vector3(Random.Range(-9.28f,9.28f), 8f, 0);
    }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enemy collided with: " + other.tag);
        
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            
                if (player != null)
                {
                    player.Damage();
                }
                _anim.SetTrigger("OnEnemyDeath");
                speed = 0;
                _audioSource.Play();
                Destroy(this.gameObject, 2.8f);
                
        }
    
        if (other.tag == "laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _anim.SetTrigger("OnEnemyDeath");
            speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
            
        }
    }

}