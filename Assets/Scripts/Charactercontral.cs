using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//这个类用于实现各个组件之间的交互，也是角色的ai，通过状态来调节各个组件之间以及各个动画之间的关系

public abstract class Charactercontral : MonoBehaviour
{
    // Start is called before the first frame update
    // 这是一个抽象的类，用以让其他的类来继承，作用是将角色的个动作分为若干个状态，用来调节各个状态之间的关系，数字越大优先级越高。
    private int statenum;//用数字来代表任意角色所处的任意状态，这种状态将会因对象而异
    protected move move;//有角色控制必定有move组件，怪物ai同理
    protected weaponscript[] weapons;
    protected Animator animator;
    private List<actions> list;
    private float cooldowntime;//当前动作剩余冷却时间
    private float cooldown;//当前动作冷却时间
    private bool actiondo;//一个变量，判断是否有新的动作发生


    private void Awake()
    {
        statenum = 0;//初始设置状态为0
        animator = GetComponent<Animator>();//获取动画组件
        weapons = GetComponentsInChildren<weaponscript>();//从子物体中获取武器
        move = GetComponent<move>();//进行角色方向控制时就调用move中的函数
        list = new List<actions>();//设置角色的动作列表角色将会根据列表做出一系列动作
        cooldowntime = 0;//冷却为0
        cooldown = 0;
        actiondo = false;//设置初始无动作


        Awakes();//调用子类中的Awake
        

        

    }

    protected abstract void Awakes();//以后在子类中写Awake,要写到Awakes里


    private void Start()
    {


        Starts();
    }

    protected abstract void Starts();

    // Update is called once per frame
    void Update()
    {
        //下面的代码解决的是用户连续输入一系列动作的解决方法：将后来的动作排列到之前的动作之后
        if(actiondo&&cooldowntime<0.0001f)
        {
            animator.SetTrigger(list[0].name);//开启一个新的动作
            cooldown = list[0].time;//将当前动画的时间设置为冷却时间
            cooldowntime = cooldown;//animator.GetCurrentAnimatorStateInfo(0).length;
            actiondo = false;


        }
        

        if (cooldowntime > 0.00001f)//当动作冷却时间大于0的时候，将其按时间减少
        {
            cooldowntime -= Time.deltaTime;
        }

        if (cooldowntime <= 0.00001f && list.Count == 1)//当唯一的动作结束
        {
            list.RemoveAt(0);//移除第一个字段
        }
        else if (cooldowntime<=0.00001f&&list.Count==2)//当前面动作的冷却时间结束
        {

            list.RemoveAt(0);//移除第一个字段
            actiondo = true;//开启一个新的动作
        }
            

        Updates();
    }

    protected abstract void Updates();

    void FixedUpdate()
    {


        FixedUpdates();
    }

    protected abstract void FixedUpdates();

    public void Setstatenum(int a)
    {
        statenum = a;//改变状态
    }

    public int GetStatenum() {
        return statenum;//获取状态
    }

    public void doit(string a,bool cando,float cartoontime)//这个函数将会把将要进行的动作排列到动作列表中，列表最多有两个动作（可能会有修改）,这个函数只能响应tigger
    {//响应的字段，是否可以响应，动画的预计时间
        float time = cartoontime / 1 + (cartoontime % 1) / 0.60000f;//将60进制时间，转化为10进制时间
        print(time);
        if (cando)
        {
            if(list.Count == 0)
            {
                list.Add(new actions(time, a));
                print("add0");
                actiondo = true;
            }
            else if (list.Count == 1 &&cooldowntime<(cooldown/2))//如果动作列表中的动作小于等于一个且（当前动画播放时间小于一半，或小于0.5秒），将动作加入动作列表中
            {
                list.Add(new actions(time, a));
                print("add1");
            }
            else if (list.Count == 2 )//列表中如果排有两个动作，取消第二个，加入当前的
            {
                list.RemoveAt(1);
                list.Add(new actions(time, a));
                actiondo = true;
                print("add2");
            }
        }
    }

    public void justdoit(string a, float cartoontime)//这个函数是强制性的动作，将会停掉列表中的所有动作，立即执行这个动作
    {
        actiondo = false;
        list.Clear();
        cooldown = 0;
        cooldowntime = 0;
        list.Add(new actions(cartoontime, a));

    }


    class actions {
        public float time;//动画时间
        public string name;//触发器名字

        public actions(float t,string n)
        {
            time = t;
            name = n;
        }

    
    }

}
