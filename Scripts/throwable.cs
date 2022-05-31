using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class throwable : MonoBehaviour
{
    public WeaponControl weapc;
    public GameObject[] weapon;
    InputManager inp;
    GameObject grenade;
    public enum Type
	{
        None,Normal_Grenade
	}
    public Type type;
    // Start is called before the first frame update
    void Start()
    { 
        inp = FindObjectOfType<InputManager>();
        //weapc.type = WeaponControl.Type.grenade;
        //type = Type.Normal_Grenade;
        switch (type)
        {
            case Type.Normal_Grenade:
                Debug.Log("throwable");
                inp.InputSpacePressedKeyEvent.AddListener(SpaceP);
                inp.InputSpaceKeyUPEvent.AddListener(SpaceU);
                grenade =  Instantiate(weapon[0],transform);
                grenade.transform.localScale = new Vector3(1f, 1f, 0);
                weapc.objectToShoot = grenade;
                break;

        }
        if(type != Type.None)
		{
        grenade.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        grenade.GetComponent<Rigidbody2D>().simulated = false;
        grenade.GetComponent<CapsuleCollider2D>().enabled =false;

		}


    }

    
    public void SpaceP()
	{
            weapc.chargePanel.SetActive(true);
		

       
    }
    public void SpaceU()
	{
            weapc.chargePanel.SetActive(false);
           
    }

  

}
