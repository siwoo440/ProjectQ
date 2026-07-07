\# Project Q 개발 기록 - 14일차



\## 1. 개발 목표



14일차의 목표는 13일차에 추가한 신규 카드 3종이 유물 시스템과 자연스럽게 연결되도록 확장하고, 카드와 유물을 함께 보유했을 때 추가 효과가 발동되는 조합 시스템을 구현하는 것이다.



이번 작업을 통해 Pierce Shot, Bomb Shot, Homing Shot은 단순히 새로 추가된 카드가 아니라, 전용 유물과 조합 효과를 통해 하나의 빌드 방향을 형성할 수 있게 되었다.



\---



\## 2. 구현한 핵심 기능



| 구분 | 구현 내용 |

|---|---|

| 신규 카드 전용 유물 추가 | Pierce Shot, Bomb Shot, Homing Shot 전용 유물 추가 |

| 신규 유물 보상 추가 | 전투 보상에서 신규 유물 등장 가능 |

| 신규 유물 필터링 | 해당 카드를 보유한 경우에만 전용 유물 등장 |

| 신규 유물 중복 방지 | 이미 보유한 유물은 다시 등장하지 않도록 처리 |

| 신규 유물 효과 적용 | 카드별 데미지, 관통 수, 폭발 범위, 유도 성능 강화 |

| 카드-유물 조합 시스템 추가 | 특정 카드와 특정 유물을 함께 보유하면 추가 효과 발동 |

| 조합 중복 발동 방지 | 같은 조합 효과가 한 번만 발동되도록 처리 |

| 카드 상세 UI 반영 | 유물 및 조합 효과로 변경된 카드 수치 표시 |

| 신규 카드 밸런스 조정 | 신규 카드 기본 수치 1차 조정 |



\---



\## 3. 추가한 신규 유물



| 유물 이름 | 대상 카드 | 효과 | 등급 |

|---|---|---|---|

| Piercing Needle | Pierce Shot | 관통 수 +1 | Rare |

| Pierce Engine | Pierce Shot | 데미지 +6 | Rare |

| Blast Powder | Bomb Shot | 폭발 범위 +0.5 | Epic |

| Blast Core | Bomb Shot | 데미지 +12 | Rare |

| Smart Chip | Homing Shot | 유도 회전 속도 +1 | Rare |



\---



\## 4. 신규 유물 등장 조건



신규 카드 전용 유물은 해당 카드를 보유하고 있을 때만 등장하도록 설정하였다.



| 조건 | 등장 가능 유물 |

|---|---|

| Pierce Shot 보유 | Piercing Needle, Pierce Engine |

| Bomb Shot 보유 | Blast Powder, Blast Core |

| Homing Shot 보유 | Smart Chip |

| 해당 카드 미보유 | 해당 카드 전용 유물 등장 불가 |



이를 통해 플레이어가 아직 가지고 있지 않은 카드의 전용 유물이 먼저 등장하는 문제를 방지하였다.



\---



\## 5. 수정한 스크립트



| 파일명 | 위치 | 수정 내용 |

|---|---|---|

| RelicType.cs | Assets/Scripts/Relic | 신규 카드 전용 유물 타입 추가 |

| RewardType.cs | Assets/Scripts/Reward | 신규 카드 전용 유물 보상 타입 추가 |

| RelicManager.cs | Assets/Scripts/Managers | 신규 유물 효과 적용, 조합 효과 확인 및 발동 |

| RewardManager.cs | Assets/Scripts/Managers | 신규 유물 보상 등록, 보상 필터링 추가 |

| RewardButton.cs | Assets/Scripts/UI | 신규 유물을 Relic 보상으로 분류 |

| CardManager.cs | Assets/Scripts/Card | 신규 카드 밸런스 조정, 특수 수치 강화 함수 사용 |

| CardSlotUI.cs | Assets/Scripts/UI | 카드 상세 UI에서 특수 수치 변화 확인 |



\---



