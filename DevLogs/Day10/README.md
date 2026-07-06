\# Project Q 개발 기록 - 10일차



\## 1. 개발 목표



10일차의 목표는 전투 보상 시스템에 유물 시스템을 추가하여, 플레이어가 전투를 반복하면서 카드뿐만 아니라 유물을 통해서도 성장할 수 있는 구조를 구현하는 것이다.



기존에는 전투 클리어 후 보상으로 카드 강화, 새 카드 획득, 플레이어 스탯 증가 정도만 선택할 수 있었다. 이번 작업에서는 여기에 유물 보상을 추가하여, 선택한 유물이 이후 전투에도 지속적으로 영향을 주도록 만들었다.



이번 10일차 작업의 핵심은 다음과 같다.



| 구분 | 내용 |

|---|---|

| 주요 목표 | 유물 시스템 기본 구조 구현 |

| 핵심 기능 | 유물 데이터, 유물 획득, 유물 효과 적용, 보유 유물 UI 표시 |

| 적용 위치 | 전투 클리어 후 랜덤 보상 |

| 결과 | 카드 보상, 스탯 보상, 유물 보상이 함께 등장 |

| 기대 효과 | 로그라이크식 빌드 성장 구조 강화 |



\---



\## 2. 작업 전 상태



10일차 작업 전까지 프로젝트 Q는 아래 흐름을 가지고 있었다.



```text

전투 시작

→ 카드 선택

→ 적 처치

→ 전투 클리어

→ 랜덤 보상 선택

→ 카드 획득 / 카드 강화 / 스탯 강화

→ 다음 전투 진행

```



하지만 이 구조에서는 플레이어 성장 방식이 주로 카드 성능 증가에 집중되어 있었다.



기존 보상 예시는 다음과 같다.



| 보상 종류 | 예시 |

|---|---|

| 카드 전체 강화 | All Card Damage +5 |

| 카드 쿨타임 감소 | All Card Cooldown -0.1s |

| 스탯 강화 | Max MP +1, Max HP +20 |

| 새 카드 획득 | New Card: Rapid Shot |

| 특정 카드 강화 | Upgrade: Wide Shot Bullet Count +1 |



이번 작업에서는 여기에 유물 보상을 추가하였다.



```text

전투 클리어

→ 랜덤 보상 등장

→ Relic 보상 선택

→ 유물 획득

→ 유물 효과가 이후 전투에 계속 적용

```



\---



\## 3. 구현한 핵심 기능



| 구분 | 구현 내용 |

|---|---|

| 유물 타입 추가 | RelicType enum을 통해 유물 종류 구분 |

| 유물 데이터 추가 | RelicData 클래스로 유물 이름, 설명, 타입, 수치 관리 |

| 유물 관리자 추가 | RelicManager가 보유 유물 목록과 효과 적용을 관리 |

| 유물 UI 추가 | RelicSlotUI로 현재 보유 유물을 화면에 표시 |

| 유물 보상 추가 | RewardManager의 보상 풀에 Relic 보상 추가 |

| 유물 효과 적용 | 카드 데미지, MP 회복, 보호막, 쿨타임, 탄환 속도에 유물 효과 적용 |

| 유물 중복 방지 | 이미 보유한 유물이 다시 보상에 나오지 않도록 필터링 |

| 전투 시작 효과 | Shield Fragment처럼 전투 시작 시 적용되는 유물 효과 처리 |



\---



\## 4. 추가한 스크립트



| 파일명 | 위치 | 역할 |

|---|---|---|

| RelicType.cs | Assets/Scripts/Relic | 유물 종류 enum 관리 |

| RelicData.cs | Assets/Scripts/Relic | 유물 하나의 데이터 저장 |

| RelicManager.cs | Assets/Scripts/Managers | 보유 유물 관리 및 유물 효과 적용 |

| RelicSlotUI.cs | Assets/Scripts/UI | 화면에 보유 유물 목록 표시 |



\---



\## 5. 수정한 스크립트



| 파일명 | 위치 | 수정 내용 |

|---|---|---|

| RewardType.cs | Assets/Scripts/Reward | 유물 보상 타입 추가 |

| RewardManager.cs | Assets/Scripts/Managers | 유물 보상을 보상 풀에 추가하고 선택 시 RelicManager로 전달 |

| BattleProgressManager.cs | Assets/Scripts/Managers | 전투 시작 시 유물 효과가 적용되도록 RelicManager 연결 |

| GameScene.unity | Assets/Scenes | RelicManager 오브젝트와 Text\_RelicSlots UI 추가 |



\---



