\# Project Q 개발 기록 - 11일차



\## 1. 개발 목표



11일차의 목표는 기존 보상 시스템을 더 로그라이크 게임에 가깝게 확장하는 것이다.



이번 작업에서는 원래 11일차와 12일차로 나누어 진행하려던 내용을 하나의 개발 단계로 합쳐 진행하였다.  

따라서 11일차에는 크게 두 가지 작업을 진행하였다.



| 구분 | 개발 내용 |

|---|---|

| 첫 번째 목표 | 보상 등급 확률 시스템 구현 |

| 두 번째 목표 | 카드 전용 유물 5종 추가 |

| 최종 목표 | 보상 선택의 가치와 빌드 구성 재미를 강화 |



기존에는 전투 클리어 후 보상이 거의 단순 랜덤으로 등장하였다.  

이번 작업 이후에는 보상이 Common, Rare, Epic 등급에 따라 확률적으로 등장하며, 플레이어가 보유한 카드에 따라 특정 카드 전용 유물이 보상에 등장하도록 개선하였다.



\---



\## 2. 작업 전 상태



11일차 작업 전까지 프로젝트 Q에는 다음 시스템이 구현되어 있었다.



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

| 전투 반복 구조 | 구현 완료 |

| Game Over / Retry | 구현 완료 |

| Prototype Clear | 구현 완료 |

| 유물 시스템 기본 구조 | 구현 완료 |



10일차까지의 보상 구조는 다음과 같았다.



```text

전투 클리어

→ 랜덤 보상 3개 표시

→ 카드 / 스탯 / 유물 중 하나 선택

→ 보상 효과 적용

→ 다음 전투 진행

```



하지만 아직 보상의 등급 확률이 제대로 적용되지 않았고, 유물도 대부분 전체 카드 또는 플레이어 스탯에 영향을 주는 단순한 효과 위주였다.



\---



\## 3. 구현한 핵심 기능



| 구분 | 구현 내용 |

|---|---|

| 보상 등급 확률 | Common / Rare / Epic 보상이 확률에 따라 등장하도록 수정 |

| 등급 가중치 | RewardManager에서 등급별 가중치를 Inspector로 조정 가능하게 구현 |

| 등급별 보상 선택 | 먼저 등급을 뽑고, 해당 등급 보상 중 하나를 선택하도록 개선 |

| 보상 중복 방지 | 한 보상창에 같은 보상이 중복 등장하지 않도록 유지 |

| 카드 전용 유물 | 특정 카드와 직접 연결되는 유물 5종 추가 |

| 카드 보유 조건 | 특정 카드를 보유해야 해당 카드 전용 유물이 등장하도록 구현 |

| 유물 중복 방지 | 이미 보유한 유물은 다시 보상에 나오지 않도록 개선 |

| 오류 수정 | float 값을 int 함수에 전달하던 CS1503 오류 수정 |



\---



\## 4. 수정한 스크립트



| 파일명 | 위치 | 수정 내용 |

|---|---|---|

| RewardManager.cs | Assets/Scripts/Managers | 보상 등급 확률, 카드 전용 유물 보상, 유물 필터링 추가 |

| RewardType.cs | Assets/Scripts/Reward | 카드 전용 유물 보상 타입 추가 |

| RelicType.cs | Assets/Scripts/Relic | 카드 전용 유물 타입 추가 |

| RelicManager.cs | Assets/Scripts/Managers | 카드 전용 유물 효과 적용 함수 추가 |



\---



\## 5. 보상 등급 확률 시스템



기존 보상 선택은 사용 가능한 보상 풀에서 랜덤으로 3개를 뽑는 방식이었다.  

이번 작업에서는 보상 등급을 먼저 확률로 결정한 뒤, 해당 등급에 맞는 보상 중 하나를 선택하도록 변경하였다.



기본 확률 설정은 다음과 같다.



| 등급 | 가중치 | 의도 |

|---|---:|---|

| Common | 70 | 일반 보상이 가장 자주 등장 |

| Rare | 25 | 강한 보상과 유물이 가끔 등장 |

| Epic | 5 | 매우 강한 보상이 낮은 확률로 등장 |



보상 선택 흐름은 다음과 같다.



