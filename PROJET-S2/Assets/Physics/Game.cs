using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Player;
using Mirror;
using UnityEngine;

class Game : NetworkBehaviour
{
    public int personnage;
    // Start is called before the first frame update
    void Start()
    {
        GameObject _go = gameObject;
        switch (personnage)
        {
            case 0:
                _go.AddComponent<Dummy>();
                break;
            case 1:
                _go.AddComponent<PlayerTest>();
                break;
        }
        _go.tag = "Joueur";
    }
}
