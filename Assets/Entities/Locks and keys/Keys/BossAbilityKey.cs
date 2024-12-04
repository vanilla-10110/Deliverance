using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAbilityKey : EnemyCountKey
{
    [SerializeField] private EnumBus.PLAYER_ABILITIES _abilityUnlock = EnumBus.PLAYER_ABILITIES.PLANT_DADDY;

    new protected void Start() {
        base.Start();
        Keytriggered.AddListener((KEY_STATE state) => {
            if (state == KEY_STATE.UNLOCKED) SignalBus.AbilityUnlockedEvent.Invoke(_abilityUnlock);
        });
    }

}
