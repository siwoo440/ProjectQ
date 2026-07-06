\# Project Q 개발 기록 - 12일차



\## 1. 개발 목표



12일차의 목표는 기존에 구현된 보상 시스템, 유물 시스템, 카드 시스템을 플레이어가 더 쉽게 이해할 수 있도록 UI를 개선하는 것이다.



이번 작업에서는 원래 12일차와 13일차로 나누어 진행하려던 내용을 하나의 개발 단계로 합쳐 진행하였다.  

따라서 12일차에는 크게 두 가지 작업을 진행하였다.



| 구분 | 개발 내용 |

|---|---|

| 첫 번째 목표 | 보상 UI 및 유물 UI 개선 |

| 두 번째 목표 | 카드 목록 UI 및 선택 카드 상세 정보 UI 개선 |

| 최종 목표 | 플레이어가 보상, 유물, 카드 상태를 더 명확하게 확인할 수 있게 만들기 |



기존에는 보상과 카드 기능은 작동했지만, 플레이어가 현재 어떤 보상이 어떤 종류인지, 어떤 카드가 선택되어 있는지, 카드 수치가 어떻게 바뀌었는지 확인하기 어려웠다.



이번 작업을 통해 보상 버튼에는 등급과 보상 종류가 표시되고, 유물 UI에는 보유 유물 개수가 표시되며, 카드 UI에는 현재 선택한 카드의 상세 능력치가 표시되도록 개선하였다.



\---



\## 2. 작업 전 상태



12일차 작업 전까지 프로젝트 Q에는 다음 시스템이 구현되어 있었다.



| 시스템 | 상태 |

|---|---|

| 플레이어 이동 | 구현 완료 |

| 회피 | 구현 완료 |

| HP / MP / Shield | 구현 완료 |

| 카드 발사 | 구현 완료 |

| 다중 카드 선택 | 구현 완료 |

| 카드 보상 | 구현 완료 |

| 카드 강화 | 구현 완료 |

| 보상 등급 확률 | 구현 완료 |

| 유물 시스템 | 구현 완료 |

| 카드 전용 유물 | 구현 완료 |

| 적 종류 | 구현 완료 |

| 적 탄막 패턴 | 구현 완료 |

| 전투 반복 구조 | 구현 완료 |

| Game Over / Retry | 구현 완료 |

| Prototype Clear | 구현 완료 |



기존 플레이 흐름은 다음과 같았다.



```text

게임 시작

→ Battle 시작

→ 카드 선택

→ 적 처치

→ 전투 클리어

→ 보상 3개 표시

→ 보상 선택

→ 카드 / 스탯 / 유물 효과 적용

→ 다음 전투 진행

```



하지만 UI 측면에서는 다음 문제가 있었다.



| 문제 | 설명 |

|---|---|

| 보상 종류 구분 부족 | 보상이 스탯 보상인지, 카드 보상인지, 유물 보상인지 한눈에 보기 어려움 |

| 보상 등급 시각 구분 부족 | Common, Rare, Epic이 텍스트로만 표시됨 |

| 유물 UI 정보 부족 | 보유 유물 이름만 표시되고 개수 정보가 없음 |

| 카드 상세 정보 부족 | 현재 선택한 카드의 데미지, 쿨타임, 탄환 수 등을 확인하기 어려움 |

| 강화 결과 확인 어려움 | 보상이나 유물로 카드가 강화되어도 수치 변화를 바로 확인하기 어려움 |



\---



\## 3. 구현한 핵심 기능



| 구분 | 구현 내용 |

|---|---|

| 보상 타입 표시 | 보상 버튼에 Stat, Relic, New Card, Card Upgrade 분류 표시 |

| 보상 등급 색상 | Common, Rare, Epic 등급에 따라 버튼 배경색 변경 |

| 보상 버튼 구조 개선 | RewardButton에서 Setup 방식으로 보상 데이터, RewardManager, 보상 번호를 처리 |

| 유물 UI 개선 | 보유 유물 개수와 최대 표시 개수 표시 |

| 유물 목록 제한 | 유물이 많아질 경우 최대 개수까지만 표시하고 나머지는 생략 문구 표시 |

| 카드 목록 UI 개선 | 보유 카드 목록과 선택 표시를 유지 |

