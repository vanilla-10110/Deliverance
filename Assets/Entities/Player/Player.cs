using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Header("References")]
    public PlayerMovementStats moveStats;
    [SerializeField] private Collider2D _feetColl;
    [SerializeField] private Collider2D _bodyColl;

    [SerializeField] private Hitbox _hitbox;

    [SerializeField] private GameObject Light;

    public Animator animator;
    private Rigidbody2D _rb;
    public TrailRenderer PlayerTrail {get; private set;}

    [NonSerialized] public Vector2 velocity = Vector2.zero;
    private Vector2 _environmentalVelocity = Vector2.zero;

    [NonSerialized] public bool isFacingRight;

    //collision check vars
    private RaycastHit2D _leftGroundHit;
    private RaycastHit2D _rightGroundHit;

    private RaycastHit2D _headHit;
    [NonSerialized] public bool isGrounded;
    [NonSerialized] public bool bumpedHead;
    [NonSerialized] public bool isClimbable;

    // jump vars
    [NonSerialized] public int numberOfJumpsUsed = 0;

    [Header("Debug")]
    public bool logStateMessages = false;

    // dash vars
    [NonSerialized] public bool dashUnlocked = false;
    [NonSerialized] public int numberOfDashesUsed = 0;

    // climb vars
    [NonSerialized] public bool climbingUnlocked = false;

    private PlayerStateManager psm;

    [Header("Attack Refs")]
    [NonSerialized] public bool canAttack = true;
    [Serializable] public enum ATTACK_DIRECTION {RIGHT, LEFT, BOTTOM, TOP};

    [SerializedDictionary("AttackAreaEnum","Hurtbox")]

    [SerializeField] private Hurtbox _leftUppiesHurtbox;
    [SerializeField] private Hurtbox _rightUppiesHurtbox;

    [Header("soundFX")]
    public AudioClip dashSoundFX;
    public AudioClip deathSoundFX;
    public AudioClip jumpSoundFX;
    public AudioClip uppiesSoundFX;
    public AudioClip scytheSoundFX;



    private void Awake(){
        isFacingRight = true;
        _rb = GetComponent<Rigidbody2D>();
        PlayerTrail = GetComponent<TrailRenderer>();
        animator = GetComponent<Animator>();
        psm = GetComponent<PlayerStateManager>();
    }

    private void Start(){
        SignalBus.newSceneLoaded.AddListener(ChangePosition);

        SignalBus.AbilityUnlockedEvent.AddListener((EnumBus.PLAYER_ABILITIES ability) => {
            if (ability == EnumBus.PLAYER_ABILITIES.DASH) dashUnlocked = true;
            else if (ability == EnumBus.PLAYER_ABILITIES.PLANT_DADDY) climbingUnlocked = true;
        });

        GameManager.Instance.playerStats.HealthDepletedEvent.AddListener(() => {
            GameManager.Instance.GameOverActions();
            animator.SetTrigger("TriggerDeath");
            SoundManager.Instance.PlaySoundFX(deathSoundFX);
        });
        
        psm.Init(this);

        if (GameObject.Find("Player")){
            Destroy(this.gameObject);
        }

        if (_hitbox){
            _hitbox.HitDetected.AddListener((int damageValue) => {GameManager.Instance.playerStats.DecreaseHealth(damageValue);});
            _hitbox.HitboxIntersectingForce.AddListener((Vector2 RepulsionForce) => {
                _environmentalVelocity = new Vector2(RepulsionForce.x, 0); // adds a opposing force if inside another hitbox
            });
        }

        PlayerTrail.emitting = false;

        GameManager.Instance.playerRef = this;
    }

    private void OnDestroy(){
        SignalBus.newSceneLoaded.RemoveListener(ChangePosition);
    }

    private void ChangePosition(){
        GameManager.Instance.MovePlayerToSpawnpoint();
    }

    public void ResetPlayerState(){
        animator.SetTrigger("TriggerRespawn");
    }

    private void Update(){

        if (GameManager.Instance.gameState == EnumBus.GAME_STATE.DEAD || GameManager.Instance.gameState == EnumBus.GAME_STATE.WIN){
            return;
        }
        
        IsGrounded();
        IsHeadBumped();

        TurnCheck(InputManager.movement);

        UpdateAttackArea();
        ActivateSecondaryAttack();

        if (InputManager.attackWasPressed){
            ActivateAttackArea(currentAttackDirection);
        }

        animator.SetBool("isOnGround", isGrounded);
    }
    
    private void FixedUpdate(){
        if (GameManager.Instance.gameState == EnumBus.GAME_STATE.DEAD || GameManager.Instance.gameState == EnumBus.GAME_STATE.WIN){
            return;
        }
        _rb.velocity = velocity;
        _rb.velocity += _environmentalVelocity * Time.fixedDeltaTime;
        _environmentalVelocity = Vector2.zero;
    }

    #region horizontal movement
    private void TurnCheck(Vector2 moveInput){
        if (isFacingRight && moveInput.x < 0){
            Turn(false);
        }
        else if (!isFacingRight && moveInput.x > 0) {
            Turn(true);
        }
    }

    private void Turn (bool turnRight){
        if (turnRight){
            isFacingRight = true;
            GetComponent<SpriteRenderer>().flipX = false;
            Light.transform.localPosition = new Vector2((float)0.57, (float)1.17);
        }

        else {
            isFacingRight = false;
            GetComponent<SpriteRenderer>().flipX = true;
            Light.transform.localPosition = new Vector2((float)-0.57, (float)1.17);
        }
    }

    #endregion

    #region CollisonChecks
    private void IsGrounded(){
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, moveStats.groundDetectionRayLength);

        // _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, moveStats.groundDetectionRayLength, moveStats.groundLayer);
        _leftGroundHit = Physics2D.Raycast(new(boxCastOrigin.x - _feetColl.bounds.size.x / 2, boxCastOrigin.y),Vector2.down, moveStats.groundDetectionRayLength, moveStats.groundLayer);
        _rightGroundHit = Physics2D.Raycast(new(boxCastOrigin.x + _feetColl.bounds.size.x / 2, boxCastOrigin.y),Vector2.down, moveStats.groundDetectionRayLength, moveStats.groundLayer);

        if (_leftGroundHit.collider != null || _rightGroundHit.collider != null){
            isGrounded = true;
        }
        else{ isGrounded = false; }
    
        if (moveStats.debugShowIsGroundedBox){
            Color rayColor;
            if (isGrounded){
                rayColor = Color.green;
            }

            else { rayColor = Color.red; }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * moveStats.groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * moveStats.groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - moveStats.groundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);

        }
    }

    private void IsHeadBumped(){
        Vector2 boxCastOrigin = new Vector2(_bodyColl.bounds.center.x, _bodyColl.bounds.max.y);
        Vector2 boxCastSize = new Vector2(moveStats.headWidth, moveStats.headDetectionRayLength);

        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, moveStats.headDetectionRayLength, moveStats.groundLayer);

        if (_headHit.collider != null){
            bumpedHead = true;
        }
        else { bumpedHead = false; }

        if (moveStats.debugShowHeadBumpBox){
            Color rayColor;
            if (bumpedHead){
                rayColor = Color.green;
            }

            else { rayColor = Color.red; }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.up * moveStats.headDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.up * moveStats.headDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y + moveStats.headDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("ENTER" + collision.tag);
        if (collision.CompareTag("Ladder"))
        {
            if (collision.gameObject.GetComponent<Interaction>().Grown == true)
            {
                isClimbable = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            if (collision.gameObject.GetComponent<Interaction>().Grown == true)
            {
                isClimbable = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Debug.Log("EXIT" + collision.tag);
        if (collision.CompareTag("Ladder"))
        {
            isClimbable = false;
        }
    }
    #endregion

    public void ChangeHealth(int value){
        if (GameManager.Instance.playerStats.health > 0){
            if (value < 0){
               GameManager.Instance.playerStats.DecreaseHealth(value); 
            }
            else if (value > 0){
                GameManager.Instance.playerStats.IncreaseHealth(value);
            }
        }
    }



    #region Attack Related


    public SerializedDictionary<ATTACK_DIRECTION, Hurtbox> attackHurtboxes;

    private ATTACK_DIRECTION currentAttackDirection = ATTACK_DIRECTION.RIGHT;
    
    // to check and update where which attack area should be active
    private void UpdateAttackArea(){
        // up or down takes input priority  
        if (InputManager.movement.y != 0){
            switch (InputManager.movement.y) {
                case 1:
                    currentAttackDirection = ATTACK_DIRECTION.TOP;
                    return;
                case -1:
                    currentAttackDirection = ATTACK_DIRECTION.BOTTOM;
                    return;
            }
        }
        switch (isFacingRight) {
            case true:
                currentAttackDirection = ATTACK_DIRECTION.RIGHT;
                return;
            case false:
                currentAttackDirection = ATTACK_DIRECTION.LEFT;
                return;
        }
    }

    private void ActivateSecondaryAttack(){
        if (InputManager.secondaryAttackWasPressed){
            animator.SetTrigger("TriggerUppies");
            SoundManager.Instance.PlaySoundFX(uppiesSoundFX);

            if (isFacingRight){
                _rightUppiesHurtbox.ActivateHurtBox(0.1f, 1, UppiesAttackEffect);
            }
            else if (!isFacingRight){
                _leftUppiesHurtbox.ActivateHurtBox(0.1f, 1, UppiesAttackEffect);
            }
        }
    }

    UnityAction<Rigidbody2D> UppiesAttackEffect = (Rigidbody2D obj) => {
        obj.AddForce(new Vector3(0, 1000, 0));
    };

    private Hurtbox GetAttackArea(ATTACK_DIRECTION dir){
        return attackHurtboxes[dir];
    }

    private void ActivateAttackArea(ATTACK_DIRECTION dir){
        if (canAttack){
            animator.StopPlayback();
            animator.SetTrigger("isAttacking");
            GetAttackArea(dir).ActivateHurtBox(0.1f, 1);
            SignalBus.AbilityUsedEvent.Invoke(EnumBus.PLAYER_ABILITIES.LIGHT_ATTACK.ToString());
            SoundManager.Instance.PlaySoundFX(scytheSoundFX);
        }
    }
    #endregion
}