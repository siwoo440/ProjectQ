\# Project Q 개발일지 - Day30



\## 개발 주제



보스방 포탈 및 다음층 이동 구조 구현



\## 개발 목표



30일차의 목표는 층의 끝방을 클리어했을 때 다음층으로 이동할 수 있는 포탈을 생성하고, 포탈 상호작용을 통해 새로운 층으로 넘어가는 구조를 구현하는 것이었다.



29일차까지는 실제 월드 좌표 기반 방 배치, 문 이동, 카메라 슬라이드, 전투방 입장, 적 스폰, 방 클리어 처리가 구현되어 있었다.



이번 작업에서는 방 단위 진행을 층 단위 진행으로 확장하여, 끝방 클리어 후 다음층으로 이동하는 기본 흐름을 만들었다.



\## 구현 내용



\### 1. 층 끝방 표시 구조 추가



RoomData에 현재 방이 층의 끝방인지 확인할 수 있는 isEndRoom 변수를 추가하였다.



이 변수는 해당 방이 현재 층에서 가장 먼 방인지, 그리고 클리어 후 다음층 포탈을 생성해야 하는 방인지 판단하는 데 사용된다.



기본값은 false이며, RoomMapManager가 층을 생성할 때 가장 먼 방을 찾은 뒤 true로 설정한다.



\### 2. RoomMapManager 끝방 지정 처리



RoomMapManager의 끝방 지정 로직에서 가장 먼 방을 찾은 뒤 isEndRoom을 true로 설정하도록 수정하였다.



층 타입에 따른 끝방 타입은 다음과 같이 유지하였다.



\- 일반층: EliteBattle

\- 중간보스층: MidBoss

\- 최종보스층: FinalBoss



이를 통해 일반층에서는 정예방을 클리어하면 포탈이 생성되고, 중간보스층과 최종보스층에서는 보스방을 클리어하면 포탈이 생성될 수 있게 하였다.



\### 3. 최대 층 번호 설정



RoomMapManager에 maxFloorNumber 값을 추가하였다.



현재 최대 층은 15층으로 설정하였다.



이를 통해 현재 층이 최대 층보다 낮으면 다음층으로 이동하고, 최대 층을 클리어하면 이후 Game Clear 흐름으로 연결할 수 있는 기반을 만들었다.



\### 4. 다음층 이동 함수 추가



RoomMapManager에 다음층 이동 관련 함수를 추가하였다.



추가한 기능은 다음과 같다.



\- CanGoNextFloor

\- GoToNextFloor



CanGoNextFloor는 현재 층이 최대 층보다 낮은지 확인한다.  

GoToNextFloor는 현재 층 번호를 1 증가시키고, 새로운 층의 랜덤 방 맵을 다시 생성한다.



\### 5. RoomPortal 스크립트 추가



다음층 이동 포탈을 담당하는 RoomPortal 스크립트를 추가하였다.



RoomPortal의 주요 기능은 다음과 같다.



\- 플레이어가 포탈 범위에 들어왔는지 감지

\- F키 입력 확인

\- 중복 사용 방지

\- RoomSceneController에 다음층 이동 요청

\- 임시 안내 로그 출력



포탈은 Trigger Collider를 사용하여 플레이어가 가까이 왔는지 확인한다.



\### 6. Portal\_NextFloor 프리팹 제작



다음층 이동에 사용할 Portal\_NextFloor 프리팹을 제작하였다.



구성은 다음과 같다.



\- Portal\_NextFloor

&#x20; - Visual\_Portal



Portal\_NextFloor에는 CircleCollider2D를 추가하고 Is Trigger를 활성화하였다.



또한 RoomPortal 스크립트를 연결하여 플레이어가 포탈 범위 안에서 F키를 누르면 다음층 이동이 실행되도록 하였다.



\### 7. PortalSpawnPoint 추가



끝방 프리팹에 포탈이 생성될 위치를 지정하기 위해 PortalSpawnPoint를 추가하였다.



