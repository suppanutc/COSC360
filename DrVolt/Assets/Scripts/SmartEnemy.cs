using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    public float enemySpeed;
    Transform rb;

    //public Trap[] traps;

    //private bool stop = false;

    private bool nearTrap;

    public LayerMask trap;

    Collider2D RayCastReturn;

    float castDistance = 0.7f;

    public bool move = true;

    [SerializeField]
    AudioSource deathSound;
    Animator anim;

    void Start()
    {
        rb = gameObject.GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Creates a raycast hit from any traps in the "traps" layer to the
        // left of the enemy.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, castDistance, trap);

        // If the raycast doesnt hit a trap, the enemy moves.
        if (hit.collider == null && move)
        {
            transform.position += Vector3.left * Time.deltaTime * enemySpeed;
            

        }

        // If the raycast hits a trap then the enemy will stop walking if that trap
        // is active.
        // Due to the size and movement of piston traps, they have a child edge collider
        // that the enemy uses to detect them instead.
        else if (hit.collider != null)
        {
            RayCastReturn = hit.collider;
            //walkSound.Stop();

            if (RayCastReturn.tag == "PistonWall")
            {


                if (RayCastReturn.gameObject.transform.parent.GetComponentInChildren<Trap>().IsOn() == false)
                {

                    transform.position += Vector3.left * Time.deltaTime * enemySpeed;

                }
            }

            else if (RayCastReturn.GetComponent<Trap>().IsOn() == false)
            {
                transform.position += Vector3.left * Time.deltaTime * enemySpeed;
            }
        }

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        
        // If the trigger is an active trap, the enemy dies
        if ((other.tag == "Trap" || other.tag == "Laser")&& other.GetComponent<Trap>().IsOn())
        {
            deathSound.Play();
            StartCoroutine(waitForDeathSound());
            return;
        }
    }

    IEnumerator waitForDeathSound()
    {
        anim.Play("Robot dead");
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponentInChildren<ParticleSystem>().Stop();
        enemySpeed = 0;
        yield return new WaitForSeconds(deathSound.clip.length);
        this.gameObject.SetActive(false);
        yield break;
    }
    // Plays the death sound
    
}