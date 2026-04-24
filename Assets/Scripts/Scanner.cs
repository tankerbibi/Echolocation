using UnityEngine;
using UnityEngine.InputSystem; // Input Systemを使うために必要

public class Scanner : MonoBehaviour
{
    public GameObject TerrainScannerPrefab;
    public float duration = 10f;
    public float size = 500f;

    // スペースキーなどの入力を受け取るための設定
    public InputActionProperty scanAction;

    void Update()
    {
        // 指定したボタン（スペースキーなど）が押された瞬間を判定
        if (scanAction.action.WasPressedThisFrame())
        {
            SpawnTerrainScanner();
        }
    }

    void SpawnTerrainScanner()
    {
        if (TerrainScannerPrefab == null)
        {
            Debug.LogWarning("Scanner Prefabがセットされていません。");
            return;
        }

        // プレイヤーの足元（現在地）にエフェクトを生成
        GameObject terrainScanner = Instantiate(TerrainScannerPrefab, transform.position, Quaternion.identity);

        // 子オブジェクト（一番目）からParticleSystemを取得
        if (terrainScanner.transform.childCount > 0)
        {
            ParticleSystem terrainScannerPS = terrainScanner.transform.GetChild(0).GetComponent<ParticleSystem>();

            if (terrainScannerPS != null)
            {
                var main = terrainScannerPS.main;
                main.startLifetime = duration;
                main.startSize = size;
            }
            else
            {
                Debug.LogWarning("子オブジェクトにParticle Systemが見当たりません。");
            }
        }

        // 指定時間（duration + 1秒）後に自動で削除
        Destroy(terrainScanner, duration + 1f);
    }
}