현재는 일반층 끝방과 임시 보스방으로 사용 중인 Room\_Elite\_01에 PortalSpawnPoint를 추가하였다.



이후 MidBoss 전용 방, FinalBoss 전용 방을 따로 만들면 해당 프리팹에도 같은 구조를 추가할 예정이다.



\### 8. RoomBattleController에 포탈 생성 기능 추가



RoomBattleController에 전투 클리어 후 포탈을 생성하는 기능을 추가하였다.



추가한 주요 항목은 다음과 같다.



\- Next Floor Portal Prefab

\- Portal Spawn Point

\- Spawned Portal Object



방 클리어 시 현재 방이 isEndRoom이면 PortalSpawnPoint 위치에 Portal\_NextFloor를 생성한다.



이미 포탈이 생성되어 있으면 중복 생성하지 않도록 처리하였다.



\### 9. 기존 포탈 정리 처리



방 전투 초기화 시 기존 포탈을 정리하는 ClearSpawnedPortal 기능을 추가하였다.



이를 통해 같은 방이 다시 초기화되거나 층이 넘어갈 때 이전 포탈이 남아 있는 문제를 방지하였다.



\### 10. RoomSceneController에 다음층 이동 처리 추가



RoomSceneController에 포탈에서 호출할 다음층 이동 함수를 추가하였다.



추가한 함수는 다음과 같다.



\- MoveToNextFloorFromPortal

\- MoveToNextFloorRoutine

\- ClearAllSpawnedRooms

\- RecoverPlayerForNextFloor



포탈을 사용하면 RoomSceneController가 기존 층의 방 오브젝트를 모두 제거하고, RoomMapManager를 통해 다음층 맵을 생성한다.



그 후 새 층의 시작 방과 인접 방을 다시 로드하고, 플레이어와 카메라를 시작 방 위치로 이동시킨다.



\### 11. 기존 방 전체 정리 기능 추가



다음층으로 넘어갈 때 이전 층의 방 오브젝트가 남지 않도록 ClearAllSpawnedRooms 함수를 추가하였다.



이 함수는 현재 생성되어 있는 모든 방 오브젝트를 제거하고, 방 Dictionary와 현재 방 컨트롤러 참조를 초기화한다.



이를 통해 층이 바뀔 때 이전 층과 새 층의 방 데이터가 섞이지 않도록 하였다.



\### 12. 다음층 시작 위치 이동 처리



다음층 이동 후 플레이어는 새 층의 시작 방 Entry\_Default 위치로 이동한다.



카메라도 새 시작 방 위치로 즉시 이동하도록 처리하였다.



이번 단계에서는 다음층 이동 시 카메라 슬라이드 연출보다, 층 전환이 명확하게 되는 것을 우선하였다.



\### 13. 다음층 회복 요청 처리



RoomSceneController에서 다음층 이동 시 플레이어에게 RecoverForNextFloor 메시지를 보내도록 처리하였다.



이 함수는 PlayerStats에 구현되어 있으면 실행되고, 없으면 에러 없이 무시된다.



다음층 이동 규칙은 다음과 같다.



\- HP 전부 회복

\- MP 전부 회복

\- Shield 유지

\- 카드 유지

\- 유물 유지

\- 카드 강화 유지



Shield는 유지해야 하므로 회복 함수에서 Shield 값은 건드리지 않는 방향으로 정리하였다.



\### 14. PlayerStats 회복 함수 추가



PlayerStats에 RecoverForNextFloor 함수를 추가하여 다음층 이동 시 HP와 MP가 회복되도록 하였다.



현재 플레이어 스탯 변수명에 맞춰 HP와 MP를 최대값으로 복구하도록 처리하였다.



\### 15. 보스 프리팹 연결 준비



RoomBattleController에는 이미 Mid Boss Prefabs와 Final Boss Prefabs 배열이 있기 때문에, 보스방 타입에서는 해당 배열의 프리팹을 생성할 수 있다.



이번 단계에서는 테스트용으로 기존 Boss\_Test 또는 강한 적 프리팹을 연결할 수 있도록 구조를 준비하였다.



