\# Project Q 개발 기록 - 15일차



\## 1. 개발 목표



15일차의 목표는 기존의 단순 전투 반복 구조를 스테이지 노드 진행 구조로 확장하는 것이다.



기존에는 전투가 클리어되면 다음 Battle 번호로 바로 넘어가는 방식이었다.  

이번 작업에서는 전투를 `NormalBattle`, `EliteBattle`, `Rest`, `BossBattle` 같은 노드 단위로 관리할 수 있도록 기본 구조를 만들었다.



이를 통해 이후 노드맵 UI, 휴식 노드, 엘리트 전투, 보스전, 챕터 클리어 구조를 구현할 수 있는 기반을 마련하였다.



\---



\## 2. 구현한 핵심 기능



| 구분 | 구현 내용 |

|---|---|

| 스테이지 노드 타입 추가 | NormalBattle, EliteBattle, Rest, BossBattle, Event 타입 추가 |

| 스테이지 노드 데이터 추가 | 노드 이름, 노드 타입, 전투 번호, 클리어 여부, 해금 여부 저장 |

| StageManager 추가 | 현재 노드 진행, 다음 노드 이동, 챕터 클리어 관리 |

| StageInfoUI 추가 | 현재 챕터와 노드 정보를 화면에 표시 |

| Next Battle 흐름 수정 | 다음 전투 버튼 클릭 시 StageManager가 다음 노드 진행 |

| NextBattleUI 구조 수정 | 전체 HUD가 꺼지지 않도록 별도 Controller 구조로 정리 |

| Rest 노드 임시 처리 | 휴식 노드는 현재 자동 통과 처리 |

| Boss 노드 임시 처리 | 보스 노드는 현재 일반 전투처럼 실행 |

| Chapter Clear 처리 | 마지막 노드 이후 챕터 클리어 표시 |



\---



\## 3. 추가한 스크립트



| 파일명 | 위치 | 역할 |

|---|---|---|

| StageNodeType.cs | Assets/Scripts/Stage | 스테이지 노드 종류 정의 |

| StageNodeData.cs | Assets/Scripts/Stage | 스테이지 노드 하나의 데이터 저장 |

| StageManager.cs | Assets/Scripts/Stage | 전체 스테이지 진행 관리 |

| StageInfoUI.cs | Assets/Scripts/UI | 현재 노드 정보 UI 표시 |



\---



\## 4. 수정한 스크립트



| 파일명 | 위치 | 수정 내용 |

|---|---|---|

| NextBattleUI.cs | Assets/Scripts/UI | Next Battle 패널 표시 / 숨김 구조 수정 |

| StageManager.cs | Assets/Scripts/Stage | NextBattleUI 숨김 처리 추가, 노드 진행 구조 추가 |



\---



\## 5. StageNodeType 구현



스테이지 노드의 종류를 구분하기 위해 `StageNodeType`을 추가하였다.



| 타입 | 설명 |

|---|---|

| NormalBattle | 일반 전투 노드 |

| EliteBattle | 엘리트 전투 노드 |

| Rest | 휴식 노드 |

| BossBattle | 보스 전투 노드 |

| Event | 이벤트 노드 |



현재는 `NormalBattle`, `EliteBattle`, `BossBattle` 모두 기존 BattleManager의 전투 시작 함수를 사용한다.  

휴식 노드와 이벤트 노드는 아직 상세 기능이 없기 때문에 자동 통과하도록 처리하였다.



\---



\## 6. StageNodeData 구현



`StageNodeData`는 스테이지 노드 하나의 정보를 저장하는 클래스이다.



| 변수 | 설명 |

|---|---|

| nodeName | 노드 이름 |

| nodeType | 노드 종류 |

| battleNumber | BattleManager에 전달할 전투 번호 |

| isCleared | 노드 클리어 여부 |

| isUnlocked | 노드 해금 여부 |



이 구조를 통해 각 노드가 어떤 전투인지, 클리어되었는지, 다음으로 진행 가능한지 관리할 수 있게 되었다.



\---



\## 7. StageManager 구현



`StageManager`는 현재 챕터의 노드 진행을 관리한다.



주요 역할은 다음과 같다.



| 기능 | 설명 |

|---|---|

| 기본 노드 생성 | Chapter 1의 기본 노드 목록 생성 |

| Run 시작 | 첫 번째 노드부터 진행 시작 |

| 현재 노드 시작 | 노드 타입에 따라 전투, 휴식, 이벤트 처리 |

| 다음 노드 이동 | 현재 노드를 클리어 처리하고 다음 노드로 이동 |

