using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Base Movement")]
    [SerializeField] private float horizontalInput = 0f;
    [SerializeField] private float verticalInput = 0f;
    [SerializeField] public float speed = 16f;
    [SerializeField] public float turnSmooth = 0.1f;
    float turnSmothVelocity;

    [Header("Mine and Cut")]
    public LayerMask treesLayer;
    public LayerMask rocksLayer;
    public LayerMask fibreLayer;
    public Transform axePoint;
    public Transform pickaxePoint;
    public Transform scythePoint;
    public float actionRate = 1f;
    float nextActionTime = 2f;

    [Header("Resoursces")]
    public int wood;
    public int stone;
    public int fibre;

    [Header("Bools")]
    public bool hand = true;
    public bool axe = false;
    public bool pickaxe = false;
    public bool scythe = false;
    public bool leftMouseInput;

    [Header("Base Components")]
    Vector3 moveDirection;
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Ray axeRay;
    [SerializeField] private Ray pickaxeRay;
    [SerializeField] private Ray scytheRay;
    [SerializeField] private float maxDistance = 1f;
    [SerializeField] private ItemContabilizer itemContabilizer;
    [SerializeField] public float gravity = -3f;

    [Header("Ui")]
    public GameObject axeUi;
    public GameObject axeUIA;
    public GameObject pickaxeUI;
    public GameObject pickaxeUIA;
    public GameObject foiceUI;
    public GameObject foiceUIA;
    public GameObject handUI;
    public GameObject HandUIA;

    [SerializeField] AudioSource Som;

    enum State { Idle, Run, Cut, Mine, ScytheCut }

    State state = State.Idle;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        leftMouseInput = Input.GetMouseButton(0);

        switch (state)
        {
            case State.Idle: IdleState(); break;
            case State.Run: RunState(); break;
            case State.Cut: CutState(); break;
            case State.Mine: MineState(); break;
            case State.ScytheCut: ScytheCutState(); break;
        }
    }

    void FixedUpdate()
    {
        //Depois criar um script separado para identificar a troca de Items das mãos

        if (Input.GetKey(KeyCode.Alpha1))
        {
            hand = true; HandUIA.SetActive(true);
            axe = false; axeUIA.SetActive(false);
            pickaxe = false; pickaxeUIA.SetActive(false);
            scythe = false; foiceUIA.SetActive(false);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            hand = false; HandUIA.SetActive(false);
            axe = true; axeUIA.SetActive(true);
            pickaxe = false; pickaxeUIA.SetActive(false);
            scythe = false; foiceUIA.SetActive(false);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            hand = false; HandUIA.SetActive(false);
            axe = false; axeUIA.SetActive(false);
            pickaxe = true; pickaxeUIA.SetActive(true);
            scythe = false; foiceUIA.SetActive(false);
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            hand = false; HandUIA.SetActive(false);
            axe = false; axeUIA.SetActive(false);
            pickaxe = false; pickaxeUIA.SetActive(false);
            scythe = true; foiceUIA.SetActive(true);
        }
    }

    void IdleState()
    {
        animator.Play("Idle");
        Som.Stop();

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
        else if (leftMouseInput && scythe)
        {
            state = State.ScytheCut;
        }
    }

    void RunState()
    {
        animator.Play("Run");

        Som.Play();

        moveDirection = new Vector3(horizontalInput, gravity, verticalInput);

        moveDirection.Normalize();

        if(horizontalInput != 0f || verticalInput != 0f)
        {
            float targetAngle = Mathf.Atan2(horizontalInput, verticalInput) * Mathf.Rad2Deg;
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
        else if (leftMouseInput && scythe)
        {
            state = State.ScytheCut;
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
                GameObject hitObject = hit.collider.gameObject; 
                hitObject.GetComponent<Tree>().TakeDamage(20);
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
        else if (leftMouseInput && scythe)
        {
            state = State.ScytheCut;
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
                GameObject hitObject = hit.collider.gameObject;
                hitObject.GetComponent<Rocks>().TakeDamage(20);
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
        else if (leftMouseInput && scythe)
        {
            state = State.ScytheCut;
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

    void ScytheCutState()
    {
        animator.Play("ScytheCut");

        if (Time.time >= nextActionTime)
        {
            scytheRay = new Ray(scythePoint.position, transform.forward);

            if (Physics.Raycast(scytheRay, out RaycastHit hit, maxDistance, fibreLayer))
            {
                GameObject hitObject = hit.collider.gameObject; 
                hitObject.GetComponent<Grass>().TakeDamage(20);
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
        else if (leftMouseInput && pickaxe)
        {
            state = State.Mine;
        }

    }

    public void AddFibre(int number)
    {
        fibre += number;
        itemContabilizer.IncreaseItemCount();
    }

    public void SpendFibre(int number)
    {
        fibre -= number;
        itemContabilizer.DecreaseItemCount();
    }
}