\## 6. RelicType 추가 내용



신규 유물을 관리하기 위해 RelicType에 다음 항목을 추가하였다.



| RelicType | 설명 |

|---|---|

| PiercingNeedle | Pierce Shot 관통 수 증가 |

| PierceEngine | Pierce Shot 데미지 증가 |

| BlastPowder | Bomb Shot 폭발 범위 증가 |

| BlastCore | Bomb Shot 데미지 증가 |

| SmartChip | Homing Shot 유도 회전 속도 증가 |



\---



\## 7. RewardType 추가 내용



신규 유물을 보상으로 등장시키기 위해 RewardType에 다음 항목을 추가하였다.



| RewardType | 설명 |

|---|---|

| RelicPiercingNeedle | Piercing Needle 유물 보상 |

| RelicPierceEngine | Pierce Engine 유물 보상 |

| RelicBlastPowder | Blast Powder 유물 보상 |

| RelicBlastCore | Blast Core 유물 보상 |

| RelicSmartChip | Smart Chip 유물 보상 |



\---



\## 8. RelicManager 수정 내용



RelicManager에서는 신규 유물 효과를 적용할 수 있도록 다음 기능을 추가하였다.



| 함수 | 역할 |

|---|---|

| ApplyPiercingNeedle | Pierce Shot 관통 수 증가 |

| ApplyPierceEngine | Pierce Shot 데미지 증가 |

| ApplyBlastPowder | Bomb Shot 폭발 범위 증가 |

| ApplyBlastCore | Bomb Shot 데미지 증가 |

| ApplySmartChip | Homing Shot 유도 회전 속도 증가 |



유물 획득 시 다음 흐름으로 효과가 적용된다.



```text

유물 획득

→ 중복 유물인지 확인

→ 보유 유물 목록에 추가

→ 유물 효과 적용

→ 조합 조건 확인

→ 유물 UI 갱신

```



\---



\## 9. RewardManager 수정 내용



RewardManager에는 신규 유물 보상을 보상 풀에 추가하였다.



\### 신규 유물 보상



| 보상 이름 | 효과 |

|---|---|

| Relic: Piercing Needle | Pierce Shot Pierce Count +1 |

| Relic: Pierce Engine | Pierce Shot Damage +6 |

| Relic: Blast Powder | Bomb Shot Explosion Radius +0.5 |

| Relic: Blast Core | Bomb Shot Damage +12 |

| Relic: Smart Chip | Homing Shot Turn Speed +1 |



또한 IsRewardAvailable 함수에 카드 보유 조건을 추가하였다.



```text

Pierce Shot을 가지고 있어야 Pierce Shot 전용 유물 등장

Bomb Shot을 가지고 있어야 Bomb Shot 전용 유물 등장

Homing Shot을 가지고 있어야 Homing Shot 전용 유물 등장

```



\---



\## 10. RewardButton 수정 내용



RewardButton의 보상 종류 분류에 신규 유물 보상을 추가하였다.



신규 유물 보상은 보상창에서 다음과 같이 표시된다.



```text

\[Rare] \[Relic]

Relic: Piercing Needle



Pierce Shot Pierce Count +1

```



```text

\[Epic] \[Relic]

Relic: Blast Powder



Bomb Shot Explosion Radius +0.5

```



이를 통해 신규 유물도 기존 유물과 동일하게 Relic 보상으로 분류된다.



\---



\## 11. 신규 카드 밸런스 조정



신규 카드 3종의 기본 수치를 1차 조정하였다.



| 카드 | MP | 쿨타임 | 데미지 | 탄속 | 지속시간 | 특수 수치 |

|---|---:|---:|---:|---:|---:|---|

| Pierce Shot | 2 | 1.7초 | 13 | 10.5 | 3초 | 관통 3 |

| Bomb Shot | 3 | 2.6초 | 16 | 7 | 3.5초 | 폭발 반경 2.0 |

