using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotscrips : MonoBehaviour
{
    public int damage = 1;//伤害
    public bool isenemy = true;//是否为敌对子弹
    public float time = 5;//子弹消失时间
    public ParticleSystem damageelect;//子弹的粒子效果
    public bool ishack=false;//子弹是否为近战攻击
    public bool continuer=false;//子弹的是否是持续的(或者说一次性的子弹)
    public float force = 10;//击退的力量
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, time);//一定时间后,子弹自动销毁
       

    }
    private void Awake()
    {
        
    }


    public void destroy(GameObject objecter)
    {
     
        
        if (damageelect != null)//如果有粒子效果
            particlescrips.particl.instantiate(damageelect, transform.position);//进行一次粒子效果
        if(!continuer)//如果不是持续攻击
        Destroy(gameObject);//将子弹销毁
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Vector3 vector = gameObject.transform.eulerAngles;
        
        Heathscrips health =  collision.gameObject.GetComponent<Heathscrips>();//获取碰撞对象的健康

        if (collision.gameObject.tag =="ground"&&!ishack)
        {
            if (damageelect != null)//如果有粒子效果
                particlescrips.particl.instantiate(damageelect, transform.position);//进行一次粒子效果
            Destroy(gameObject);
        }

        if (health != null && health.isenemay != isenemy)
        {
            Vector2 forcer = gameObject.transform.right*force;
            collision.gameObject.GetComponent<move>().AddSpeeds(forcer);//给予对象物理击退
            backer(forcer);//给予自身物理击退
        }
    }

    private void backer(Vector2 forcer)//子弹的反冲力,将会返回给武器
    {
       if( gameObject.transform.parent!=null)
        {
            weaponscript wep= gameObject.GetComponentInParent<weaponscript>();
            wep.forceback(forcer);
        }
    }


    private Vector2 RotationMatrix(Vector2 v, float angle)//将向量旋转任意角度(逆时针)
    {
        var x = v.x;
        var y = v.y;
        var sin = System.Math.Sin(System.Math.PI * angle / 180);
        var cos = System.Math.Cos(System.Math.PI * angle / 180);
        var newX = x * cos + y * sin;
        var newY = x * sin + y * cos;
        return new Vector2((float)newX, (float)newY);
    }

}

