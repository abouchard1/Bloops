﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum EventEnum
{
    SLOW,
    KILL
}

public class PlayerCharacter : MonoBehaviour
{

    public float speed;//Floating point variable to store the player's movement speed.

    private float currentSpeed;

    private List<string> events;

    private Rigidbody2D character;//Store a reference to the Rigidbody2D component required to use 2D Physics.

    private Dictionary<EventEnum, bool> coroutineState = new Dictionary<EventEnum, bool>();

    private IEnumerator coroutine;

    private Animator anim;

    internal void EventCoroutine(float timer, EventEnum typeEvent, Action fctStart, Action fctEnd)
    {
        if (coroutineState.ContainsKey(typeEvent) && coroutineState[typeEvent] == true)
        {
            print("je skipp");
            return;

        }
        coroutineState[typeEvent] = true;
        fctStart();
        coroutine = WaitAndPrint(timer, typeEvent, fctEnd);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndPrint(float waitTime, EventEnum typeEvent, Action callback)
    {
        yield return new WaitForSeconds(waitTime);
        callback();
        coroutineState[typeEvent] = false;
    }
    
    internal void Start()
    {    
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        character = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentSpeed = speed;
    }

    public void LaunchPlayer(Vector3 dragDistance)
    {
        anim.SetBool("is_launching", true);

        SoundManager.PlaySound("launch_1");
        // Reset the force
        character.velocity = new Vector2(0f, 0f);

        character.AddForce(-dragDistance * speed, ForceMode2D.Impulse);

        GameInstanceInfo.nbBounce++;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        character.velocity = Vector2.zero;
        anim.SetBool("is_launching", false);

        // rdm sound just for test
        int rdmSound = UnityEngine.Random.Range(1, 4);
        Debug.Log(rdmSound);
        SoundManager.PlaySound("impact_"+ rdmSound);

        if (collision.gameObject.tag == "Finish")
        {
            GameInstance.EndLevel();
        }
        if (collision.gameObject.tag == "Bloc_Kill")
        {
            
            print($"Le personnage est mort il à touché le bloc qui tue");
            GameInstanceInfo.nbTry++;
            character.position = GameInstanceInfo.positionDepart;
        }
        if (collision.gameObject.tag == "Bloc_Slow")
        {
            print($"Le personnage est ralentit");
            EventCoroutine(5F, EventEnum.SLOW, () => SetPlayerSpeed(0.2F, EventEnum.SLOW), () => SetPlayerSpeed(1F, EventEnum.SLOW));
        }
        if (collision.gameObject.tag == "Bloc_Moving")
        {
            print($"Le personnage bouge avec l'obstacle");
            
        }
    }

    void SetPlayerSpeed(float speedRate, EventEnum typeEvent)
    {
        float newSpeed = speed * speedRate;
        print($"La vitesse du joueur à été modifier, current: {currentSpeed} => new: {newSpeed}");
        currentSpeed = newSpeed;
        coroutineState[typeEvent] = false;
    }
}
