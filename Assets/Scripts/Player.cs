using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    private CharacterController _controller;
    private UIManager _uiManager;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _gravity = 1.0f;
    [SerializeField] private float _jumpHeight = 19.0f;
    [SerializeField] private int _coins;
    [SerializeField] private int _lives = 3;
    private float _yVelocity;
    private bool _canDoubleJump = false;
    [SerializeField] private AudioClip _coinSound;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _fireWorksSound;
    
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_uiManager == null)
        {
            Debug.LogError("The ui manager is null");
        }
        _uiManager.UpdateLivesDisplay(_lives);
    }

    // Update is called once per frame
    void Update()
    {
        //Moving The player on horizontal axis
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(horizontalInput, 0, 0);
        Vector3 velocity = direction * _speed;

        if (_controller.isGrounded == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
                _canDoubleJump = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space)) //double jump
            {
                if (_canDoubleJump == true)
                {
                    _yVelocity = _jumpHeight;
                    _canDoubleJump = false;
                }
            }
            _yVelocity -= _gravity; 
        }

        velocity.y = _yVelocity;
        _controller.Move(velocity * Time.deltaTime);
    }

    public void AddCoins()
    {
        _coins++;
        _uiManager.UpdateCoinDisplay(_coins);
        _audioSource.clip = _coinSound;
        _audioSource.Play();
        //play audio coins
    }

    public void Damage()
    {
        _lives--;
        _uiManager.UpdateLivesDisplay(_lives);
        _audioSource.clip = _fireWorksSound;
        _audioSource.Play();

        if (_lives < 1)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            int sceneNum = currentScene.buildIndex;
            SceneManager.LoadScene(sceneNum);
        }
    }

    public void WinLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int sceneNum = currentScene.buildIndex;
        SceneManager.LoadScene(sceneNum + 1);
    }

    public void addLives()
    {
        _lives++;
        _uiManager.UpdateLivesDisplay(_lives);
    }
}