| 카드 상세 UI 추가 | 현재 선택한 카드의 상세 능력치 표시 |

| 카드 최종 수치 표시 | 전체 데미지 보너스, 탄환 속도 보너스, 쿨타임 감소값 반영 가능 |

| 카드 강화 후 UI 갱신 | 카드 강화, 유물 효과, 새 카드 획득 후 UI 갱신 |



\---



\## 4. 수정한 스크립트



| 파일명 | 위치 | 수정 내용 |

|---|---|---|

| RewardButton.cs | Assets/Scripts/UI | 보상 등급 색상, 보상 타입 표시, Setup 구조 개선 |

| RewardManager.cs | Assets/Scripts/Managers | RewardButton 연결 방식 수정, SelectReward 인덱스 방식 대응 |

| RelicSlotUI.cs | Assets/Scripts/UI | 보유 유물 개수 표시, 최대 표시 개수 제한 |

| CardSlotUI.cs | Assets/Scripts/UI | 카드 목록과 선택 카드 상세 정보 표시 |

| CardManager.cs | Assets/Scripts/Card | 카드 UI 갱신 시 선택 카드 상세 정보 갱신 |



\---



\## 5. 보상 UI 개선



기존 보상 버튼은 보상 등급, 이름, 설명만 표시하는 구조였다.



기존 표시 예시는 다음과 같다.



```text

\[Rare]

Relic: Blood Core



All Card Damage +2

```



이번 작업 이후 보상 버튼은 보상 등급과 보상 종류를 함께 표시한다.



변경 후 표시 예시는 다음과 같다.



```text

\[Rare] \[Relic]

Relic: Blood Core



All Card Damage +2

```



보상 종류는 다음 기준으로 분류하였다.



| 표시 문구 | 해당 보상 |

|---|---|

| Stat | HP, MP, Shield, 전체 데미지, 전체 쿨타임, 탄환 속도 등 |

| New Card | Rapid Shot, Heavy Shot 같은 신규 카드 획득 |

| Card Upgrade | 특정 카드 데미지, 쿨타임, 탄환 수 강화 |

| Relic | Blood Core, Mana Stone, Pixel Lens 같은 유물 획득 |



\---



\## 6. 보상 등급 색상 적용



RewardButton에서 보상 등급에 따라 버튼 배경색이 달라지도록 구현하였다.



| 등급 | 색상 방향 | 의도 |

|---|---|---|

| Common | 회색 계열 | 일반 보상 |

| Rare | 파란색 계열 | 희귀 보상 |

| Epic | 보라색 계열 | 강력한 보상 |



이를 통해 플레이어가 보상창을 열었을 때 어떤 보상이 더 높은 등급인지 더 빠르게 파악할 수 있게 되었다.



\---



\## 7. RewardButton 구조 개선



RewardButton은 다음 정보를 저장하도록 수정하였다.



| 변수 | 역할 |

|---|---|

| button | 실제 클릭 가능한 Button 컴포넌트 |

| rewardText | 보상 정보를 표시할 TextMeshProUGUI |

| backgroundImage | 등급에 따라 색상이 바뀌는 Image |

| rewardManager | 보상 선택을 처리할 RewardManager |

| rewardIndex | 현재 버튼이 몇 번째 보상인지 저장 |

| rewardData | 현재 버튼에 표시할 보상 데이터 |



보상 버튼 설정 흐름은 다음과 같다.



```text

RewardManager

→ currentRewards에서 보상 3개 선택

→ RewardButton.Setup(rewardData, rewardManager, index)

→ RewardButton이 텍스트와 색상 갱신

→ 버튼 클릭 시 rewardManager.SelectReward(index) 호출

```



기존에는 RewardManager가 RewardButton 내부 변수에 직접 접근하는 방식이 섞여 있었지만, 이번 작업에서 Setup 중심 구조로 정리하였다.



\---



\## 8. RewardManager 수정 내용



RewardButton 구조가 바뀌면서 RewardManager도 함께 수정하였다.



기존 방식에서는 RewardManager가 다음처럼 RewardButton 내부 변수에 직접 접근하려고 했다.



```text

rewardButton01.rewardManager = this

rewardButton02.rewardManager = this

rewardButton03.rewardManager = this

```



