\# Project Q 개발 기록 - 13일차



\## 1. 개발 목표



13일차의 목표는 카드 시스템을 확장하여 신규 카드 3종을 추가하고, 각 카드가 서로 다른 전투 방식을 가지도록 특수 탄환 기능을 구현하는 것이다.



기존 카드들은 주로 데미지, 탄환 수, 쿨타임 차이로 구분되었다. 이번 작업에서는 관통, 폭발, 유도라는 특수 효과를 가진 카드를 추가하여 플레이어가 전투 중 더 다양한 선택지를 가질 수 있도록 개선하였다.



\---



\## 2. 구현한 핵심 기능



| 구분 | 구현 내용 |

|---|---|

| 신규 카드 추가 | Pierce Shot, Bomb Shot, Homing Shot 추가 |

| 탄환 효과 타입 추가 | Normal, Pierce, Bomb, Homing 타입 추가 |

| 관통 탄환 구현 | 적을 맞춰도 일정 횟수 동안 사라지지 않고 계속 진행 |

| 폭발 탄환 구현 | 적중 시 주변 적에게 범위 피해 적용 |

| 유도 탄환 구현 | 가까운 적을 탐색하여 방향을 보정 |

| 신규 카드 보상 추가 | 전투 보상에서 신규 카드 획득 가능 |

| 신규 카드 강화 보상 추가 | 신규 카드 전용 강화 보상 추가 |

| 보상 필터링 확장 | 보유하지 않은 신규 카드의 강화 보상 제외 |

| 카드 상세 UI 확장 | 신규 카드의 특수 효과 수치 표시 |



\---



\## 3. 추가한 신규 카드



| 카드 이름 | 역할 | 특징 |

|---|---|---|

| Pierce Shot | 관통형 카드 | 적을 여러 번 관통하는 직선 탄환 발사 |

| Bomb Shot | 폭발형 카드 | 적중 시 주변 적에게 범위 피해 |

| Homing Shot | 유도형 카드 | 가까운 적을 향해 탄환 방향 보정 |



\---



\## 4. 신규 카드 기본 수치



| 카드 | MP | 쿨타임 | 데미지 | 탄속 | 지속시간 | 특수 효과 |

|---|---:|---:|---:|---:|---:|---|

| Pierce Shot | 2 | 1.6초 | 14 | 11 | 3초 | 관통 3회 |

| Bomb Shot | 3 | 2.4초 | 18 | 7 | 3.5초 | 폭발 반경 2.2 |

| Homing Shot | 2 | 1.8초 | 10 | 8 | 4초 | 유도 범위 7, 회전 속도 4 |



\---



\## 5. 추가한 스크립트



| 파일명 | 위치 | 역할 |

|---|---|---|

| BulletEffectType.cs | Assets/Scripts/Bullet | 탄환 특수 효과 타입 관리 |



\---



\## 6. 수정한 스크립트



| 파일명 | 위치 | 수정 내용 |

|---|---|---|

| CardType.cs | Assets/Scripts/Card | PierceShot, BombShot, HomingShot 타입 추가 |

| CardData.cs | Assets/Scripts/Card | 탄환 효과 타입, 관통 수, 폭발 범위, 유도 수치 추가 |

| Bullet.cs | Assets/Scripts/Bullet | 관통, 폭발, 유도 탄환 기능 추가 |

| CardManager.cs | Assets/Scripts/Card | 신규 카드 생성, 특수 탄환 초기화, 특수 강화 함수 추가 |

| RewardType.cs | Assets/Scripts/Reward | 신규 카드 획득 및 강화 보상 타입 추가 |

| RewardManager.cs | Assets/Scripts/Managers | 신규 카드 보상, 강화 보상, 보상 필터링 추가 |

| RewardButton.cs | Assets/Scripts/UI | 신규 카드 보상 분류 표시 추가 |

| CardSlotUI.cs | Assets/Scripts/UI | 신규 카드 역할과 특수 효과 수치 표시 추가 |



\---



\## 7. BulletEffectType 추가



탄환의 특수 효과를 구분하기 위해 BulletEffectType을 추가하였다.



| 타입 | 설명 |

|---|---|

| Normal | 일반 탄환 |

| Pierce | 관통 탄환 |

| Bomb | 폭발 탄환 |

| Homing | 유도 탄환 |



이 타입을 통해 하나의 Bullet 스크립트에서 여러 종류의 탄환 효과를 처리할 수 있도록 구성하였다.



\---



\## 8. CardData 확장



CardData에는 카드별 특수 탄환 효과를 저장하기 위한 변수를 추가하였다.



