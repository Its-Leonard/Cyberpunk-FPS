using UnityEngine;

public class InteractManager : MonoBehaviour
{
    #region Variables

	private Camera fpCam;

    [SerializeField] public float pickupRange;
    [SerializeField] public LayerMask pickable;

    #endregion

    #region Functions

    void Start(){
        fpCam = this.transform.Find("FP Camera").GetComponent<Camera>();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.F)){
            RaycastHit hit;
            if (Physics.Raycast(fpCam.transform.position, fpCam.transform.forward, out hit, pickupRange, pickable, QueryTriggerInteraction.Collide)){
                if (hit.transform.tag == "PrimaryGun"){
                    GunManager.setPrimaryGun(hit.transform.parent.name);
                }
                if (hit.transform.tag == "SecondaryGun"){
                    Debug.Log(hit.transform.name + " *** " + GunManager.getSecondaryGun());
                    GunManager.setSecondaryGun(hit.transform.parent.name);
                }
            }
        }
    }

    #endregion
}
