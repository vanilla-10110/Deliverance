using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlockCollectable : BaseCollectable
{
    [SerializeField] private EnumBus.PLAYER_ABILITIES ability = EnumBus.PLAYER_ABILITIES.DASH;
    void Start(){
        CollectedEvent.AddListener(OnCollectedEvent);
    }

    private void OnCollectedEvent(){
    }

    private void OnTriggerEnter2D(Collider2D other){
        
        if(other.gameObject.CompareTag("Hitbox")){
            SoundManager.Instance.PlaySoundFX(_pickupSound);
            SignalBus.AbilityUnlockedEvent.Invoke(ability);
            Destroy(this.gameObject);

        }
    }


}
