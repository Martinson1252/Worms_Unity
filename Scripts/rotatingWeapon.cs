using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class rotatingWeapon : MonoBehaviour
{
    Action action;
    [HideInInspector] InputManager inp;
    public GameObject weapon;
    Vector3 rotateAngle;
    // Start is called before the first frame update
    void Start()
    {
        inp = FindObjectOfType<InputManager>();
        action += Rotate;
        inp.InputArrowUPkey.AddListener(PressArrowUP);
        inp.InputArrowUPkeyDown.AddListener(PressArrowUP_Down);
        inp.InputArrowDOWNkey.AddListener(PressArrowDOWN);
        inp.InputArrowDOWNkeyDown.AddListener(PressArrowDOWN_Down);
        rotateAngle.z = transform.eulerAngles.z;
    }
	
    public void TurnON()
	{
        action += Rotate;
	} 
    public void TurnOFF()
	{
        action -= Rotate;
	}

    // Update is called once per frame
    void Update()
    {
        action?.Invoke();
    }

    void Rotate()
	{
        rotateAngle.z = transform.eulerAngles.z;

        transform.localRotation = Quaternion.Euler( clampAngle( rotateAngle ));
       
    }

  

    void PressArrowDOWN()
    {
        rotateAngle.z = transform.eulerAngles.z;
        rotateAngle.z = rotateAngle.z - 22 * Time.fixedDeltaTime;
        transform.localRotation = Quaternion.Euler(clampAngle(rotateAngle));
    }
    void PressArrowUP_Down()
    {
        rotateAngle.z = transform.eulerAngles.z;
        rotateAngle.z = rotateAngle.z + 2 * Time.fixedDeltaTime;
        transform.localRotation = Quaternion.Euler(clampAngle(rotateAngle));
    }

    void PressArrowUP()
    {
        rotateAngle.z = transform.eulerAngles.z;
        rotateAngle.z = rotateAngle.z + 22 * Time.fixedDeltaTime;
        transform.localRotation = Quaternion.Euler(clampAngle(rotateAngle));
    }

    void PressArrowDOWN_Down()
    {
        rotateAngle.z = transform.eulerAngles.z;
        rotateAngle.z = rotateAngle.z - 2 * Time.fixedDeltaTime;
        transform.localRotation = Quaternion.Euler(clampAngle(rotateAngle));
    }

    Vector3 clampAngle(Vector3 value)
	{
        if (value.z > 180 && value.z < 270) return new Vector3(0,0,270);
        if (value.z > 90 && value.z < 180) return new Vector3(0,0,90);
        else return value;
	}



	

}
