\# Project Q 개발일지 - Day27



\## 개발 주제



방 프리팹 여러 개 확장 및 방 타입별 프리팹 선택 구현



\## 개발 목표



27일차의 목표는 기존에 Room\_Test\_01 하나만 반복 생성되던 구조를 수정하여, 방 타입에 따라 서로 다른 방 프리팹이 생성되도록 만드는 것이었다.



26일차까지는 랜덤 방 이동, 문 잠금, 전투방 적 스폰, 방 클리어 처리까지 구현하였다.  

이번 작업에서는 NormalBattle, EliteBattle, Event, Rest 등 RoomType에 따라 알맞은 방 프리팹을 선택하고 생성하는 구조를 추가하였다.



\## 구현 내용



\### 1. 방 프리팹 여러 개 제작



기존 Room\_Test\_01 프리팹을 복제하여 여러 방 프리팹을 제작하였다.



제작한 방 프리팹은 다음과 같다.



\- Room\_Normal\_01

\- Room\_Normal\_02

\- Room\_Elite\_01

\- Room\_Event\_01

\- Room\_Rest\_01



처음에는 Room\_Test\_01의 구조를 유지한 상태로 복제하였고, 이후 방 타입을 구분하기 쉽도록 임시 색상과 형태 차이를 적용하였다.



\### 2. 방 프리팹 공통 구조 유지



모든 방 프리팹이 기존 방 시스템과 호환되도록 공통 구조를 유지하였다.



공통 구조는 다음과 같다.



\- Grid

\- Doors

\- EntryPoints

\- SpawnPoints

\- ObstacleParent

\- EnemyParent

\- RoomObjectRandomizer

\- RoomController

\- RoomBattleController



방 프리팹이 달라져도 문 이동, 구조물 랜덤 배치, 전투 시작, 방 클리어 처리가 동일하게 작동하도록 구성하였다.



\### 3. 방 타입별 프리팹 배열 추가



RoomSceneController에 방 타입별 프리팹 배열을 추가하였다.



추가한 배열은 다음과 같다.



\- Start Room Prefabs

\- Normal Battle Room Prefabs

\- Elite Battle Room Prefabs

\- Event Room Prefabs

\- Rest Room Prefabs

\- Mid Boss Room Prefabs

\- Final Boss Room Prefabs



이제 RoomSceneController는 현재 RoomData의 RoomType을 확인하고, 해당 타입에 맞는 프리팹 배열에서 방을 선택한다.



\### 4. RoomData에 프리팹 인덱스 저장



같은 방에 다시 들어갔을 때 매번 다른 프리팹이 생성되는 문제를 막기 위해 RoomData에 roomPrefabIndex를 추가하였다.



roomPrefabIndex는 해당 방에 한 번 선택된 프리팹 인덱스를 저장한다.



이를 통해 아래와 같은 흐름이 가능해졌다.



처음 방 입장

→ 방 타입에 맞는 프리팹 배열 확인

→ 랜덤 프리팹 인덱스 선택

→ RoomData.roomPrefabIndex에 저장



다시 같은 방 입장

→ 저장된 roomPrefabIndex 사용

→ 이전과 같은 방 프리팹 생성



\### 5. RoomSceneController 수정



RoomSceneController가 더 이상 defaultRoomPrefab만 생성하지 않고, 현재 방 타입에 맞는 프리팹을 선택하도록 수정하였다.



수정된 흐름은 다음과 같다.



문으로 방 이동

→ RoomMapManager의 현재 방 좌표 변경

→ 현재 RoomData 가져오기

→ RoomType 확인

→ 방 타입에 맞는 프리팹 배열 선택

→ 저장된 roomPrefabIndex 확인

→ 선택된 방 프리팹 생성

→ RoomController.Initialize 실행

→ 플레이어 EntryPoint 이동



\### 6. 기본 프리팹 fallback 처리



특정 방 타입의 프리팹 배열이 비어 있거나 선택된 프리팹이 null인 경우를 대비하여 defaultRoomPrefab을 fallback으로 사용하도록 처리하였다.



이를 통해 프리팹 연결이 일부 누락되어도 게임이 즉시 멈추지 않고 기본 방을 생성할 수 있도록 하였다.



\### 7. 방 타입별 생성 확인



각 RoomType에 따라 다른 방 프리팹이 생성되는지 확인하였다.



확인한 타입은 다음과 같다.



\- Start

\- NormalBattle

\- EliteBattle

\- Event

\- Rest



NormalBattle 방에서는 Room\_Normal\_01 또는 Room\_Normal\_02가 생성되고, EliteBattle 방에서는 Room\_Elite\_01이 생성되는 것을 확인하였다.



Event 방과 Rest 방에서도 각각 Room\_Event\_01, Room\_Rest\_01이 생성되는 것을 확인하였다.



