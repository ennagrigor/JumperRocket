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
        float horizontalInput = Input.GetAxis("Horizontal");      // Moving The player on horizontal axis
        Vector3 direction = new Vector3(horizontalInput, 0, 0);   // getting direction
        Vector3 velocity = direction * _speed;                    // calculating velocity

        if (_controller.isGrounded == true)                       // if the player is on the ground than he can jump
        {
            if (Input.GetKeyDown(KeyCode.Space))                  // jumps when the space key is pressed
            {
                _yVelocity = _jumpHeight;
                _canDoubleJump = true;                            // sets double jump to true so he can jump again
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))       // double jump
            {
                if (_canDoubleJump == true)
                {
                    _yVelocity = _jumpHeight;          // jumps again and sets to false so it is onl double
                    _canDoubleJump = false;
                }
            }
            _yVelocity -= _gravity;                    // takes gravity in account
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
        _coins++;                                  // add coins
        _uiManager.UpdateCoinDisplay(_coins);      // display on canvas
        _audioSource.clip = _coinSound;            // play audio for coins
        _audioSource.Play();
    }
```
<img src="https://github.com/ennagrigor/JumperRocket/blob/master/Photos/Star_Coin.png" width=400>

The player also has a damage funstion that removes lives and checks if we are dead - and if we are dead then re-starts the level.
It checks using scene manager which scene to load and than loads it.

```C#
    public void Damage()
    {
        _lives--;                                                // removes a life when called
        _uiManager.UpdateLivesDisplay(_lives);                   // updates the UI 
        _audioSource.clip = _fireWorksSound;
        _audioSource.Play();                                     // playes damage audio

        if (_lives < 1)                                          // if we are dead
        {
            Scene currentScene = SceneManager.GetActiveScene();
            int sceneNum = currentScene.buildIndex;
            SceneManager.LoadScene(sceneNum);
        }
    }
```

The player has a win function - that tells the game which level to go next:

```C#
public void WinLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();  // gets active scene
        int sceneNum = currentScene.buildIndex;              // gets index of active scene
        SceneManager.LoadScene(sceneNum + 1);                // loads next scene
    }
```

The last function is the add lives that happens when you collect a heart object:

```C#
public void addLives() //collecting hearts
    {
        _lives++;                                   // adds a life
        _uiManager.UpdateLivesDisplay(_lives);      // updates the UI
    }
```

<img src="https://github.com/ennagrigor/JumperRocket/blob/master/Photos/Heart.png" width=400>

## The moving platforms

The platforms are the objects that the player jumps on.
They consist two kinds:
Static platforms that the player jumps on and moving platforms. 
The moving platforms use two points that are added through the inspector.
On trigger with the player make the player it's child so it moves with the platform and doesn't fall off.
Once it jumps off the moving platform - it is no longer a child of the platform.

The code for moving platform:

```C#
void FixedUpdate()
    {
        if (_switching == false)      
        {
            // move to taget B
            transform.position = Vector3.MoveTowards(transform.position, _targetB.position, _speed * Time.deltaTime);
        }
        else if (_switching == true)
        {
            // move to target A
            transform.position = Vector3.MoveTowards(transform.position, _targetA.position, _speed * Time.deltaTime);
        }

        if (transform.position == _targetB.position)         // boolean flags to know which ay to go
        {
            _switching = true;
        }
        else if(transform.position == _targetA.position)
        {
            _switching = false;
        }
    }
```
The code for making the platform a parent so the player moves with platform:

```C#
private void OnTriggerEnter(Collider other) //in order to move with the platform become child of platform
    {
        if (other.tag == "Player")
        {
            other.transform.parent = this.transform;
        }
    }
private void OnTriggerExit(Collider other) //exiting platform -> no longer child of platform
    {
        if (other.tag =="Player")
        {
            other.transform.parent = null;
        }
    }
```
## Portal / Win Object

The way to go to next level is to reach the top of current level and trigger the portal.

```C#
private void OnTriggerEnter(Collider other)                // Portal to next level
    {
        if (other.tag == "Player")                         // if other is the player
        {
            Player player = other.GetComponent<Player>();  // get player components
            if (player != null)
            {
                player.WinLevel();                        // go to win function that takes the player to next scene
            }
        }
    }
```

## DeadZone

The DeadZone is when the player falls and triggers the zone that removes a life and respawns the pler to the begining
or starts over the game if there is no lives left.
 
 ```C#
