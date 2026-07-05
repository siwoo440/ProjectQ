\# Project Q 개발 기록 - 3일차



\## 1. 개발 목표



3일차의 목표는 전투 방 하나의 기본 흐름을 완성하는 것이다.



기존에는 적을 직접 씬에 배치해서 테스트했지만, 이번 작업에서는 전투 시작 시 적이 자동으로 생성되고, 모든 적을 처치하면 전투 클리어가 표시되며, 이후 보상 선택 UI가 등장하도록 구현하였다.



\---



\## 2. 구현한 기능



| 구분 | 구현 내용 |

|---|---|

| 전투 상태 관리 | Ready, Battle, Clear 상태 구조 추가 |

| BattleManager | 전투 시작, 적 수 관리, 전투 클리어 판정 담당 |

| EnemySpawner | SpawnPoint 위치에 적 프리팹 자동 생성 |

| 적 자동 등록 | 생성된 적을 BattleManager에 등록 |

| 적 사망 알림 | 적이 죽으면 BattleManager에 사망 알림 |

| 전투 클리어 | 살아있는 적 수가 0이면 전투 클리어 처리 |

| 클리어 UI | 화면 중앙에 BATTLE CLEAR 표시 |

| 보상 UI | 전투 클리어 후 보상 선택 UI 표시 |

| 보상 선택 | 보상 3개 중 1개 선택 가능 |

| 보상 적용 | 선택한 보상이 실제 플레이어 또는 카드 수치에 적용 |



\---



\## 3. 추가한 스크립트



| 파일명 | 위치 | 역할 |

|---|---|---|

| BattleManager.cs | Assets/Scripts/Managers | 전투 시작, 적 수 관리, 클리어 판정 |

| EnemySpawner.cs | Assets/Scripts/Enemy | 적 프리팹 생성 |

| BattleClearUI.cs | Assets/Scripts/UI | 전투 클리어 UI 표시 |

| RewardManager.cs | Assets/Scripts/Managers | 보상 UI 표시와 보상 적용 |

| RewardButton.cs | Assets/Scripts/UI | 보상 버튼 정보와 클릭 처리 |



\---



\## 4. 수정한 스크립트



| 파일명 | 수정 내용 |

|---|---|

| EnemyHealth.cs | 적 사망 시 BattleManager에 알림을 보내도록 수정 |

| BattleManager.cs | 전투 클리어 후 RewardManager를 호출하도록 수정 |

| CardManager.cs | 보상으로 픽셀 샷 데미지와 쿨타임을 변경할 수 있도록 수정 |

| PlayerStats.cs | 보상으로 최대 MP를 증가시킬 수 있도록 수정 |



\---



\## 5. 현재 플레이 흐름



```text

게임 시작

→ BattleManager가 전투 시작

→ EnemySpawner가 적 3마리 생성

→ 적들이 플레이어에게 접근

→ 적들이 탄환 발사

→ 플레이어가 픽셀 샷으로 적 처치

→ 모든 적 처치

→ BATTLE CLEAR 표시

→ 보상 선택 UI 표시

→ 보상 3개 중 1개 선택

→ 선택 보상 적용

→ 보상 UI 닫힘

6\. 보상 종류

보상 이름	효과

Pixel Shot Upgrade	Pixel Shot Damage +5

Quick Reload	Pixel Shot Cooldown -0.1s

Mana Expansion	Max MP +1

7\. 테스트 결과

테스트 항목	결과

적 자동 생성	정상 작동

적 이동	정상 작동

적 탄환 발사	정상 작동

적 사망 처리	정상 작동

적 수 감소	정상 작동

전투 클리어 판정	정상 작동

클리어 UI 표시	정상 작동

보상 UI 표시	정상 작동

보상 선택	정상 작동

보상 효과 적용	정상 작동

8\. 다음 개발 목표

우선순위	개발 내용

1	보상 선택 후 다음 전투 시작 구조

2	전투 방 반복 구조

3	카드 보상 추가

4	유물 보상 추가

5	노드 선택 화면 기초 구현