이후 MidBoss, FinalBoss 전용 프리팹과 보스 패턴을 연결하면 보스방 전투 구조로 확장할 수 있다.



\## 수정 및 추가된 주요 파일



| 파일 또는 오브젝트 | 작업 내용 |

|---|---|

| RoomData.cs | isEndRoom 변수 추가 |

| RoomMapManager.cs | 끝방 표시, 최대 층 번호, 다음층 이동 함수 추가 |

| RoomPortal.cs | 포탈 상호작용 및 다음층 이동 요청 스크립트 추가 |

| RoomBattleController.cs | 끝방 클리어 시 포탈 생성 기능 추가 |

| RoomSceneController.cs | 포탈 기반 다음층 이동, 기존 방 전체 정리, 새 층 로드 기능 추가 |

| PlayerStats.cs | 다음층 이동 시 HP / MP 회복 함수 추가 |

| Portal\_NextFloor Prefab | 다음층 이동 포탈 프리팹 제작 |

| Room\_Elite\_01 Prefab | PortalSpawnPoint 추가 및 포탈 생성 위치 연결 |



\## 테스트 내용



\### 끝방 표시 테스트



RoomMapManager가 층을 생성할 때 가장 먼 방을 isEndRoom으로 표시하는지 확인하였다.



\### 일반층 끝방 테스트



1층 일반층에서 가장 먼 EliteBattle 방을 클리어했을 때 포탈이 생성되는지 확인하였다.



\### 포탈 생성 테스트



방 클리어 후 PortalSpawnPoint 위치에 Portal\_NextFloor가 생성되는지 확인하였다.



\### 포탈 상호작용 테스트



플레이어가 포탈 범위에 들어간 뒤 F키를 눌렀을 때 다음층 이동 요청이 정상적으로 실행되는지 확인하였다.



\### 다음층 이동 테스트



포탈 사용 후 기존 층 방 오브젝트가 제거되고, 다음층의 랜덤 방 맵이 새로 생성되는지 확인하였다.



\### 새 시작 방 배치 테스트



다음층 이동 후 플레이어가 새 층의 시작 방 Entry\_Default 위치에 배치되는지 확인하였다.



\### 카메라 위치 테스트



다음층 이동 후 카메라가 새 시작 방 위치로 이동하는지 확인하였다.



\### 미니맵 갱신 테스트



다음층 이동 후 미니맵이 새 층 기준으로 갱신되는지 확인하였다.



\### 회복 테스트



다음층 이동 시 RecoverForNextFloor 함수가 호출되어 HP와 MP가 회복되는지 확인하였다.



\### Shield 유지 확인



다음층 이동 시 Shield 값은 초기화하지 않고 유지하는 방향으로 정리하였다.



\## 완료 결과



30일차 작업을 통해 Project Q의 방 단위 전투 구조가 층 단위 진행 구조로 확장되었다.



이제 층의 끝방을 클리어하면 다음층 이동 포탈이 생성되고, 플레이어가 포탈 근처에서 F키를 누르면 새로운 층으로 이동할 수 있다.



다음층 이동 시 기존 층의 방 오브젝트는 정리되고, 새 층의 랜덤 방 맵이 생성되며, 플레이어는 시작 방에서 다시 진행하게 된다.



이번 작업으로 Project Q의 로그라이크 진행 흐름은 다음 구조를 갖게 되었다.



층 시작

→ 방 탐색

→ 전투방 클리어

→ 끝방 도달

→ 끝방 클리어

→ 포탈 생성

→ 다음층 이동



\## 다음 개발 방향



31일차에는 보스방 전용 연출과 보스 UI를 강화한다.



주요 작업은 다음과 같다.



\- MidBoss / FinalBoss 전용 방 프리팹 제작

\- 보스방 입장 시 보스 이름 표시

\- 오른쪽 위 보스 초상화 UI 표시

\- 보스 체력바 연결

\- MidBoss / FinalBoss 구분 표시

\- 최종층 클리어 시 Game Clear UI 연결 준비

