using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponscript : MonoBehaviour
{
    //这个是武器脚本，通常在一个子物体当中，根据子物体的旋转来达到旋转方向的目的，

    public Transform wepprefab; //武器的子弹
    public float wepingRate = 0.25f; //武器冷却时间
    private float wepCooldown=0f;//武器当前剩余的冷却
    private Transform parent;//武器的父物体
    public int weaponnum;//武器的编号
    public bool back;// 是否有反冲力
    public float backer;
    public float force;//设置击退力量

    public bool canattack
    {
        get
        {
            return wepCooldown <= 0.00f;
        }
    }


    private void Awake()
    {
        parent = gameObject.transform.parent;//让parent记录为父物体的transform,
    }
    // Start is called before the first frame update
    void Start()
    {
        
       
        wepCooldown = 0.00f;
    }

    void Update()
    {
        wepCooldown -= Time.deltaTime;//冷却减少


       
    }




    public void attack(bool isenemy)//进行攻击，bool表示是否为敌人,这个函数,将会让武器射出子弹
    {
        if(canattack)
        {
        
            wepCooldown = wepingRate;
            Transform shotTransform = Instantiate(wepprefab) as Transform;
            move move = shotTransform.GetComponent<move>();
            shotTransform.position = transform.position;//继承武器的位置和旋转
            shotTransform.rotation = transform.rotation;

            if (move!=null)//该子弹有move说明是远程攻击,设置其move的方向
            {
                move.right();
            }

             else//该子弹没有move说明是近战攻击
            {
                shotTransform.parent = transform;   //将子弹的父物体设置为该武器
            }

            shotscrips shot = shotTransform.GetComponent<shotscrips>();//获取子弹的shot组件
            if(shot!= null)
            {
                shot.isenemy = isenemy;
                shot.force = force;
            }

            

        }
    }



    public void forceback(Vector2 vector2)//武器的物理击退,将会反馈给武器的主人
    {
        move mover = parent.GetComponent<move>();//如果父物体有move
        if(mover != null&&back)
        {
            Vector2 back = new Vector2();//将得到的力反过来作为反冲力
            back.x = -vector2.x;
            back.y = -vector2.y;
            mover.AddSpeeds(back*backer);//将向量传递给move
        }
    }

    public bool cooldown()//返回武器是否冷却完了
    {
        return canattack;
    }

}