| 변수 | 설명 |

|---|---|

| bulletEffectType | 카드가 발사할 탄환 효과 타입 |

| pierceCount | 관통 가능한 적 수 |

| explosionRadius | 폭발 범위 |

| homingRange | 유도 탄환의 적 탐색 범위 |

| homingTurnSpeed | 유도 탄환의 방향 전환 속도 |



이를 통해 카드마다 서로 다른 탄환 효과를 설정할 수 있게 되었다.



\---



\## 9. Bullet.cs 수정 내용



Bullet.cs는 기존 일반 탄환 처리에서 확장되어 다음 기능을 담당하게 되었다.



| 기능 | 설명 |

|---|---|

| 일반 이동 | 지정된 방향으로 이동 |

| 생존 시간 처리 | lifeTime이 끝나면 자동 제거 |

| 적 충돌 처리 | EnemyHealth를 가진 적에게 피해 적용 |

| 관통 처리 | Pierce 타입일 경우 남은 관통 횟수만큼 유지 |

| 폭발 처리 | Bomb 타입일 경우 주변 적에게 추가 피해 |

| 유도 처리 | Homing 타입일 경우 가까운 적 방향으로 이동 방향 보정 |



\---



\## 10. Pierce Shot 구현



Pierce Shot은 적을 맞춰도 바로 사라지지 않고, 지정된 관통 횟수만큼 여러 적을 공격할 수 있는 카드이다.



동작 흐름은 다음과 같다.



```text

Pierce Shot 발사

→ 적과 충돌

→ 적에게 피해 적용

→ 남은 관통 횟수 감소

→ 관통 횟수가 남아 있으면 계속 이동

→ 관통 횟수가 0이 되면 제거

```



이 카드는 적이 일렬로 몰려올 때 효과적이다.



\---



\## 11. Bomb Shot 구현



Bomb Shot은 적중 시 주변 범위 안의 적들에게 추가 피해를 주는 카드이다.



동작 흐름은 다음과 같다.



```text

Bomb Shot 발사

→ 적과 충돌

→ 충돌한 적에게 피해 적용

→ 폭발 범위 안의 적 탐색

→ 주변 적에게 추가 피해 적용

→ 탄환 제거

```



현재는 폭발 이펙트 없이 기능만 구현하였다.  

추후 폭발 이펙트와 사운드를 추가하면 타격감을 높일 수 있다.



\---



\## 12. Homing Shot 구현



Homing Shot은 가까운 적을 탐색하고, 해당 적 방향으로 탄환 이동 방향을 조금씩 보정하는 카드이다.



동작 흐름은 다음과 같다.



```text

Homing Shot 발사

→ 유도 범위 안의 가장 가까운 적 탐색

→ 적이 있으면 이동 방향을 적 방향으로 보정

→ 적이 없으면 기존 방향으로 이동

→ 적중 시 피해 적용 후 제거

```



이 카드는 정확한 조준이 어려운 상황에서 유용하다.



\---



\## 13. CardManager 수정 내용



CardManager에서는 신규 카드 3종을 생성할 수 있도록 CreateCardByType을 확장하였다.



| 카드 | 생성 시 설정 |

|---|---|

| Pierce Shot | BulletEffectType.Pierce, pierceCount 3 |

| Bomb Shot | BulletEffectType.Bomb, explosionRadius 2.2 |

| Homing Shot | BulletEffectType.Homing, homingRange 7, homingTurnSpeed 4 |



또한 탄환을 발사할 때 기존 Initialize 대신 InitializeSpecial을 사용하여 카드의 특수 효과 정보를 Bullet로 전달하도록 수정하였다.



```text

CardManager

→ 카드 데이터 확인

→ 최종 데미지와 최종 탄속 계산

→ Bullet.InitializeSpecial 호출

→ Bullet이 카드 효과에 맞게 동작

```



\---



\## 14. 신규 카드 강화 함수



신규 카드의 특수 수치를 강화하기 위해 CardManager에 다음 함수를 추가하였다.



| 함수 | 역할 |

|---|---|

| UpgradeCardPierceCount | 관통 수 증가 |

| UpgradeCardExplosionRadius | 폭발 범위 증가 |

| UpgradeCardHomingTurnSpeed | 유도 회전 속도 증가 |



이를 통해 보상 시스템에서 신규 카드의 특수 성능을 강화할 수 있게 되었다.



\---



\## 15. RewardType 추가 내용



신규 카드 획득과 강화 보상을 위해 RewardType을 확장하였다.



| RewardType | 역할 |