| Homing Shot | 2 | 1.9초 | 9 | 8 | 4초 | 유도 범위 7, 회전 속도 4 |



\### 밸런스 의도



| 카드 | 의도 |

|---|---|

| Pierce Shot | 일렬로 몰려오는 적에게 강하지만 단일 대상에게는 과하지 않게 조정 |

| Bomb Shot | 적 무리에 강하지만 MP와 쿨타임 부담을 크게 설정 |

| Homing Shot | 맞추기 쉬운 대신 데미지를 낮게 설정 |



\---



\## 12. 유물 적용 후 성장 예상



\### Pierce Shot



| 상태 | 관통 수 | 데미지 |

|---|---:|---:|

| 기본 | 3 | 13 |

| Piercing Needle 획득 | 4 | 13 |

| Pierce Engine 획득 | 3 | 19 |

| 둘 다 획득 | 4 | 19 |



\### Bomb Shot



| 상태 | 폭발 반경 | 데미지 |

|---|---:|---:|

| 기본 | 2.0 | 16 |

| Blast Powder 획득 | 2.5 | 16 |

| Blast Core 획득 | 2.0 | 28 |

| 둘 다 획득 | 2.5 | 28 |



\### Homing Shot



| 상태 | 유도 회전 속도 | 데미지 |

|---|---:|---:|

| 기본 | 4 | 9 |

| Smart Chip 획득 | 5 | 9 |



\---



\## 13. 카드-유물 조합 시스템 추가



유물을 개별적으로 획득하는 것에서 끝나지 않고, 특정 카드와 특정 유물을 함께 보유했을 때 추가 효과가 발동되도록 조합 시스템을 추가하였다.



조합 시스템을 위해 RelicComboType을 추가하였다.



| RelicComboType | 설명 |

|---|---|

| PiercingArsenal | Pierce Shot 관통 빌드 조합 |

| VolatileReactor | Bomb Shot 폭발 빌드 조합 |

| SmartGuidance | Homing Shot 유도 빌드 조합 |



\---



\## 14. 조합 발동 목록 관리



RelicManager에는 이미 발동한 조합을 저장하는 activatedCombos 목록을 추가하였다.



이 목록을 통해 같은 조합 효과가 여러 번 발동되는 것을 방지하였다.



```text

조합 조건 만족

→ 이미 발동한 조합인지 확인

→ 발동하지 않은 조합이면 효과 적용

→ activatedCombos에 기록

→ 이후 같은 조합은 다시 발동하지 않음

```



\---



\## 15. Piercing Arsenal 조합



\### 발동 조건



| 조건 |

|---|

| Pierce Shot 보유 |

| Piercing Needle 보유 |

| Pierce Engine 보유 |

| Piercing Arsenal 미발동 상태 |



\### 발동 효과



| 효과 |

|---|

| Pierce Shot 데미지 +4 |

| Pierce Shot 관통 수 +1 |



\### 의도



Pierce Shot 중심의 관통 빌드를 완성하는 조합이다.  

적이 일렬로 몰려오는 상황에서 더 강한 성능을 낼 수 있도록 설계하였다.



\---



\## 16. Volatile Reactor 조합



\### 발동 조건



| 조건 |

|---|

| Bomb Shot 보유 |

| Blast Powder 보유 |

| Blast Core 보유 |

| Volatile Reactor 미발동 상태 |



\### 발동 효과



| 효과 |

|---|

| Bomb Shot 폭발 반경 +0.3 |

| Bomb Shot 쿨타임 -0.2초 |



\### 의도



Bomb Shot 중심의 폭발 빌드를 완성하는 조합이다.  

적 무리를 처리하는 능력을 강화하되, 기본 쿨타임이 길기 때문에 조합 완성 시 사용감을 조금 개선하도록 하였다.



\---



\## 17. Smart Guidance 조합



\### 발동 조건



| 조건 |

|---|

| Homing Shot 보유 |

| Smart Chip 보유 |

| Bullet Engine 보유 |

