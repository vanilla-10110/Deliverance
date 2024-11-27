
using UnityEngine;
using UnityEngine.Assertions;

public class AngelEnemyStateManager : BaseStateManager<AngelEnemyStateManager.ANGEL_STATES> {
    public enum ANGEL_STATES {
        IDLE,
        PATROLLING,
        SMITE_ATTACK,
        DEAD,
        CHASING
    }

    [SerializeField] private AngelEnemy angelRef;
    private AngelStateContext _context;

    [SerializeField] private Collider2D _leftPlayerDetectionArea;
    [SerializeField] private Collider2D _rightPlayerDetectionArea;
    [SerializeField] private Collider2D _leftSmiteArea;
    [SerializeField] private Collider2D _rightSmiteArea;


    public void Init(AngelEnemy newAngelRef){
        angelRef = newAngelRef;
    }

    void Awake(){
        ValidateReferences();

        _context = new AngelStateContext(InitialState, angelRef, angelRef.GetComponent<Rigidbody2D>(), _leftPlayerDetectionArea, _rightPlayerDetectionArea, _leftSmiteArea, _rightSmiteArea);

        InitiateStates();
    }


    private void ValidateReferences(){
        Assert.IsNotNull(angelRef, "AngelRef is not assigned");
    }

    private void InitiateStates(){
        // add states to the dictionary and assign their enum
        States.Add(ANGEL_STATES.IDLE, new AngelIdleState(_context, ANGEL_STATES.IDLE, this));
        States.Add(ANGEL_STATES.PATROLLING, new AngelPatrollingState(_context, ANGEL_STATES.PATROLLING, this));
        States.Add(ANGEL_STATES.SMITE_ATTACK, new AngelSmiteAttackState(_context, ANGEL_STATES.SMITE_ATTACK, this));
        States.Add(ANGEL_STATES.DEAD, new AngelDeadState(_context, ANGEL_STATES.DEAD, this));
        States.Add(ANGEL_STATES.CHASING, new AngelChasingState(_context, ANGEL_STATES.CHASING, this));


        CurrentState = States[InitialState];
    }

    // private void OnTriggerEnter(Collider2D){}

}