하지만 RewardButton의 rewardManager 변수를 private으로 관리하게 되면서 직접 접근할 수 없게 되었다.



따라서 다음 방식으로 수정하였다.



```text

RewardManager

→ rewardButton.Setup(currentRewards\[0], this, 0)

→ RewardButton 내부에서 rewardManager 저장

```



또한 RewardButton이 보상 번호를 전달하는 구조에 맞춰, RewardManager의 SelectReward 함수도 보상 번호를 받아 currentRewards에서 실제 RewardData를 꺼내는 방식으로 정리하였다.



\---



\## 9. 유물 UI 개선



기존 유물 UI는 보유 유물 이름만 표시하였다.



기존 예시는 다음과 같다.



```text

Relics

\- Blood Core

\- Pixel Lens

```



이번 작업 이후 유물 UI는 현재 보유 유물 수와 최대 표시 개수를 함께 보여준다.



변경 후 예시는 다음과 같다.



```text

Relics 2/10

\- Blood Core

\- Pixel Lens

```



유물이 없을 때는 다음과 같이 표시한다.



```text

Relics 0/10

None

```



\---



\## 10. 유물 목록 최대 표시 개수



RelicSlotUI에 maxDisplayCount 값을 추가하였다.



| 변수 | 기본값 | 설명 |

|---|---:|---|

| maxDisplayCount | 10 | 화면에 표시할 최대 유물 수 |



유물이 최대 표시 개수를 넘으면, 화면에 모든 유물을 표시하지 않고 일부를 생략하도록 만들었다.



예시는 다음과 같다.



```text

Relics 12/10

\- Blood Core

\- Mana Stone

\- Shield Fragment

\- Quick Gear

\- Bullet Engine

\- Pixel Lens

\- Focus Lens

\- Wide Barrel

\- Rapid Battery

\- Heavy Core

... +2 more

```



이 구조를 통해 유물이 많아져도 UI가 화면을 과하게 차지하지 않도록 하였다.



\---



\## 11. 카드 UI 개선



기존 카드 UI는 보유 카드 목록만 보여주는 구조였다.



기존 예시는 다음과 같다.



```text

Cards

> \[1] Pixel Shot

&#x20; \[2] Focus Shot

&#x20; \[3] Wide Shot

```



이번 작업 이후에는 카드 목록 옆에 선택 카드 상세 정보가 표시되도록 개선하였다.



변경 후 예시는 다음과 같다.



```text

Cards

> \[1] Pixel Shot

&#x20; \[2] Focus Shot

&#x20; \[3] Wide Shot



Selected Card

Name: Pixel Shot

Type: PixelShot

Role: Basic

MP Cost: 1

Cooldown: 0.9s

Damage: 15

Bullet Count: 3

Bullet Speed: 11

Life Time: 3s

Spread: 10

```



\---



\## 12. 카드 상세 정보 표시 항목



선택한 카드 상세 UI에는 다음 정보를 표시한다.



| 항목 | 설명 |

|---|---|

| Name | 카드 이름 |

| Type | 카드 타입 |

| Role | 카드 역할 |

| MP Cost | 카드 사용에 필요한 MP |

| Cooldown | 카드 재사용 대기시간 |

| Damage | 탄환 데미지 |

| Bullet Count | 한 번에 발사되는 탄환 수 |

| Bullet Speed | 탄환 속도 |

| Life Time | 탄환 지속 시간 |

| Spread | 탄퍼짐 각도 |



\---



\## 13. 카드 역할 표시



카드 타입에 따라 역할 문구를 표시하도록 구현하였다.



| 카드 | 역할 문구 |

|---|---|

| Pixel Shot | Basic |

| Focus Shot | High Damage |

| Wide Shot | Wide Area |

| Rapid Shot | Fast Fire |

| Heavy Shot | Heavy Damage |



이를 통해 카드 이름만 보는 것보다 각 카드가 어떤 전투 역할을 가지는지 더 쉽게 이해할 수 있다.



\---



\## 14. 카드 최종 수치 표시



CardSlotUI는 기본 카드 수치뿐 아니라, 전체 카드 보너스가 반영된 최종 수치를 표시할 수 있도록 구성하였다.



반영 가능한 보너스는 다음과 같다.



| 보너스 | 설명 |

|---|---|

