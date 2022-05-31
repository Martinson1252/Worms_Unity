using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;
public class WeaponControl : MonoBehaviour
{
    public float time, rounds, weaponDamage,weaponForce,weaponRange;
    public Transform firepoint, crosshair;
    public Animator WeapAnim;
    public Action SpaceUP, SpacePressed, SpaceDown, ShootAction;
    public GameObject objectToShoot, chargePanel;
     [Range(0, 8)] public float shootPower,panelCharged;
    public Image[] powerCell;
    [HideInInspector] public Explosives ex;
    [HideInInspector] public WormUI ui;
    [HideInInspector] public CameraSC cam;
    [SerializeField] InputManager inp;
    [HideInInspector] public RoundManager rm;
    PhotonView pv;
    public Type type;
    private void Start()
    {
        rm = FindObjectOfType<RoundManager>();
        cam = Camera.main.GetComponent<CameraSC>();
        ex = FindObjectOfType<Explosives>();
        ui = FindObjectOfType<WormUI>();
        inp = FindObjectOfType<InputManager>();
        pv = FindObjectOfType<PhotonView>();
        SpaceUP = null;
        SpacePressed = null;
        //SpacePressed -= ChargePowerPanel;
        //inp.InputSpaceKeyUPEvent.RemoveAllListeners();
        //inp.InputSpacePressedKeyEvent.RemoveAllListeners();
        //inp.InputSpaceKeyDownEvent.RemoveAllListeners();
        ShootAction = null;
        inp.InputSpaceKeyUPEvent.AddListener(OnSpaceUP);
        inp.InputSpacePressedKeyEvent.AddListener(OnSpacePressed);
        inp.InputSpaceKeyDownEvent.AddListener(OnSpaceDown);
        Set();
    }




	public enum Type
    {
        Bazooka,
        Grenade,
        AK_47,
        Double_barrel
    }
    


    void OnSpaceUP()
    {
        SpaceUP?.Invoke();
    }
    void OnSpacePressed()
    {
        
        SpacePressed?.Invoke();
    }

    void OnSpaceDown()
	{
        SpaceDown?.Invoke();
	}


    public void Set()
    {

        switch (type)
        {
            case Type.Grenade:
                gameObject.GetComponent<throwable>().type = throwable.Type.Normal_Grenade;
                gameObject.GetComponent<throwable>().enabled = true;
                if (rm.ismyTurn)
                {
                    SpacePressed += ChargePowerPanel;
                    SpacePressed += () => pv.RPC("SpacePressOnlineShootInfo", RpcTarget.Others, panelCharged, shootPower);
                }
                else
                    SpacePressed += RECEIVEDChargePowerPanel;
                SpacePressed += ui.Disable_RoundCounter;
                SpaceUP += ThrowGrenade;
                break;
            case Type.Bazooka:
                if (rm.ismyTurn)
                {
                    SpacePressed += ChargePowerPanel;
                    SpacePressed += () => pv.RPC("SpacePressOnlineShootInfo", RpcTarget.Others, panelCharged, shootPower);
                }
                else
                    SpacePressed += RECEIVEDChargePowerPanel;

                SpacePressed += ui.Disable_RoundCounter;
                SpaceUP += ShootMissile;
                break;
            case Type.AK_47:
                SpaceDown += StartShootRifle;
                rounds = 20;
                weaponForce = .8f;
                weaponDamage = 5;
                weaponRange = 6;
                ui.CreateAmmoBar(rounds, WormUI.WeaponType.Rifle);
                SpaceDown += SetAmount;
                break;
            case Type.Double_barrel:
                rounds = 2;
                ui.CreateAmmoBar(rounds, WormUI.WeaponType.Shotgun);
                weaponForce = 1.2f;
                weaponDamage = 30;
                weaponRange = 4.5f;
                SpaceDown += SetAmound_SemiAuto;
                SpaceDown += ShootRifle;
                break;
        }


    }

    

    public void ShootMissile()
    {
        //inp.InputEKeyEvent.RemoveListener(ui.RoundKeys);
        inp.InputSpaceKeyUPEvent.RemoveAllListeners();
        inp.InputSpacePressedKeyEvent.RemoveAllListeners();
        GameObject MISSILE = Instantiate(objectToShoot);
        if (gameObject.transform.parent.rotation.y == 0)
            MISSILE.GetComponent<Missile>().missileRotat = Vector2.up;
        else
            MISSILE.GetComponent<Missile>().missileRotat = Vector2.down;

        gameObject.GetComponent<rotatingWeapon>().TurnOFF();
        gameObject.transform.parent.parent.GetComponent<WormMovement>().RemoveAllMoveEvents();
        cam.drag = false;
        Camera.main.fieldOfView = 1.26f;
        cam.SelectedObject = MISSILE;
        cam.StartFollow();
        Vector2 dir = firepoint.position - gameObject.transform.position;
        MISSILE.transform.position = firepoint.position;
        MISSILE.transform.rotation = firepoint.transform.rotation;
        MISSILE.GetComponent<Missile>().shootpoint = firepoint.position;
        MISSILE.GetComponent<Missile>().ex = ex;
        MISSILE.GetComponent<Rigidbody2D>().AddForce(dir * 5 * Mathf.Ceil(shootPower), ForceMode2D.Impulse);
        Debug.Log("ShootForce " + shootPower);
        Debug.Log("Panels active " + panelCharged);
        shootPower = 0;

        for (int a = 0; a < 8; a++)
            powerCell[a].enabled = false;




    }

