using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GuardianUpgradeManager : MonoBehaviour
{
    public GuardianStatus[] GuardianStatuses;  // GuardianStatuses 배열: Guardian의 다양한 상태를 저장하는 배열

    public Image AttackRangeImg; // AttackRangeImg : Guardian의 공격 범위를 표시하는 이미지
    public Button UpgradeIconButton;  // UpgradeIconButton: Guardian을 업그레이드하는 버튼

    private Guardian _currentUpgradeGuardian; // 현재 업그레이드 중인 Guardian을 저장하는 변수

    public bool bIsUpgrading = false;  // 업그레이드 중인지 여부를 나타내는 변수
    private bool _isOnButtonHover = false; // 버튼 위에 마우스가 올라가 있는지 여부를 나타내는 변수


     public void Start() // 게임 시작 시 호출되는 함수
    {
        ShowUpgradeIconAndRange(false); // 업그레이드 아이콘과 범위를 숨김
        GameManager.Inst.guardianBuildManager.OnBuild.AddListener(() => ShowUpgradeIconAndRange(false)); // Guardian 건설 매니저에서 건설 완료 시, 업그레이드 아이콘과 범위를 숨기는 함수를 호출하도록 이벤트 설정
    }

    private void Update() // 매 프레임마다 호출되는 함수
    {
        UpdateKeyInput();  // 키 입력 업데이트 함수 호출
    }

    public void UpgradeGuardian(Guardian guardian) // Guardian을 업그레이드하는 함수
    {
        ShowUpgradeIconAndRange(true);// 업그레이드 아이콘과 범위를 보이도록 설정
        _currentUpgradeGuardian = guardian; // 현재 업그레이드 중인 Guardian 설정

        Vector3 guardianPos = _currentUpgradeGuardian.transform.position; // Guardian의 위치를 화면 좌표로 변환하여 공격 범위 이미지의 위치로 설정 
        Vector3 attackImgPos = Camera.main.WorldToScreenPoint(guardianPos);// 현재 업그레이드 중인 Guardian 설정

        // 공격 범위 설정 3줄 
        float attackRadius = (_currentUpgradeGuardian.GuardianStatus.AttackRadius) + 1.5f;
        AttackRangeImg.rectTransform.localScale = new Vector3(attackRadius, attackRadius, 1);
        AttackRangeImg.rectTransform.position = attackImgPos;


        // 업그레이드 아이콘 버튼의 크기 설정
        UpgradeIconButton.transform.localScale = new Vector3(1 / attackRadius, 1 / attackRadius, 1);

        // 업그레이드 버튼에 클릭 이벤트 추가
        UpgradeIconButton.onClick.AddListener(() => {
            Upgrade(_currentUpgradeGuardian);
            Debug.Log("C");
            });

        // 업그레이드 중인 상태로 설정
        bIsUpgrading = true;
    }

    public void ShowUpgradeIconAndRange(bool active) // 업그레이드 아이콘과 범위를 보이거나 숨기는 함수
    {
        AttackRangeImg.gameObject.SetActive(active);
        UpgradeIconButton.gameObject.SetActive(active);
    }

    private void Upgrade(Guardian guardian)// Guardian을 업그레이드하는 함수
    {
        // Guardian의 레벨이 최대 레벨보다 작을 때
        if (guardian.Level < GuardianStatuses.Length - 1)
        {
            // 플레이어 캐릭터 및 업그레이드 비용 설정
            PlayerCharacter player = GameManager.Inst.playerCharacter;
            int cost = GuardianStatuses[guardian.Level + 1].UpgradeCost;

            // 코인 사용 가능 여부 확인
            if (player.CanUseCoin(cost))
            {
                // 코인 사용 및 Guardian 업그레이드
                player.UseCoin(cost);
                guardian.Upgrade(GuardianStatuses[guardian.Level + 1]);
                Debug.Log("C");
                // 업그레이드 중인 상태 해제 및 아이콘/범위 숨김
                bIsUpgrading = false;
                ShowUpgradeIconAndRange(false);
            }
        }
    }

    public void OnPointerEnter()  // 마우스가 버튼 위에 올라가 있는지 여부를 나타내는 함수
    {
        _isOnButtonHover = true;
    }
    public void OnPointerExit() // 마우스가 버튼 위에서 벗어났는지 여부를 나타내는 함수
    {
        _isOnButtonHover = false;
    }

    private void UpdateKeyInput() // 키 입력 업데이트 함수
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼이 클릭되었을 때
        {
            if (_isOnButtonHover)  // 버튼 위에 마우스가 올라가 있을 경우 함수 종료
            {
                return;
            }


            // 업그레이드 중인 상태 해제 및 아이콘/범위 숨김
            bIsUpgrading = false;
            ShowUpgradeIconAndRange(false);
        }
    }
}