\## 6. 유물 시스템 구조



이번 작업에서 유물 시스템은 아래 구조로 구성하였다.



```text

RewardManager

→ 유물 보상 표시

→ 플레이어가 유물 선택

→ RelicManager.AddRelic()

→ 보유 유물 목록에 추가

→ 유물 효과 적용

→ RelicSlotUI 갱신

```



유물 데이터는 `RelicData`로 관리한다.



| 데이터 | 설명 | 예시 |

|---|---|---|

| relicName | 유물 이름 | Blood Core |

| description | 유물 설명 | All Card Damage +2 |

| relicType | 유물 종류 | BloodCore |

| value | 효과 수치 | 2 |



\---



\## 7. 추가한 유물 목록



이번 10일차에서는 기본 유물 5종을 추가하였다.



| 유물 이름 | 타입 | 효과 | 적용 시점 |

|---|---|---|---|

| Blood Core | BloodCore | 모든 카드 데미지 +2 | 획득 즉시 |

| Mana Stone | ManaStone | MP 회복 속도 +0.3 | 획득 즉시 |

| Shield Fragment | ShieldFragment | 전투 시작 시 Shield +1 | 전투 시작 시 |

| Quick Gear | QuickGear | 모든 카드 쿨타임 -0.05초 | 획득 즉시 |

| Bullet Engine | BulletEngine | 모든 카드 탄환 속도 +0.5 | 획득 즉시 |



\---



\## 8. 유물별 상세 설명



\### 8.1 Blood Core



| 항목 | 내용 |

|---|---|

| 효과 | 모든 카드 데미지 +2 |

| 적용 대상 | CardManager |

| 적용 함수 | IncreaseBulletDamage |

| 적용 시점 | 유물 획득 즉시 |



Blood Core는 모든 카드의 추가 데미지를 증가시키는 유물이다.  

카드별 기본 데미지를 직접 수정하는 방식이 아니라, CardManager의 보너스 데미지 수치를 증가시키는 방식으로 처리하였다.



```text

Blood Core 획득

→ CardManager.IncreaseBulletDamage(2)

→ 모든 카드 최종 데미지 증가

```



\---



\### 8.2 Mana Stone



| 항목 | 내용 |

|---|---|

| 효과 | MP 회복 속도 +0.3 |

| 적용 대상 | PlayerStats |

| 적용 함수 | IncreaseManaRegen |

| 적용 시점 | 유물 획득 즉시 |



Mana Stone은 카드 사용을 더 자주 할 수 있도록 MP 회복 속도를 증가시키는 유물이다.



```text

Mana Stone 획득

→ PlayerStats.IncreaseManaRegen(0.3)

→ MP 회복 속도 증가

```



\---



\### 8.3 Shield Fragment



| 항목 | 내용 |

|---|---|

| 효과 | 전투 시작 시 Shield +1 |

| 적용 대상 | PlayerStats |

| 적용 함수 | AddShield |

| 적용 시점 | 다음 전투 시작 시 |



Shield Fragment는 다른 유물과 다르게 획득 즉시 효과를 적용하지 않는다.  

전투가 새로 시작될 때마다 보호막을 추가하는 방식으로 구현하였다.



```text

Shield Fragment 획득

→ 보유 유물 목록에 저장

→ 다음 전투 시작

→ RelicManager.ApplyRelicsOnBattleStart()

→ Shield +1

```



\---



\### 8.4 Quick Gear



| 항목 | 내용 |

|---|---|

| 효과 | 모든 카드 쿨타임 -0.05초 |

| 적용 대상 | CardManager |

| 적용 함수 | ReduceCooldown |

| 적용 시점 | 유물 획득 즉시 |



Quick Gear는 카드 사용 템포를 빠르게 만드는 유물이다.



```text

Quick Gear 획득

→ CardManager.ReduceCooldown(0.05)

→ 모든 카드 최종 쿨타임 감소

```



\---



\### 8.5 Bullet Engine



| 항목 | 내용 |

|---|---|

| 효과 | 모든 카드 탄환 속도 +0.5 |

| 적용 대상 | CardManager |

| 적용 함수 | IncreaseBulletSpeed |

| 적용 시점 | 유물 획득 즉시 |



Bullet Engine은 탄환 속도를 증가시켜 카드 사용감을 더 빠르게 만드는 유물이다.



```text

Bullet Engine 획득

→ CardManager.IncreaseBulletSpeed(0.5)

→ 모든 카드 탄환 속도 증가

```



\---



\## 9. RewardManager 수정 내용



RewardManager에는 유물 보상을 처리하기 위해 다음 요소를 추가하였다.



