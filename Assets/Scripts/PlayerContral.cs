using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContral : Charactercontral
{
    // Start is called before the first frame update
    // 这个类中不会有具体控制刚体的代码，而是调用武器和移动来控制角色
    //主要是角色的移动控制，其次是武器的控制


    // 技能动作脚本
    private Dash dash;
    //健康脚本
    private Heathscrips health;


    //角色的状态有：0正常 1移动 2攻击 3技能 4僵直
    public float jumphigh;//跳起速度


    //以下是判断按键的方式，根据按键，角色将会做出不同的动作
    private bool Up//是否按了上键
    {
        get
        {
            return Input.GetKey(KeyCode.W);
        }
    }
    private bool Down//是否按了下键
    {
        get
        {
            return Input.GetKey(KeyCode.S);
        }
    }

    //以下是其他函数，用来具体控制
    protected override void Awakes()
    {
        dash = gameObject.GetComponent<Dash>();
        health = gameObject.GetComponent<Heathscrips>();
    }

    protected override void Starts()
    {

    }
    // Update is called once per frame
    protected override void Updates()
    {

        //用来响应按键
        float inputX = Input.GetAxis("Horizontal");//让input的左右定义为a和s，
        bool J = Input.GetKeyDown(KeyCode.J);//攻击
        bool K = Input.GetKeyDown(KeyCode.K);//跳跃
        bool L = Input.GetKeyDown(KeyCode.L);//冲刺
        bool A = Input.GetKey(KeyCode.A);
        bool D = Input.GetKey(KeyCode.D);
        bool Kup= Input.GetKeyUp(KeyCode.K);
        if (A && D)
        {
            Setstatenum(0);
        }
        else if (A)//输入为左，即为负  
        {
            if (GetStatenum() <= 1)
            {
                move.Turn(false);//向左转向 ，即为负  角色向左走
                Setstatenum(1);//如果不是移动状态设置为移动状态
                
            }
            if (GetStatenum() <= 2)
                move.moves(false);//如果不是技能状态，就可以移动    但是此状态下无法转向
        }
        else if (D)//输入为右，即为正  角色向右走
        {
            if (GetStatenum() <= 1)
            {
                move.Turn(true);//向右转向
                Setstatenum(1);//如果不是移动状态设置为移动状态
                
            }
            if (GetStatenum() <= 2)
                move.moves(true);//如果不是技能状态，就可以移动    但是此状态下无法转向
        }
        else //停止状态
        {
            Setstatenum(0);
        }

        if (J)
        {
            if (GetStatenum() <= 2)
            {
                if(health.cando(3))
                doit("hack", canAttack(0),0.1700f);
            }
        }

        if (K)
        {
            if(GetStatenum() <= 2&&move.isground()&& health.cando(4))
            {
                
                move.AddSpeed(0,jumphigh);
                health.consumeqi(4);
            }
        }

        if (Kup && move.rispeed().y > 0.0001f && move.rispeed().y < jumphigh)
        {
            move.setY(0);
        }


        if (L)
        {
            if (move.isground())
            {
                dash.dash(this);
            }
        }


        if (GetStatenum() < 2)
        {
            health.setrecoverqi(true);
        }
        else
        {
            health.setrecoverqi(false);
        }


    }


    protected override void FixedUpdates()
    {
        //物理的设置



        if (move.rispeed().y < -0.0001f && move.isgroundcharater())
        {
            move.AddSpeed(0, jumphigh*2/3);
        }



        //动画的设置
        animator.SetBool("ground", move.isground());//设置着陆为move的着陆算法
        animator.SetFloat("speedy", move.rispeed().y);//让speedy成为刚体的纵向速度
        
        animator.SetBool("up", Up);
        animator.SetBool("down", Down);

        
        if (GetStatenum() == 1)
        {
            animator.SetBool("move", true);//为状态1时为移动状态
        }

        if(GetStatenum() == 0 ||!move.isground())
        {
            animator.SetBool("move", false);//为状态0时为站立状态
        }

        if(GetStatenum()>1)
        {
            animator.SetBool("move", false);
        }

    }

    public void Attack(int num)//角色启用武器,武器要有编号,方便角色调用
    {
        foreach (weaponscript weapon in weapons)
            if (weapon != null)
            {
                if (weapon.weaponnum == num)//调用那个指定的武器
                    weapon.attack(false);
                health.consumeqi(3);

            }
    }



    public bool canAttack(int num)
    {
        animator.SetBool("move", false);
        foreach (weaponscript weapon in weapons)
            if (weapon != null)
            {
                if ( weapon.cooldown())//调用那个指定的武器
                    return true;

            }
        return false;
    }

}
