using Mirror;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    #region Variables

    [SerializeField] public KeyCode jumpKey;
    [SerializeField] public KeyCode crouchKey;

    [SerializeField] public Transform standCamPos;
    [SerializeField] public Transform crouchCamPos;


    [SerializeField] public GameObject feetPos;
    private const float feetCheckRadius = 0.25f;

	private CharacterController controller;


    [SerializeField] public float fallingThreshold;


    [SerializeField] public float gravity;

    private float speed;

    private const float crouchSpeed = 5f;
    private const float crouchUnderSpeed = 2f; /*TODO:*/
    private const float aimSpeed = 3f;
    private const float walkSpeed = 6f;
    private const float fallSpeed = 6f;

    private const float jumpForce = 7f;

    private bool isGrounded;
    private bool lastIsGrounded;

    private float beginFallY;

    public float minFallDamageDistance;
    public float fallDamageMultiplier;

    private bool isCrouching;
    private bool isAiming;
    private bool isFalling;

    private Vector3 moveDir;

    private Camera fpCam;

    #endregion

    #region Functions

    private void Awake(){
        controller = this.GetComponent<CharacterController>();
    }

    private void Start(){
        speed = walkSpeed;

        isCrouching = false;
        isGrounded = false;
        lastIsGrounded = isGrounded;
        isFalling = false;

        moveDir = Vector3.zero;

        fpCam = this.transform.Find("FP Camera").GetComponent<Camera>();

        fpCam.transform.position = standCamPos.position;
    }

    private void Update(){
        if (!this.isLocalPlayer)
            return;


        LayerMask layerMask = LayerMask.GetMask("Ground");
        bool colliding = isColliding(feetPos.transform.position, feetCheckRadius, layerMask);

        if (colliding)
            isGrounded = true;
        else
            isGrounded = false;


        if (controller.velocity.y < fallingThreshold)
            isFalling = true;
        else
            isFalling = false;

        if (Input.GetKey(crouchKey)){
            isCrouching = true;
        } 
        else{
            isCrouching = false;
        }


        if (lastIsGrounded){
            if (!isGrounded){
                beginFallY = this.transform.position.y;
            }
        }
        if (isGrounded){
            if (!lastIsGrounded){
                float endFallY = this.transform.position.y;

                float fallDistance = beginFallY - endFallY;
                if (fallDistance >= minFallDamageDistance){
                    float fallDamageDistance = fallDistance - minFallDamageDistance;
                    float fallDamage = fallDamageDistance * fallDamageMultiplier;

                    /*TODO: REMOVE FALLDAMAGE FROM HEALTH */
                }
            }
        }

        lastIsGrounded = isGrounded;
    }


    private void FixedUpdate(){
        if (!this.isLocalPlayer)
            return;

        if (isCrouching){
            fpCam.transform.position = Vector3.MoveTowards(fpCam.transform.position, crouchCamPos.transform.position, 10f * Time.fixedDeltaTime);
        }
        else{
            fpCam.transform.position = Vector3.MoveTowards(fpCam.transform.position, standCamPos.transform.position, 10f * Time.fixedDeltaTime);
        }

        speed = walkSpeed;

        if (isCrouching)
            speed = crouchSpeed;
        if (isAiming)
            speed = aimSpeed;
        if (isFalling)
            speed = fallSpeed;

        if (isGrounded){
            moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDir = transform.TransformDirection(moveDir);
            moveDir *= speed;

            if (Input.GetKey(jumpKey)){
                moveDir.y = jumpForce;
            }
        }

        moveDir.y -= gravity * Time.fixedDeltaTime;



        controller.Move(moveDir * Time.fixedDeltaTime);
    }


    private bool isColliding(Vector3 center, float radius, LayerMask layerMask){
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, layerMask);

        if (hitColliders.Length == 0)
            return false;

        return true;
    }


    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(feetPos.transform.position, feetCheckRadius);
    }

    public bool getIsLocalPlayer(){
        return this.isLocalPlayer;
    }

    #endregion
}
