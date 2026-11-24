using FlyRabbit.StateMachine;
using FlyRabbit.StateMachine.FoxState;
using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CheckGrounded2D))]
public class Fox : Item
{
    #region Backing Fields
    private int m_CurrentAnimation;
    #endregion
    private Animator Animator { get; set; }
    public Rigidbody2D Rigidbody2D { get; private set; }
    public CheckGrounded2D CheckGrounded2D { get; private set; }
    /// <summary>状态机</summary>
    private StateMachine<Fox> m_StateMachine;

    /// <summary>跳跃力，根据跳跃高度计算,如果跳跃高度是固定的其实可以采取缓存策略</summary>
    public float JumpForce { get => Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * JumpHight); }
    /// <summary>跳跃高度</summary>
    public float JumpHight { get; } = 1.2f;
    /// <summary>最大移动速度</summary>
    public float MaxMovementSpeed { get; } = 3.4f;
    /// <summary>移动加速度</summary>
    public float MovementAcceleration { get; } = 12f;
    /// <summary>运动学摩擦力</summary>
    public float KinematicFriction { get; } = 12f;//运动学摩擦力
    /// <summary>面朝方向</summary>
    public int FacingDiraction { get; private set; } = 1;//面朝方向


    public int AnimationHash_VerticalState { get; private set; }
    public int AnimationHash_Idle { get; private set; }
    public int AnimationHash_Move { get; private set; }
    public int AnimationHash_Dead { get; private set; }
    public int AnimationHash_Attack { get; private set; }
    public int AnimationHash_InAir { get; private set; }


    public Idle IdleState { get; private set; }
    public Move MoveState { get; private set; }
    public Frozen FrozenState { get; private set; }
    public Dead DeadState { get; private set; }
    public Attack AttackState { get; private set; }
    public InAir InAirState { get; private set; }

    /// <summary>
    /// 当前动画Hash，用于解决Bug，不要用Animator.Play，而是修改此项数值。
    /// Bug:当物体被禁用时Animator.Play会失效，
    /// </summary>
    public int CurrentAnimationHash
    {
        get
        {
            return m_CurrentAnimation;
        }
        set
        {
            m_CurrentAnimation = value;
            if (isActiveAndEnabled)
            {
                Animator.Play(m_CurrentAnimation);
            }
        }
    }
    /// <summary>动画播放速度</summary>
    public float AnimationSpeed
    {
        get
        {
            return Animator.speed;
        }
        set
        {
            Animator.speed = value;
        }
    }
    private void Awake()
    {
        //1.获取组件依赖
        Animator = GetComponent<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        CheckGrounded2D = GetComponent<CheckGrounded2D>();
        //2.初始化动画参数
        AnimationHash_VerticalState = Animator.StringToHash("VerticalState");
        AnimationHash_Idle = Animator.StringToHash("Idle");
        AnimationHash_Move = Animator.StringToHash("Move");
        AnimationHash_Dead = Animator.StringToHash("Dead");
        AnimationHash_Attack = Animator.StringToHash("Attack");
        AnimationHash_InAir = Animator.StringToHash("InAir");
        //3.初始化状态机
        m_StateMachine = new StateMachine<Fox>(this);
        IdleState = new Idle(m_StateMachine);
        MoveState = new Move(m_StateMachine);
        FrozenState = new Frozen(m_StateMachine);
        DeadState = new Dead(m_StateMachine);
        AttackState = new Attack(m_StateMachine);
        InAirState = new InAir(m_StateMachine);
        m_StateMachine.ChangeState(IdleState);
    }
    private void OnEnable()
    {
        //Bug Fixed
        Animator.Play(CurrentAnimationHash);
    }
    private void Update()
    {
        m_StateMachine.Update();
    }

    private void FixedUpdate()
    {
        m_StateMachine.FixedUpdate();
    }


    public override void Initialize()
    {
        FacingDiraction = 1;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    protected override void OnFreeze()
    {
        m_StateMachine.ChangeState(FrozenState);
    }
    protected override void OnUnfreeze()
    {
        m_StateMachine.ChangeState(IdleState);
    }
    #region Public Methods
    /// <summary>
    /// 更新动画参数：VerticalState，
    /// 这个参数用来代表玩家当前是在上升还是在下落，
    /// -1为下落，1为上升。
    /// </summary>
    public void UpdateVerticalState()
    {
        Animator.SetFloat(AnimationHash_VerticalState, Rigidbody2D.velocity.y > 0 ? 1 : -1);
    }

    public void Idle()
    {
        //获取当前x轴速度
        float currentX = Rigidbody2D.velocity.x;
        //获取这一帧的加速度
        float acceleration = MovementAcceleration * Time.fixedDeltaTime;
        //计算新的x轴速度
        float newX = Mathf.MoveTowards(currentX, 0, acceleration);
        //赋值
        Rigidbody2D.velocity = new Vector2(newX, Rigidbody2D.velocity.y);
    }

    /// <summary>跳跃</summary>
    public void Jump()
    {
        if (CheckGrounded2D.Grounded == false)
        {
            return;
        }
        AudioManager.PlaySFX(AudioPath.SFX.GetRandomJumpPath());
        Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, JumpForce);
    }
    /// <summary>在空中移动</summary>
    public void MoveInAir()
    {
        //在地面上无效
        if (CheckGrounded2D.Grounded == true)
        {
            return;
        }
        //获取移动方向
        int movementFactor = InputManager.Instance.MovementFactor;
        //获取当前x轴速度
        float currentX = Rigidbody2D.velocity.x;
        //获取这一帧的加速度
        float acceleration = MovementAcceleration * Time.fixedDeltaTime;
        //计算出新的x轴速度
        float newX = Mathf.MoveTowards(currentX, MaxMovementSpeed * movementFactor, acceleration);
        //赋值速度
        Rigidbody2D.velocity = new Vector2(newX, Rigidbody2D.velocity.y);
        //确保面朝移动方向
        if (movementFactor != 0 && movementFactor != FacingDiraction)
        {
            Flip();
        }
    }
    /// <summary>在地面移动</summary>
    public void MoveOnGround()
    {
        //不在地面上无效
        if (CheckGrounded2D.Grounded == false)
        {
            return;
        }
        //获取移动方向
        int movementFactor = InputManager.Instance.MovementFactor;
        //获取当前x轴速度
        float currentX = Rigidbody2D.velocity.x;
        //获取这一帧的加速度
        float acceleration = MovementAcceleration * Time.fixedDeltaTime;
        //获取这一帧的运动学摩擦力
        float Friction = KinematicFriction * Time.fixedDeltaTime;
        //如果玩家要向右走，这时候有向左的速度，则摩擦力也提供加速度，让玩家更快转向，反之也一样。
        if ((currentX < 0 && movementFactor == 1) || (currentX > 0 && movementFactor == -1))
        {
            acceleration += Friction;
        }
        //计算新的x轴速度
        float newX;
        //变速规则：当玩家的当前朝向的速度大于最大移动速度时，使用摩擦力减速，否则使用加速度加速
        if ((movementFactor == 1 && Rigidbody2D.velocity.x > MaxMovementSpeed) || (movementFactor == -1 && Rigidbody2D.velocity.x < -MaxMovementSpeed))
        {
            newX = Mathf.MoveTowards(currentX, MaxMovementSpeed * movementFactor, Friction);
        }
        else
        {
            newX = Mathf.MoveTowards(currentX, MaxMovementSpeed * movementFactor, acceleration);
        }
        //赋值新速度
        Rigidbody2D.velocity = new Vector2(newX, Rigidbody2D.velocity.y);
        //确保面朝移动方向
        if (movementFactor != 0 && movementFactor != FacingDiraction)
        {
            Flip();
        }
    }

    public void Attack()
    {
        m_StateMachine.ChangeState(AttackState);
        AudioManager.PlaySFX(AudioPath.SFX.GetRandomAttackMissPath());
    }

    public void Place()
    {

    }
    #endregion

    #region Private Methods
    /// <summary>给这个游戏物体翻面</summary>
    public void Flip()
    {
        FacingDiraction *= -1;
        transform.Rotate(0f, 180f, 0f);
    }
    #endregion
}
