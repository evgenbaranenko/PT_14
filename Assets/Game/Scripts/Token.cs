using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Token : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Camera m_camera;

    private Vector3 m_pointerPositionBeforeDrag;

    private Vector3 m_positionBeforeDrag;

    private int[] m_dragSpace;

    private int m_tokenType;

    // ����������� ���, ���� ���������� ������ �� ��'���
    // � � ���������� ������� �������� ������ ����

    // ��'��� PointerEventData ������ ������ �������
    // ���������� ��� ��, �� ����������,
    public void OnBeginDrag(PointerEventData eventData)
    {
        GetDragSpace();
        //Գ����� ��������� ��������� ����� �� ��������
        //(������ �� ������)
        m_pointerPositionBeforeDrag = m_camera.ScreenToWorldPoint(eventData.position);
        m_positionBeforeDrag = transform.position;
    }

    #region OnDrag(Explanation)
    // � ����� OnDrag(), ���� ����������� ������, ���� ����������� ��������� �������� 
    // (����� ���, ���� ���������� ������� ������ �� ������), ������ ���, ���� �������� ����� �� �������:
    #endregion
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mouseWorldPosition = m_camera.ScreenToWorldPoint(eventData.position);

        //�������� ������� ������� (������) ���� �����, ����� ������� ����:
        Vector3 totalDrag = mouseWorldPosition - m_pointerPositionBeforeDrag;

        //���������, ������� ����� �� ���������� ��� �� ��������:
        if (Mathf.Abs(totalDrag.x) > Mathf.Abs(totalDrag.y))
        {
            //�������� ���������� ���� �������� �������� �������� ����
            float posX = Mathf.Clamp(mouseWorldPosition.x, m_positionBeforeDrag.x -
            m_dragSpace[1], m_positionBeforeDrag.x + m_dragSpace[0]);

            //��������� �����
            transform.position = new Vector3(posX, m_positionBeforeDrag.y,
            transform.position.z);
        }
        else
        {
            //�������� ���������� ���� �������� �������� �������� ����
            float posY = Mathf.Clamp(mouseWorldPosition.y, m_positionBeforeDrag.y -
            m_dragSpace[3], m_positionBeforeDrag.y + m_dragSpace[2]);

            //��������� �����
            transform.position = new Vector3(m_positionBeforeDrag.x, posY,
            transform.position.z);
        }
        // �������, �� Mathf.Clamp ������ �������� � ������� �����:
        // Mathf.Clamp(�����, �������� ��������, ����������� ��������).����� �� ������ ������
        // ����� �����, �� ��������� ������� �� ��� ����, � ���� �������� ���� ������.
    }


    // �����, ���� ������������ ����� �� ����:
    private void AlignOnGrid()
    {
        Vector3 alignedPosition = transform.position;
        alignedPosition.x = Mathf.Round(transform.position.x);
        alignedPosition.y = Mathf.Round(transform.position.y);
        transform.position = alignedPosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        AlignOnGrid();
        Controller.Instance.TurnDone();
    }

    #region GetDragSpace(Explanation)
    // �� �������������� ���������� �����, ��������� ���������� ��� ��, ������ ����� ��
    // �� ������ � ������� � �������� ��������.���������� �������������� � �������������
    // ����� � �������� ��������: [0] � ��������, [1] � ����, [2] � ������, [3] � �����.������
    // �������, 0 � 2 � �������� �������� X �� Y, 1 �� 3 � �������� ��������.
    // �����, ���� ������������� ����� private int[] m_dragSpace ������� �� ��������� ����� �� ���
    // ��� �� ���������� �����, ���� ���� ��������� �������� ������� �������� ������� �����
    // �� ���� �� �� ����� ��������� ����� �����.����� �� ������ ������ ����, ������� ����
    // ������� (���� �����), � ����� ����� �����.
    // ����� �����, � ������ ���� ����� �� ���� ���� ����� ������ �����, �� �������� field � 2.
    #endregion
    private void GetDragSpace()
    {
        #region ��������  ������� ���� � ������/�������� ��������� ����� 

        int OddEven = 1; // ����� ���� 

        if (Controller.Instance.Level.FieldSize % 2 != 0)
        {
            OddEven = 0; // ������� ���� 
        }
        #endregion

        m_dragSpace = new int[] { 0, 0, 0, 0 };
        int halfField = (Controller.Instance.FieldSize - 1) / 2;

        // ���, ���� ������ ����� ������ �����
        // �������� �� ����� (����� �������� ������)
        // ��� ���������� ������� ������ ����� ������ �� �����
        m_dragSpace[0] = CheckSpace(Vector2.right);

        if (m_dragSpace[0] == -1)
        {
            m_dragSpace[0] = halfField - (int)transform.position.x + OddEven;
        }

        // ��� ���������� ������� ������ ����� ���� �� �����
        m_dragSpace[1] = CheckSpace(Vector2.left);

        if (m_dragSpace[1] == -1)
        {
            m_dragSpace[1] = halfField + (int)transform.position.x;
        }

        // ��� ��� ���������� Y
        m_dragSpace[2] = CheckSpace(Vector2.up);

        if (m_dragSpace[2] == -1)
        {
            m_dragSpace[2] = halfField - (int)transform.position.y + OddEven;
        }

        m_dragSpace[3] = CheckSpace(Vector2.down);

        if (m_dragSpace[3] == -1)
        {
            m_dragSpace[3] = halfField + (int)transform.position.y;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main;
        AlignOnGrid();

        m_tokenType = UnityEngine.Random.Range(0, Controller.Instance.TokenTypes);

        #region myMaterial
        // ��� �� ��������� �� ��'��� ��������, ����� �������� �������� ������ �� ��������
        // ��'����, � ������ ���� � ��� "_Color" �������� ����� ������ � ������ ������� ����������.

        Material myMaterial = gameObject.GetComponent<Renderer>().material;
        myMaterial.SetColor("_Color", Controller.Instance.TokenColors[m_tokenType]);
        #endregion

        //��� ����, ��� ����� ��� �������� �������� ���� � �����
        Controller.Instance.TokensByTypes[m_tokenType].Add(this);
    }

    #region CheckSpace(Explanation)
    // ��� ����� ������� ������ �� ������ ������ � �������� �������� 
    // (�������� �������� � ������ ���������).
    // ��� ������� ���� RaycastAll, ������� ��������� Raycast ������
    // ����������� � ������ ���������� ��������� ���� �����. �� ���������� �� ������
    // �������� ���������, ���� �� �������� ������, �� �� �������� ���� �����. ���� �����
    // ���������, ����� ������� ��������� � ����� �������(��� �������� ������ ������ ��
    // ������ ����� �� ����) �������� ������ �� ���������.
    // ����� �� �������� ������, ���������, ��������. ������ ������ ������� �������� ����
    // �����, ��� �� ��������� ������� ����������. ���� ������ ������� ����-���� ���������
    // ��������, ���������, ����� ���� ������� �� �����, �� ���������, �� ������� �� ������
    // ����� �� ����� ��������� ���� ������� 1,5. ���������� � ����� �������(Mathf.Floor),
    // �������� 1 � ���� ������ �� ��������, �������.
    // ���� ������ �� ������ ����� ��������, ����� ������� -1.
    #endregion
    private int CheckSpace(Vector2 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject != gameObject)
            {
                return Mathf.FloorToInt(hits[i].distance);
            }
        }
        return -1;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