| bonusDamage | 전체 카드 추가 데미지 |

| bonusSpeed | 전체 탄환 속도 증가 |

| cooldownReduction | 전체 카드 쿨타임 감소 |



최종 수치는 다음 구조로 계산한다.



```text

표시 데미지 = 카드 기본 데미지 + 전체 데미지 보너스

표시 탄환 속도 = 카드 기본 속도 + 전체 속도 보너스

표시 쿨타임 = 카드 기본 쿨타임 - 전체 쿨타임 감소

```



쿨타임은 너무 낮아지지 않도록 최소값을 적용하였다.



```text

최소 쿨타임 = 0.05초

```



\---



\## 15. CardManager 수정 내용



CardManager에서는 카드 UI가 올바르게 갱신되도록 수정하였다.



카드 UI는 다음 상황에서 갱신되어야 한다.



| 상황 | 이유 |

|---|---|

| 게임 시작 시 | 초기 카드 목록 표시 |

| 카드 선택 시 | 선택 표시와 상세 정보 변경 |

| Q / E 입력 시 | 이전 / 다음 카드 선택 반영 |

| 마우스 휠 입력 시 | 카드 선택 변경 반영 |

| 새 카드 획득 시 | 카드 목록에 새 카드 추가 |

| 카드 강화 시 | 카드 상세 능력치 변경 |

| 유물 획득 시 | 유물 효과로 바뀐 카드 수치 반영 |

| 전체 보너스 획득 시 | 최종 데미지, 속도, 쿨타임 표시 변경 |



이를 위해 CardManager의 UpdateCardUI 함수에서 CardSlotUI로 보유 카드 목록, 선택 인덱스, 전체 보너스 값을 전달하도록 개선하였다.



\---



\## 16. UI 오브젝트 추가 내용



이번 작업에서 다음 UI 오브젝트를 추가하거나 수정하였다.



| UI 오브젝트 | 역할 |

|---|---|

| Text\_CardDetail | 선택한 카드의 상세 능력치 표시 |

| Text\_RelicSlots | 보유 유물 개수와 목록 표시 |

| Button\_Reward\_01 | 첫 번째 보상 버튼 |

| Button\_Reward\_02 | 두 번째 보상 버튼 |

| Button\_Reward\_03 | 세 번째 보상 버튼 |



\---



\## 17. Inspector 연결 내용



이번 작업 후 Inspector에서 확인한 연결은 다음과 같다.



\### RewardButton



| 변수 | 연결 대상 |

|---|---|

| Button | 자기 자신의 Button 컴포넌트 |

| Reward Text | 버튼 내부 TMP Text |

| Background Image | 자기 자신의 Image 컴포넌트 |



\### RelicSlotUI



| 변수 | 연결 대상 |

|---|---|

| Relic Text | Text\_RelicSlots |

| Max Display Count | 10 |



\### CardSlotUI



| 변수 | 연결 대상 |

|---|---|

| Card Text | 기존 카드 목록 TMP Text |

| Card Detail Text | Text\_CardDetail |

| Show Final Stats | 체크 |



\---



\## 18. 테스트 결과



| 테스트 항목 | 결과 |

|---|---|

| 보상 버튼 등급 표시 | 정상 |

| 보상 버튼 타입 표시 | 정상 |

| Common 색상 표시 | 정상 |

| Rare 색상 표시 | 정상 |

| Epic 색상 표시 | 정상 |

| 보상 버튼 클릭 | 정상 |

| 보상 선택 후 Next Battle 표시 | 정상 |

| 유물 UI 초기 표시 | Relics 0/10, None 정상 표시 |

| 유물 획득 후 UI 갱신 | 정상 |

| 유물 개수 표시 | 정상 |

| 카드 목록 표시 | 정상 |

| 선택 카드 표시 | 정상 |

| 카드 상세 정보 표시 | 정상 |

| 숫자키 카드 선택 | 정상 |

| Q / E 카드 선택 | 정상 |

| 새 카드 획득 후 카드 목록 갱신 | 정상 |

| 카드 강화 후 상세 수치 갱신 | 정상 |

| 유물 효과 후 카드 수치 갱신 | 정상 |



\---



\## 19. 발생한 문제와 해결



| 문제 | 원인 | 해결 |

