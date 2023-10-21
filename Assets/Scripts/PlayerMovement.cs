using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Equipe 6+")]
    [Header("Base Movement")]
    private float horizontalInput = 0f;
    private float verticalInput = 0f;
    public float speed = 6f;
    public float turnSmooth = 0.1f;
    float turnSmothVelocity; 

    [Header("Mine and Cut")]
    public LayerMask treesLayer;
    public LayerMask rocksLayer;
    public Transform axePoint;
    public Transform pickaxePoint;
    public float actionRate = 1f;
    float nextActionTime = 2f;

    [Header("Resoursces")]
    public int wood;
    public int stone;

    [Header("Bools")]
    public bool hand = true;
    public bool axe = false;
    public bool pickaxe = false;
    public bool leftMouseInput;

    [Header("Base Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private GameObject objectAxe;
    [SerializeField] private Ray axeRay;
    [SerializeField] private Ray pickaxeRay;
    [SerializeField] private float maxDistance = 1f;
    [SerializeField] private ItemContabilizer itemContabilizer;

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
            axeRay = new Ray(axePoint.position, transform.forward);

            if (Physics.Raycast(axeRay, out RaycastHit hit, maxDistance, treesLayer))
            {
                GameObject hitObject = hit.collider.gameObject; hitObject.GetComponent<Tree>().TakeDamage(20);
                Debug.Log(hit.collider.gameObject.name + " was hit!");
            }

            nextActionTime = Time.time + 1f / actionRate;
        }

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

    public void AddWood(int number)
    {
        wood += number;
        itemContabilizer.IncreaseItemCount();
    }

    public void SpendWood(int number)
    {
        wood -= number;
        itemContabilizer.DecreaseItemCount();
    }

    void MineState()
    {
        animator.Play("Mine");

        if (Time.time >= nextActionTime)
        {
            pickaxeRay = new Ray(pickaxePoint.position, transform.forward);

            if (Physics.Raycast(pickaxeRay, out RaycastHit hit, maxDistance, rocksLayer))
            {
                GameObject hitObject = hit.collider.gameObject; hitObject.GetComponent<Rocks>().TakeDamage(20);
                Debug.Log(hit.collider.gameObject.name + " was hit!");
            }

            nextActionTime = Time.time + 1f / actionRate;
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
    }

    public void AddStone(int number)
    {
        stone += number;
        itemContabilizer.IncreaseItemCount();
    }

    public void SpendStone(int number)
    {
        stone -= number;
        itemContabilizer.DecreaseItemCount();
    }
}


