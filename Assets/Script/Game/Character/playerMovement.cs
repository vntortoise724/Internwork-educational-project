using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Adjust Speed")]
    public float speed = 2f;

    [Header("Status")]
    public int level = 1;

    private readonly string xMoveAxis = "Horizontal";
    private readonly bool isMoving;
    private float moveIntentionX = 0;
    private Rigidbody2D rb2D = null;
    private Animator animator;

    private enum MovementStatus { idle, walk }

    public Rigidbody2D Getrb2D
    {
        get { return rb2D; }
        protected set { rb2D = value; }
    }

    private void GetInput()
    {
        if (BetterDialManager.freezeControl == false)
        {
            moveIntentionX = Input.GetAxis(xMoveAxis);
            _ = Input.GetMouseButtonDown(0);
        } else
        {
            moveIntentionX = 0;
        }
    }

    private void HandleWalkWithKeyBoard()
    {
        if (moveIntentionX < 0 && transform.rotation.y == 0)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        else if (moveIntentionX > 0 && transform.rotation.y != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        Getrb2D.velocity = new Vector2(moveIntentionX * speed, Getrb2D.velocity.y);
    }

    private void HandleAnimation()
    {
        MovementStatus Status;

        if ((Mathf.Abs(moveIntentionX) > 0.1f) || isMoving)
        {
            Status = MovementStatus.walk;
        }

        else
        {
            Status = MovementStatus.idle;
        }

        animator.SetInteger("Status", (int)Status);
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (GetComponent<Rigidbody2D>())
        {
            rb2D = GetComponent<Rigidbody2D>();
        }

        if (GetComponent<Animator>())
        {
            animator = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        if (moveIntentionX > 0 || moveIntentionX < 0)
        {
            HandleWalkWithKeyBoard();
        }
    }
}
