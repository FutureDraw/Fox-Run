using System.Collections;
using Unity.Properties;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public bool affectCamera = true;
    public Vector3 cameraLockPosition;
    public float cameraZoom = 5f;
    public DialogueData dialogue;
    public GameObject Canvas;
    private CameraController _cameraController;
    private float _originalZoom;
    private bool hasDialoguePlayed = false;


    void Start()
    {
        _cameraController = Camera.main.GetComponent<CameraController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        StartCoroutine(PlayCutscene());
        Canvas.SetActive(true);

    }

    IEnumerator PlayCutscene()
    {
        // ������
        _originalZoom = _cameraController.targetZoom;
        _cameraController.SetCameraLock(cameraLockPosition);
        _cameraController.SetZoom(cameraZoom);

        // ������
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.StartDialogue(dialogue);

        yield return new WaitUntil(() => !dialogueManager.IsDialogueActive);  // ��� ���������� �������

        // ����������� ������
        _cameraController.ReleaseCameraLock();
        _cameraController.SetZoom(_originalZoom);
    }
}
