using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _speedMultiplier;

    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.25f;
    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldsActive = false;

    public bool isPlayerOne = false;
    public bool isPlayerTwo = false;

    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;
    [SerializeField]
    private int _score = 0;

    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;
    private GameManager _gameManager;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }
        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource on the player is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        if (!_gameManager.isCoopMode)
        {
            transform.position = new Vector3(0, -3.5f, 0);
        }
    }

    void Update()
    {
        if (isPlayerOne)
        {
            CalculateMovement();
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0)) && Time.time > _canFire)
            {
                FireLaser();
            }
        }
        if (isPlayerTwo)
        {
            Debug.Log("Player Two Update method called.");
            PlayerTwoMovement();
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("KeypadEnter detected.");
                FireLaser();
            }
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (_isSpeedBoostActive)
        {
            transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -9.23f, 9.23f),
            Mathf.Clamp(transform.position.y, -3.5f, 0),
            transform.position.z
        );
    }

    void PlayerTwoMovement()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.Keypad8)) { direction += Vector3.up; }
        if (Input.GetKey(KeyCode.Keypad6)) { direction += Vector3.right; }
        if (Input.GetKey(KeyCode.Keypad5)) { direction += Vector3.down; }
        if (Input.GetKey(KeyCode.Keypad4)) { direction += Vector3.left; }

        if (_isSpeedBoostActive)
        {
            transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -9.23f, 9.23f),
            Mathf.Clamp(transform.position.y, -3.5f, 0),
            transform.position.z
        );
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldsActive)
        {
            _isShieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.CheckForBestScore();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldsActive()
    {
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
