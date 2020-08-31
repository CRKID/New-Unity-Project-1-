using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIofPlayer : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform player;
    private Heathscrips hps;
    private float maxhealth;
    private float nowhealth;
    Vector3 size;
    private void Awake()
    {
        if (player != null)
        {
            hps = player.gameObject.GetComponent<Heathscrips>();
        }
        size= gameObject.transform.localScale;
        maxhealth = hps.hpmax;


    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nowhealth = hps.hp;
        if (nowhealth > maxhealth)
        {
            nowhealth = maxhealth;
        }

        if(nowhealth < 0)
        {
            nowhealth = 0;
        }

        Vector3 nowsize = size; 
        nowsize.x=nowsize.x* (nowhealth / maxhealth);
        gameObject.transform.localScale = nowsize;

    }
}
