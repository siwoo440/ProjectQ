\# Project Q 개발 기록 - 8일차



\## 1. 개발 목표



8일차의 목표는 적 공격 패턴을 연사, 산탄, 원형 탄막으로 확장하고, 전투 UI와 카메라 추적 기능을 개선하여 1차 프로토타입 형태를 정리하는 것이다.



기존에는 적이 단일 탄환 중심으로 공격했지만, 이번 작업에서는 적마다 다른 탄막 패턴을 사용할 수 있도록 구조를 확장하였다. 또한 현재 Battle 번호와 남은 적 수를 UI에 표시하고, 카메라가 플레이어를 중심으로 부드럽게 따라가도록 수정하였다.



\---



\## 2. 구현한 핵심 기능



| 구분 | 구현 내용 |

|---|---|

| 적 공격 패턴 추가 | Single, Burst, Spread, Circle 패턴 추가 |

| 연사 패턴 | 짧은 간격으로 여러 발의 탄환 발사 |

| 산탄 패턴 | 플레이어 방향 기준 부채꼴 탄환 발사 |

| 원형 탄막 | 적 주변 360도 방향으로 탄환 발사 |

| 전투 UI 개선 | 현재 Battle 번호와 남은 적 수 표시 |

| 적 수 갱신 | 적이 죽을 때마다 Enemies 수 감소 |

| 프로토타입 클리어 | 마지막 Battle 이후 PROTOTYPE CLEAR 표시 |

| 카메라 추적 | Main Camera가 플레이어를 중심으로 부드럽게 따라가도록 구현 |

| 카메라 흔들림 수정 | 이동 방향 보정 기능을 제거하고 단순 중심 추적 방식으로 수정 |



\---



\## 3. 추가한 스크립트



| 파일명 | 위치 | 역할 |

|---|---|---|

| EnemyAttackPattern.cs | Assets/Scripts/Enemy | 적 공격 패턴 enum 관리 |

| CameraFollow2D.cs | Assets/Scripts/Camera | 플레이어 중심 카메라 추적 처리 |



\---



\## 4. 수정한 스크립트



| 파일명 | 수정 내용 |

|---|---|

| EnemyShooter.cs | 단일, 연사, 산탄, 원형 탄막 패턴 처리 추가 |

| BattleInfoUI.cs | Battle 번호와 남은 적 수를 함께 표시하도록 수정 |

| BattleManager.cs | 적 등록/사망 시 전투 정보 UI 갱신 |

| BattleProgressManager.cs | 최대 전투 수와 PROTOTYPE CLEAR 처리 추가 |



\---



\## 5. 적 공격 패턴



| 패턴 | 설명 |

|---|---|

| Single | 플레이어 방향으로 탄환 1발 발사 |

| Burst | 짧은 간격으로 여러 발을 연속 발사 |

| Spread | 플레이어 방향 기준으로 여러 발을 부채꼴로 발사 |

| Circle | 적 중심에서 모든 방향으로 탄환 발사 |



\---



\## 6. 추가한 적 프리팹



| 프리팹 이름 | 공격 패턴 | 역할 |

|---|---|---|

| Enemy\_BurstShooter.prefab | Burst | 연사형 원거리 적 |

| Enemy\_SpreadShooter.prefab | Spread | 산탄형 원거리 적 |

| Enemy\_CircleShooter.prefab | Circle | 원형 탄막 적 |



\---



\## 7. 전투 진행 구조



EnemySpawner의 Enemy Prefabs 배열 순서에 따라 전투 번호별로 새로운 적이 해금되도록 구성하였다.



| 전투 번호 | 등장 가능한 적 |

|---:|---|

| Battle 1 | Chaser |

| Battle 2 | Chaser, Shooter |

| Battle 3 | Chaser, Shooter, Charger |

| Battle 4 | Chaser, Shooter, Charger, BurstShooter |

| Battle 5 | Chaser, Shooter, Charger, BurstShooter, SpreadShooter |

| Battle 6 | Chaser, Shooter, Charger, BurstShooter, SpreadShooter, CircleShooter |



\---



\## 8. 전투 UI 개선



기존에는 Battle 번호만 표시했지만, 이번 작업에서 남은 적 수도 함께 표시하도록 수정하였다.



```text

Battle 1

Enemies: 3



적이 죽을 때마다 Enemies 값이 감소한다.



Battle 1

Enemies: 2

9\. 카메라 추적 기능



Main Camera에 CameraFollow2D를 추가하여 플레이어를 중심으로 따라가도록 구현하였다.



처음에는 플레이어 이동 방향으로 살짝 앞을 보는 Look Ahead 방식을 적용했지만, 이동 중 화면이 떠는 문제가 발생하여 단순한 플레이어 중심 추적 방식으로 수정하였다.



항목	설정

Target	Player

Smooth Time	0.10 \~ 0.12

Offset	X 0, Y 0, Z -10

Follow 방식	플레이어 중심 부드러운 추적

Look Ahead	제거

회피 방향 보정	제거

10\. 현재 플레이 흐름

게임 시작

→ Battle 1 시작

→ 카드 선택 및 공격

→ 적 패턴 회피

→ 적 처치

→ 남은 적 수 UI 갱신

→ 전투 클리어

→ 랜덤 보상 선택

→ 다음 전투 진행

→ Battle 6 이후 PROTOTYPE CLEAR 표시

11\. 테스트 결과

테스트 항목	결과

Single 탄환 발사	정상 작동

Burst 연사 패턴	정상 작동

Spread 산탄 패턴	정상 작동

Circle 원형 탄막	정상 작동

전투 번호 UI 표시	정상 작동

남은 적 수 표시	정상 작동

적 처치 시 UI 갱신	정상 작동

Battle 6 진행	정상 작동

PROTOTYPE CLEAR 표시	정상 작동

카메라 플레이어 추적	정상 작동

카메라 흔들림 수정	정상 반영

12\. 발생한 문제와 해결

문제	원인	해결

카메라가 플레이어 이동 중 떨림	이동 방향 Look Ahead 보정이 Rigidbody 이동과 맞물려 흔들림 발생	Look Ahead 기능 제거 후 플레이어 중심 추적 방식으로 수정

Text\_BattleNumber가 두 줄을 표시하기에 작음	Battle과 Enemies를 함께 표시하면서 높이 부족	Text\_BattleNumber Height를 늘리고 Font Size 조정

원형 탄막 난이도가 높을 수 있음	Circle Bullet Count와 Bullet Speed가 높음	필요 시 Circle Bullet Count, Bullet Speed, Damage 감소 예정

13\. 다음 개발 목표

우선순위	개발 내용

1	적 패턴별 색상과 외형 구분

2	카드 보상 필터링 개선

3	보유하지 않은 카드 강화 보상 제외

4	전투 난이도 밸런스 조정

5	1차 프로토타입 플레이 테스트 및 버그 수정