| 추가 내용 | 설명 |

|---|---|

| relicManager 변수 | 유물 획득 처리를 RelicManager에 전달하기 위해 추가 |

| 유물 RewardType | RelicBloodCore, RelicManaStone 등 추가 |

| 유물 보상 데이터 | CreateRewardPool에 유물 5종 추가 |

| ApplyRelicReward 함수 | 유물 보상 선택 시 RelicManager.AddRelic 호출 |

| 유물 중복 필터링 | 이미 보유한 유물이 다시 보상에 나오지 않도록 처리 |



보상 풀에 추가한 유물 보상은 다음과 같다.



| Reward 이름 | RewardType | RelicType |

|---|---|---|

| Relic: Blood Core | RelicBloodCore | BloodCore |

| Relic: Mana Stone | RelicManaStone | ManaStone |

| Relic: Shield Fragment | RelicShieldFragment | ShieldFragment |

| Relic: Quick Gear | RelicQuickGear | QuickGear |

| Relic: Bullet Engine | RelicBulletEngine | BulletEngine |



\---



\## 10. RelicManager 역할



RelicManager는 이번 작업에서 새로 추가된 핵심 관리자이다.



RelicManager가 담당하는 역할은 다음과 같다.



| 역할 | 설명 |

|---|---|

| 보유 유물 저장 | ownedRelics 리스트에 현재 가진 유물 저장 |

| 중복 확인 | HasRelic으로 이미 보유한 유물인지 확인 |

| 유물 추가 | AddRelic으로 새 유물 획득 처리 |

| 획득 즉시 효과 적용 | Blood Core, Mana Stone 등 즉시 효과 적용 |

| 전투 시작 효과 적용 | Shield Fragment처럼 전투 시작 시 발동되는 효과 처리 |

| UI 갱신 | RelicSlotUI를 통해 보유 유물 목록 표시 |



\---



\## 11. RelicSlotUI 역할



RelicSlotUI는 화면에 현재 보유한 유물을 표시하는 UI 스크립트이다.



유물이 없을 때는 아래처럼 표시한다.



```text

Relics

None

```



유물을 얻으면 아래처럼 표시한다.



```text

Relics

\- Blood Core

\- Mana Stone

```



현재는 간단한 텍스트 목록 방식으로 구현하였다.  

추후에는 아이콘, 등급 색상, 툴팁, 설명 패널 형태로 확장할 수 있다.



\---



\## 12. UI 추가 내용



Canvas\_GameHUD 아래에 유물 표시용 Text를 추가하였다.



| UI 오브젝트 | 역할 |

|---|---|

| Text\_RelicSlots | 현재 보유한 유물 목록 표시 |



추천 배치 값은 다음과 같다.



| 항목 | 값 |

|---|---|

| Anchor | Top Right |

| Pos X | -170 |

| Pos Y | -130 |

| Width | 320 |

| Height | 200 |

| Font Size | 20 |

| Alignment | Left |



\---



\## 13. BattleProgressManager 수정 내용



Shield Fragment처럼 전투 시작 시 발동되는 유물 효과를 처리하기 위해 BattleProgressManager에 RelicManager 연결을 추가하였다.



전투 시작 흐름은 다음과 같이 변경되었다.



```text

StartNextBattle()

→ currentBattleNumber 증가

→ RelicManager.ApplyRelicsOnBattleStart()

→ BattleManager.StartBattle(currentBattleNumber)

```



이 구조를 통해 전투가 시작될 때 보유 유물 중 전투 시작 효과가 자동으로 적용된다.



\---



\## 14. 현재 보상 구조



10일차 이후 보상 구조는 다음처럼 확장되었다.



```text

전투 클리어

→ 랜덤 보상 3개 표시

→ 보상 선택

&#x20;  → 스탯 강화

&#x20;  → 카드 전체 강화

&#x20;  → 새 카드 획득

&#x20;  → 특정 카드 강화

&#x20;  → 유물 획득

→ 보상 효과 적용

→ Next Battle

```



보상 종류를 표로 정리하면 다음과 같다.



| 보상 종류 | 예시 |

|---|---|

| 스탯 강화 | Max HP +20, Max MP +1 |

| 카드 전체 강화 | All Card Damage +5 |

| 새 카드 획득 | New Card: Rapid Shot |

| 특정 카드 강화 | Upgrade: Wide Shot |

| 유물 획득 | Relic: Blood Core |



\---



\## 15. 테스트 결과



| 테스트 항목 | 결과 |

|---|---|

| RelicType 생성 | 정상 |

