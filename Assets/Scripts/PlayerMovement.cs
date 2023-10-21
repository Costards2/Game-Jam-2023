using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Base Move")]
    private float horizontalInput = 0f;
    private float verticalInput = 0f;
    public float speed = 6f;
    public float turnSmooth = 0.1f;
    float turnSmothVelocity; 

    [Header("Mine and Cut")]
    public LayerMask treesLayer;
    public Transform axePoint;
    public Transform pickaxePoint;
    public float actionRate = 2f;
    float nextActionTime = 0f;
    float radius = 10f;

    [Header("Resoursces")]
    [SerializeField] private int wood;
    [SerializeField] private int stone;

    [Header("Bools")]
    public bool hand = true;
    public bool axe = false;
    public bool pickaxe = false;
    public bool leftMouseInput;

    [Header("Base Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CharacterController characterController;

    enum State { Idle, Run, Cut, Mine }

    State state = State.Idle;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        leftMouseInput = Input.GetMouseButton(0);

    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            hand = true;
            axe = false;
            pickaxe = false;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            hand = false;
            axe = true;
            pickaxe = false;
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            hand = false;
            axe = false;
            pickaxe = true;
        }

        switch (state)
        {
            case State.Idle: IdleState(); break;
            case State.Run: RunState(); break;
            case State.Cut: CutState(); break;
            case State.Mine: MineState(); break;
        }
    }

    void IdleState()
    {
        animator.Play("Idle");

        if (horizontalInput != 0f || verticalInput != 0f)
        {
            state = State.Run;
        }
        else if (leftMouseInput && axe)
        {
            state = State.Cut;
        }
        else if (leftMouseInput && pickaxe)
        {
            state = State.Mine;
        }
    }

    void RunState()
    {
        animator.Play("Run");

        Vector3 moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

        if(moveDirection.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection .x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmothVelocity, turnSmooth);
            transform.rotation = Quaternion.Euler(0.0f, targetAngle ,0.0f);

            characterController.Move(moveDirection * speed * Time.deltaTime);
        }

        if (horizontalInput != 0f || verticalInput != 0f)
        {
            state = State.Run;
        }
        else if (horizontalInput == 0f)
        {
            state = State.Idle;
        }
        else if (leftMouseInput && axe)
        {
            state = State.Cut;
        }
        else if (leftMouseInput && pickaxe)
        {
            state = State.Mine;
        }
    }

    void CutState()
    {
        animator.Play("Cut");

        if (Time.time >= nextActionTime)
        {

            Vector3 center = transform.position;

            Collider[] trees = Physics.OverlapSphere(center, radius, treesLayer);


            foreach (Collider tree in trees)
            {
                tree.GetComponent<Tree>().TakeDamage(20);
            }

            nextActionTime = Time.time + 1f / actionRate;


            if (horizontalInput != 0f || verticalInput != 0f)
            {
                state = State.Run;
            }
            else if (horizontalInput == 0f)
            {
                state = State.Idle;
            }
            else if (leftMouseInput && pickaxe)
            {
                state = State.Mine;
            }
        }
    }

    void MineState()
    {
        animator.Play("Mine");

        if (horizontalInput != 0f || verticalInput != 0f)
        {
            state = State.Run;
        }
        else if (horizontalInput == 0f)
        {
            state = State.Idle;
        }
        else if (leftMouseInput && axe)
        {
            state = State.Cut;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (pickaxePoint || axePoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(pickaxePoint.position, radius);

        Gizmos.DrawWireSphere(axePoint.position, radius);
    }
}


