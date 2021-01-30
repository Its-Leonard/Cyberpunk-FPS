using UnityEngine;

public class FPCameraController : MonoBehaviour
{
    #region Variables

	private const float mouseSensitivityX = 100f;
    private const float mouseSensitivityY = 75f;

    private float xRotation = 0f;

    [SerializeField] public Transform playerBody;

    #endregion

    #region Functions


    private void Awake(){

    }

    private void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update(){

    }

    private void FixedUpdate(){
        float mouseX = getMouseInput().Item1, mouseY = getMouseInput().Item2;

        LockXRotation(mouseY);

        RotateCamera();

        RotatePlayer(mouseX);
    }


    private (float, float) getMouseInput(){
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime;

        return (mouseX, mouseY);
    }

    private void LockXRotation(float mouseY){
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }

    private void RotateCamera(){
        this.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void RotatePlayer(float mouseX){
        playerBody.Rotate(Vector3.up * mouseX);
    }

    #endregion
}
