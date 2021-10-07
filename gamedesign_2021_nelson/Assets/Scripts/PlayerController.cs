using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Public variables in categories
    [Header("Combat Settings")]
    public float playerHealth = 100f;
    [HideInInspector]
    public float playerMaxHealth = 100f;
    [SerializeField]
    public float attackRange = 0.5f;
    [SerializeField]
    public float meleeDamage = 10f;
    [SerializeField]
    public float meleeAttackInterval = 2f;
    [SerializeField]
    public float shurikenAttackInterval = 0.3f;
    [SerializeField]
    public int maxShuriken = 10;
    [SerializeField]
    public int currentShuriken = 0;
    [SerializeField]
    public float bombAttackInterval = 1f;
    [SerializeField]
    public int maxBombs = 5;
    [SerializeField]
    public int currentBombs = 0;
    [SerializeField]
    public int maxHealthPotions = 1;
    [SerializeField]
    public int currentHealthPotions = 0;
    
    // The slots for player consumable items
    [SerializeField]
    GameObject playerConsumableSlotOne;
    [SerializeField]
    GameObject playerConsumableSlotTwo;
    [SerializeField]
    GameObject playerConsumableSlotThree;
    

    [Header("Jump Settings")]
    public float playerMaxJumpCounter = 1;
    public float playerJumpCounter = 0;
    [SerializeField]
    float playerBaseJumpHeight = 14f;
    public float playerCurrentJumpHeight;
    [SerializeField]
    float playerGravity = 2.5f;
    [SerializeField]
    float fallingGravityMultiplier = 2f;

    [Header("Ability Settings")]
    public float wallClimbValue = 0f;
    public float dashUnlockedCheck = 0f;
    public float dashDistance = 4f;
    public float dashInterval = 2f;
    public float teleportUnlockedCheck;
    [SerializeField]
    float teleportRange;
    public float teleportInterval;

    [Header("Animation Settings")]
    [SerializeField]
    private EdgeCollider2D[] colliders;
    //index 0: idle/throw
    //index 1: walk
    //index 2: dash
    private int currentColliderIndex = 0;


    // Player variables needed in other scripts
    [HideInInspector]
    public int consumableSelection = 0;
    [HideInInspector]
    public float dashIntervalTimer;
    [HideInInspector]
    public float teleportIntervalTimer;
    [HideInInspector]
    public int yellowCount = 0;
    [HideInInspector]
    public int blueCount = 0;
    [HideInInspector]
    public int redCount = 0;
    
    // Player statistics which are only needed in this script
    float playerSpeed = 10f;
    float playerMaxSpeed = 8f;
    float groundedCheckRayLength = 0.01f;
    float nextMeleeTimer;
    float nextShurikenTimer;
    float nextBombTimer;
    float teleportDestinationDistance;
    bool teleportIntervalPassed = true;
    bool meleeIntervalPassed = true;
    bool shurikenIntervalPassed = true;
    bool bombIntervalPassed = true;
    bool dashIntervalPassed = true;
    bool hasNotJumped = true;
    bool dashUsed;

    // HUD Icons check if ever obtained, otherwise hide them
    [HideInInspector]
    public int shurikenObtainedCheck = 0;
    [HideInInspector]
    public GameObject shurikenIcon;
    [HideInInspector]
    public int bombObtainedCheck = 0;
    [HideInInspector]
    public GameObject bombIcon;
    [HideInInspector]
    public int healthPotionObtainedCheck = 0;
    [HideInInspector]
    public GameObject healthPotionIcon;

    // Keys have changed
    public event EventHandler OnKeysChanged;
    // Key holding list
    List<KeyCards.KeyType> keyList;
    
    // Player components
    Animator playerAnimator;
    Rigidbody2D rigidBody;
    EdgeCollider2D edgeCollider;
    SpriteRenderer spriteRenderer;
    Vector2 playerDirection;
    Transform attackPoint;
    Transform rangedPoint;
    LayerMask terrainLayerMask;
    LayerMask enemyLayers;
    Color rayColor;
    GameObject consumableSelectionOne;
    GameObject consumableSelectionTwo;
    GameObject consumableSelectionThree;
    GameObject teleportDestination;
    AbilityIcons abilitiesObject;



    //-----Keycard related functions etc. here-----

    // Make a list of every key type in awake
    void Awake()
    {
        keyList = new List<KeyCards.KeyType>();
    }

    // Return the list of keys
    public List<KeyCards.KeyType> GetKeyList()
    {
        return keyList;
    }

    // Add a key to the list
    public void AddKey(KeyCards.KeyType keyType)
    {
        //Debug.Log("Added Key" + keyType);
        keyList.Add(keyType);
        KeyCounter(keyType, 1);

        // Adding a key invokes the OnKeysChanged event
        OnKeysChanged?.Invoke(this, EventArgs.Empty);
    }

    // Remove a key from the list
    public void RemoveKey(KeyCards.KeyType keyType)
    {
        keyList.Remove(keyType);
        KeyCounter(keyType, -1);

        // Removing a key invokes the OnKeysChanged event
        OnKeysChanged?.Invoke(this, EventArgs.Empty);
    }

    // Check whether a specific key type is in the list
    public bool ContainsKey(KeyCards.KeyType keyType)
    {
        return keyList.Contains(keyType);
    }

    // Counter which tracks which keys have been obtained for saving purposes
    public void KeyCounter(KeyCards.KeyType clonedKeyType, int counter)
    {
        if (clonedKeyType == KeyCards.KeyType.Yellow)
        {
            yellowCount += counter;
        }

        else if (clonedKeyType == KeyCards.KeyType.Blue)
        {
            blueCount += counter;
        }

        else if (clonedKeyType == KeyCards.KeyType.Red)
        {
            redCount += counter;
        }

        //Debug.Log(yellowCount + " / " + blueCount + " / " + redCount);
    }

    //-----Keycard functions end here----


    // Start is called before the first frame update
    void Start()
    {
        // Assign the player's current jump height as the base jump height
        playerCurrentJumpHeight = playerBaseJumpHeight;

        // Find the necessary components of the player object
        playerAnimator = GetComponent<Animator>();
        attackPoint = GameObject.Find("attackPoint").GetComponent<Transform>();
        rangedPoint = GameObject.Find("rangedPoint").GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody2D>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        terrainLayerMask = LayerMask.GetMask("Terrain");
        enemyLayers = LayerMask.GetMask("Enemies");
        abilitiesObject = GameObject.FindWithTag("AbilitiesObject").GetComponent<AbilityIcons>();

        // Find if there is a saved amount of health for the player or use default (100f)
        playerHealth = PlayerPrefs.GetFloat("PlayerHealth", 100f);

        // Set the interval comparison time for dashing
        dashIntervalTimer = 0f;

        // Set the interval comparison time for teleporting
        teleportIntervalTimer = 0f;

        // Set the starting distance for teleportation distance comparison
        teleportDestinationDistance = teleportRange + 1f;

        // Set the interval comparison time for melee attacks
        nextMeleeTimer = 0f;

        // Set the interval comparison time for shuriken throws
        nextShurikenTimer = 0f;

        // Set the interval comparison time for bomb throws
        nextBombTimer = 0f;

        // Set gravity to a default if not set in editor
        if (playerGravity == 0)
        {
            playerGravity = 2.5f;
        }

        shurikenObtainedCheck = PlayerPrefs.GetInt("ShurikenObtained", 0);
        bombObtainedCheck = PlayerPrefs.GetInt("BombObtained", 0);
        healthPotionObtainedCheck = PlayerPrefs.GetInt("HealthPotionObtained", 0);

        shurikenIcon = GameObject.FindWithTag("ConsumableSlot1").transform.Find("ShurikenIcon").gameObject;
        bombIcon = GameObject.FindWithTag("ConsumableSlot2").transform.Find("BombIcon").gameObject;
        healthPotionIcon = GameObject.FindWithTag("ConsumableSlot3").transform.Find("HealthPotionIcon").gameObject;

        if (shurikenObtainedCheck == 0)
        {
            shurikenIcon.SetActive(false);
        }

        if (bombObtainedCheck == 0)
        {
            bombIcon.SetActive(false);
        }

        if (healthPotionObtainedCheck == 0)
        {
            healthPotionIcon.SetActive(false);
        }

        // Set the selected consumable slot
        consumableSelection = PlayerPrefs.GetInt("ConsumableSelection", 0);

        // Find the consumable slot selections in the HUD
        consumableSelectionOne = GameObject.FindWithTag("ConsumableSlot1").transform.Find("SelectionIcon").gameObject;
        consumableSelectionTwo = GameObject.FindWithTag("ConsumableSlot2").transform.Find("SelectionIcon").gameObject;
        consumableSelectionThree = GameObject.FindWithTag("ConsumableSlot3").transform.Find("SelectionIcon").gameObject;

        // Disable all of the consumable slot selections
        consumableSelectionOne.SetActive(false);
        consumableSelectionTwo.SetActive(false);
        consumableSelectionThree.SetActive(false);
        
        // If slot one is selected activate it
        if (consumableSelection == 0)
        {
            consumableSelectionOne.SetActive(true);
        }

        // If slot two is selected activate it
        else if (consumableSelection == 1)
        {
            consumableSelectionTwo.SetActive(true);
        }

        // If slot three is selected activate it
        if (consumableSelection == 2)
        {
            consumableSelectionThree.SetActive(true);
        }

        // Find the amounts of consumables the player has
        currentShuriken = PlayerPrefs.GetInt("ShurikenAmount", 0);
        currentBombs = PlayerPrefs.GetInt("BombAmount", 0);
        currentHealthPotions = PlayerPrefs.GetInt("HealthPotionAmount", 0);

        // Find the amount of keys of each type the player has obtained
        yellowCount = PlayerPrefs.GetInt("YellowKeyCount", 0);
        blueCount = PlayerPrefs.GetInt("BlueKeyCount", 0);
        redCount = PlayerPrefs.GetInt("RedKeyCount", 0);

        // For each yellow key obtained
        for (int i=0; i < yellowCount; i++)
        {
            // Add a yellow key to the list
            keyList.Add(KeyCards.KeyType.Yellow);
        }

        // For each blue key obtained
        for (int i=0; i < blueCount; i++)
        {
            // Add a blue key to the list
            keyList.Add(KeyCards.KeyType.Blue);
        }

        // For each red key obtained
        for (int i=0; i < redCount; i++)
        {
            // Add a red key to the list
            keyList.Add(KeyCards.KeyType.Red);
        }

        // Invoke HUD change for the amount of keys
        OnKeysChanged?.Invoke(this, EventArgs.Empty);

        /*
         ___________________________________________________________________
        |                       ABILITY TEMPLATE                            |
        |                                                                   |
        |           variable = PlayerPrefs.GetInt("Ability_X", 0);          |
        |                                                                   |
        |___________________________________________________________________|
        */

        // ----------ABILITIES----------

        // ABILITY 0 - Find out if double jump is unlocked (2) or use default value (1)
        playerMaxJumpCounter = PlayerPrefs.GetFloat("Ability_0", 1);

        // ABILITY 1 - Find out if wall climb is unlocked (0.02f) or use default value (0)
        wallClimbValue = PlayerPrefs.GetFloat("Ability_1", 0);

        // ABILITY 2 - Find out if dash is unlocked (1) or use default value (0)
        dashUnlockedCheck = PlayerPrefs.GetFloat("Ability_2", 0);
        
        // ABILITY 3 - Find out if teleport is unlocked (1) or use default value (0)
        teleportUnlockedCheck = PlayerPrefs.GetFloat("Ability_3", 0);

        // Import the coordinates to your location in the room or use default if unavailable, then warp to the location
        float tempXCoordinate = PlayerPrefs.GetFloat("Room " + SceneManager.GetActiveScene().buildIndex + " X Coordinate", GameObject.FindWithTag("SpawnPointLocation").transform.position.x);
        float tempYCoordinate = PlayerPrefs.GetFloat("Room " + SceneManager.GetActiveScene().buildIndex + " Y Coordinate", GameObject.FindWithTag("SpawnPointLocation").transform.position.y);
        transform.position = new Vector2(tempXCoordinate, tempYCoordinate);
    }

    // Update is called once per frame
    void Update()
    {
        // If the player's health drops to zero or bugs out to negative
        if (playerHealth <= 0) 
        {
            // Respawn the player
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // If an health container would overheal the player or the player's health bugs out to over max
        else if (playerHealth > playerMaxHealth)
        {
            // Reduce the player's health to the maximum
            playerHealth = playerMaxHealth;
        }

        // Jumping up with W or Up Arrow if your jump counter is not maxed
        if (Input.GetButtonDown("Jump") && playerJumpCounter < playerMaxJumpCounter)
        {
            hasNotJumped = false;
            Jump();
        }

        // Jumping up with Spacebar if your jump counter is not maxed
        if (Input.GetButtonDown("Jump2") && playerJumpCounter < playerMaxJumpCounter)
        {
            hasNotJumped = false;
            Jump();
        }

        // Check if enough time has passed since last melee attack
        if (Time.time > nextMeleeTimer + meleeAttackInterval)
        {
            meleeIntervalPassed = true;
        }

        // // Check if enough time has passed since last shuriken throw
        if (Time.time > nextShurikenTimer + shurikenAttackInterval)
        {
            shurikenIntervalPassed = true;
        }

        // // Check if enough time has passed since last bomb throw
        if (Time.time > nextBombTimer + bombAttackInterval)
        {
            bombIntervalPassed = true;
        }
        
        // Check if enough time has passed since last use of Dash
        if (dashUnlockedCheck == 1 && Time.time > dashIntervalTimer + dashInterval)
        {
            dashIntervalPassed = true;
        }

        // Check if enough time has passed since last use of Teleport
        if (teleportUnlockedCheck == 1 && Time.time > teleportIntervalTimer + teleportInterval)
        {
            teleportIntervalPassed = true;
        }
        
        // Get the Horizontal input of Input manager
        playerDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


        playerAnimator.SetFloat("Speed", Math.Abs(Input.GetAxis("Horizontal")));

        // While moving right
        if (playerDirection.x > 0) 
        {
            //playerAnimator.SetTrigger("walkStart");
            
            // Make the local scale's X positive to make the player face right
            Vector3 newScale = new Vector3(1, 1, 1);
            transform.localScale = newScale;
            //rotate rangedPoint to make it face right
            rangedPoint.rotation =  Quaternion.Euler(0, 0, 0);
        }

        // While moving left
        if (playerDirection.x < 0)
        {
            //playerAnimator.SetTrigger("walkStart");
            // Make the local scale's X negative to make the player face left
            Vector3 newScale = new Vector3(-1, 1, 1);
            transform.localScale = newScale;
            //rotate rangedPoint to make it face left
            rangedPoint.rotation = Quaternion.Euler(0, 180, 0);
        }

        //If player has stopped moving
        /*if (playerDirection.x == 0){
            playerAnimator.SetTrigger("walkEnd");
        }*/

        // Dashing with Left Shift if it is unlocked
        if (Input.GetButtonDown("Dash") && dashUnlockedCheck == 1 && dashIntervalPassed && rigidBody.velocity.x != 0)
        {
            //trigger dash animation
            playerAnimator.SetTrigger("UseDash");

            // If dash has not been used in this room yet
            if (abilitiesObject.firstDashUsage == false)
            {
                // Enable HUD timerS
                abilitiesObject.firstDashUsage = true;
            }

            // Mark that Dash was used
            dashUsed = true;

            // Reset Dash timer
            dashIntervalPassed = false;
            dashIntervalTimer = Time.time;
        }

        // Teleporting with F if it is unlocked
        if (Input.GetButtonDown("Teleport") && teleportUnlockedCheck == 1 && teleportIntervalPassed)
        {
            // Detect every enemy in teleportation range
            Collider2D[] foundEnemies = Physics2D.OverlapCircleAll(transform.position, teleportRange, enemyLayers);

            // For each enemy inside teleportation range
            foreach(Collider2D enemy in foundEnemies)
            {
                // If the enemy is not a boss or its minion
                if (enemy.tag == "Enemy")
                {
                    // Get the distance between the enemy and the player
                    Vector3 temporaryDistance = enemy.transform.position - transform.position;

                    // If the distance to this enemy is shorter than the previous shortest distance to an enemy
                    if (Mathf.Abs(temporaryDistance.magnitude) < teleportDestinationDistance)
                    {
                        // Change this enemy to the teleportation destination
                        teleportDestinationDistance = Mathf.Abs(temporaryDistance.magnitude);
                        teleportDestination = enemy.gameObject;
                    }
                }
            }

            // If no enemies are in range do not teleport
            if (foundEnemies.Length == 0)
            {
                teleportDestinationDistance = teleportRange + 1f;
                teleportDestination = null;
            }

            // If it exists
            if (teleportDestination != null)
            {
                // If teleport has not been used in this room yet
                if (abilitiesObject.firstTeleportUsage == false)
                {
                    // Enable HUD timer
                    abilitiesObject.firstTeleportUsage = true;
                }

                // Teleport to and instakill the enemy
                float temporaryPlayerHealth = playerHealth;
                transform.position = teleportDestination.GetComponent<BoxCollider2D>().bounds.center;
                teleportDestination.GetComponent<EnemyScript>().enemyHealth = 0;
                playerHealth = temporaryPlayerHealth;

                // Reset teleportation variables
                teleportDestinationDistance = teleportRange + 1f;
                teleportIntervalPassed = false;
                teleportIntervalTimer = Time.time;
            }
        }

        // Check for melee attack input
        if (Input.GetButtonDown("Attack") && meleeIntervalPassed)
        {
            // Attack
            MeleeAttack();

            // Reset attack timer
            meleeIntervalPassed = false;
            nextMeleeTimer = Time.time;
        }

        // Check for consumable input
        if (Input.GetButtonDown("Consumable"))
        {
            // If consumable slot 1 is selected and interval has passed
            if (consumableSelection == 0 && shurikenIntervalPassed)
            {
                // Throw a shuriken
                ThrowShuriken(playerConsumableSlotOne);

                // Reset shuriken cooldown
                shurikenIntervalPassed = false;
                nextShurikenTimer = Time.time;
            }
            
            // If consumable slot 2 is selected and interval has passed
            if (consumableSelection == 1 && bombIntervalPassed)
            {
                // Throw a bomb
                ThrowBomb(playerConsumableSlotTwo);

                // Reset bomb cooldown
                bombIntervalPassed = false;
                nextBombTimer = Time.time;
            }

            // If consumable slot 3 is selected
            if (consumableSelection == 2)
            {
                // Consume a health potion
                ConsumeHealthPotion(playerConsumableSlotThree);
            }
        }

        // Switch between consumable slots and selection icons
        if (Input.GetButtonDown("SelectConsumable1"))
        {
            consumableSelection = 0;
            consumableSelectionOne.SetActive(true);
            consumableSelectionTwo.SetActive(false);
            consumableSelectionThree.SetActive(false);
        }

        if (Input.GetButtonDown("SelectConsumable2"))
        {
            consumableSelection = 1;
            consumableSelectionOne.SetActive(false);
            consumableSelectionTwo.SetActive(true);
            consumableSelectionThree.SetActive(false);
        }

        if (Input.GetButtonDown("SelectConsumable3"))
        {
            consumableSelection = 2;
            consumableSelectionOne.SetActive(false);
            consumableSelectionTwo.SetActive(false);
            consumableSelectionThree.SetActive(true);
        }

        // If you obtain more shurikens than is max make shuriken count 10
        if (currentShuriken > maxShuriken)
        {
            currentShuriken = maxShuriken;
        }

        // If you obtain more bombs than is max make bomb count 5
        if (currentBombs > maxBombs)
        {
            currentBombs = maxBombs;
        }

        if (currentHealthPotions > maxHealthPotions)
        {
            currentHealthPotions = maxHealthPotions;
        }
    }

    // Fixed update occurs at the same time regardless of framerate
    private void FixedUpdate()
    {
        // Resetting the jump counter & jump height damage boost when player hits the ground
        // Inform the system that the player can fall again
        // Set gravity back to normal
        if (IsGrounded() && rigidBody.velocity.y == 0 || IsWallClimbing())
        {
            playerCurrentJumpHeight = playerBaseJumpHeight;
            playerJumpCounter = 0;
            hasNotJumped = true;
            rigidBody.gravityScale = playerGravity;
        }

        // Call movement
        PlayerMovement(playerDirection.x);

        // If falling and vertical velocity is lower than the bump velocity limit
        if (rigidBody.velocity.y < -1.67f)
        {
            // Increase gravity by the multiplier
            rigidBody.gravityScale = playerGravity * fallingGravityMultiplier;
        }
        
        // If the dash was used
        if (dashUsed)
        {
            // Move the player for dash distance and set the ability on cooldown
            rigidBody.MovePosition(transform.position + new Vector3(Input.GetAxis("Horizontal"), 0) * dashDistance);
            dashUsed = false;
        }

        // If the player has not jumped but their vertical velocity is lower than the bump velocity limit
        if (hasNotJumped && rigidBody.velocity.y < -1.67f)
        {
            // Count it as a fall and increase jump counter
            playerJumpCounter += 1;
            hasNotJumped = false;
        }
    }

    // The player movement function
    private void PlayerMovement(float direction)
    {
        // Set the speed and direction
        rigidBody.velocity = new Vector2(direction * playerSpeed, rigidBody.velocity.y);

        // If velocity exceeds the player's max speed
        if (Mathf.Abs(rigidBody.velocity.x) > playerMaxSpeed)
        {
            // Limit velocity
            rigidBody.velocity = new Vector2(Mathf.Sign(rigidBody.velocity.x) * playerMaxSpeed, rigidBody.velocity.y);
        }
    }

    // The function which checks if the player is grounded
    private bool IsGrounded() 
    {
        // Cast the box to check for ground
        RaycastHit2D rayCastHit = Physics2D.BoxCast(edgeCollider.bounds.center, new Vector3(edgeCollider.bounds.size.x / 2, edgeCollider.bounds.size.y, edgeCollider.bounds.size.z), 0f, Vector2.down, groundedCheckRayLength, terrainLayerMask);

        // Return the value so script knows whether the player's jump counter is reset or not
        return rayCastHit.collider;
    }

    // The function which checks if the player is wall climbing
    private bool IsWallClimbing()
    {
        // Cast the box to check for walls
        RaycastHit2D rayCastHit = Physics2D.BoxCast(edgeCollider.bounds.center, new Vector3(edgeCollider.bounds.size.x + wallClimbValue, edgeCollider.bounds.size.y / 2, edgeCollider.bounds.size.z), 0f, Vector2.down, groundedCheckRayLength, terrainLayerMask);

        // Return the value so script knows whether the player's jump counter is reset or not
        return rayCastHit.collider;
    }

    public void takeDamage(float enemyDamage)
    {
        playerAnimator.SetTrigger("TakeDamage");
       playerHealth -= enemyDamage;
    }

    // Melee attack function
    private void MeleeAttack()
    {
        Debug.Log("Melee triggered");
        // Create attack collider
        //playerAnimator.SetTrigger("useMelee");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Damage enemies
            if (enemy.TryGetComponent<EnemyScript>(out var enemyScript))
            {
                enemyScript.TakeDamage(meleeDamage);
            }
                
            else
            {
                return;
            }
        }
    }

    // Function to instantiate a thrown shuriken
    private void ThrowShuriken(GameObject chosenSlotItem)
    {
        // If the player has a shuriken
        if (currentShuriken > 0)
        {
            //play throw animation
            playerAnimator.SetTrigger("ThrowConsumable");
            // Throw a shuriken and remove one shuriken from the player's possession
            StartCoroutine(WaitAndThrowShuriken(0.25f, chosenSlotItem));
        }

        // If the player has no shurikens return
        else
        {
            //play a sound
            //Debug.Log("no shuriken to throw!");
            return;
        }
    }

    // Function to instantiate a thrown bomb
    private void ThrowBomb(GameObject chosenSlotItem)
    {
        // If the player has a bomb
        if (currentBombs > 0)
        {
            //play throw animation
            playerAnimator.SetTrigger("ThrowConsumable");
            // Throw a bomb and remove one bomb from the player's possession
            StartCoroutine(WaitAndThrowBomb(0.25f, chosenSlotItem));
        }

        // If the player has no bombs return
        else
        {
            //play a sound
            //Debug.Log("no bombs to throw!");
            return;
        }
    }

    // Function to consume a health potion
    private void ConsumeHealthPotion(GameObject chosenSlotItem)
    {
        // If the player has a health potion
        if (currentHealthPotions > 0)
        {
            // Replenish health and remove one health potion from the player's possession
            playerHealth += chosenSlotItem.GetComponent<PlayerHealthPotion>().healthReplenished;
            currentHealthPotions -= 1;
        }

        // If the player has no health potions return
        else
        {
            return;
        }
    }

    //waiting coroutines for consumables
    IEnumerator WaitAndThrowShuriken(float time, GameObject chosenSlotItem)
    {
        yield return new WaitForSecondsRealtime(time);
        Instantiate(chosenSlotItem, rangedPoint.position, rangedPoint.rotation);
        currentShuriken -= 1;
    }

    IEnumerator WaitAndThrowBomb(float time, GameObject chosenSlotItem)
    {
        yield return new WaitForSecondsRealtime(time);
        Instantiate(chosenSlotItem, rangedPoint.position, rangedPoint.rotation);
        currentBombs -= 1;
    }

    // Draw either the attack area or the teleportation range in editor
    private void OnDrawGizmosSelected() {

        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
        
        if (teleportUnlockedCheck == 1)
        {
            Gizmos.DrawWireSphere(transform.position, teleportRange);
        }
    }

    // Jump function
    private void Jump() 
    {
        // Set gravity and drag to normal, add an upwards force to the player and increase jump counter
        rigidBody.gravityScale = playerGravity;
        rigidBody.drag = 0f;
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        rigidBody.AddForce(Vector2.up * playerCurrentJumpHeight, ForceMode2D.Impulse);
        playerJumpCounter += 1;
    }


    public void SetColliderForAnimation( int animationIndex )
    {
    //disable collider of the last animation
    colliders[currentColliderIndex].enabled = false;
    currentColliderIndex = animationIndex;
    //enable collider of the new animatison
    colliders[currentColliderIndex].enabled = true;

    //reassign edgeCollider for other functions
    edgeCollider = colliders[animationIndex];
    }

    

    // Player's save function
    public void SavePlayer()
    {
        // Saves the player
        SaveSystem.SavePlayer(this);

        // Shows where it saves the file
        Debug.Log(Application.persistentDataPath);
    }

    
    // Player load function
    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        // Saved variables
        playerHealth = data.savedPlayerHealth;
        playerMaxHealth = data.savedPlayerMaxHealth;
        attackRange = data.savedAttackRange;
        meleeDamage = data.savedMeleeDamage;
        meleeAttackInterval = data.savedMeleeAttackInterval;
        shurikenAttackInterval = data.savedShurikenAttackInterval;
        currentShuriken = data.savedCurrentShuriken;
        currentBombs = data.savedCurrentBombs;
        maxBombs = data.savedMaxBombs;
        maxShuriken = data.savedMaxShuriken;
        wallClimbValue = data.savedWallClimbValue;
        dashUnlockedCheck = data.savedDashUnlockedCheck;
        dashDistance = data.savedDashDistance;
        dashInterval = data.savedDashInterval;
        teleportUnlockedCheck = data.savedTeleportUnlockedCheck;
        teleportInterval = data.savedTeleportInterval;
        yellowCount = data.savedYellowCount;
        blueCount = data.savedBlueCount;
        redCount = data.savedRedCount;


        SceneManager.LoadScene(data.savedSceneNumber);

        // Sets the player's position
        Vector2 position;
        position.x = data.savedPlayerPosition[0];
        position.y = data.savedPlayerPosition[1];
        transform.position = position;
    }
    
}