using UnityEngine;

public class PlayerNoise : MonoBehaviour
{
    // 状態ごとの範囲サイズ
    public float idleRadius = 2f;
    public float walkRadius = 5f;
    public float runRadius = 10f;
    public float landingRadius = 15f; // 着地時の大きな揺れ

    private SphereCollider noiseCollider; // インスペクターで見せなくていいのでprivateに
    private PlayerMovement movement; // 移動スクリプトへの参照

    void Awake()
    {
        // 自分のオブジェクトに付いているSphereColliderを取得
        noiseCollider = GetComponent<SphereCollider>();
        movement = GetComponentInParent<PlayerMovement>();

        // もしコライダーを付け忘れていたらエラーを出す（デバッグ用）
        if (noiseCollider == null)
        {
            Debug.LogError("SphereColliderが見つかりません！");
        }
    }

    void Update()
    {
        float speed = movement.CurrentMoveVelocity;

        // 【追加】コンソールに今の速度を表示する
        Debug.Log($"今の速度: {speed} / 半径: {noiseCollider.radius}");

        if (speed > 7.0f) // 走り（runSpeedが10なら7以上くらい）
            noiseCollider.radius = 10.0f;
        else if (speed > 0.1f) // 歩き
            noiseCollider.radius = 5.0f;
        else // 静止
            noiseCollider.radius = 2.0f;

    }

    // 着地した時に一時的に範囲を広げるメソッド（着地イベントから呼ぶ）
    public void OnLand()
    {
        StopAllCoroutines(); // 前の着地演出があれば止める
        StartCoroutine(TemporaryExpand(landingRadius, 0.5f));
    }

    private System.Collections.IEnumerator TemporaryExpand(float targetRadius, float duration)
    {
        noiseCollider.radius = targetRadius;
        yield return new WaitForSeconds(duration);
        // その後、Update()の処理で自動的に元のサイズに戻る
    }

    void OnDrawGizmos()
    {
        if (noiseCollider == null) noiseCollider = GetComponent<SphereCollider>();

        // 半透明の赤い色をつける
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        // コライダーと同じ位置、同じサイズで球を描く
        Gizmos.DrawSphere(transform.position, noiseCollider.radius);
    }


}
