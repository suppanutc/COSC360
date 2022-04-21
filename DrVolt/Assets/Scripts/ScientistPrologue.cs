
using UnityEngine;

public class ScientistPrologue : MonoBehaviour
{
    // The related action/movement keys
    KeyCode left = GameMaster.left,
        right = GameMaster.right,
        interact = GameMaster.chargeKey;
    [SerializeField]
    float playerSpeed;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    // Reference to animator component
    Animator anim;
    [SerializeField]
    DialogueManager dialogue;
    [SerializeField]
    Prologuelevelmaster levelMaster;
    CircleCollider2D activationCircle1 = null;
    float horizontalMovement;
    bool positionFreeze;
    bool notInMachine = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (!positionFreeze)
        {
            horizontalMovement = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(horizontalMovement * playerSpeed, 0f);
        } else if (notInMachine)
        {
            horizontalMovement = -1;
            rb.velocity = new Vector2(horizontalMovement * playerSpeed, 0f);
        }
        if (horizontalMovement > 0)
        {
            sprite.flipX = false;


        }
        else if (horizontalMovement < 0)
        {
            sprite.flipX = true;
        }
        if (horizontalMovement != 0)
        {
            anim.Play("PrologueScientistWalk");
        }
        else
        {
            anim.Play("PrologueScientistIdle");
        }

        if (Input.GetKey(interact) && activationCircle1 != null)
        {
            notInMachine = true;
            
            activationCircle1.enabled = false;
            activationCircle1 = null;
        }
    }
    // Triiger collider handler
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "DialogueTrigger":
                collision.gameObject.GetComponent<DialogueTrigger>().DisableTrigger();

                if (!GameMaster.deactivateHUD)
                {

                    dialogue.gameObject.SetActive(true);
                    dialogue.BeginDialogue();
                }
                GameMaster.hintCounter++;
                break;
            case "MindSwap1":
                activationCircle1 = collision.GetComponent<CircleCollider2D>();
                positionFreeze = true;
                horizontalMovement = 0;
                rb.velocity = new Vector2(horizontalMovement * playerSpeed, 0f);
                break;
            case "MindSwap2":
                
                notInMachine = false;
                horizontalMovement = 0;
                
                rb.velocity = new Vector2(horizontalMovement * playerSpeed, 0f);
                anim.Play("PrologueScientistIdle");
                levelMaster.startSequence = true;
                collision.GetComponent<CircleCollider2D>().enabled = false;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "MindSwap1":
                activationCircle1 = null;
                break;
        }
    }
}

