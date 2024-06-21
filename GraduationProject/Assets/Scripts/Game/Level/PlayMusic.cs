using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public string Musicname;
    // Start is called before the first frame update
    void Start()
    {
        MusicManager.Instance.PlayBgMusic("Music/"+ Musicname);
    }

    
}