private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")                          // if player triggers deadzone
        {
            Player player = other.GetComponent<Player>();  // gets player's components

            if (player != null)
            {
                player.Damage();                           // calls damage function in player
            }

            CharacterController cc = other.GetComponent<CharacterController>();

            if (cc != null)
            {
                cc.enabled = false;                        // so it falls and respawns
            }

            other.transform.position = _respawnPoint.transform.position;  // the location of respawn

            StartCoroutine(CCEnableRoutine(cc));                // using the coroutine function
        }
    }

    IEnumerator CCEnableRoutine(CharacterController controller) // access to charecter controller to reenable after fall 
    {
        yield return new WaitForSeconds(1.0f);
        controller.enabled = true;
    }
```

## Coin/Star

The coins are objects that look like 3D stars that the player must collect.
They have a trigger that calls the coins function in player and destroys them once collected. 

```C#
private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") // if the coin collided with player
        {
            Player player = other.GetComponent<Player>(); //get player's components

            if(player != null) // if player is not null
            {
                player.AddCoins(); 
            }
            Destroy(this.gameObject);
        }
    }
```

## Obsticales

The obsticles are moving balls that move from left to right and if the player triggers them - 
it loses a life (just like the deadZone) and respawns the player to begininng.
The balls move just like the moving platforms from point A to point B.

```C#
private void OnTriggerEnter(Collider other) // Player hits enemy
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            CharacterController cc = other.GetComponent<CharacterController>();

            if (cc != null)
            {
                cc.enabled = false; // so it falls and respawns
            }

            other.transform.position = _respawnPoint.transform.position;

            StartCoroutine(CCEnableRoutine(cc));
        }
    }

    IEnumerator CCEnableRoutine(CharacterController controller) // access to charecter controller to reenable after fall 
    {
        yield return new WaitForSeconds(1.0f);
        controller.enabled = true;
    }

    void FixedUpdate()
    {
        if (_switching == false)
        {
            // move to taget B
            transform.position = Vector3.MoveTowards(transform.position, _targetB.position, _speed * Time.deltaTime);
        }
        else if (_switching == true)
        {
            // move to target A
            transform.position = Vector3.MoveTowards(transform.position, _targetA.position, _speed * Time.deltaTime);
        }

        if (transform.position == _targetB.position)
        {
            _switching = true;
        }
        else if (transform.position == _targetA.position)
        {
            _switching = false;
        }
    }
}
```

## Level 1

It is the first level that consists only 9 regular platforms and 2 moving platforms, the spacing between the platforms is
pretty east to pass and they are all the same size.

<img src="https://github.com/ennagrigor/JumperRocket/blob/master/Photos/Level1.png" width=400>

## Level 2

The second level adds more platforms, it has 14 regular platforms and 2 moving platforms. 
The spacing between the platforms is a little bit more distant so it makes the level a bit harder to pass.
This level also introduces the player to moving balls that act as an enemy that if the player hits the ball they loss a life. 
The platforms are still all the same size.

<img src="https://github.com/ennagrigor/JumperRocket/blob/master/Photos/Level2.png" width=400>

<img src="https://github.com/ennagrigor/JumperRocket/blob/master/Photos/Level2_B.png" width=400>

## Level 3

The third and final level is the hardest to pass, it consists 20 regular platforms and 3 moving platforms. 
This level also has moving balls that act as enemies to the player. 
The spacing between the platforms is more felt and the player must make more precise moves and think before they jump. 
In this level the platforms are also in different sizes (some platforms are smaller) so it makes it harder to get to the platform. 
This level also adds one more life towards the end of the platforms to help the player reach the end if they fell. 

<img src="https://github.com/ennagrigor/JumperRocket/blob/master/Photos/Level3.png" width=400>

<img src="https://github.com/ennagrigor/JumperRocket/blob/master/Photos/Level3_B.png" width=400>

## ReloadGame:

Once passed all levels then the player gos to the reload game scene and can restart while cheering audio is played.

<img src="https://github.com/ennagrigor/JumperRocket/blob/master/Photos/Finish_Game.png" width=400>

## Audio

The game has a number of audio sounds:

- Background
- Coin sound when collecting stars
- Rocket sound when hit or fallen
- Cheering when won

## Link to ITCH.IO
[Jumper Rocket Game](https://ennagrigor.itch.io/jumperrocket)
