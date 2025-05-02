using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public Vector3 cutsceneCameraPosition;
    public float cutsceneZoom = 6f; // насколько камера приблизится к игроку
    public float cutsceneSmoothness = 0.5f;

    public float duration = 3f; // Сколько длится кат-сцена
    private bool triggered = false;

    private CameraController cam;

    void Start()
    {
        cam = Camera.main.GetComponent<CameraController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player")) return;

        triggered = true;

        // Запуск каст-сцены
        cam.PlayCutscene(cutsceneCameraPosition, cutsceneZoom, cutsceneSmoothness);
        Invoke(nameof(EndCutscene), duration);
    }

    void EndCutscene()
    {
        cam.EndCutscene();
    }
}
