using UnityEngine;

public class FPCameraController : MonoBehaviour
{
    #region Variables

    private PlayerController playerController;

	private const float mouseSensitivityX = 100f;
    private const float mouseSensitivityY = 75f;

    private float xRotation = 0f;

    private Transform playerBody;

    #endregion

    #region Functions

    private void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerBody = this.transform.parent;
        playerController = this.transform.parent.GetComponent<PlayerController>();
    }

    private void FixedUpdate(){
        if (!playerController.getIsLocalPlayer()){
            this.GetComponent<Camera>().enabled = false;

            return;
        }

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
