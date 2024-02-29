using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GuardianUpgradeManager : MonoBehaviour
{
    public GuardianStatus[] GuardianStatuses;  // GuardianStatuses �迭: Guardian�� �پ��� ���¸� �����ϴ� �迭

    public Image AttackRangeImg; // AttackRangeImg : Guardian�� ���� ������ ǥ���ϴ� �̹���
    public Button UpgradeIconButton;  // UpgradeIconButton: Guardian�� ���׷��̵��ϴ� ��ư

    private Guardian _currentUpgradeGuardian; // ���� ���׷��̵� ���� Guardian�� �����ϴ� ����

    public bool bIsUpgrading = false;  // ���׷��̵� ������ ���θ� ��Ÿ���� ����
    private bool _isOnButtonHover = false; // ��ư ���� ���콺�� �ö� �ִ��� ���θ� ��Ÿ���� ����


     public void Start() // ���� ���� �� ȣ��Ǵ� �Լ�
    {
        ShowUpgradeIconAndRange(false); // ���׷��̵� �����ܰ� ������ ����
        GameManager.Inst.guardianBuildManager.OnBuild.AddListener(() => ShowUpgradeIconAndRange(false)); // Guardian �Ǽ� �Ŵ������� �Ǽ� �Ϸ� ��, ���׷��̵� �����ܰ� ������ ����� �Լ��� ȣ���ϵ��� �̺�Ʈ ����
    }

    private void Update() // �� �����Ӹ��� ȣ��Ǵ� �Լ�
    {
        UpdateKeyInput();  // Ű �Է� ������Ʈ �Լ� ȣ��
    }

    public void UpgradeGuardian(Guardian guardian) // Guardian�� ���׷��̵��ϴ� �Լ�
    {
        ShowUpgradeIconAndRange(true);// ���׷��̵� �����ܰ� ������ ���̵��� ����
        _currentUpgradeGuardian = guardian; // ���� ���׷��̵� ���� Guardian ����

        Vector3 guardianPos = _currentUpgradeGuardian.transform.position; // Guardian�� ��ġ�� ȭ�� ��ǥ�� ��ȯ�Ͽ� ���� ���� �̹����� ��ġ�� ���� 
        Vector3 attackImgPos = Camera.main.WorldToScreenPoint(guardianPos);// ���� ���׷��̵� ���� Guardian ����

        // ���� ���� ���� 3�� 
        float attackRadius = (_currentUpgradeGuardian.GuardianStatus.AttackRadius) + 1.5f;
        AttackRangeImg.rectTransform.localScale = new Vector3(attackRadius, attackRadius, 1);
        AttackRangeImg.rectTransform.position = attackImgPos;


        // ���׷��̵� ������ ��ư�� ũ�� ����
        UpgradeIconButton.transform.localScale = new Vector3(1 / attackRadius, 1 / attackRadius, 1);

        // ���׷��̵� ��ư�� Ŭ�� �̺�Ʈ �߰�
        UpgradeIconButton.onClick.AddListener(() => {
            Upgrade(_currentUpgradeGuardian);
            Debug.Log("C");
            });

        // ���׷��̵� ���� ���·� ����
        bIsUpgrading = true;
    }

    public void ShowUpgradeIconAndRange(bool active) // ���׷��̵� �����ܰ� ������ ���̰ų� ����� �Լ�
    {
        AttackRangeImg.gameObject.SetActive(active);
        UpgradeIconButton.gameObject.SetActive(active);
    }

    private void Upgrade(Guardian guardian)// Guardian�� ���׷��̵��ϴ� �Լ�
    {
        // Guardian�� ������ �ִ� �������� ���� ��
        if (guardian.Level < GuardianStatuses.Length - 1)
        {
            // �÷��̾� ĳ���� �� ���׷��̵� ��� ����
            PlayerCharacter player = GameManager.Inst.playerCharacter;
            int cost = GuardianStatuses[guardian.Level + 1].UpgradeCost;

            // ���� ��� ���� ���� Ȯ��
            if (player.CanUseCoin(cost))
            {
                // ���� ��� �� Guardian ���׷��̵�
                player.UseCoin(cost);
                guardian.Upgrade(GuardianStatuses[guardian.Level + 1]);
                Debug.Log("C");
                // ���׷��̵� ���� ���� ���� �� ������/���� ����
                bIsUpgrading = false;
                ShowUpgradeIconAndRange(false);
            }
        }
    }

    public void OnPointerEnter()  // ���콺�� ��ư ���� �ö� �ִ��� ���θ� ��Ÿ���� �Լ�
    {
        _isOnButtonHover = true;
    }
    public void OnPointerExit() // ���콺�� ��ư ������ ������� ���θ� ��Ÿ���� �Լ�
    {
        _isOnButtonHover = false;
    }

    private void UpdateKeyInput() // Ű �Է� ������Ʈ �Լ�
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư�� Ŭ���Ǿ��� ��
        {
            if (_isOnButtonHover)  // ��ư ���� ���콺�� �ö� ���� ��� �Լ� ����
            {
                return;
            }


            // ���׷��̵� ���� ���� ���� �� ������/���� ����
            bIsUpgrading = false;
            ShowUpgradeIconAndRange(false);
        }
    }
}