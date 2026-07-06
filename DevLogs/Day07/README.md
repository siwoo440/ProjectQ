\# Project Q 개발 기록 - 7일차



\## 1. 개발 목표



7일차의 목표는 전투 보상에서 새로운 카드를 얻거나 기존 카드를 강화할 수 있는 카드 보상 시스템을 구현하고, 전투에 등장하는 적 종류를 확장하여 전투 패턴을 다양하게 만드는 것이다.



기존에는 기본 카드 3종과 단순한 적 구조만 존재했지만, 이번 작업에서는 카드 보상, 카드 강화, 신규 카드 획득, 적 종류 분화, 적 이동 패턴 확장, 적끼리 충돌하지 않는 Layer 설정까지 함께 구현하였다.



\---



\## 2. 구현한 핵심 기능



| 구분 | 구현 내용 |

|---|---|

| 카드 보상 추가 | 전투 보상에서 새로운 카드를 획득할 수 있도록 구현 |

| 카드 강화 추가 | 특정 카드의 데미지, 쿨타임, 탄환 수를 강화할 수 있도록 구현 |

| 신규 카드 추가 | Rapid Shot, Heavy Shot 카드 타입 추가 |

| 중복 카드 처리 | 이미 보유한 카드를 다시 얻으면 카드 강화로 전환 |

| 적 종류 확장 | Chaser, Shooter, Charger 적 3종 구조 구현 |

| 적 이동 타입 추가 | 추적형, 거리 유지형, 돌진형 이동 방식 구현 |

| 적 접촉 피해 | 적과 플레이어가 닿으면 피해를 받도록 구현 |

| 적 탄환 개선 | 적 탄환이 플레이어 방향으로 이동하도록 수정 |

| 적 랜덤 스폰 | 전투 번호에 따라 해금된 적 프리팹 중 랜덤 생성 |

| Enemy 충돌 제거 | Enemy Layer를 만들고 Enemy끼리 충돌하지 않도록 Physics 2D 설정 |



\---



\## 3. 카드 보상 시스템



전투 클리어 후 등장하는 랜덤 보상에 카드 관련 보상을 추가하였다.



| 보상 이름 | 효과 |

|---|---|

| New Card: Rapid Shot | Rapid Shot 카드를 덱에 추가 |

| New Card: Heavy Shot | Heavy Shot 카드를 덱에 추가 |

| Upgrade: Pixel Shot | Pixel Shot Damage +3 |

| Upgrade: Focus Shot | Focus Shot Damage +10 |

| Upgrade: Wide Shot | Wide Shot Bullet Count +1 |

| Upgrade: Rapid Shot | Rapid Shot Cooldown -0.05s |

| Upgrade: Heavy Shot | Heavy Shot Damage +15 |



\---



\## 4. 신규 카드



| 카드 이름 | 특징 |

|---|---|

| Rapid Shot | 낮은 MP 비용과 짧은 쿨타임을 가진 빠른 단일 탄환 카드 |

| Heavy Shot | 높은 MP 비용과 긴 쿨타임을 가지지만 강한 피해를 주는 단일 탄환 카드 |



\---



\## 5. 카드 시스템 수정 내용



| 파일명 | 수정 내용 |

|---|---|

| CardType.cs | RapidShot, HeavyShot 타입 추가 |

| RewardType.cs | 신규 카드 획득 및 카드 강화 보상 타입 추가 |

| CardManager.cs | 새 카드 추가, 중복 카드 처리, 카드별 강화 함수 추가 |

| RewardManager.cs | 카드 보상 풀 추가 및 보상 적용 로직 확장 |



\---



\## 6. 적 종류 확장



전투에 등장하는 적을 3종으로 나누어 전투 패턴을 다양화하였다.



| 적 이름 | 이동 방식 | 특징 |

|---|---|---|

| Enemy\_Chaser | Chase | 플레이어를 계속 추적하는 기본 적 |

| Enemy\_Shooter | KeepDistance | 일정 거리를 유지하며 탄환을 발사하는 원거리 적 |

| Enemy\_Charger | Charge | 준비 시간 후 플레이어 방향으로 빠르게 돌진하는 적 |



\---