| 현재 노드 재시작 | 현재 노드를 다시 시작 |

| 챕터 클리어 | 마지막 노드 이후 챕터 클리어 처리 |

| UI 갱신 | 현재 노드 정보를 StageInfoUI에 표시 |

| Next Battle UI 숨김 | 새 노드 시작 시 Next Battle 버튼 숨김 |



\---



\## 8. 기본 스테이지 노드 구성



15일차에서는 코드에서 기본 노드 목록을 자동 생성하도록 구성하였다.



현재 기본 구성은 다음과 같다.



| 순서 | 노드 이름 | 노드 타입 | 전투 번호 |

|---:|---|---|---:|

| 1 | First Contact | NormalBattle | 1 |

| 2 | Second Wave | NormalBattle | 2 |

| 3 | Elite Gate | EliteBattle | 3 |

| 4 | Temporary Shelter | Rest | 0 |

| 5 | Boss Gate | BossBattle | 4 |



현재 Rest 노드는 임시로 자동 통과하며, BossBattle 노드는 일반 전투처럼 실행된다.



\---



\## 9. StageInfoUI 구현



현재 진행 중인 노드 정보를 화면에 표시하기 위해 `StageInfoUI`를 추가하였다.



화면에는 다음 정보가 표시된다.



```text

Chapter 1

Node 1 / 5

Type: NormalBattle

Name: First Contact

Battle No: 1

```



이를 통해 현재 플레이어가 어느 노드에 있는지 확인할 수 있게 되었다.



\---



\## 10. 기존 전투 반복 구조 변경



기존 흐름은 다음과 같았다.



```text

Battle 1

→ 보상

→ Battle 2

→ 보상

→ Battle 3

```



15일차 이후 흐름은 다음과 같이 바뀌었다.



```text

Chapter 1 Start

→ Node 1: Normal Battle

→ 보상

→ Node 2: Normal Battle

→ 보상

→ Node 3: Elite Battle

→ 보상

→ Node 4: Rest

→ Node 5: Boss Battle

→ Chapter Clear

```



아직 노드맵 UI는 없지만, 코드상으로는 스테이지 노드 진행 구조가 만들어졌다.



\---



\## 11. Next Battle UI 구조 수정



처음에는 `NextBattleUI`에 `Canvas\_GameHUD`를 연결하여 UI 전체가 꺼지는 문제가 발생하였다.



문제 원인은 다음과 같다.



```text

NextBattleUI.Hide()

→ 연결된 오브젝트를 SetActive(false)

→ Canvas\_GameHUD를 연결하면 HUD 전체가 꺼짐

```



이를 해결하기 위해 `NextBattleUI\_Controller` 오브젝트를 따로 만들고, 실제로 켜고 끌 패널만 `Panel\_NextBattle`로 연결하였다.



최종 구조는 다음과 같다.



```text

Canvas\_GameHUD

├── RewardPanel

├── Panel\_NextBattle

│   └── Button\_NextBattle

└── NextBattleUI\_Controller

```



연결 구조는 다음과 같다.



| 오브젝트 | 컴포넌트 | 연결 |

|---|---|---|

| NextBattleUI\_Controller | NextBattleUI | Next Battle Panel = Panel\_NextBattle |

| RewardManager | Next Battle UI | NextBattleUI\_Controller |

| StageManager | Next Battle UI | NextBattleUI\_Controller |

| Button\_NextBattle | OnClick | StageManager.StartNextNode |



\---



\## 12. Next Battle 버튼 흐름



보상 선택 후 다음 전투로 넘어가는 흐름은 다음과 같다.



```text

전투 클리어

→ 보상 선택

→ RewardManager가 NextBattleUI.Show 호출

→ Panel\_NextBattle 표시

→ Next Battle 버튼 클릭

→ StageManager.StartNextNode 실행

→ Panel\_NextBattle 숨김

→ 다음 노드 시작

```



이 구조를 통해 전투 중에는 Next Battle 버튼이 보이지 않고, 보상 선택 후에만 표시되도록 수정하였다.



\---



\## 13. 발생한 문제와 해결



| 문제 | 원인 | 해결 |

|---|---|---|

| StageNodeData 모호성 오류 | StageNodeData.cs가 Stage 폴더와 Camera 폴더에 중복 생성됨 | Camera 폴더의 중복 StageNodeData.cs 삭제 |

| Next Battle 버튼이 계속 표시됨 | 다음 노드 시작 시 버튼을 숨기는 코드가 없음 | StageManager에서 StartCurrentNode 시작 시 HideNextBattleUI 호출 |