```text

전투 클리어

→ 사용 가능한 보상 풀 생성

→ 보상 등급 확률 계산

→ Common / Rare / Epic 중 하나 결정

→ 해당 등급 보상 목록 추출

→ 그중 하나를 랜덤 선택

→ 중복 없이 보상창에 3개 표시

```



\---



\## 6. RewardManager에 추가한 등급 확률 변수



RewardManager에는 등급 확률 조정을 위해 다음 변수를 추가하였다.



| 변수명 | 기본값 | 설명 |

|---|---:|---|

| commonWeight | 70 | Common 보상 등장 가중치 |

| rareWeight | 25 | Rare 보상 등장 가중치 |

| epicWeight | 5 | Epic 보상 등장 가중치 |



이 값들은 Unity Inspector에서 직접 조정할 수 있다.  

테스트 중에는 Epic 보상을 자주 확인하기 위해 임시로 Epic Weight 값을 높일 수 있다.



\---



\## 7. 추가한 보상 선택 함수



RewardManager에 다음 함수들을 추가하였다.



| 함수명 | 역할 |

|---|---|

| GetRandomRewardByRarity | 등급 확률을 적용하여 보상 하나 선택 |

| GetWeightedRandomRarity | Common / Rare / Epic 중 하나를 가중치 기반으로 선택 |

| GetRewardsByRarity | 특정 등급에 해당하는 보상 목록만 추출 |

| IsRelicAvailable | 특정 유물이 이미 있는지 확인하고 보상 등장 가능 여부 판단 |



이를 통해 보상 시스템이 단순 랜덤 구조에서 등급 확률 기반 구조로 개선되었다.



\---



\## 8. 기존 보상 등급 정리



현재 보상 등급은 다음과 같이 정리하였다.



\### Common 보상



| 보상 이름 | 효과 |

|---|---|

| Card Damage Up | 모든 카드 데미지 +5 |

| Quick Reload | 모든 카드 쿨타임 -0.1초 |

| Mana Expansion | 최대 MP +1 |

| Vital Boost | 최대 HP +20 |

| Shield Core | Shield +1 |

| Bullet Speed Up | 모든 카드 탄환 속도 +1 |

| Upgrade: Pixel Shot | Pixel Shot 데미지 +3 |



\### Rare 보상



| 보상 이름 | 효과 |

|---|---|

| Triple Focus | 모든 카드 데미지 +10 |

| Mana Recovery | MP 회복 속도 +0.5 |

| Emergency Guard | Shield +2 |

| New Card: Rapid Shot | Rapid Shot 카드 획득 |

| New Card: Heavy Shot | Heavy Shot 카드 획득 |

| Upgrade: Focus Shot | Focus Shot 데미지 +10 |

| Upgrade: Wide Shot | Wide Shot 탄환 수 +1 |

| Upgrade: Rapid Shot | Rapid Shot 쿨타임 -0.05초 |

| Relic: Blood Core | 모든 카드 데미지 +2 |

| Relic: Mana Stone | MP 회복 속도 +0.3 |

| Relic: Shield Fragment | 전투 시작 시 Shield +1 |

| Relic: Quick Gear | 모든 카드 쿨타임 -0.05초 |

| Relic: Bullet Engine | 모든 카드 탄환 속도 +0.5 |



\### Epic 보상



| 보상 이름 | 효과 |

|---|---|

| Upgrade: Heavy Shot | Heavy Shot 데미지 +15 |

| Relic: Heavy Core | Heavy Shot 데미지 +20 |



\---



\## 9. 카드 전용 유물 추가



이번 작업에서는 특정 카드의 성능을 직접 강화하는 카드 전용 유물 5종을 추가하였다.



| 유물 이름 | 효과 | 등장 조건 |

|---|---|---|

| Pixel Lens | Pixel Shot 데미지 +5 | Pixel Shot 보유 |

| Focus Lens | Focus Shot 데미지 +15 | Focus Shot 보유 |

| Wide Barrel | Wide Shot 탄환 수 +2 | Wide Shot 보유 |

| Rapid Battery | Rapid Shot 쿨타임 -0.08초 | Rapid Shot 보유 |

| Heavy Core | Heavy Shot 데미지 +20 | Heavy Shot 보유 |



이 유물들은 특정 카드와 직접 연결된다.  

예를 들어 Rapid Shot을 아직 획득하지 않았다면 Rapid Battery 유물은 보상에 등장하지 않는다.



