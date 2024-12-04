using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AngelChasingState : BaseAngelState<AngelEnemyStateManager.ANGEL_STATES> {

    // private Task<AngelEnemyStateManager.ANGEL_STATES> ChangeToIdleTimer;

    // private List<Collider2D> _collidersInAttackArea = new();

    private RaycastHit2D _playerInRangeOfLeft;
    private RaycastHit2D _playerInRangeOfRight;

    public AngelChasingState(AngelStateContext context, AngelEnemyStateManager.ANGEL_STATES stateKey, AngelEnemyStateManager esm) : base(context, stateKey, esm) {}

    public override void EnterState(){
        TurnAngel(Context.lastPlayerDirection);
        Context.AngelRef._animator.SetBool("isWalking", true);


        IsPlayerInAttackRange(Context.lastPlayerDirection);

        // Debug.Log(Context.rightSmiteBounds + "" + Context.leftSmiteBounds);
    }

    public override void UpdateState(){
        // should mainly be used for checking the condition for when to change to another state
        CheckColliderForPlayer(Context.LeftPlayerDetectionArea, -1);
        if (!Context.playerDetected) CheckColliderForPlayer(Context.RightPlayerDetectionArea, 1);

        IsPlayerInAttackRange(Context.lastPlayerDirection);
    }

    private void IsPlayerInAttackRange(int direction){
        // Debug.Log(direction);


        Bounds collider = direction == 1 ? Context.rightSmiteBounds : Context.leftSmiteBounds;
        Bounds hitbox = Context.AngelRef._hitbox.gameObject.GetComponent<Collider2D>().bounds;

        Vector2 boxCastOrigin = new Vector2(hitbox.center.x, hitbox.center.y);

        if (direction == -1){
            _playerInRangeOfLeft = Physics2D.Raycast(new(boxCastOrigin.x, boxCastOrigin.y), Vector2.left, collider.size.x + 5, LayerMask.GetMask("Player"));
        }
        else {
            _playerInRangeOfRight = Physics2D.Raycast(new(boxCastOrigin.x, boxCastOrigin.y),Vector2.right, collider.size.x + 5, LayerMask.GetMask("Player"));
        }

    }

    public override void FixedUpdateState(){
        // should mainly be used for any physics calcs - like moving a rigidbody
        Context.RigidBody.velocity = new Vector2(3 * (Context.isFacingRight == true ? 1 : -1), Context.RigidBody.velocity.y);
    }

    public override void ExitState(){
        // anything this states should do before the state machine switches to another state
        // Context.playerDetected = false;
    }

    public override AngelEnemyStateManager.ANGEL_STATES GetNextState(){
        if (Context.AngelRef.enemyStats.health <= 0 ){
            return AngelEnemyStateManager.ANGEL_STATES.DEAD;
        }

        if(_playerInRangeOfLeft.collider != null){
            if (_playerInRangeOfLeft.collider.gameObject.CompareTag("Hitbox")) return AngelEnemyStateManager.ANGEL_STATES.SMITE_ATTACK;
        } 
        
        if(_playerInRangeOfRight.collider != null){
            if (_playerInRangeOfRight.collider.gameObject.CompareTag("Hitbox")) return AngelEnemyStateManager.ANGEL_STATES.SMITE_ATTACK;
        }

        if (!Context.playerDetected){
            return AngelEnemyStateManager.ANGEL_STATES.IDLE;
        }

        return StateKey;
    }

    public override void OnTriggerEnter(Collider collider){
        
    }

    public override void OnTriggerStay(Collider collider){}

    public override void OnTriggerExit(Collider collider){}
}