|---|---|

| NewPierceShot | Pierce Shot 획득 |

| NewBombShot | Bomb Shot 획득 |

| NewHomingShot | Homing Shot 획득 |

| UpgradePierceShotDamage | Pierce Shot 데미지 강화 |

| UpgradePierceShotPierceCount | Pierce Shot 관통 수 강화 |

| UpgradeBombShotDamage | Bomb Shot 데미지 강화 |

| UpgradeBombShotRadius | Bomb Shot 폭발 범위 강화 |

| UpgradeHomingShotDamage | Homing Shot 데미지 강화 |

| UpgradeHomingShotTurnSpeed | Homing Shot 유도 회전 속도 강화 |



\---



\## 16. RewardManager 수정 내용



RewardManager에는 신규 카드 관련 보상을 추가하였다.



\### 신규 카드 획득 보상



| 보상 이름 | 효과 |

|---|---|

| New Card: Pierce Shot | Pierce Shot 카드 획득 |

| New Card: Bomb Shot | Bomb Shot 카드 획득 |

| New Card: Homing Shot | Homing Shot 카드 획득 |



\### 신규 카드 강화 보상



| 보상 이름 | 효과 |

|---|---|

| Upgrade: Pierce Shot | Pierce Shot 데미지 +8 |

| Upgrade: Pierce Core | Pierce Shot 관통 수 +1 |

| Upgrade: Bomb Shot | Bomb Shot 데미지 +10 |

| Upgrade: Bomb Radius | Bomb Shot 폭발 범위 +0.5 |

| Upgrade: Homing Shot | Homing Shot 데미지 +6 |

| Upgrade: Homing Core | Homing Shot 유도 회전 속도 +1 |



\---



\## 17. 보상 필터링 확장



신규 카드도 기존 카드와 동일하게 보유 상태에 따라 보상 등장 여부를 제어하였다.



| 조건 | 처리 |

|---|---|

| Pierce Shot 미보유 | New Card: Pierce Shot 등장 가능 |

| Pierce Shot 보유 | New Card: Pierce Shot 등장 불가 |

| Pierce Shot 보유 | Pierce Shot 강화 보상 등장 가능 |

| Bomb Shot 미보유 | Bomb Shot 강화 보상 등장 불가 |

| Homing Shot 미보유 | Homing Shot 강화 보상 등장 불가 |



이를 통해 보유하지 않은 카드의 강화 보상이 먼저 등장하는 문제를 방지하였다.



\---



\## 18. RewardButton 수정 내용



RewardButton의 보상 종류 분류에 신규 카드 보상을 추가하였다.



| 분류 | 포함된 신규 보상 |

|---|---|

| New Card | NewPierceShot, NewBombShot, NewHomingShot |

| Card Upgrade | Pierce, Bomb, Homing 관련 강화 보상 |



따라서 보상 버튼에는 신규 카드 보상도 기존과 동일하게 분류가 표시된다.



예시:



```text

\[Rare] \[New Card]

New Card: Pierce Shot



Add Pierce Shot to your deck

```



```text

\[Epic] \[Card Upgrade]

Upgrade: Bomb Radius



Bomb Shot Explosion Radius +0.5

```



\---



\## 19. CardSlotUI 수정 내용



CardSlotUI에는 신규 카드의 역할 문구와 특수 효과 표시를 추가하였다.



| 카드 | 역할 문구 |

|---|---|

| Pierce Shot | Pierce |

| Bomb Shot | Explosion |

| Homing Shot | Homing |



카드 상세 정보에는 다음 특수 수치가 추가로 표시된다.



| 탄환 효과 | 표시 정보 |

|---|---|

| Pierce | Pierce Count |

| Bomb | Explosion Radius |

| Homing | Homing Range, Turn Speed |



\---



\## 20. 테스트 결과



| 테스트 항목 | 결과 |

|---|---|

| 기존 카드 5종 발사 | 정상 |

| Pierce Shot 획득 | 정상 |

| Bomb Shot 획득 | 정상 |

| Homing Shot 획득 | 정상 |

| Pierce Shot 관통 기능 | 정상 |

| Bomb Shot 폭발 피해 | 정상 |

| Homing Shot 유도 기능 | 정상 |

| 신규 카드 보상 등장 | 정상 |

| 신규 카드 중복 획득 방지 | 정상 |

| 신규 카드 강화 보상 필터링 | 정상 |

| 신규 카드 상세 UI 표시 | 정상 |

| RewardButton 보상 분류 표시 | 정상 |

| 기존 보상 시스템 유지 | 정상 |