\---



\## 10. RelicType 추가 내용



RelicType에는 다음 유물 타입을 새로 추가하였다.



| RelicType | 설명 |

|---|---|

| PixelLens | Pixel Shot 전용 유물 |

| FocusLens | Focus Shot 전용 유물 |

| WideBarrel | Wide Shot 전용 유물 |

| RapidBattery | Rapid Shot 전용 유물 |

| HeavyCore | Heavy Shot 전용 유물 |



기존 유물 타입은 유지하고, 카드 전용 유물 타입만 추가하였다.



\---



\## 11. RewardType 추가 내용



RewardType에는 다음 보상 타입을 새로 추가하였다.



| RewardType | 연결되는 유물 |

|---|---|

| RelicPixelLens | Pixel Lens |

| RelicFocusLens | Focus Lens |

| RelicWideBarrel | Wide Barrel |

| RelicRapidBattery | Rapid Battery |

| RelicHeavyCore | Heavy Core |



RewardManager는 이 RewardType을 기준으로 RelicData를 생성한 뒤 RelicManager에 전달한다.



\---



\## 12. 카드 전용 유물 효과 적용 방식



카드 전용 유물은 RelicManager에서 효과를 적용한다.



| 유물 이름 | 적용 함수 |

|---|---|

| Pixel Lens | UpgradeCardDamage(CardType.PixelShot, value) |

| Focus Lens | UpgradeCardDamage(CardType.FocusShot, value) |

| Wide Barrel | UpgradeCardBulletCount(CardType.WideShot, value) |

| Rapid Battery | UpgradeCardCooldown(CardType.RapidShot, value) |

| Heavy Core | UpgradeCardDamage(CardType.HeavyShot, value) |



데미지와 탄환 수는 정수 값이 필요하기 때문에 `Mathf.RoundToInt(value)`를 사용하여 float 값을 int로 변환하였다.



```text

float value

→ Mathf.RoundToInt(value)

→ int 값으로 카드 강화 함수에 전달

```



\---



\## 13. 보상 필터링 개선



이번 작업에서 보상 필터링도 확장하였다.



기존에는 보유하지 않은 카드의 강화 보상이 나오지 않도록 처리하였다.  

이번에는 카드 전용 유물에도 같은 규칙을 적용하였다.



| 조건 | 처리 |

|---|---|

| Pixel Shot 보유 | Pixel Lens 등장 가능 |

| Focus Shot 보유 | Focus Lens 등장 가능 |

| Wide Shot 보유 | Wide Barrel 등장 가능 |

| Rapid Shot 미보유 | Rapid Battery 등장 불가 |

| Heavy Shot 미보유 | Heavy Core 등장 불가 |

| 이미 유물 보유 | 같은 유물 다시 등장 불가 |



이 구조를 통해 보상창에 현재 플레이 상황과 맞지 않는 보상이 등장하는 문제를 줄였다.



\---



\## 14. 보상 선택 흐름 변경



11일차 이후 보상 선택 흐름은 다음과 같다.



```text

전투 클리어

→ 현재 보유 카드 확인

→ 현재 보유 유물 확인

→ 사용 가능한 보상 풀 생성

→ 등급 확률 계산

→ 해당 등급 보상 중 랜덤 선택

→ 보상 3개 표시

→ 플레이어가 보상 선택

→ 카드 / 스탯 / 유물 효과 적용

```



이제 보상은 단순히 무작위로 나오는 것이 아니라, 현재 플레이어의 카드와 유물 상태를 반영해서 등장한다.



\---



\## 15. 테스트 결과



| 테스트 항목 | 결과 |

|---|---|

| Common / Rare / Epic 등급 표시 | 정상 |

| 등급별 보상 확률 적용 | 정상 |

| 보상 3개 중복 방지 | 정상 |

| 유물 중복 방지 | 정상 |

| Pixel Lens 등장 조건 | Pixel Shot 보유 시 정상 등장 |

| Focus Lens 등장 조건 | Focus Shot 보유 시 정상 등장 |

| Wide Barrel 등장 조건 | Wide Shot 보유 시 정상 등장 |

| Rapid Battery 등장 조건 | Rapid Shot 보유 후 정상 등장 |

| Heavy Core 등장 조건 | Heavy Shot 보유 후 정상 등장 |

