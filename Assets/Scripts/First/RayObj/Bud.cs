﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class Bud : MonoBehaviour {
    public int nowRayNum;
    [HideInInspector]    public int needRayNum;//需要点亮几片叶子，才会长出藤蔓
    [HideInInspector]   public  bool hasGrowrn = false;
    TagLeave[] leaves;
    Material blackBud;
    Material glowBud;
    Transform creeper;
    Animator creeperAnim;
    Renderer creeperRenderer;
    float growIndex = 0;
    bool startShow = false;
    public float showSpeed = 1.0f;
    public Color creeperColor;
    [HideInInspector]public  bool hasLighted = false;

    public GameObject door;

    private void Awake()
    {
        leaves = GetComponentsInChildren<TagLeave>();        
        needRayNum = leaves.Length;
        nowRayNum = 0;
        blackBud = Resources.Load<Material>(MainContainer.materialFolder+ "myLampNor");
        glowBud = Resources.Load<Material>(MainContainer.materialFolder + "myLampEmi");
        for (int i = 0; i < needRayNum; i++)
        {
            leaves[i].gameObject.GetComponent<MeshRenderer>().material = blackBud;
        }
        creeper = transform.Find("creeper");
        creeper.gameObject.AddComponent<HighlightableObject>();
        creeperRenderer = creeper.GetComponent<MeshRenderer>();
        creeperRenderer.material.SetFloat("_dissolveAmount", 0);

        if (door != null)
        {
            door.gameObject.SetActive(true);
        }

    }	

	void Update () {
        LightAndDarkLeaves();        
        GrowCreeper();
        if (startShow)
        {
            growIndex = Mathf.Lerp(growIndex, 1, Time.deltaTime * showSpeed);
            creeperRenderer.material.SetFloat("_dissolveAmount", growIndex);
        }
        if (creeperRenderer.material.GetFloat("_dissolveAmount") > 0.99f)
        {
            startShow = false;
        }
    }
    void LightAndDarkLeaves()
    {
        if (hasLighted) return;
        nowRayNum = Mathf.Clamp(nowRayNum, 0, needRayNum);
        for (int i = 0; i < nowRayNum; i++)
        {
            leaves[i].gameObject.GetComponent<MeshRenderer>().material = glowBud;
        }
        for (int i = nowRayNum; i < needRayNum; i++)
        {
            leaves[i].gameObject.GetComponent<MeshRenderer>().material = blackBud;
        }
        if (nowRayNum == needRayNum) hasLighted = true;
    }
    void GrowCreeper()
    {
        if (nowRayNum >= needRayNum && ! hasGrowrn)
        {
            AudioManager._instance.PlayEffect("creeper");   
            startShow = true;
            //StartCoroutine(HighlightCreeper());
            hasGrowrn = true;
            if (door != null)
            {
                door.gameObject.SetActive(false);
            }
        }       
    }

    IEnumerator HighlightCreeper()
    {
        HighlightableObject obj = creeper.gameObject.GetComponent<HighlightableObject>();        
        obj.ConstantOn(creeperColor);
        yield return new WaitForSeconds(5);
        obj.ConstantOff();
    }
}
