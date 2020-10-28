using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Перечисление всех возможных видов оружия
/// включая щит для его совершенствования
/// </summary>

public enum WeaponType
{
    none,       // Без оружия
    blaster,    // Обычный бластер
    spread,     // Веерная пушка
    phazer,     // Волновой фазер
    missile,    // Самонаводящиеся ракеты
    laser,      // Лазер, наносящий урон при долгом воздействии
    shield      // Увеличение shieldLevel
}

/// <summary>
/// Класс позволяет настраивать свойства конкретного вида оружия в Инспекторе
/// </summary>
[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;   // Буква на бонусе
    public Color color = Color.white;
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float damageOnHit = 0; // Мощность оружия
    public float continuousDamage = 0;
    public float delayBetweenShots = 0;
    public float velocity = 20;
}

public class Weapon : MonoBehaviour
{
   
}
