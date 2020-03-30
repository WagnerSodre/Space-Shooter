using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _nextFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
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
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1.05f, 0), Quaternion.identity);
        }
    }

    public void Damage()
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
