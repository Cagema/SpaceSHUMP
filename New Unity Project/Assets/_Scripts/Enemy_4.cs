using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Сериализуемый класс, хранящий части корабля
/// </summary>
[System.Serializable]
public class Part
{
    public string name;
    public float health;
    public string[] protectedBy; // Части, защищающие эту

    [HideInInspector]
    public GameObject go;
    [HideInInspector]
    public Material mat; // Материал для отображения повреждений

}

/// <summary>
///  Создается за верхней границей, выбирает случайную точку и движется к ней
///  И так пока не будет разрушен
/// </summary>
public class Enemy_4 : Enemy
{
    [Header("Set in Inspector: Enemy_4")]
    public Part[] parts;

    private Vector3 p0, p1; // Две точки для интерполяции
    private float timeStart; // Время создания корабля
    private float duration = 4f; // Длительность движения

    private void Start()
    {
        // Начальная точка уже создана в Main.SpawnEnemy()
        // Запишем её
        p0 = p1 = pos;
        InitMovement();

        Transform t;
        foreach (Part item in parts)
        {
            t = transform.Find(item.name);
            if (t != null)
            {
                item.go = t.gameObject;
                item.mat = item.go.GetComponent<Renderer>().material;
            }
        }
    }

    private void InitMovement()
    {
        p0 = p1;
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;
        p1.x = Random.Range(-widMinRad, widMinRad);
        p1.y = Random.Range(-hgtMinRad, hgtMinRad);

        timeStart = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - timeStart) / duration;

        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);
        pos = (1 - u) * p0 + u * p1;
    }

    Part FindPart(string n)
    {
        foreach(Part item in parts)
        {
            if (item.name == n)
            {
                return item;
            }
        }
        return null;
    }

    Part FindPart(GameObject go)
    {
        foreach (Part item in parts)
        {
            if (item.go == go)
            {
                return item;
            }
        }
        return null;
    }

    bool Destroyed(GameObject go)
    {
        return Destroyed(FindPart(go));
    }

    bool Destroyed(string n)
    {
        return Destroyed(FindPart(n));
    }

    bool Destroyed(Part item)
    {
        if (item == null)
        {
            return true;
        }
        return (item.health <= 0);
    }

    void ShowLocalizedDamage(Material m)
    {
        m.color = Color.red;
        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        switch (other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                if (!bndCheck.isOnScreen) // Если корабль за экраном, просто разрушить снаряд
                {
                    Destroy(other);
                    break;
                }

                GameObject goHit = collision.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);
                if (prtHit == null) // Если не нашли часть, так случается редко, пробуем другой способ
                {
                    goHit = collision.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }

                if (prtHit.protectedBy != null) // Если есть защищающая часть, просто разрушаем снаряд
                {
                    foreach(string s in prtHit.protectedBy)
                    {
                        if (!Destroyed(s))
                        {
                            Destroy(other);
                            return;
                        }
                    }
                }

                prtHit.health -= Main.GetWeaponDefinition(p.type).damageOnHit; // Нанести повреждения
                ShowLocalizedDamage(prtHit.mat); // Показать повреждения
                if (prtHit.health <= 0) // Разрушить часть
                {
                    prtHit.go.SetActive(false);
                }

                bool allDestroyed = true; // Предположим, что частей не осталось
                foreach(Part item in parts) // Если части есть, опровергнем строку выше
                {
                    if (!Destroyed(item))
                    {
                        allDestroyed = false;
                        break;
                    }
                }
                if (allDestroyed) // Если не осталось частей, разрушить корабль
                {
                    Main.S.ShipDestroyed(this);
                    Destroy(this.gameObject);
                }

                Destroy(other); // Удалить снаряд
                break;
        }
    }
}
