using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGameWinKey : EnemyCountKey
{
    [SerializeField] private AudioClip winSong;
    new void Start()
    {
        base.Start();
        Keytriggered.AddListener((KEY_STATE state) => {
            GameManager.Instance.WinGameActions();
            SoundManager.Instance.PlayBackgroundTrack(winSong);
        });

    }

}