    public void ThrowGrenade()
    {
        //inp.InputEKeyEvent.RemoveListener(ui.RoundKeys);
        inp.InputSpaceKeyUPEvent.RemoveAllListeners();
        inp.InputSpacePressedKeyEvent.RemoveAllListeners();
        GameObject grenade = Instantiate(objectToShoot);
        grenade.GetComponent<Rigidbody2D>().simulated = true;
        grenade.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        grenade.GetComponent<CapsuleCollider2D>().enabled = true;
        grenade.transform.localScale = new Vector3(0.2f, 0.2f, 0);
        Camera.main.fieldOfView = 1.26f;
        //cam.drag = false;
        cam.SelectedObject = grenade;
        cam.StartFollow();
        gameObject.transform.parent.parent.GetComponent<WormMovement>().RemoveAllMoveEvents();
        gameObject.GetComponent<rotatingWeapon>().TurnOFF();
        Vector2 dir = firepoint.position - gameObject.transform.position;
        grenade.transform.position = firepoint.position;
        grenade.transform.rotation = firepoint.transform.rotation;
        grenade.transform.position = firepoint.position;
        grenade.GetComponent<Rigidbody2D>().AddForce(dir * 5 * Mathf.Ceil(shootPower), ForceMode2D.Impulse);
        grenade.GetComponent<Rigidbody2D>().AddTorque(1);
        StartCoroutine(ex.WaitAndDo(5, ex.DoFunc = () => ex.DestroyObjectAndExplode(grenade, 1.5f, 8, 30)));



        shootPower = 0;
        for (int a = 0; a < 8; a++)
            powerCell[a].enabled = false;

    }
/*
    void SetShoot()
    {
        ShootAction += () => WeapAnim.SetBool("fire", true);
        ShootAction += Shoot;
        ShootAction += ()=> { inp.InputEKeyEvent.RemoveAllListeners(); };
        Debug.Log("SET AK");
        ShootAction += SetAmount;
    }
*/
    void SetAmount()
	{
        ui.wi.SetTeamInventoryItem(-1,type);
        inp.InputEKeyEvent.RemoveAllListeners();
        SpaceDown -= SetAmount;
	}

    void SetAmound_SemiAuto()
	{
        ui.wi.SetTeamInventoryItem(-1, type);
        inp.InputEKeyEvent.RemoveAllListeners();
        SpaceDown -= SetAmound_SemiAuto;
    }

    public void Shoot()
    {
        ui.SetAmmoBar(-1, ref rounds);
        if (rounds <= 0) StopShootRifle();
        RaycastHit2D hit = Physics2D.Raycast(firepoint.position, transform.right, weaponRange);
        if (hit.collider != null)
            if (hit.collider.gameObject.TryGetComponent<Worm>(out Worm w))
            {
                w.rb.AddForce((w.transform.position - firepoint.position).normalized * weaponForce, ForceMode2D.Impulse);
                w.DecreaseHealth(weaponDamage);
            }
    }

    public void StartShootRifle()
    {
        WeapAnim.SetBool("fire", true);
    }

    public void ShootRifle()
	{
        WeapAnim.SetTrigger("fire");
	}

   

    public void StopShootRifle()
    {
        WeapAnim.SetBool("fire", false);
        ShootAction = null;
        WeapAnim.SetBool("fire", false);
        if (rm.amIHost) WaitSeconds_Do(1f, () => rm._host.SetOnlineRound());

        gameObject.transform.parent.parent.GetComponent<WormMovement>().RemoveAllMoveEvents();
        Camera.main.GetComponent<CameraSC>().StopFollow_Drag();
    }

    public void ChargePowerPanel()
    {

        shootPower = Mathf.Clamp(shootPower += Time.deltaTime * 2, 0, 8);
        for (int a = 0; a < 8; a++)
        {
           panelCharged = Mathf.Floor(shootPower);
            if (panelCharged >= a)
                powerCell[a].enabled = true;
        }

    }

    public void RECEIVEDChargePowerPanel()
	{
        for (int a = 0; a < 8; a++)
        {
            if (panelCharged >= a)
                powerCell[a].enabled = true;
        }
    }

    void WaitSeconds_Do(float until,Action Do)
	{

        StartCoroutine(W_D());
        IEnumerator W_D()
        {
            yield return new WaitForSeconds(until);
            Do?.Invoke();
           
        }

    }
  

        void Wait_Do(float until, Action Do)
    {
        if (time >= until)
        {
            time = 0;
            Do?.Invoke();
        } else
        {
            time += Time.fixedDeltaTime;
            //WeapAnim.SetBool("fire", false);
        }
    }



}

    


