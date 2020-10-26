using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float rotationPerSecond = 0.1f;

    [Header("Set Dynamically")]
    public int levelShown = 0;

    //скрытые переменные, не появляющиеся в Инспекторе
    Material mat;


    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //Прочитать текущую можность поля из Hero
        int currLevel = Convert.ToInt32(Hero.S.shieldLevel);
        //Если она отличается от levelShown...
        if (levelShown != currLevel)
        {
            levelShown = currLevel;
            //Скорректировать смещение в текстуре, чтобы отобразить поле с другой мощностью
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }

        //Поворачивать поле в каждом кадре с постоянной скоростью
        float rZ = -(rotationPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }
}
