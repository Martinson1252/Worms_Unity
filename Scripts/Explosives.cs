using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Explosives : MonoBehaviour
{
    public GameObject expl;
    public RoundManager rm;
    public Host hos;
    public CameraSC cam;
    public Action DoFunc;
    // Start is called before the first frame update
    void Start()
    {
        DoFunc = null;
    }

    public IEnumerator WaitAndDo(float time, Action Fun)
    {
        yield return new WaitForSeconds(time);
        Fun?.Invoke();
    }

    public void Explosion(Vector2 point, float radius, float force, float damage)
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(point, radius);
        foreach (Collider2D coli in hit)
        {
            Vector2 dir = new Vector2(coli.transform.position.x, coli.transform.position.y) - point;
            if (coli.GetComponent<Rigidbody2D>() != null && !coli.isTrigger)
            {
                if (coli.TryGetComponent<Worm>(out Worm w))
                {
                    w.DecreaseHealth(damage);
                    //rm.ui.SetBar(-damage, w.team);
                }

                coli.GetComponent<Rigidbody2D>().AddForce(dir.normalized * force, ForceMode2D.Impulse);

            }
        }
    }

    //destroys throwable weapon after specific time
    public void DestroyObjectAndExplode(GameObject obj, float radius, float force, float damage)
    {
        DoFunc = null;
        obj.SetActive(false);
        Explosion(obj.transform.position, radius, force, damage);
        GameObject Exp = Instantiate(expl);
        Exp.transform.position = obj.transform.position;
        cam.StopFollow_Drag();
        cam.CameraBehaviour -= cam.FollowObject;
      
        Destroy(Exp, .6f);
        Destroy(obj, .5f);


        if(rm.amIHost) DoFunc += hos.SetOnlineRound;

        StartCoroutine(WaitAndDo(1, DoFunc));
    }
}
