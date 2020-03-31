using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _nextFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private bool _isTripleShotEnabled = false;
    private bool _isShieldEnabled = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;

    private UIManager _uiManager;
    private int _score;

    // Start is called before the first frame update
    void Start()
    {
        _score = 0;
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
}

    // Update is called once per frame
    void Update()
    {
        MovementController();
        FiringController();
    }

    void MovementController()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4f, 0), 0);

        if (transform.position.x > 11f)
            transform.position = new Vector3(-11f, transform.position.y, 0);
        else if (transform.position.x < -11f)
            transform.position = new Vector3(11f, transform.position.y, 0);
    }

    void FiringController() 
    {
        if (Input.GetKeyDown(KeyCode.Space) && _nextFire < Time.time) {
            _nextFire = Time.time + _fireRate;
            if (!_isTripleShotEnabled)
                Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1.05f, 0), Quaternion.identity);
            else
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
    }

    public void Damage()
    {
        if (_isShieldEnabled)
        {
            _isShieldEnabled = false;
            _shieldVisualizer.SetActive(false);
        }
        else
        {
            if (_lives > 1)
                _lives--;
            else
            {
                Destroy(this.gameObject);
                _spawnManager.OnPlayerDeath();
            }
        }
    }

    public void ActivateTripleShot()
    {
        _isTripleShotEnabled = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotEnabled = false;
    }

    public void ActivateSpeedBoost()
    {
        _speed = 8f;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(10f);
        _speed = 3.5f;
    }

    public void ActivateShield()
    {
        _isShieldEnabled = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
