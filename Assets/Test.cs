using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public GameObject agentObject;
    public NavMeshAgent agent;
    public Transform target;
    public Image img;
    public Vector3[] positions;
    bool effecting = false;
    bool isFullScreen = true;
    // Start is called before the first frame update
    void Start()
    {
        agent = agentObject.GetComponent<NavMeshAgent>();
        agent.transform.position = positions[Random.Range(0, positions.Length)];
        agent.enabled = true;
        agent.SetDestination(target.position);

        // 마우스 커서 숨기기
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // agent가 target에 근접하면 agent를 멈춘다.
        if (Vector3.Distance(agentObject.transform.position, target.position) <= 3 && !effecting)
        {
            Debug.Log("도착!!!!!");
            StartCoroutine("MoveRandomPlace");
        }

        // ESC를 눌렀을 때, 에디터이면 에디터를 종료하고, 빌드된 게임이면 게임을 종료한다.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        // 전체 화면 해제
        if (Input.GetKeyDown(KeyCode.F11))
        {
            isFullScreen = !isFullScreen;
        }

        if (isFullScreen)
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        else
            Screen.fullScreen = false;
    }

    // 코루틴을 이용해서 페이드 아웃 효과 구현
    IEnumerator MoveRandomPlace()
    {
        effecting = true;
        agent.isStopped = true;
        agent.enabled = false;
        // 페이드 아웃 효과
        for (float i = 0; i <= 1; i += 0.01f)
        {
            img.color = new Color(1, 1, 1, i);
            yield return new WaitForSeconds(0.01f);
        }

        transform.position = positions[Random.Range(0, positions.Length)];

        yield return new WaitForSeconds(3f);

        // 페이드 인 효과
        for (float i = 1; i >= 0; i -= 0.01f)
        {
            img.color = new Color(1, 1, 1, i);
            yield return new WaitForSeconds(0.01f);
        }
        agent.enabled = true;
        agent.SetDestination(target.position);
        agent.isStopped = false;
        effecting = false;
    }
}

