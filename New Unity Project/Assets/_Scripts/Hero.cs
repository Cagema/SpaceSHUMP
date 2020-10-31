using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S; //Одиночка

    [Header("Set in Inspector")]
    //Поля, управляющие движением корабля
    public float speed = 30f;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;

    private GameObject lastTriggerGo = null;

    // Объявление делегата
    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;

    void Awake()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
           // Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Извлечь информацию из класса Input
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        //Изменить transform.position, опираясь на информацию по осям
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        //Повернуть корабль для динамизма
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TempFire();
        //}

        // Произвести выстрел из всех видов оружия
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
      //  print("Triggered: " + go.name);

        if (go == lastTriggerGo)
        {
            return;
        }
        lastTriggerGo = go;

        if (go.tag == "Enemy")
        {
            shieldLevel--;
            Destroy(go);
        }
        else
            print("Triggered by non-enemy: " + go.name);
    }

    public float shieldLevel
    {
        get
        {
            return _shieldLevel;
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            if (value < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }
}
