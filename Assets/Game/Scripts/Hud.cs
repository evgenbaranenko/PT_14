using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    private static Hud m_instance;
    public static Hud Instance { get { return m_instance; } }

    [SerializeField] private Text[] m_scoreValue;

    [SerializeField] private Text m_turnsValue;

    [SerializeField] private Slider m_musicSlider;

    [SerializeField] private Slider m_soundSlider;

    [SerializeField] private CanvasGroup m_LevelCompletedWindow;

    #region Raycaster
    /* - Raycaster - �� ����� ���������, ���� ��������, �� �������� ������, ��������� � -��
      �������� (������ �� ���������� ������, ��� ������� ����), � ������ ������������� ��'��� .
      � ��� ����������: Graphic Raycaster , Physics Raycaster �� Physics2D Raycaster , ��
      �������� �������� � ��'������ �������, ��������� ����������� (3D) �� ����������� 2D
      ������. �� ������� �������� ���� ��� �� �������� Physics2D Raycaster �� ������, ���
      �������� ��������� ������ �����, ����������� Physics2D �����������. �� ������
      ����������� �������� Graphic Raycaster . ���� ���� ���������, �� �������� �������
      ����������� ��������� �� ��䳿 ������ , �� ��� ����� �������.*/
    #endregion
    private GraphicRaycaster m_raycaster;

    private void Awake()
    {
        m_instance = this;

        m_raycaster = gameObject.GetComponent<GraphicRaycaster>();
    }

    //�����, ���� ������ ���� LevelCompletedWindow
    public void CountScore(int to)
    {
        ShowWindow(m_LevelCompletedWindow);
        /*��� ������� �������� ����� � ����� StartCoroutine ����������
          ��������� �� ����� � ��'�� ��������, ��� � ���� ��������.*/
        StartCoroutine(Count(to, 0.3f));
    }
    #region Corutina
    /* - �������� - �� �����, �� ������� ��'��� ���� IEnumerator,
      ���� ���� ���������� ��������� �� ��������� ����� ����� ��
      ������������ ������ ���� ��������� �����. �������� ��������� ����������� ��
      ��������� �������� ��� yield return <��'���-�����>.
       - �� �������� ������� ����, ���� ������������ ������ ����, ������ ������� �������� ����
      (����� ���������� ����� �������� to ). �� ������� ����� �������� �������� ����� ������
      ������� ���� (����������� ���������� delay ), ���� ���� ������� ����� ����� Controller ,
      �� ���� �� ���� ������ ����� �� ���� ��� (� ������ ������� ������������ ����). */
    #endregion
    private IEnumerator Count(int to, float delay)
    {
        m_raycaster.enabled = false;

        for (int i = 1; i <= to; i++)
        {
            yield return new WaitForSeconds(delay);

            Controller.Instance.Score.AddTurnBonus();
        }

        m_raycaster.enabled = true;
    }

    // ������� �� ��������� �����
    public void Next()
    {
        Controller.Instance.InitializeLevel();
    }
    public void UpdateOptions()
    {
        m_musicSlider.value = Controller.Instance.Audio.MusicVolume;

        m_soundSlider.value = Controller.Instance.Audio.SfxVolume;
    }

    public void UpdateTurnsValue(int value)
    {
        m_turnsValue.text = value.ToString(); Debug.Log(value);
    }


    public void UpdateScoreValue(int value)
    {
        for (int i = 0; i < m_scoreValue.Length; i++)
        {
            m_scoreValue[i].text = value.ToString();
        }

    }

    public void ShowWindow(CanvasGroup window)
    {
        window.alpha = 1f;

        window.blocksRaycasts = true;

        window.interactable = true;
    }
    public void HideWindow(CanvasGroup window)
    {
        window.alpha = 0f;

        window.blocksRaycasts = false;

        window.interactable = false;
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Reset()
    {
        Controller.Instance.Reset();
    }
    //������, �� ������ �������� �������� ����� �� ������:
    public void SetMusicVolume(float volume)
    {
        Controller.Instance.Audio.MusicVolume = volume;
    }
    public void SetSoundVolume(float volume)
    {
        Controller.Instance.Audio.SfxVolume = volume;
    }
    private void Start()
    {
        //HideWindow();
    }
}