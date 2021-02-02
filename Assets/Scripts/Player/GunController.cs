using Mirror;
using UnityEngine;

public class GunController : NetworkBehaviour
{
    #region Variables

    [SerializeField] public KeyCode shootKey;

	[SerializeField] public GameObject wallHitParticle;
    [SerializeField] public GameObject bodyHitParticle;
    [SerializeField] public GameObject headHitParticle;

    private string currentGun;

    [SerializeField] public const float pistolTimer = 1f;
    [HideInInspector, SerializeField] public float currentPistolTimer;
    [HideInInspector, SerializeField] public const float pistolHeadDamage = 50f;
    [HideInInspector, SerializeField] public const float pistolBodyDamage = 15f;

    private const string startGun = "Glock";

    private Camera fpCam;

    #endregion

    #region Functions

    void Start(){
        fpCam = this.transform.Find("FP Camera").GetComponent<Camera>();

        currentGun = "Pistol";

        float gunTimer = (float) getVariableValue(currentGun.ToLower() + "Timer");

        setVariableValue("current" + currentGun + "Timer", gunTimer);

        GunManager.setSecondaryGun(startGun);
    }

    void Update(){
        if (!this.isLocalPlayer)
            return;

        //Debug.Log(secondaryGun);

        float currentGunTimer = (float) getVariableValue("current" + currentGun + "Timer");
        float gunTimer = (float) getVariableValue(currentGun.ToLower() + "Timer");

        if (Input.GetKeyDown(shootKey)){
            if (currentGunTimer <= 0f){
                RaycastHit hit;
                if (Physics.Raycast(fpCam.transform.position, fpCam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Collide)){
                    if (hit.collider.tag == "Head"){
                        Instantiate(headHitParticle, hit.point, Quaternion.identity);

                        Transform player = hit.transform.parent.parent;

                        player.GetComponent<PlayerManager>().currentHealth -= (float) getVariableValue(currentGun.ToLower() + "HeadDamage");
                        Debug.Log(player.GetComponent<PlayerManager>().currentHealth);
                    }
                    else if (hit.collider.tag == "Body"){
                        Instantiate(bodyHitParticle, hit.point, Quaternion.identity);

                        Transform player = hit.transform.parent.parent;

                        player.GetComponent<PlayerManager>().currentHealth -= (float) getVariableValue(currentGun.ToLower() + "BodyDamage");
                        Debug.Log(player.GetComponent<PlayerManager>().currentHealth);
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

    public static void test(){
        Debug.Log("huh");
    }

    #endregion
}




public class GunManager 
{
    private static string primaryGun, secondaryGun;
    


    public static string getPrimaryGun(){
        return primaryGun;
    }
    public static string getSecondaryGun(){
        return secondaryGun;
    }
    public static void setPrimaryGun(string name){
        primaryGun = name;
    }
    public static void setSecondaryGun(string name){
        secondaryGun = name;
    }
}