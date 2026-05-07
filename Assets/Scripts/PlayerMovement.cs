using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float gravity = -9.81f;

    public InputActionProperty moveAction;
    public InputActionProperty runAction;

    // 【重要】ここにMain Cameraをドラッグ＆ドロップしてください
    private Transform mainCameraTransform;
    Vector3 velocity;

    public float CurrentMoveVelocity => new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;

    void Start()
    {
        // タグが「MainCamera」になっているカメラを自動で探して入れる
        if (Camera.main != null )
        {
            mainCameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("シーンに MainCamera タグが付いたカメラが見つかりません！");
        }
    }


    void Update()
    {
        // 1. 入力の取得
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        bool isRunning = runAction.action.IsPressed();
        float currentSpeed = (isRunning && input.magnitude > 0) ? runSpeed : walkSpeed;

        // 2. カメラの向きに基づいた移動方向の計算
        Vector3 forward = mainCameraTransform.forward;
        Vector3 right = mainCameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // 水平方向のベクトルを計算（ここではまだ Move しない）
        Vector3 horizontalMove = (right * input.x + forward * input.y) * currentSpeed;

        // 3. 重力処理
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 地面に吸い付くための微小な下方向の力
        }
        velocity.y += gravity * Time.deltaTime;

        // 4. 【ここが修正ポイント】全ての力を合算して、Moveを1回だけ呼ぶ
        // (水平方向の動き) + (垂直方向の重力)
        Vector3 finalMovement = horizontalMove + Vector3.up * velocity.y;

        // 最後に一気に動かす。これで controller.velocity に正しく全ての成分が入る
        controller.Move(finalMovement * Time.deltaTime);
    }
}