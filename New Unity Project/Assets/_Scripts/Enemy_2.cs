using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    [Header("Set in Inspector: Enemy_2")]
    //Выраженность синусоидальности движения
    public float sinEccentricity = 0.6f;
    public float lifeTime = 10;

    [Header("Set Dynamically: Enemy_2")]
    public Vector3 p0;
    public Vector3 p1;
    public float birthTime;

    private void Start()
    {
        //случайная точка на левой границе экрана
        p0 = Vector3.zero;
        p0.x = -bndCheck.camWidth - bndCheck.radius;
        p0.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        //случайная точка на правой границе экрана
        p1 = Vector3.zero;
        p1.x = bndCheck.camWidth + bndCheck.radius;
        p1.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        //Случайно поменять начальную и конечную точку местами
        if (Random.value > 0.5f)
        {
            p0 *= -1;
            p1 *= -1;
        }

        //Записать в birthTime текущее время
        birthTime = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;
        //Если u>1 значит корабль завершил жизненный цикл
        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        //корректировка добавлением кривой, изменяющейся по синусоиде
        u = u + sinEccentricity * Mathf.Sin(u * Mathf.PI * 2);

        pos = (1 - u) * p0 + u * p1;
    }
}
