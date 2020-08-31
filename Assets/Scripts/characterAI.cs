using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterAI : MonoBehaviour
{
    private weaponscript[]  weapons;
    public bool isenemy = true;
    public bool attack = true;
    public bool move = true;
    public float shottime=0f;
    public bool shotting=false;
    private Animator animator;
    private move movescrip;
    
    // Start is called before the first frame update

    public float minAttackCooldown = 3f;
    public float maxAttackCooldown = 4f;
    private float aiCooldown;
    private void Awake()
    {
        aiCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);
        animator = GetComponent<Animator>();
        movescrip = GetComponent<move>();
        weapons = GetComponentsInChildren<weaponscript>();
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(movescrip!=null)
        animator.SetBool("move", movescrip.moving);

        aiCooldown -= Time.deltaTime;
        eventer();
        

    }

    void eventer()
    {
        if (aiCooldown < 0)
        {
            aiCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);
            animator.SetTrigger("shot");

        }
        
    }

    public void stopmove()
    {
        if(movescrip!=null&&move)
        movescrip.automove = false;
    }

    public void startmove()
    {
        if(movescrip != null&&move)
        movescrip.automove = true;
    }
    public void Attack()
    {
        
        if (attack)
            foreach (weaponscript weapon in weapons)
                if (weapon != null)
                {
                    weapon.attack(true);
                }
    }
}