| Smart Guidance 미발동 상태 |



\### 발동 효과



| 효과 |

|---|

| Homing Shot 데미지 +4 |

| Homing Shot 유도 회전 속도 +0.7 |



\### 의도



Homing Shot 중심의 유도 빌드를 완성하는 조합이다.  

Smart Chip과 Bullet Engine을 함께 보유했을 때 유도 탄환의 성능이 더욱 강화되도록 구성하였다.



\---



\## 18. 조합 발동 흐름



조합 효과는 유물을 획득할 때마다 확인된다.



```text

유물 획득

→ AddRelic 실행

→ 유물 효과 적용

→ CheckRelicCombos 실행

→ 각 조합 조건 확인

→ 조건을 만족하면 조합 효과 발동

→ 발동된 조합은 activatedCombos에 저장

```



조합 발동 시 Console에 다음 로그가 출력된다.



```text

Combo Activated : Piercing Arsenal

Combo Activated : Volatile Reactor

Combo Activated : Smart Guidance

```



\---



\## 19. 카드 상세 UI 반영



유물 효과와 조합 효과는 CardManager의 카드 강화 함수를 통해 적용된다.



따라서 카드 상세 UI에서도 변경된 수치를 확인할 수 있다.



| 조합 | UI에서 확인할 수 있는 변화 |

|---|---|

| Piercing Arsenal | Pierce Shot Damage, Pierce Count 증가 |

| Volatile Reactor | Bomb Shot Cooldown 감소, Explosion Radius 증가 |

| Smart Guidance | Homing Shot Damage, Turn Speed 증가 |



\---



\## 20. 테스트 결과



| 테스트 항목 | 결과 |

|---|---|

| 신규 유물 타입 추가 | 정상 |

| 신규 유물 보상 타입 추가 | 정상 |

| 신규 유물 보상 등장 | 정상 |

| 신규 카드 미보유 시 전용 유물 제외 | 정상 |

| 신규 카드 보유 시 전용 유물 등장 | 정상 |

| 신규 유물 중복 방지 | 정상 |

| Piercing Needle 효과 적용 | 정상 |

| Pierce Engine 효과 적용 | 정상 |

| Blast Powder 효과 적용 | 정상 |

| Blast Core 효과 적용 | 정상 |

| Smart Chip 효과 적용 | 정상 |

| Piercing Arsenal 조합 발동 | 정상 |

| Volatile Reactor 조합 발동 | 정상 |

| Smart Guidance 조합 발동 | 정상 |

| 조합 효과 중복 발동 방지 | 정상 |

| 카드 상세 UI 수치 반영 | 정상 |

| 기존 카드 발사 유지 | 정상 |

| 기존 유물 효과 유지 | 정상 |

| 보상 선택 후 다음 전투 진행 | 정상 |



\---



\## 21. 발생한 문제와 해결



| 문제 | 원인 | 해결 |

|---|---|---|

| 신규 유물이 카드 없이 등장할 가능성 | 보상 필터링 조건 부족 | IsRewardAvailable에 cardManager.HasCard 조건 추가 |

| 신규 유물이 Stat으로 표시될 가능성 | RewardButton 분류에 신규 유물 누락 | GetRewardCategoryText에 신규 유물 case 추가 |

| 조합 효과가 여러 번 발동될 가능성 | 발동 여부 저장 구조 필요 | activatedCombos 목록 추가 |

| 카드 상세 UI 수치 미반영 가능성 | 카드 강화 함수에서 UI 갱신 누락 가능 | 강화 함수에서 UpdateCardUI 호출 확인 |

| 조합 조건 확인 누락 가능성 | 유물 획득 후 조합 검사 호출 필요 | AddRelic에서 CheckRelicCombos 호출 |



\---



\## 22. 현재 카드 빌드 방향



현재 카드와 유물을 통해 다음 빌드 방향이 생겼다.



| 빌드 | 핵심 카드 | 핵심 유물 | 특징 |

