using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;
    public float gravity = -9.81f;

    public InputActionProperty moveAction;

    // 【重要】ここにMain Cameraをドラッグ＆ドロップしてください
    public Transform cameraTransform;

    Vector3 velocity;

    void Update()
    {
        // 1. 体の向きをカメラの水平方向と同期させる
        if (cameraTransform != null)
        {
            // カメラの正面ベクトルを取得
            Vector3 cameraForward = cameraTransform.forward;
            // 上下の傾き（y成分）を0にして、水平方向だけの向きにする
            cameraForward.y = 0;

            // 向きがゼロ（真上や真下を向いている時など）でなければ、体をその方向に向ける
            if (cameraForward.sqrMagnitude > 0.01f)
            {
                transform.forward = cameraForward;
            }
        }

        // 2. 移動処理
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        // transform.forwardがカメラの向きと一致しているので、これでカメラ方向に進む
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        controller.Move(move * speed * Time.deltaTime);

        // 3. 重力処理
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}