\## 7. 적 해금 구조



EnemySpawner에서 전투 번호에 따라 사용할 수 있는 적 종류가 늘어나도록 구현하였다.



| 전투 번호 | 등장 가능한 적 |

|---:|---|

| Battle 1 | Chaser |

| Battle 2 | Chaser, Shooter |

| Battle 3 이후 | Chaser, Shooter, Charger |



\---



\## 8. 적 관련 수정 내용



| 파일명 | 수정 내용 |

|---|---|

| EnemyMoveType.cs | Chase, KeepDistance, Charge 이동 타입 추가 |

| EnemyMovement.cs | 적 이동 타입별 이동 방식 구현 |

| EnemyContactDamage.cs | 적과 플레이어 접촉 시 피해 처리 |

| EnemyShooter.cs | 플레이어 방향 탄환 발사 및 산탄 발사 구조 추가 |

| EnemyBullet.cs | 방향, 속도, 피해량, 생존 시간을 받아 이동하도록 수정 |

| EnemySpawner.cs | 여러 적 프리팹 중 전투 번호에 따라 랜덤 생성하도록 수정 |



\---



\## 9. 추가한 적 프리팹



| 프리팹 이름 | 역할 |

|---|---|

| Enemy\_Chaser.prefab | 추적형 적 |

| Enemy\_Shooter.prefab | 원거리 공격 적 |

| Enemy\_Charger.prefab | 돌진형 적 |



\---



\## 10. Enemy 충돌 문제 해결



적끼리 서로 부딪혀 이동하지 못하는 문제가 발생하였다.



이를 해결하기 위해 Enemy 전용 Layer를 만들고, Physics 2D의 Layer Collision Matrix에서 Enemy와 Enemy 사이의 충돌을 해제하였다.



| 항목 | 설정 |

|---|---|

| 추가 Layer | Enemy |

| 적용 대상 | Enemy\_Chaser, Enemy\_Shooter, Enemy\_Charger |

| Physics 2D 설정 | Enemy ↔ Enemy 충돌 체크 해제 |

| 유지되는 충돌 | Enemy ↔ Player, Enemy ↔ PlayerBullet |



\---



\## 11. 현재 플레이 흐름



```text

전투 시작

→ 여러 카드 중 선택

→ 적 종류에 따라 다른 방식으로 회피

→ 적 처치

→ 전투 클리어

→ 랜덤 보상 표시

→ 새 카드 획득 또는 기존 카드 강화

→ 다음 전투 진행

→ 전투 번호에 따라 새로운 적 종류 등장

12\. 테스트 결과

테스트 항목	결과

Rapid Shot 획득	정상 작동

Heavy Shot 획득	정상 작동

중복 카드 강화 처리	정상 작동

Pixel Shot 강화	정상 작동

Focus Shot 강화	정상 작동

Wide Shot 탄환 수 증가	정상 작동

Chaser 추적	정상 작동

Shooter 거리 유지 및 탄환 발사	정상 작동

Charger 돌진	정상 작동

적 접촉 피해	정상 작동

적 탄환 피해	정상 작동

적 랜덤 스폰	정상 작동

Enemy끼리 충돌 제거	정상 작동

13\. 발생한 문제와 해결

문제	원인	해결

보유하지 않은 카드 강화 보상이 나올 수 있음	현재 보상 풀에서 보유 여부를 검사하지 않음	추후 보상 필터링 기능으로 개선 예정

새 카드가 숫자키 4, 5로 선택되지 않음	기존 입력이 1, 2, 3까지만 지원	Q, E, 마우스 휠로 선택 가능하며 추후 숫자키 확장 예정

Enemy끼리 서로 막힘	Enemy끼리 물리 충돌이 발생함	Enemy Layer 추가 후 Physics 2D에서 Enemy ↔ Enemy 충돌 해제

14\. 다음 개발 목표

우선순위	개발 내용

1	보유하지 않은 카드의 강화 보상이 나오지 않도록 보상 필터링

2	새 카드 숫자키 4, 5 선택 지원

3	적 탄막 패턴 추가

4	적 종류별 색상 및 외형 구분

5	전투 난이도 밸런스 조정