| RelicData 생성 | 정상 |

| RelicManager 생성 | 정상 |

| RelicSlotUI 생성 | 정상 |

| 유물 UI 초기 표시 | Relics / None 정상 표시 |

| 유물 보상 등장 | 보상창에 Relic 보상 정상 등장 |

| Blood Core 선택 | 모든 카드 데미지 증가 정상 |

| Mana Stone 선택 | MP 회복 속도 증가 정상 |

| Quick Gear 선택 | 모든 카드 쿨타임 감소 정상 |

| Bullet Engine 선택 | 모든 카드 탄환 속도 증가 정상 |

| Shield Fragment 선택 | 다음 전투 시작 시 보호막 증가 정상 |

| 유물 UI 갱신 | 획득한 유물 이름 정상 표시 |

| 유물 중복 방지 | 이미 보유한 유물 보상이 다시 나오지 않음 |

| 다음 전투 유지 | 유물 효과가 다음 전투에도 유지됨 |



\---



\## 16. 발생한 문제와 해결



| 문제 | 원인 | 해결 |

|---|---|---|

| RelicManager가 없다는 오류 | Hierarchy에 RelicManager 오브젝트가 없거나 연결 누락 | 빈 오브젝트 생성 후 RelicManager 컴포넌트 추가 |

| 유물 UI가 표시되지 않음 | RelicSlotUI와 Text\_RelicSlots 연결 누락 | Canvas\_GameHUD에 RelicSlotUI 추가 후 Text 연결 |

| 유물 보상이 선택되어도 효과가 없음 | RewardManager에 RelicManager 연결 누락 | RewardManager의 Relic Manager 필드 연결 |

| Shield Fragment가 즉시 적용되지 않음 | 전투 시작형 유물로 설계됨 | 다음 전투 시작 시 적용되는 것으로 확인 |

| 이미 보유한 유물이 다시 나옴 | IsRewardAvailable에 유물 중복 필터링 누락 | 유물별 HasRelic 조건 추가 |



\---



\## 17. 현재 플레이 흐름



10일차 이후 현재 플레이 흐름은 다음과 같다.



```text

게임 시작

→ Battle 시작

→ 카드 선택

→ 적 패턴 회피

→ 적 처치

→ 전투 클리어

→ 랜덤 보상 3개 표시

→ 카드 / 스탯 / 유물 중 하나 선택

→ 선택한 보상 효과 적용

→ 보유 유물 UI 갱신

→ Next Battle

→ 유물 효과가 유지된 상태로 다음 전투 진행

```



\---



\## 18. 현재 프로젝트 상태



10일차까지 구현된 핵심 시스템은 다음과 같다.



| 시스템 | 상태 |

|---|---|

| 플레이어 이동 | 구현 완료 |

| 회피 | 구현 완료 |

| HP / MP / Shield | 구현 완료 |

| 카드 발사 | 구현 완료 |

| 다중 카드 선택 | 구현 완료 |

| 카드 보상 | 구현 완료 |

| 카드 강화 | 구현 완료 |

| 적 종류 | 구현 완료 |

| 적 탄막 패턴 | 구현 완료 |

| 전투 반복 | 구현 완료 |

| Game Over / Retry | 구현 완료 |

| Prototype Clear | 구현 완료 |

| 카메라 추적 | 구현 완료 |

| 유물 시스템 기본 구조 | 구현 완료 |



\---



\## 19. 다음 개발 목표



10일차 이후에는 유물 시스템을 더 확장하는 것이 좋다.



| 우선순위 | 개발 내용 |

|---:|---|

| 1 | 유물 보상 확률 조정 |

| 2 | 유물 등급 추가 |

| 3 | 유물 5종 추가 |

| 4 | 유물 아이콘 UI 구현 |

| 5 | 카드와 유물 조합 효과 추가 |

| 6 | 유물 효과 설명 툴팁 추가 |

| 7 | 2차 프로토타입 밸런스 조정 |



\---



\## 20. 10일차 정리



10일차 작업을 통해 프로젝트 Q는 단순히 카드를 강화하는 구조에서 벗어나, 유물을 통해 장기적인 성장 효과를 얻는 구조로 확장되었다.



이번 작업으로 플레이어는 전투 보상에서 아래 선택지를 가지게 되었다.



```text

스탯을 강화할 것인가

새 카드를 얻을 것인가

기존 카드를 강화할 것인가

유물을 획득해 지속 효과를 얻을 것인가

```



이 구조는 프로젝트 Q를 로그라이크식 빌드 구성 게임으로 확장하기 위한 중요한 기반이 된다.

