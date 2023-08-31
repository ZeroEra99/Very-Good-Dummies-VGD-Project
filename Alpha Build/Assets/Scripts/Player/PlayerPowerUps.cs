using System;
using System.Collections;
using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    //Cooldown parameters
    private readonly float _godModeDuration = 10;
    private readonly float _speedHackDuration = 15;
    private readonly float _fireFistsDuration = 10;

    public static float _speedHackOffset = 2;
    public static int _fireFistOffset = 2;
    public static int _fireBallMana = PlayerAttack.FireBallManaUse;

    public static bool GodModeEnabled = false;
    public static bool InfiniteMana = false;



    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public void Update()
    {
        if (GameManager.cheat)
            StopAllCoroutines();
    }

    private void GodMode()
    {
        GodModeEnabled = true;
        StartCoroutine(WaitForGodMode(_godModeDuration));
    }

    private void SpeedHack()
    {
        IncreaseSPeed();
        StartCoroutine(WaitForSpeedHAck(_speedHackDuration));
    }

    private void FireFist()
    {
        IncreaseAttack();
        StartCoroutine(WaitForFireFist(_fireFistsDuration));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("godModeCollectible") && !GameManager.cheat) GodMode();
        if (other.CompareTag("speedHackCollectible") && !GameManager.cheat) SpeedHack();
        if (other.CompareTag("fireFistCollectible") && !GameManager.cheat) FireFist();
    }


    private IEnumerator WaitForFireFist(float time)
    {
        yield return new WaitForSeconds(time);
        DecreaseAttack();
    }

    private IEnumerator WaitForSpeedHAck(float time)
    {
        yield return new WaitForSeconds(time);
        DecreaseSPeed();
    }

    private IEnumerator WaitForGodMode(float time)
    {
        yield return new WaitForSeconds(time);
        GodModeEnabled = false;
    }


    public static void IncreaseSPeed()
    {
        PlayerMovement.normalMode = false;
        PlayerMovement.walkingSpeed *= _speedHackOffset;
        PlayerMovement.sprintSpeed *= _speedHackOffset;
        PlayerManager.CurrentStamina = 100;
    }

    public static void DecreaseSPeed()
    {
        PlayerMovement.normalMode = true;
        PlayerMovement.walkingSpeed /= _speedHackOffset;
        PlayerMovement.sprintSpeed /= _speedHackOffset;
    }

    public static void IncreaseAttack()
    {
        PlayerAttack.MinFistDamage *= _fireFistOffset;
        PlayerAttack.MaxFistDamage *= _fireFistOffset;
        PlayerAttack.FistCooldown /= 2;
        PlayerAttack.FireballCooldown /= 2;
        PlayerAttack.HasFireFist = true;
        PlayerAttack.FireBallManaUse -= _fireBallMana;
        PlayerBullet._minDamage *= 2;
        PlayerBullet._maxDamage *= 2;
        PlayerAttack.BulletSpeed *= 2;
    }

    public static void DecreaseAttack()
    {
        PlayerAttack.HasFireFist = false;
        PlayerAttack.FistCooldown *= 2;
        PlayerAttack.MinFistDamage /= _fireFistOffset;
        PlayerAttack.MaxFistDamage /= _fireFistOffset;
        PlayerAttack.FireballCooldown *= 2;
        PlayerAttack.FireBallManaUse += _fireBallMana;
        PlayerBullet._minDamage /= 2;
        PlayerBullet._maxDamage /= 2;
        PlayerAttack.BulletSpeed /= 2;
    }


}
