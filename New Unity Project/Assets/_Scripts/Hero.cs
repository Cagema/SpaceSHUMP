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

    [Header("Set Dynamically")]
    public float shieldLevel = 1;

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

    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