| Canvas 전체 UI가 꺼짐 | NextBattleUI에 Canvas\_GameHUD를 연결함 | NextBattleUI\_Controller와 Panel\_NextBattle 구조로 분리 |

| Next Battle 패널이 안 뜸 | 컨트롤러, 패널, RewardManager 연결이 꼬임 | NextBattleUI\_Controller에 Panel\_NextBattle 연결, RewardManager와 StageManager에 Controller 연결 |

| Rest 노드가 바로 지나감 | 아직 휴식 UI를 구현하지 않음 | 15일차에서는 임시 자동 통과 처리 |



\---



\## 14. 테스트 결과



| 테스트 항목 | 결과 |

|---|---|

| 게임 시작 시 Node 1 시작 | 정상 |

| StageInfoUI 표시 | 정상 |

| Node 1 클리어 후 보상 표시 | 정상 |

| 보상 선택 후 Next Battle 패널 표시 | 정상 |

| Next Battle 버튼 클릭 후 패널 숨김 | 정상 |

| Node 2 이동 | 정상 |

| EliteBattle 노드 이동 | 정상 |

| Rest 노드 자동 통과 | 정상 |

| BossBattle 노드 이동 | 정상 |

| 마지막 노드 이후 Chapter Clear 표시 | 정상 |

| HUD 전체가 꺼지는 문제 | 해결 |

| Console 빨간 오류 | 없음 |



\---



\## 15. 현재 프로젝트 흐름



15일차 이후 현재 프로젝트 흐름은 다음과 같다.



```text

게임 시작

→ Chapter 1 시작

→ Stage Node 1 진행

→ 전투 클리어

→ 보상 선택

→ Next Battle 버튼 표시

→ 다음 노드 이동

→ 일반 전투 / 엘리트 전투 / 휴식 / 보스 노드 진행

→ 마지막 노드 클리어

→ Chapter Clear

```



\---



\## 16. 현재 프로젝트 상태



15일차까지 완료된 핵심 시스템은 다음과 같다.



| 시스템 | 상태 |

|---|---|

| 플레이어 이동 / 회피 | 완료 |

| HP / MP / Shield | 완료 |

| 카드 발사 | 완료 |

| 다중 카드 선택 | 완료 |

| 카드 상세 UI | 완료 |

| 보상 시스템 | 완료 |

| 보상 등급 확률 | 완료 |

| 보상 UI 개선 | 완료 |

| 유물 시스템 | 완료 |

| 유물 UI 개선 | 완료 |

| 카드 전용 유물 | 완료 |

| 카드-유물 조합 효과 | 완료 |

| 신규 카드 3종 | 완료 |

| 관통 / 폭발 / 유도 탄환 | 완료 |

| 적 스폰 | 완료 |

| 적 탄막 패턴 | 완료 |

| Game Over / Retry | 완료 |

| Stage Node 구조 | 완료 |

| StageManager | 완료 |

| StageInfoUI | 완료 |

| Next Battle UI 흐름 수정 | 완료 |

| Chapter Clear 기본 처리 | 완료 |



\---



\## 17. 다음 개발 목표



다음 개발에서는 현재 코드상으로만 존재하는 스테이지 노드 구조를 실제 화면에서 볼 수 있도록 노드맵 UI를 구현하는 것이 좋다.



| 우선순위 | 다음 작업 |

|---:|---|

| 1 | 노드맵 / 스테이지 선택 UI 구현 |

| 2 | 현재 노드, 클리어 노드, 잠긴 노드 표시 |

| 3 | Normal / Elite / Rest / Boss 노드 아이콘 구분 |

| 4 | Rest 노드 상세 기능 구현 |

| 5 | EliteBattle과 BossBattle을 일반 전투와 분리 |

| 6 | 챕터 클리어 화면 개선 |



\---



\## 18. 15일차 정리



15일차 작업을 통해 프로젝트 Q는 단순히 전투를 반복하는 구조에서 벗어나, 스테이지 노드를 따라 진행되는 구조로 확장되었다.



이번 작업의 핵심 변화는 다음과 같다.



```text

기존:

Battle 1

→ Battle 2

→ Battle 3



변경 후:

Chapter 1

→ Normal Battle Node

→ Normal Battle Node

→ Elite Battle Node

→ Rest Node

→ Boss Battle Node

→ Chapter Clear

```



아직 노드맵 UI와 보스 전용 구조는 구현되지 않았지만, 이후 스테이지 선택, 휴식 노드, 보스전, 챕터 클리어 화면을 구현하기 위한 기반이 마련되었다.

```



