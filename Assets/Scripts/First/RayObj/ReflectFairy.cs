﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReflectFairy : Fairy
{
    protected Hair_PlayerMove playerMove;
    protected ReflectRayEmitter reflectRay;
    protected const int dirNum = 8;
    Vector3 disToTarget;
    public  Button rotateButton;
    public Button overTurnButton;
    
    public  void  Awake () {
      
        base.Init();
        InitButtons();
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Hair_PlayerMove>();     
        InitReflectRay();
      
    }
    public virtual  void InitButtons()
    {
        fairySorts = FairySorts.Reflect;
        if (rotateButton != null)
        {
            rotateButton.onClick.AddListener(RotateFairy);
        }
        //rotateButton = GameObject.FindWithTag("rotateBtn").GetComponent<Button>();
        if (overTurnButton != null)
        {
            overTurnButton.onClick.AddListener(OverTurnFairy);
        }
        //overTurnButton = GameObject.FindWithTag("turnBtn").GetComponent<Button>();

    }
    
    public  virtual  void InitReflectRay()
    {
        reflectRay = GetComponentInChildren<ReflectRayEmitter>();
        reflectRay.startPoint = transform.position;
        reflectRay.dirNum = dirNum;
        reflectRay.dirs = new Vector3[dirNum] //16个方向，记住序号，下次继续往下轮
        {
            new Vector3(1,1,0), new Vector3(1,1,-1),
            new Vector3(0,1,-1),new Vector3(-1,1,-1),
            new Vector3(-1,1,0),new Vector3(-1,1,1),
            new Vector3(0,1,1), new Vector3(1,1,1)
        };
    }

    public  override void FollowTarget()
    {
        if (!canFollow) return;
        if (!isPicked)
        {
            transform.position = Vector3.Lerp(transform.position,playerMove.gameObject.transform.position+transform.right*(-0.5f)+Vector3.up * 0.5f, Time.deltaTime);
            transform.forward = Vector3.Lerp(transform.forward,playerMove.transform.forward,Time.deltaTime);
        }
    }

    public   void RotateFairy()
    {
        if (truePicked)
        {
            int index = Random.Range(1,6);
            AudioManager._instance.PlayEffect("r"+index.ToString());
            //在8个顶点上进行旋转，改变dir
            reflectRay.index++;
            reflectRay.index %= 8;
        }        
    }
    public  void OverTurnFairy()
    {
        if (truePicked)
        {
            AudioManager._instance.PlayEffect("X");
            for (int i = 0; i < 8; i++)
            {
                reflectRay.dirs[i] = new Vector3(reflectRay.dirs[i].x, -1 * reflectRay.dirs[i].y, reflectRay.dirs[i].z);
            }
        }
    }

    public  override void RenderRay()
    {
        base.RenderRay();
        if (rayHited)
        {
            reflectRay.lineRenderer.enabled = true;
        }
        else
        {
            reflectRay.lineRenderer.enabled = false;
        }
    }
    
    public  override void MoveFairy()
    {
        //显示可移动的点阵
        ShowMovePoint();

        if (!truePicked) return;  
        Vector3 playerPos = playerMove.transform.position;

        if (canMove)
        {
            transform.position = Vector3.Lerp(transform.position,targetPos,Time.deltaTime*moveSpeed);
            if (Vector3.Distance(transform.position, targetPos) < 0.02f)
            {
                canMove = false;
            }
        }
        else
        {
            //移动
            if (Input.GetKeyUp(KeyCode.E))       InputResponse(Vector3.up);
            if (Input.GetKeyUp(KeyCode.Q))        InputResponse(Vector3.up * (-1));
            if (Input.GetKeyUp(KeyCode.A))        InputResponse(Vector3.right * (-1));
            if (Input.GetKeyUp(KeyCode.D))       InputResponse(Vector3.right);
            if (Input.GetKeyUp(KeyCode.S))       InputResponse(Vector3.forward * (-1));
            if (Input.GetKeyUp(KeyCode.W))        InputResponse(Vector3.forward);
            //控制角色在坐标点上
            float y = Mathf.Clamp(Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(playerPos.y - 1), Mathf.RoundToInt(playerPos.y + 2));
            float x = Mathf.Clamp(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(playerPos.x - 2), Mathf.RoundToInt(playerPos.x + 2));
            float z = Mathf.Clamp(Mathf.RoundToInt(transform.position.z), Mathf.RoundToInt(playerPos.z - 2), Mathf.RoundToInt(playerPos.z + 2));
            transform.position = new Vector3(x, y, z);
        }
    }
  
    public  override void ShowMovePoint()
    {
        if (truePicked)
        {
            int y = Mathf.RoundToInt(transform.position.y); int x = Mathf.RoundToInt(transform.position.x);
            int z = Mathf.RoundToInt(transform.position.z); Vector3 ori = new Vector3(x, y, z);
            Vector3 playerPos = playerMove.transform.position;
            //y
            SetPointsPos(2, 1, playerPos.y, yArray, yShow, Vector3.up, ori.y, ori);
            //x
            SetPointsPos(2, 2, playerPos.x, xArray, xShow, Vector3.right, ori.x, ori);
            //z
            SetPointsPos(2, 2, playerPos.z, zArray, zShow, Vector3.forward, ori.z, ori);
        }
        else
        {
            foreach (var i in pointsObjects)
            {
                Destroy(i.gameObject);
                
            }
            pointsObjects.Clear();
            InitPointsArray();
        }
    }
}
