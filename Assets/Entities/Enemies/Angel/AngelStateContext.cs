
using UnityEngine;

public class AngelStateContext {
    private AngelEnemy _angelRef;
    private AngelEnemyStateManager.ANGEL_STATES _prevState;
    private Rigidbody2D _rb;
    private Collider2D _leftPlayerDetectionArea;
    private Collider2D _rightPlayerDetectionArea;

    private Collider2D _leftSmiteAttackArea;
    private Collider2D _rightSmiteAttackArea;



    public bool isFacingRight;
    public bool playerDetected;
    public int lastPlayerDirection;


    public AngelStateContext(AngelEnemyStateManager.ANGEL_STATES initialState, AngelEnemy angelRef, Rigidbody2D rb, Collider2D leftPlayerDetect, Collider2D rightPlayerDetect, Collider2D leftSmite, Collider2D rightSmite) {
        _angelRef = angelRef;
        _prevState = initialState;
        _rb = rb;
        _leftPlayerDetectionArea = leftPlayerDetect;
        _rightPlayerDetectionArea = rightPlayerDetect;
        _leftSmiteAttackArea = leftSmite;
        _rightSmiteAttackArea = rightSmite;


        isFacingRight = false;
        playerDetected = false;
        lastPlayerDirection = 1;
    }

    public AngelEnemy AngelRef => _angelRef;
    public AngelEnemyStateManager.ANGEL_STATES PrevState => _prevState;
    public Rigidbody2D RigidBody => _rb;

    public Collider2D LeftPlayerDetectionArea => _leftPlayerDetectionArea;
    public Collider2D RightPlayerDetectionArea => _rightPlayerDetectionArea;
    public Collider2D LeftSmiteArea => _leftSmiteAttackArea;
    public Collider2D RightSmiteArea => _rightSmiteAttackArea;

}