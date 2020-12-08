﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net.Sockets;
using System.Net;



public class NetworkMan : MonoBehaviour
{
    public Camera clientHandlerCamera;
    public GameObject player;
    private bool connected = false;
    private bool playerSpawned = false;

    public UdpClient udp;
    // Start is called before the first frame update
    void Start()
    {
        udp = new UdpClient();
        
        udp.Connect("13.59.240.23", 12345);

        Byte[] sendBytes = Encoding.ASCII.GetBytes("connect");
      
        udp.Send(sendBytes, sendBytes.Length);

        udp.BeginReceive(new AsyncCallback(OnReceived), udp);

        InvokeRepeating("HeartBeat", 1, 1);

        InvokeRepeating("UpdatePlayers",5.0f,5.0f);
    }

    void OnDestroy(){
        udp.Dispose();
    }


    public enum commands{
        NEW_CLIENT,
        UPDATE
    };
    
    [Serializable]
    public class Message{
        public commands cmd;
    }
    
    [Serializable]
    public class Player{
        [Serializable]
        public struct receivedColor{
            public float R;
            public float G;
            public float B;
        }
        public string id;
        public receivedColor color;

        public Vector3 position;

    }

    [Serializable]
    public class NewPlayer{
        
    }

    [Serializable]
    public class GameState{
        public Player[] players;

    }

    [Serializable]
    public class playerPos
    {
        public float x;
        public float y;
        public float z;
    }

  
    //public void testmsg(string txt)
    //{
    //    string text = "hello";
    //}
    
    public Message latestMessage;
    public GameState lastestGameState;
    void OnReceived(IAsyncResult result){

        
        // this is what had been passed into BeginReceive as the second parameter:
        UdpClient socket = result.AsyncState as UdpClient;
        
        // points towards whoever had sent the message:
        IPEndPoint source = new IPEndPoint(0, 0);

        // get the actual message and fill out the source:
        byte[] message = socket.EndReceive(result, ref source);
        
        // do what you'd like with `message` here:
        string returnData = Encoding.ASCII.GetString(message);
        
        Debug.Log("Got this: " + returnData);

   


        latestMessage = JsonUtility.FromJson<Message>(returnData);
        try{
            switch(latestMessage.cmd){
                case commands.NEW_CLIENT:
                    break;
                case commands.UPDATE:
                    connected = true;
                    lastestGameState = JsonUtility.FromJson<GameState>(returnData);
                    break;
                default:
                    Debug.Log("Error");
                    break;
            }
        }
        catch (Exception e){
            Debug.Log(e.ToString());
        }
        
        // schedule the next receive operation once reading is done:
        socket.BeginReceive(new AsyncCallback(OnReceived), socket);
    }

    void SpawnPlayers(){
        player = Instantiate(player, transform.position, transform.rotation) as GameObject;
        MyPlayerScript myPlayerScript = player.GetComponentInChildren<MyPlayerScript>();
        playerSpawned = true;
        
    }

    public float x;
    public float z;
    public float y;

    public string pos; 

    void UpdatePlayers(){
        //Player playera = new Player();
        //x = player.transform.position.x;
        //z = player.transform.position.z;
        //y = player.transform.rotation.y;

        //pos = "X Pos: " + x.ToString() + " Z Pos: " + z.ToString() + " Y Rot: " + y.ToString();

        foreach (var i in lastestGameState.players)
        {
            Color color = new Color(i.color.R, i.color.G, i.color.B);
            player.GetComponent<Renderer>().material.SetColor("_Color", color);
        }
    }

    void DestroyPlayers(){

    }
    
    
    void HeartBeat(){
        Byte[] sendBytes = Encoding.ASCII.GetBytes("heartbeat");
        udp.Send(sendBytes, sendBytes.Length);
    }

    //public void SendPosition(float posx, float posz, float roty)
    //{
    //    playerPos info = new playerPos();
    //    info.x = posx;
    //    info.z = posz;
    //    info.y = roty;
    //    string jsonString = JsonUtility.ToJson(info);
    //    Debug.Log(jsonString);
    //    Byte[] sendBytes = Encoding.ASCII.GetBytes(jsonString);
    //    udp.Send(sendBytes, sendBytes.Length);
    //}



    void Update(){

        if (connected == true)
        {
            if (playerSpawned == false)
            {
                SpawnPlayers();
            }
            UpdatePlayers();

        }
        else if (connected == false)
        {
            clientHandlerCamera.enabled = true;
        }
  
        UpdatePlayers();
        DestroyPlayers();
    }
}
