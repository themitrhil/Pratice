using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    private new Animator animation;
    private float FloatSpeed = 5;
    private bool isGrounded;
    private bool isRunning;
    private bool canAttack;
    private bool canMove;
    private bool canRun;

    void Start()
    {
        //Get animation controller
        animation = GetComponent<Animator>();

        //Ready to Move
        canMove = true;
        canRun  = true;
         
        //Ready to Attack
        canAttack = true;
    }


    void OnCollisionStay(Collision collision)
    {
        //Check if the player is on the floor
        isGrounded = true;
    }

    void Update()
    {
        {
            //Moviment
            var PlayerMoviment = new Vector3(0, 0, Input.GetAxisRaw("Vertical"));
            var NormalizedSpeed = PlayerMoviment.normalized;
            var Speed = FloatSpeed * NormalizedSpeed;
            var Distance = Speed * Time.deltaTime;
            transform.Translate(Distance);
            transform.Rotate(0, Input.GetAxisRaw("Horizontal"), 0);

            //Moving
            if (canMove)
                FloatSpeed = 5;

            //Standing
            if (!canMove)
                FloatSpeed = 0;

            //Running
            if (Input.GetKey(KeyCode.LeftShift) && canRun)
            {
                isRunning = true;
                FloatSpeed = 10;
            }

            //Jumping
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                var jump = new Vector3(0, 6, 0);
                rb.AddForce(jump, ForceMode.Impulse);
                isGrounded = false;
            }

            //Animations
            if (Input.GetKeyDown(KeyCode.R))
            {
                animation.SetTrigger("Hit1");
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                animation.SetTrigger("Fall1");
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                animation.SetTrigger("Up");
            }
            animation.SetFloat("speedv", Input.GetAxis("Vertical"));


            //Attacking
            if (canAttack)

                if (Input.GetMouseButtonDown(0))
                {
                canRun = false;
                canMove = false;
                canAttack = false;
                StartCoroutine(AttackingDelay());
                animation.SetTrigger("Attack1h1");
                StartCoroutine(HitFrame());
                }

        }
    }

    IEnumerator HitFrame()
    {
        //Sweet trigger timer
        yield return new WaitForSeconds(0.4f);
        SwordScript.SwordCollider.isTrigger = true;
        yield return new WaitForSeconds(0.4f);
        SwordScript.SwordCollider.isTrigger = false;
    }

    IEnumerator AttackingDelay()
    {
        //Return player moviment to normal
        yield return new WaitForSeconds(1.2f);
        canAttack = true;
        canMove = true;
        canRun = true;
    }
}
