using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heathscrips : MonoBehaviour
{
    public int hp = 10;
    public int hpmax = 10;
    public bool isenemay = true;
    public ParticleSystem damageelect;
    public float weight = 1;

    public int qi = 10;
    public int qimax = 10;
    public int recoverqi = 3;
    public float recoverqitime=1;
    private float cooldownqi=0;
    private bool canrecoverqi;

    private void Update()
    {
        if (canrecoverqi&&qi<qimax)
        {
            cooldownqi += Time.deltaTime;
            if (cooldownqi >= recoverqitime)
            {
                cooldownqi = 0;
                qi += recoverqi;
                if (qi > qimax)
                {
                    qi = qimax;
                }
            }
        }

        else
        {
            cooldownqi = 0;
        }
    }
    public void Damage(int damageint)
    {
        hp -= damageint;
        if (damageelect != null)
        particlescrips.particl.instantiate(damageelect, transform.position);
    
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        shotscrips shot = collision.gameObject.GetComponent<shotscrips>();
        if(shot!= null)
        {
            if (shot.isenemy!=isenemay)
            {
                Damage(shot.damage);

                shot.destroy(this.gameObject);
            }
        }
    }

    public void consumeqi(int num)
    {
        qi -= num;
        if (qi < 0)
        {
            qi = 0;
        }
    }

    public void setrecoverqi(bool a)
    {
        canrecoverqi = a;
    }

    public bool cando()
    {
        return qi> 0;
    }

    public bool cando(int a)
    {
        return qi >= a;
    }
}
