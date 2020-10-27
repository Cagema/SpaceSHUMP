﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy_1 реализует перемещение сверху вниз по синусоиде
/// </summary>

public class Enemy_1 : Enemy
{
    [Header("Set in Inspector: Enemy_1")]
    //Количество секунд полной синусоиды
    public float waveFrequency = 2;
    //Ширина синусоиды в метрах
    public float waveWidth = 4;
    public float waveRotY = 45;

    private float x0;
    private float birthTime;

    private void Start()
    {
        x0 = pos.x;

        birthTime = Time.time;
    }

    public override void Move()
    {
        Vector3 tempPos = pos;

        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + sin * waveWidth;
        pos = tempPos;

        //Поворот
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);

        base.Move();

        //print(bndCheck.isOnScreen);
    }
}
