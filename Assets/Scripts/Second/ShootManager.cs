﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : MonoBehaviour
{
    public static ShootManager _instance { get; private set; }
    public Transform wuzei;
    public BatteryAIO battery;
    public bool canChange2Battery = true;

    public Transform batteryRevive;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        InitCamera();
    }

    public void Change2BatteryView()
    {
        AudioManager._instance.PlayEffect("toBattery");//音效
        Level2UIManager._instance.simpleCross.gameObject.SetActive(false);
        Level2UIManager._instance.batteryCross.gameObject.SetActive(true);
        Level2UIManager._instance.tipText.SetActive(false);
        wuzei.position = batteryRevive.position;
        wuzei.rotation = batteryRevive.rotation;
        wuzei.gameObject.SetActive(false);
        battery.canShoot = true; battery.enableCameraMovement = true;
        canChange2Battery = false;
    }
    public void Change2PlayerView()
    {
        AudioManager._instance.PlayEffect("toBattery");//音效
        Level2UIManager._instance.simpleCross.gameObject.SetActive(true);
        Level2UIManager._instance.batteryCross.gameObject.SetActive(false);
        Level2UIManager._instance.tipText.SetActive(true);
        wuzei.position= batteryRevive.position;
        wuzei.rotation = batteryRevive.rotation;
        wuzei.gameObject.SetActive(true);
        battery.ResetBatteryPos();
        battery.canShoot = false;
        battery.enableCameraMovement = false;
    }
   public  void InitCamera()
    {
        Level2UIManager._instance.simpleCross.gameObject.SetActive(true);
        Level2UIManager._instance.batteryCross.gameObject.SetActive(false);
        wuzei.gameObject.SetActive(true);
        battery.ResetBatteryPos();
        battery.canShoot = false;
        battery.enableCameraMovement = false;
    }
    void Update()
    {
        
    }
}