\---



\## 21. 발생한 문제와 해결



| 문제 | 원인 | 해결 |

|---|---|---|

| finalBulletSpeed 이름 오류 | 탄환 초기화 코드에서 최종 탄속 변수를 선언하지 않음 | finalBulletSpeed 변수를 생성하여 카드 기본 탄속과 보너스 탄속을 합산 |

| 특수 탄환 초기화 필요 | 기존 Initialize 함수는 특수 효과 정보를 받지 않음 | InitializeSpecial 함수를 추가하여 특수 효과 정보를 전달 |

| 신규 카드 강화 보상 조건 필요 | 보유하지 않은 신규 카드 강화 보상이 나올 수 있음 | IsRewardAvailable에 신규 카드 보유 조건 추가 |

| 신규 카드 보상 분류 필요 | RewardButton에서 신규 보상을 분류하지 못할 수 있음 | GetRewardCategoryText에 신규 카드 보상 타입 추가 |

| 카드 상세 UI 정보 부족 | 신규 카드의 특수 수치가 표시되지 않음 | CardSlotUI에 Effect, Pierce Count, Explosion Radius, Homing Range, Turn Speed 표시 추가 |



\---



\## 22. 현재 카드 목록



현재 구현된 카드 목록은 다음과 같다.



| 카드 | 역할 |

|---|---|

| Pixel Shot | 기본 다중 발사 |

| Focus Shot | 고화력 단일 발사 |

| Wide Shot | 넓은 범위 산탄 |

| Rapid Shot | 빠른 연사 |

| Heavy Shot | 강한 한 방 |

| Pierce Shot | 관통 공격 |

| Bomb Shot | 폭발 공격 |

| Homing Shot | 유도 공격 |



\---



\## 23. 현재 플레이 흐름



13일차 이후 현재 플레이 흐름은 다음과 같다.



```text

게임 시작

→ 카드 목록 확인

→ Battle 시작

→ 카드 선택

→ 일반 / 관통 / 폭발 / 유도 탄환 사용

→ 적 처치

→ 전투 클리어

→ 신규 카드 또는 강화 보상 선택

→ 카드 성능 변화

→ 카드 상세 UI 갱신

→ 다음 전투 진행

```



\---



\## 24. 현재 프로젝트 상태



13일차까지 완료된 핵심 시스템은 다음과 같다.



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

| 적 스폰 | 완료 |

| 적 종류 확장 | 완료 |

| 적 탄막 패턴 | 완료 |

| 신규 카드 3종 | 완료 |

| 관통 탄환 | 완료 |

| 폭발 탄환 | 완료 |

| 유도 탄환 | 완료 |

| 신규 카드 보상 연동 | 완료 |

| Game Over / Retry | 완료 |

| Prototype Clear | 완료 |



\---



\## 25. 다음 개발 목표



다음 개발에서는 신규 카드가 추가된 상태에서 전투 밸런스를 더 정리하거나, 카드와 유물 간 조합 효과를 확장하는 것이 좋다.



| 우선순위 | 다음 작업 |

|---:|---|

| 1 | 신규 카드 밸런스 조정 |

| 2 | 신규 카드 전용 유물 추가 |

| 3 | 카드와 유물 조합 효과 구현 |

| 4 | 보상 등장 확률 세부 조정 |

| 5 | 적 체력과 웨이브 난이도 재조정 |

| 6 | 카드 아이콘 UI 추가 |

| 7 | 폭발 / 관통 / 유도 이펙트 추가 |



\---



\## 26. 13일차 정리



13일차 작업을 통해 프로젝트 Q의 카드 시스템은 단순 수치 차이 중심에서, 전투 방식이 달라지는 카드 구조로 확장되었다.



이번 작업의 핵심 변화는 다음과 같다.



```text

기존 카드 구조

→ 데미지, 쿨타임, 탄환 수 차이 중심



변경 후 카드 구조

→ 관통, 폭발, 유도처럼 전투 방식 자체가 다른 카드 추가

```



이제 플레이어는 전투 상황에 따라 다음과 같은 선택을 할 수 있다.



```text

많은 적이 일렬로 몰려오면 Pierce Shot

적이 모여 있으면 Bomb Shot

조준이 어려운 상황에서는 Homing Shot

강한 단일 적에게는 Focus Shot 또는 Heavy Shot

넓은 범위는 Wide Shot

빠른 연사는 Rapid Shot

```



이번 작업은 이후 카드와 유물 조합, 보스전, 스테이지 진행 구조를 확장하기 위한 중요한 기반이 된다.

