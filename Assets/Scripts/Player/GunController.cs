using UnityEngine;

public class GunController : MonoBehaviour
{
    #region Variables

    [SerializeField] public GlobalVariables globalVars;

    [SerializeField] public KeyCode shootKey;

	[SerializeField] public GameObject wallHitParticle;
    [SerializeField] public GameObject bodyHitParticle;
    [SerializeField] public GameObject headHitParticle;

    private string currentGun;

    [SerializeField] public const float pistolTimer = 1f;
    [HideInInspector, SerializeField] public float currentPistolTimer;

    #endregion

    #region Functions

    void Start(){
        currentGun = "Pistol";

        float gunTimer = (float) getVariableValue(currentGun.ToLower() + "Timer");

        setVariableValue("current" + currentGun + "Timer", gunTimer);
    }

    void Update(){
        float currentGunTimer = (float) getVariableValue("current" + currentGun + "Timer");
        float gunTimer = (float) getVariableValue(currentGun.ToLower() + "Timer");

        if (Input.GetKeyDown(shootKey)){
            if (currentGunTimer <= 0f){
                RaycastHit hit;
                if (Physics.Raycast(globalVars.fpCam.transform.position, globalVars.fpCam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Collide)){
                    if (hit.collider.tag == "Head"){
                        Instantiate(headHitParticle, hit.point, Quaternion.identity);
                    }
                    else if (hit.collider.tag == "Body"){
                        Instantiate(bodyHitParticle, hit.point, Quaternion.identity);
                    }
                    else{
                        Instantiate(wallHitParticle, hit.point, Quaternion.identity);
                    }
                }


                setVariableValue("current" + currentGun + "Timer", gunTimer);
            }
        }

        if (currentGunTimer > 0)
            setVariableValue("current" + currentGun + "Timer", currentGunTimer - Time.deltaTime);
    }


    private void setVariableValue(string name, object value){
        this.GetType().GetField(name).SetValue(this, value);
    }

    private object getVariableValue(string name){
        object value = this.GetType().GetField(name).GetValue(this);

        return value;
    }

    #endregion
}