|---|---|---|---|

| 관통 빌드 | Pierce Shot | Piercing Needle, Pierce Engine | 일렬 적 처리에 강함 |

| 폭발 빌드 | Bomb Shot | Blast Powder, Blast Core | 적 무리 처리에 강함 |

| 유도 빌드 | Homing Shot | Smart Chip, Bullet Engine | 조준 부담이 적고 안정적임 |

| 기본 공격 빌드 | Pixel Shot | Pixel Lens, Blood Core | 균형 잡힌 기본 공격 강화 |

| 고화력 빌드 | Heavy Shot | Heavy Core | 강한 단일 대상 처리 |



\---



\## 23. 현재 플레이 흐름



14일차 이후 현재 플레이 흐름은 다음과 같다.



```text

게임 시작

→ 카드 선택

→ 전투 진행

→ 적 처치

→ 보상 선택

→ 신규 카드 획득

→ 신규 카드 전용 유물 획득

→ 카드 수치 강화

→ 특정 유물 조합 완성

→ 조합 효과 발동

→ 카드 상세 UI에서 변화 확인

→ 다음 전투 진행

```



\---



\## 24. 현재 프로젝트 상태



14일차까지 완료된 핵심 시스템은 다음과 같다.



| 시스템 | 상태 |

|---|---|

| 플레이어 이동 / 회피 | 완료 |

| HP / MP / Shield | 완료 |

| 기본 카드 발사 | 완료 |

| 다중 카드 선택 | 완료 |

| 카드 상세 UI | 완료 |

| 보상 등급 확률 | 완료 |

| 보상 UI 개선 | 완료 |

| 유물 시스템 | 완료 |

| 유물 UI 개선 | 완료 |

| 카드 전용 유물 | 완료 |

| 신규 카드 3종 | 완료 |

| 관통 탄환 | 완료 |

| 폭발 탄환 | 완료 |

| 유도 탄환 | 완료 |

| 신규 카드 전용 유물 | 완료 |

| 카드-유물 조합 효과 | 완료 |

| 조합 중복 발동 방지 | 완료 |

| 적 스폰 | 완료 |

| 적 종류 확장 | 완료 |

| 적 탄막 패턴 | 완료 |

| 전투 반복 구조 | 완료 |

| Game Over / Retry | 완료 |

| Prototype Clear | 완료 |



\---



\## 25. 다음 개발 목표



다음 개발에서는 기능을 더 추가하기보다, 현재 구현된 카드, 유물, 조합 효과가 전투 난이도와 잘 맞는지 확인하는 것이 좋다.



| 우선순위 | 다음 작업 |

|---:|---|

| 1 | 카드 / 유물 / 조합 효과 밸런스 조정 |

| 2 | 적 체력, 적 수, 적 탄막 빈도 조정 |

| 3 | Battle 1\~6 전체 플레이 테스트 |

| 4 | 너무 강한 보상 또는 유물 수치 조정 |

| 5 | 너무 약한 카드 상향 |

| 6 | 전투 결과 화면 개선 |

| 7 | 2차 프로토타입 정리 |



\---



\## 26. 14일차 정리



14일차 작업을 통해 신규 카드 3종은 단순한 추가 카드가 아니라, 전용 유물과 조합 효과를 통해 빌드를 형성하는 카드로 확장되었다.



이번 작업의 핵심 변화는 다음과 같다.



```text

기존:

카드 획득

유물 획득

각 효과가 개별적으로 적용



변경 후:

카드 획득

전용 유물 획득

특정 조합 완성

추가 조합 효과 발동

빌드 방향 형성

```



이제 플레이어는 단순히 강한 보상을 고르는 것이 아니라, 자신이 선택한 카드에 맞는 유물을 모아 특정 빌드를 완성하는 방향으로 성장할 수 있다.



이번 작업은 이후 전투 밸런스 조정, 보스전, 스테이지 진행 구조를 만들기 위한 중요한 기반이 된다.

