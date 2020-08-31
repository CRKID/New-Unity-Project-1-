using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public Vector2 speed = new Vector2(5, 5);//xy方向的速度和加速度
    private Vector2 direction = new Vector2(1, 1);
    private Vector2 movement;
    private Rigidbody2D ri;

    public bool automove = false;//是否自动移动(点了这个就没有moving什么事了)
    public bool moving = true;    //是否可以由ai控制移动
    public bool xable = true;//x方向移动是否自主
    public bool yable = true;//y方向移动是否自主
    


    // Start is called before the first frame update
    void Start()
    {
        ri = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        movement = ri.velocity;

        //movement.x = speed.x * direction.x;
        //movement.y = speed.y * direction.y;

        float x = speed.x * direction.x;
        float y = speed.y * direction.y;


        if (xable)//x方向可以自主移动
        {

            if (x >= 0.01f)//应对物理击退，移动x正向
            {
                if (ri.velocity.x < 0f)//当方向相反时，移动速度的10倍作为加速度
                {
                    movement.x = movement.x + x * Time.deltaTime * 10;
                }
                else if (ri.velocity.x >= 0f && ri.velocity.x <= x + 0.01f)//当速度大于0，速度小于移动速度，将速度至为移动速度
                {
                       
                    movement.x = x;
                    if (!moving)
                        movement.x = 0;
                }
                else if (ri.velocity.x > x + 0.01f)//当速度大于移动速度，速度减小，加速度为移动速度
                {
                    movement.x = movement.x - x * Time.deltaTime * 10;
                }
            }

            else if (x <= -0.01f)//应对物理击退，移动x负向
            {
                if (ri.velocity.x > 0f)//当方向相反时，移动速度作为加速度
                {
                    movement.x = movement.x + x * Time.deltaTime * 10;
                }
                else if (ri.velocity.x <= 0f && ri.velocity.x >= x - 0.01f)//当速度大于0，速度小于移动速度，将速度至为移动速度
                {

                    movement.x = x;
                    if (!moving)
                        movement.x = 0;
                }
                else if (ri.velocity.x < x - 0.01f)//当速度大于移动速度(负向为小于)，速度减小，加速度为移动速度
                {
                    movement.x = movement.x - x * Time.deltaTime * 10;
                }
            }

            else
                movement.x = x;
        }

        if (yable)
        {

            if (y >= 0.01f)//应对物理击退，移动y正向
            {
                if (ri.velocity.y < 0f)//当方向相反时，移动速度作为加速度
                {
                    movement.y = movement.y + y * Time.deltaTime * 10;
                }
                else if (ri.velocity.y >= 0f && ri.velocity.y <= y + 0.01f)//当速度大于0，速度小于移动速度，将速度至为移动速度
                {

                    movement.y = y;
                }
                else if (ri.velocity.y > y + 0.01f)//当速度大于移动速度，速度减小，加速度为移动速度
                {
                    movement.y = movement.y - y * Time.deltaTime * 10;
                }
            }

            else if (y <= -0.01f)//应对物理击退，移动y负向
            {
                if (ri.velocity.y > 0f)//当方向相反时，移动速度作为加速度
                {
                    movement.y = movement.y + y * Time.deltaTime * 10;
                }
                else if (ri.velocity.y <= 0f && ri.velocity.y >= y - 0.01f)//当速度大于0，速度小于移动速度，将速度至为移动速度
                {

                    movement.y = y;
                }
                else if (ri.velocity.y < y - 0.01f)//当速度大于移动速度(负向为小于)，速度减小，加速度为移动速度
                {
                    movement.y = movement.y - y * Time.deltaTime * 10;
                }
            }
            else
                movement.y = y;
        }

        moving = false;//重设移动状态
        ri.velocity = movement+addedspeed;//刚体移动轨迹为原来地移动+突然增加的速度
        direction.x = 1;//重新设置方向为正
        addedspeed = Vector2.zero;//重新设置突然增加的速度为0

        if (automove)
            moving = true;//自动移动状态下,moving永远是打开的
    }

    public void moves(bool right)
    {
        if (right)
        {
            moving = true;//自然地向右走
        }
        else
        {
            moving = true;
            if(!(direction.x<0))
            direction.x *= -1;//向反方向走
        }
    }

    public void mover(Vector2 vector2)//按向量移动
    {
        moving = true;
        speed = vector2;
    }

    public void mover( float x, float y)//按照数值移动
    {
        moving = true;
        speed.x = x;
        speed.y = y;
    }
    private int groundCount = 0;
    private int characterCount = 0;
    void OnTriggerEnter2D(Collider2D collider)//判断是否落地
    {
        if (collider.gameObject.tag == "ground")
        {
            groundCount++;
        }
        if (collider.gameObject.tag == "character")
        {
            characterCount++;
        }

    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "ground")
        {
            groundCount--;
        }

        if (collider.gameObject.tag == "character")
        {
            characterCount--;
        }
    }

    public bool isground()//判断是否落地
    {
        return groundCount > 0;
    }
    public bool isgroundcharater()//判断是否落在角色头上
    {
        return characterCount > 0;
    }
    private Vector2 addedspeed=Vector2.zero;//添加速瞬时速度

    public void AddSpeed(Vector2 vector2)//按向量加速度
    {
        addedspeed = vector2;
    }

    public void AddSpeeds(Vector2 vector2)//增加固定速度
    {
        Vector2 ris = ri.velocity;//记录当前刚体的速度

        if(vector2.x <0f && ris.x < 0f)//增加的速度不会超过刚体的速度
        {
            if (ris.x > vector2.x)
                ris.x = vector2.x;
        }

        else if (vector2.x > 0f && ris.x > 0f)
        {
            if(ris.x < vector2.x)
                ris.x = vector2.x;
        }

        else 
        {
            ris.x = vector2.x;
        }

        if (vector2.y < 0f && ris.y < 0f)//增加的速度不会超过刚体的速度
        {
            if (ris.y > vector2.y)
                ris.y = vector2.y;
        }

        else if (vector2.y > 0f && ris.y > 0f)
        {
            if (ris.y < vector2.y)
                ris.y = vector2.y;
        }

        else
        {
            ris.y = vector2.y;
        }
        ri.velocity = ris;
    }
    public void AddSpeed(float x, float y)//按xy加速度
    {
        addedspeed.x = x;
        addedspeed.y = y;
    }

    public void AddVector(float angle,float speed)//按角度加速度
    {
        addedspeed.y = Mathf.Sin((angle / 360) * Mathf.PI * 2);
        addedspeed.x = Mathf.Cos((angle / 360) * Mathf.PI * 2);
    }

    public void Addforcer(Vector2 force)
    {
        ri.AddForce(force);
    }

    public void Turn(bool right)//转向
    {
        if (right)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    public bool Turn()//判断是否朝向右
    {
        if (transform.localRotation == Quaternion.Euler(0, 0, 0))
            return true;
        else
            return false;
    }

    public Vector3 rispeed()//获取刚体的速度
    {
        return ri.velocity;
    }

    public void setX(float x)//设置x方向的速度
    {
        Vector3 a = ri.velocity;
        a.x = x;
        ri.velocity = a;
    }

    public void setY(float y)//设置y方向的速度
    {
        Vector3 a = ri.velocity;
        a.y = y;
        ri.velocity = a;
    }

    public void right()//向向量的正右方移动
    {
        speed = transform.right*speed.x;
    }

}
