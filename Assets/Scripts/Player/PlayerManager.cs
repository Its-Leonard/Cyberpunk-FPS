using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Variables

	[SerializeField] public float maxHealth;
    [HideInInspector] public float currentHealth;

    #endregion

    #region Functions

    void Start(){
        currentHealth = maxHealth;
    }

    void Update(){
        
    }

    #endregion
}
