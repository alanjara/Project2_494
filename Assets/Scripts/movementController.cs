﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class movementController : MonoBehaviour {
    public static movementController moveControl;
    KeyCode LEFT = KeyCode.LeftArrow;
    KeyCode RIGHT = KeyCode.RightArrow;
    KeyCode UP = KeyCode.UpArrow;
    KeyCode D_KEY = KeyCode.D;
    KeyCode ONE = KeyCode.Alpha1;
    KeyCode TWO = KeyCode.Alpha2;
    KeyCode THREE = KeyCode.Alpha3;
    public List<GameObject> time_travellers;
    public List<GameObject> players;
    GameObject Player;
    public float move_speed = 50;
    public float max_run_speed = 50;
    Vector3 cam_target_position;
    Rigidbody body;
    bool left_first = false;
    bool grounded = false;
    bool replay = false;
    int current_player = 0;
    // Use this for initialization
    void Start() {
        moveControl = this;
        GameObject[] all_players = GameObject.FindGameObjectsWithTag("Player");
        List<GameObject> temp_players = new List<GameObject>();
        foreach (GameObject new_player in all_players) {
            temp_players.Add(new_player);
        }
        while (temp_players.Count > 0) {
            int smallest = 100;
            GameObject chosen = null;
            foreach (GameObject new_player in temp_players) {
                if (new_player.GetComponent<timeTravel>().number < smallest) {
                    smallest = new_player.GetComponent<timeTravel>().number;
                    chosen = new_player;
                }
            }
            players.Add(chosen);
            temp_players.Remove(chosen);
        }
        Player = players[0];
        body = Player.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    public void addTraveller(GameObject traveller) {
        time_travellers.Add(traveller);
    }
    void FixedUpdate() {
        if (!replay) {
            Player.GetComponent<timeTravel>().derail();
        }
        cam_target_position = Player.transform.position;
        if (Player.GetComponent<timeTravel>().grounded)
            cam_target_position += Vector3.up * 2f;
        Vector3 to_player = transform.position - cam_target_position;
        to_player.z = 0;
        if (to_player.magnitude > 2) {
            Vector3 new_pos = transform.position;
            Vector3 displacement = cam_target_position - transform.position;
            float multiplier = 1;
            displacement.z = 0;
            if (displacement.magnitude > 4)
                multiplier = displacement.magnitude / 4;
            displacement.Normalize();
            displacement = displacement * 5f * multiplier * multiplier * Time.deltaTime;
            while (to_player.magnitude < displacement.magnitude)
                displacement = displacement * 0.9f;
            new_pos += displacement;
            transform.position = new_pos;

        }
        Vector3 temp_vel = body.velocity;
        temp_vel.x = 0;
        body.velocity = temp_vel;
        if (Input.GetKey(LEFT) && Input.GetKey(RIGHT)) {

        } else if (Input.GetKey(LEFT) && body.velocity.x > -max_run_speed) {
            //  body.AddForce(left);
            temp_vel.x = -move_speed;
            body.velocity = temp_vel;
        } else if (Input.GetKey(RIGHT) && body.velocity.x < max_run_speed) {
            // body.AddForce(left);
            temp_vel.x = move_speed;
            body.velocity = temp_vel;
        }

        if (Input.GetKey(UP) && Player.GetComponent<timeTravel>().grounded) {
            Vector3 jump = new Vector3(0, 100, 0);
            body.AddForce(jump);
            Player.GetComponent<timeTravel>().grounded = false;
        }
        if (Input.GetKey(D_KEY)) {
            go_to_start();
            replay = true;
        }
        if (Input.GetKey(ONE)) {
            Player = players[0];
            body = Player.GetComponent<Rigidbody>();
            Player.GetComponent<timeTravel>().last_velocity = Vector3.zero;
            go_to_start();
        }
        if (Input.GetKey(TWO)) {
            Player = players[1];
            body = Player.GetComponent<Rigidbody>();
            Player.GetComponent<timeTravel>().last_velocity = Vector3.zero;
            go_to_start();
        }
        if (Input.GetKey(THREE)) {
            Player = players[2];
            body = Player.GetComponent<Rigidbody>();
            Player.GetComponent<timeTravel>().last_velocity = Vector3.zero;
            go_to_start();
        }
        //        Player.GetComponent<timeTravel>().last_velocity = Vector3.zero;
    }
    void go_to_start() {
        foreach (GameObject traveller in time_travellers) {
            traveller.GetComponent<timeTravel>().goBack(0);
            traveller.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        replay = false;
    }
}