| Pixel Lens 효과 | Pixel Shot 데미지 증가 정상 |

| Focus Lens 효과 | Focus Shot 데미지 증가 정상 |

| Wide Barrel 효과 | Wide Shot 탄환 수 증가 정상 |

| Rapid Battery 효과 | Rapid Shot 쿨타임 감소 정상 |

| Heavy Core 효과 | Heavy Shot 데미지 증가 정상 |

| Relic UI 표시 | 획득한 유물 이름 정상 표시 |



\---



\## 16. 발생한 문제와 해결



| 문제 | 원인 | 해결 |

|---|---|---|

| CS1503 오류 발생 | float 값을 int 매개변수에 전달함 | Mathf.RoundToInt(value)로 변환 |

| Rapid Battery가 Rapid Shot 없이 나올 가능성 | 카드 보유 조건 누락 가능 | cardManager.HasCard(CardType.RapidShot) 조건 추가 |

| Heavy Core가 Heavy Shot 없이 나올 가능성 | 카드 보유 조건 누락 가능 | cardManager.HasCard(CardType.HeavyShot) 조건 추가 |

| 유물 중복 등장 가능성 | 유물 중복 확인 로직이 분산됨 | IsRelicAvailable 함수로 정리 |

| Epic 보상이 잘 안 보임 | Epic Weight 기본값이 낮음 | 테스트 시 임시로 Epic Weight를 높여 확인 |



\---



\## 17. 현재 프로젝트 상태



11일차까지 완료된 핵심 시스템은 다음과 같다.



| 시스템 | 상태 |

|---|---|

| 플레이어 이동 / 회피 | 완료 |

| HP / MP / Shield | 완료 |

| 카드 발사 | 완료 |

| 다중 카드 선택 | 완료 |

| 카드 보상 | 완료 |

| 카드 강화 | 완료 |

| 유물 시스템 | 완료 |

| 보상 등급 확률 | 완료 |

| 카드 전용 유물 | 완료 |

| 적 스폰 | 완료 |

| 적 종류 확장 | 완료 |

| 적 탄막 패턴 | 완료 |

| 전투 반복 | 완료 |

| Game Over / Retry | 완료 |

| Prototype Clear | 완료 |

| 카메라 추적 | 완료 |



\---



\## 18. 현재 플레이 흐름



현재 게임의 플레이 흐름은 다음과 같다.



```text

게임 시작

→ Battle 시작

→ 카드 선택

→ 적 공격 회피

→ 카드로 적 처치

→ 전투 클리어

→ 보상 등급 확률 계산

→ 카드 / 스탯 / 유물 보상 표시

→ 보상 선택

→ 선택한 보상 효과 적용

→ 보유 카드와 유물 상태 갱신

→ 다음 전투 진행

```



\---



\## 19. 다음 개발 목표



다음 개발에서는 카드와 유물의 수를 늘리는 것보다, 현재 시스템을 더 보기 좋고 관리하기 쉽게 만드는 작업이 필요하다.



| 우선순위 | 다음 작업 |

|---:|---|

| 1 | 유물 UI 개선 |

| 2 | 유물 등급 색상 표시 |

| 3 | 보상 버튼 색상 등급별 구분 |

| 4 | 카드 상세 정보 UI 개선 |

| 5 | 카드 5\~7종 추가 |

| 6 | 카드와 유물 조합 효과 추가 |

| 7 | 전투 밸런스 조정 |



\---



\## 20. 11일차 정리



11일차 작업을 통해 프로젝트 Q의 보상 시스템은 단순 랜덤 보상 구조에서 벗어나, 등급 확률과 보유 상태를 반영하는 구조로 개선되었다.



또한 카드 전용 유물 5종을 추가하면서 플레이어가 어떤 카드를 보유하고 있는지에 따라 보상 가치가 달라지는 구조를 만들었다.



이번 작업 이후 프로젝트 Q는 다음과 같은 방향으로 발전하였다.



```text

단순 카드 강화 게임

→ 카드와 유물을 조합하는 로그라이크 성장 구조

```



이제 플레이어는 전투 후 보상을 선택할 때 단순히 높은 수치를 고르는 것이 아니라, 현재 보유한 카드와 유물 조합을 고려하여 빌드를 구성할 수 있게 되었다.

