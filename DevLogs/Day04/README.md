\# Project Q 개발 기록 - 4일차



\## 1. 개발 목표



4일차의 목표는 전투를 한 번만 진행하고 끝나는 구조에서 벗어나, 보상 선택 후 다음 전투로 이어지는 반복 전투 구조를 구현하는 것이다.



이전 단계에서는 전투 클리어 후 보상 선택까지 가능했지만, 보상 선택 이후 게임 진행이 멈추는 상태였다.



이번 작업에서는 Next Battle 버튼을 추가하여 보상 선택 후 다음 전투로 넘어갈 수 있게 만들고, 전투 번호에 따라 적 수와 체력이 증가하도록 구성하였다.



\---



\## 2. 구현한 기능



| 구분 | 구현 내용 |

|---|---|

| 전투 번호 관리 | 현재 Battle 번호를 저장하고 다음 전투마다 증가 |

| BattleProgressManager | 전체 전투 진행 흐름 관리 |

| Next Battle UI | 보상 선택 후 다음 전투 버튼 표시 |

| 전투 재시작 | Next Battle 버튼 클릭 시 다음 전투 시작 |

| 적 재생성 | 다음 전투 시작 시 새로운 적 생성 |

| 적 수 증가 | 전투 번호에 따라 적 수 증가 |

| 적 체력 증가 | 전투 번호에 따라 적 체력 증가 |

| 이전 오브젝트 정리 | 다음 전투 시작 시 남은 적과 탄환 삭제 |

| 경고 수정 | FindObjectsOfType을 FindObjectsByType으로 변경 |



\---



\## 3. 추가한 스크립트



| 파일명 | 위치 | 역할 |

|---|---|---|

| BattleProgressManager.cs | Assets/Scripts/Managers | 현재 전투 번호와 다음 전투 진행 관리 |

| NextBattleUI.cs | Assets/Scripts/UI | Next Battle 버튼 표시와 클릭 처리 |



\---



\## 4. 수정한 스크립트



| 파일명 | 수정 내용 |

|---|---|

| BattleManager.cs | 전투 번호를 받아 재시작할 수 있도록 수정 |

| EnemySpawner.cs | 전투 번호에 따라 적 수와 체력이 증가하도록 수정 |

| EnemyHealth.cs | 외부에서 최대 체력을 설정할 수 있도록 수정 |

| RewardManager.cs | 보상 선택 후 Next Battle 버튼을 표시하도록 수정 |

| NextBattleUI.cs | BattleProgressManager 연결 및 다음 전투 시작 처리 |

| BattleManager.cs | FindObjectsOfType 경고를 FindObjectsByType으로 수정 |



\---



\## 5. 현재 플레이 흐름



```text

게임 시작

→ Battle 1 시작

→ 적 3마리 생성

→ 모든 적 처치

→ BATTLE CLEAR 표시

→ 보상 선택 UI 표시

→ 보상 선택

→ Next Battle 버튼 표시

→ 버튼 클릭

→ Battle 2 시작

→ 적 4마리 생성

→ 적 체력 증가

→ 반복 진행

6\. 전투별 난이도 변화

전투 번호	적 수	적 체력

Battle 1	3	50

Battle 2	4	60

Battle 3	5	70

Battle 4	6	80

Battle 5	7	90

7\. 테스트 결과

테스트 항목	결과

Battle 1 시작	정상 작동

전투 클리어	정상 작동

보상 선택	정상 작동

Next Battle 버튼 표시	정상 작동

Battle 2 시작	정상 작동

적 수 증가	정상 작동

적 체력 증가	정상 작동

반복 전투	정상 작동

기존 탄환 정리	정상 작동

Unity 경고 수정	정상 반영

8\. 발생한 문제와 해결

문제	원인	해결

StartBattle 인수 오류	BattleProgressManager는 전투 번호를 넘기지만 BattleManager의 StartBattle이 인수를 받지 않음	StartBattle(int battleNumber) 형태로 수정

BattleProgressManager 미연결 오류	NextBattleUI에 BattleProgressManager가 연결되지 않음	Inspector에서 연결하고 자동 탐색 코드 추가

FindObjectsOfType 경고	최신 Unity에서 사용되지 않는 함수	FindObjectsByType으로 변경

9\. 다음 개발 목표

우선순위	개발 내용

1	전투 번호 UI 표시

2	플레이어 사망 및 Game Over 처리

3	보상 랜덤화

4	카드 보상 추가

5	유물 보상 추가

