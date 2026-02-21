using DG.Tweening;
using UnityEngine;
using Zenject;
using static UnityEngine.RuleTile.TilingRuleOutput;

public interface IState
{
    void Enter();
    void Update();
    void Exit();
}

public class StateMachine
{
    private IState currentState;

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}

public class IdleState : IState
{
    private MadBoatBehaviour car;
    private IInputHandler input;

    public IdleState(MadBoatBehaviour car, IInputHandler input)
    {
        this.car = car;
        this.input = input;
    }

    public void Enter()
    {
    }

    public void Update()
    {
        car.Stop();

        if (input.IsMovePressed)
            car.ChangeToDriving();
    }

    public void Exit() { }
}

public class DrivingState : IState
{
    private MadBoatBehaviour car;
    private IInputHandler input;

    public DrivingState(MadBoatBehaviour car, IInputHandler input)
    {
        this.car = car;
        this.input = input;
    }

    public void Enter() 
    {
        car.GetSpeedController().Reset();
    }

    public void Update()
    {
        car.MoveTowards(input.MouseWorldPosition);

        if (!input.IsMovePressed)
            car.ChangeToIdle();
    }

    public void Exit() { }
}


public class MadBoatBehaviour : MonoBehaviour
{
    private IInputHandler input;
    private StateMachine stateMachine;

    private IdleState idleState;
    private DrivingState drivingState;

    [Header("Movement Settings: ")]    
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float moveSpeed = 8f;

    [Header("Wobbling Effect Settings: ")]
    [SerializeField] private float amplitude = 9f; 
    [SerializeField] private float frequency = 2f;

    private float time;

    private SpeedController speedController = new SpeedController();
    public SpeedController GetSpeedController() => speedController;

    [Inject]
    public void Construct(IInputHandler input)
    {
        this.input = input;
    }

    private void Awake()
    {
        stateMachine = new StateMachine();

        idleState = new IdleState(this, input);
        drivingState = new DrivingState(this, input);
    }

    private void Start()
    {
        stateMachine.ChangeState(idleState);
        StartFloatingEffect();
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void StartFloatingEffect()
    {
        DOTween.To(() => time, x => time = x, Mathf.PI * 2f, 1f / frequency).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart)
        .OnUpdate(() =>
        {
            float angleX = Mathf.Sin(time) * amplitude;
            float angleZ = Mathf.Sin(time) * angleX;

            Vector3 rot = transform.localEulerAngles;
            rot.x = angleX;
            rot.z = angleZ;
            transform.localEulerAngles = rot;
        });
    }


    public void MoveTowards(Vector3 target)
    {
        RotateTowards(target);

        float speed = speedController.Accelerate(moveSpeed, acceleration);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0;

        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void Stop()
    {
        if (speedController.CurrentSpeed == 0)
        {
            return;
        }

        float speed = speedController.Decelerate(acceleration);
        WobbleEffect();

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void WobbleEffect()
    {
        Vector3 wobble = transform.right * Mathf.Sin(Time.time * 0.5f) * 0.3f;
        Vector3 direction = transform.forward + wobble;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void ChangeToIdle() => stateMachine.ChangeState(idleState);
    public void ChangeToDriving() => stateMachine.ChangeState(drivingState);
}

public class SpeedController
{
    private float currentSpeed = 0f;

    public float CurrentSpeed => currentSpeed;

    public float Accelerate(float targetSpeed, float acceleration)
    {
        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            acceleration * Time.deltaTime
        );

        return currentSpeed;
    }

    public float Decelerate(float braking)
    {
        currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, (braking * Time.deltaTime) / 2); //ААААААА ДОЛБАННЫЕ МАГИЧЕСКИЕ ЦИФРЫ!!!!
        return currentSpeed;
    }

    public void Reset()
    {
        currentSpeed = 0f;
    }
}