\### 8. 기존 전투 시스템 유지 확인



방 프리팹 선택 구조가 바뀌어도 26일차에 구현한 전투방 시스템이 정상적으로 작동하는지 확인하였다.



전투방 입장 시 다음 기능이 유지되는 것을 확인하였다.



\- 적 스폰

\- 문 잠금

\- 문 색상 변경

\- DoorBlocker Collider 활성화

\- 적 처치 후 방 클리어

\- 문 잠금 해제

\- 미니맵 클리어 상태 갱신



\### 9. Event / Rest 방 비전투 처리 확인



Event 방과 Rest 방은 전투방이 아니므로 적이 스폰되지 않고 문도 잠기지 않아야 한다.



RoomBattleController가 RoomData.IsBattleRoom()을 확인하기 때문에 Event와 Rest 방에서는 전투가 시작되지 않는 것을 확인하였다.



\### 10. 재입장 시 같은 프리팹 유지 확인



roomPrefabIndex를 저장한 덕분에 같은 방에 다시 들어갔을 때 이전과 같은 방 프리팹이 유지되는 것을 확인하였다.



이를 통해 랜덤 방 구조이면서도 한 번 생성된 방은 같은 층 안에서 일관성을 유지할 수 있게 되었다.



\## 수정 및 추가된 주요 파일



| 파일 또는 오브젝트 | 작업 내용 |

|---|---|

| RoomData.cs | roomPrefabIndex 추가 |

| RoomSceneController.cs | 방 타입별 프리팹 선택 로직 추가 |

| Room\_Test\_01 Prefab | 기본 fallback 방으로 유지 |

| Room\_Normal\_01 Prefab | 일반 전투방 프리팹 추가 |

| Room\_Normal\_02 Prefab | 일반 전투방 프리팹 추가 |

| Room\_Elite\_01 Prefab | 정예 전투방 프리팹 추가 |

| Room\_Event\_01 Prefab | 이벤트방 프리팹 추가 |

| Room\_Rest\_01 Prefab | 휴식방 프리팹 추가 |



\## 테스트 내용



\### 시작 방 생성 테스트



게임 시작 시 Start Room Prefabs에 연결된 방 프리팹이 정상적으로 생성되는지 확인하였다.



\### 일반 전투방 생성 테스트



NormalBattle 타입의 방에 들어갔을 때 Room\_Normal\_01 또는 Room\_Normal\_02가 생성되는지 확인하였다.



\### 정예 전투방 생성 테스트



EliteBattle 타입의 방에 들어갔을 때 Room\_Elite\_01이 생성되는지 확인하였다.



\### 이벤트방 생성 테스트



Event 타입의 방에 들어갔을 때 Room\_Event\_01이 생성되고, 적이 스폰되지 않는지 확인하였다.



\### 휴식방 생성 테스트



Rest 타입의 방에 들어갔을 때 Room\_Rest\_01이 생성되고, 전투가 시작되지 않는지 확인하였다.



\### 문 이동 유지 테스트



방 프리팹이 바뀌어도 문 자동 이동과 EntryPoint 배치가 정상적으로 작동하는지 확인하였다.



\### 전투방 기능 유지 테스트



NormalBattle과 EliteBattle 방에서 적 스폰, 문 잠금, 방 클리어 처리가 기존처럼 작동하는지 확인하였다.



\### 같은 방 재입장 테스트



한 번 방문한 방에 다시 들어갔을 때 같은 방 프리팹이 유지되는지 확인하였다.



\## 완료 결과



27일차 작업을 통해 Project Q의 방 생성 구조가 Room\_Test\_01 하나만 반복 사용하는 방식에서 방 타입별 프리팹을 선택하는 방식으로 확장되었다.



이제 방의 RoomType에 따라 일반 전투방, 정예 전투방, 이벤트방, 휴식방 프리팹이 구분되어 생성된다.



또한 같은 방에 다시 들어갔을 때 동일한 프리팹이 유지되도록 roomPrefabIndex를 저장하여, 랜덤성과 일관성을 함께 유지할 수 있게 되었다.



이번 작업을 통해 이후 지역별 방 프리팹 관리, 보스방 전용 프리팹, RoomPrefabDatabase 구조로 확장할 기반이 마련되었다.



\## 다음 개발 방향



28일차에는 RoomPrefabDatabase 구조를 구현한다.



주요 작업은 다음과 같다.



\- RoomPrefabDatabase ScriptableObject 생성

\- 지역별 방 프리팹 목록 관리

\- Forest, Beach, Desert, Snowfield, ServerRoom 지역 구분

\- RoomType과 RegionType을 함께 사용하여 방 프리팹 선택

\- RoomSceneController의 프리팹 배열 관리 구조 정리

