# gamedev-08-unity-physics
Contains the fourth question: [gamedev-5780](https://github.com/erelsgl-at-ariel/gamedev-5780)

**Created by:**

[Chen Ostrovski](https://github.com/ChenOst)

[Enna Grigor](https://github.com/ennagrigor)

This is a game that consists a 3D rocket that jumps through different platforms and collects stars/coins and has to avoid obsticles.
This game has three different levels that get harder as you go along.

## The Rocket / Player

The Player is a 3D rocket that has 4 main functions for movement. 
- `space` - makes the rocket jump.
- `double space` - makes the rocket double jump.
- `right key` - moves the rocket right.
- `left key` - moves the rocket left.

The code uses character controller that allows us to control phsical elements and use phisics. 

We use these parameters to initialize the character controller, speed, gravity, jump hight and velocity:

```C#
    private CharacterController _controller;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _gravity = 1.0f;
    [SerializeField] private float _jumpHeight = 19.0f;
    private float _yVelocity;
    private bool _canDoubleJump = false;
```

The movement then is implemented in this code:

```C#
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");              // Moving The player on horizontal axis
        Vector3 direction = new Vector3(horizontalInput, 0, 0);           // getting direction
        Vector3 velocity = direction * _speed;                            // calculating velocity

        if (_controller.isGrounded == true)                               // if the player is on the ground than he can jump
        {
            if (Input.GetKeyDown(KeyCode.Space))                          // jumps when the space key is pressed
            {
                _yVelocity = _jumpHeight;
                _canDoubleJump = true;                                    // sets double jump to true so he can jump again
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))                          // double jump
            {
                if (_canDoubleJump == true)
                {
                    _yVelocity = _jumpHeight;                             // jumps again and sets to false so it is onl double
                    _canDoubleJump = false;
                }
            }
            _yVelocity -= _gravity;                                       // takes gravity in account
        }

        velocity.y = _yVelocity;
        _controller.Move(velocity * Time.deltaTime);
    }
```

The player also has multiple functions.
The fist one is to add coins and display them on the canvas using UI manager and play a coin audio:

```C#
public void AddCoins()
    {
        _coins++;                                                         // add coins
        _uiManager.UpdateCoinDisplay(_coins);                             // display on canvas
        _audioSource.clip = _coinSound;                                   // play audio for coins
        _audioSource.Play();
    }
```
<img src="https://github.com/ennagrigor/JumperRocket/blob/master/Photos/Star_Coin.png" width=400>

The player also has a damage funstion that removes lives and checks if we are dead - and if we are dead then re-starts the level.
It checks using scene manager which scene to load and than loads it.

```C#
    public void Damage()
    {
        _lives--;                                                         // removes a life when called
        _uiManager.UpdateLivesDisplay(_lives);                            // updates the UI 
        _audioSource.clip = _fireWorksSound;
        _audioSource.Play();                                              // playes damage audio

        if (_lives < 1)                                                   // checks if we are dead 
        {
            Scene currentScene = SceneManager.GetActiveScene();           // checks which scene we are at
            string sceneName = currentScene.name;

            if (sceneName.Equals("Level 1"))                              // loads scene that was played
            {
                SceneManager.LoadScene(0);
            }
            if (sceneName.Equals("Level 2"))
            {
                SceneManager.LoadScene(1);
            }
            if (sceneName.Equals("Level 3"))
            {
                SceneManager.LoadScene(2);
            }
        }
    }
```

The player has a win function - that tells the game which level to go next:

```C#
public void WinLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();                 // gets active scene
        string sceneName = currentScene.name;                               // gets it's name

        if (sceneName.Equals("Level 1"))                                    // loads the next scene according to current.
        {
            SceneManager.LoadScene(1);
        }
        if (sceneName.Equals("Level 2"))
        {
            SceneManager.LoadScene(2);
        }
        if (sceneName.Equals("Level 3"))
        {
            SceneManager.LoadScene(3);
        }
    }
```

The last function is the add lives that happens when you collect a heart object:

```C#
public void addLives() //collecting hearts
    {
        _lives++;                                                             // adds a life
        _uiManager.UpdateLivesDisplay(_lives);                                // updates the UI
    }
```

<img src="https://github.com/ennagrigor/JumperRocket/blob/master/Photos/Heart.png" width=400>

## The moving platforms

## DeadZone

## Coin/Star

## Obsticales

## Level 1

## Level 2

## Level 3

### ReloadGame:

## Audio

## Link to ITCH.IO
[Jumper Rocket Game](https://ennagrigor.itch.io/jumperrocket)