|---|---|---|

| RewardButton.rewardManager 접근 오류 | RewardButton의 rewardManager가 private인데 RewardManager가 직접 접근함 | ConnectButtons 함수 제거, Setup 방식으로 정리 |

| SelectReward 매개변수 불일치 가능성 | RewardButton은 int index를 전달하고 RewardManager는 RewardData를 받는 구조였음 | SelectReward(int rewardIndex) 방식으로 수정 |

| 보상 텍스트가 길어짐 | 등급과 타입 표시가 추가되어 텍스트 양 증가 | TMP Auto Size와 버튼 크기 조정 |

| 유물 UI가 길어질 가능성 | 유물이 많아질 경우 화면을 차지함 | maxDisplayCount로 표시 개수 제한 |

| 카드 상세 UI가 안 보일 가능성 | Text\_CardDetail 연결 누락 가능 | CardSlotUI에 Card Detail Text 연결 |

| 카드 강화 후 수치가 바로 안 바뀔 가능성 | 강화 함수에서 UI 갱신이 누락될 수 있음 | 강화 함수 마지막에 UpdateCardUI 호출 확인 |



\---



\## 20. 현재 플레이 흐름



12일차 이후 현재 플레이 흐름은 다음과 같다.



```text

게임 시작

→ 카드 목록과 선택 카드 상세 정보 확인

→ Battle 시작

→ 카드 선택

→ 적 공격 회피

→ 적 처치

→ 전투 클리어

→ 보상 등급과 타입이 표시된 보상창 확인

→ 카드 / 스탯 / 유물 보상 선택

→ 선택한 보상 효과 적용

→ 카드 상세 정보 또는 유물 UI 갱신

→ Next Battle

→ 다음 전투 진행

```



\---



\## 21. 현재 프로젝트 상태



12일차까지 완료된 핵심 시스템은 다음과 같다.



| 시스템 | 상태 |

|---|---|

| 플레이어 이동 / 회피 | 완료 |

| HP / MP / Shield | 완료 |

| 카드 발사 | 완료 |

| 다중 카드 선택 | 완료 |

| 카드 보상 | 완료 |

| 카드 강화 | 완료 |

| 카드 상세 UI | 완료 |

| 보상 등급 확률 | 완료 |

| 보상 등급 색상 표시 | 완료 |

| 보상 타입 표시 | 완료 |

| 유물 시스템 | 완료 |

| 유물 UI 개선 | 완료 |

| 카드 전용 유물 | 완료 |

| 적 스폰 | 완료 |

| 적 종류 확장 | 완료 |

| 적 탄막 패턴 | 완료 |

| 전투 반복 | 완료 |

| Game Over / Retry | 완료 |

| Prototype Clear | 완료 |

| 카메라 추적 | 완료 |



\---



\## 22. 다음 개발 목표



다음 개발에서는 UI를 더 확장하거나, 실제 콘텐츠를 늘리는 방향으로 진행할 수 있다.



| 우선순위 | 다음 작업 |

|---:|---|

| 1 | 카드 5\~7종 추가 |

| 2 | 카드별 개성 강화 |

| 3 | 유물 아이콘 UI 추가 |

| 4 | 카드 아이콘 UI 추가 |

| 5 | 카드와 유물 조합 효과 추가 |

| 6 | 전투 밸런스 조정 |

| 7 | 스테이지 진행 구조 준비 |



\---



\## 23. 12일차 정리



12일차 작업을 통해 프로젝트 Q는 기능적으로만 작동하는 전투 프로토타입에서, 플레이어가 자신의 성장 상태를 확인할 수 있는 구조로 개선되었다.



이번 작업의 핵심 변화는 다음과 같다.



```text

보상 기능만 존재

→ 보상 등급과 종류를 시각적으로 구분



유물 이름만 표시

→ 보유 유물 개수와 목록 표시



카드 목록만 표시

→ 선택 카드 상세 능력치 표시

```



이제 플레이어는 보상과 유물을 선택했을 때 실제로 어떤 카드 수치가 바뀌었는지 화면에서 바로 확인할 수 있다.



이번 작업은 이후 카드 아이콘, 유물 아이콘, 카드 상세 설명, 카드 선택창, 덱 관리 화면으로 확장하기 위한 기반이 된다.

