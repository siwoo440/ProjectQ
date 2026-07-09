\# Project Q 개발일지 - Day28



\## 개발 주제



RoomPrefabDatabase 기반 지역별 방 프리팹 관리 구조 구현



\## 개발 목표



28일차의 목표는 RoomSceneController가 직접 방 프리팹 배열을 관리하던 구조를 개선하여, RoomPrefabDatabase ScriptableObject에서 지역과 방 타입별 프리팹을 관리하도록 만드는 것이었다.



27일차까지는 RoomSceneController 안에 Start, NormalBattle, EliteBattle, Event, Rest, MidBoss, FinalBoss 방 프리팹 배열을 직접 넣는 방식이었다.



이번 작업에서는 이 구조를 데이터베이스 방식으로 분리하여, 이후 Forest, Beach, Desert, Snowfield, ServerRoom 지역별 방 프리팹을 쉽게 추가하고 관리할 수 있도록 확장하였다.



\## 구현 내용



\### 1. RoomPrefabSet 추가



지역 하나가 사용할 방 프리팹 목록을 저장하기 위해 RoomPrefabSet 클래스를 추가하였다.



RoomPrefabSet은 특정 RegionType에 대한 방 타입별 프리팹 배열을 가진다.



관리하는 방 타입은 다음과 같다.



\- Start

\- NormalBattle

\- EliteBattle

\- Event

\- Rest

\- MidBoss

\- FinalBoss



이를 통해 지역 하나마다 시작 방, 일반 전투방, 정예 전투방, 이벤트방, 휴식방, 보스방 프리팹을 따로 관리할 수 있게 되었다.



\### 2. RoomPrefabDatabase 추가



전체 지역의 방 프리팹 목록을 관리하기 위해 RoomPrefabDatabase ScriptableObject를 추가하였다.



RoomPrefabDatabase의 주요 역할은 다음과 같다.



\- 지역별 RoomPrefabSet 관리

\- RegionType과 RoomType을 기준으로 후보 프리팹 배열 반환

\- 특정 지역의 fallback 방 프리팹 반환

\- 전체 기본 fallback 방 프리팹 관리



이제 방 생성 로직은 RoomSceneController가 직접 배열을 들고 있는 방식이 아니라, RoomPrefabDatabase에 현재 지역과 방 타입을 전달하여 알맞은 프리팹을 받아오는 방식으로 변경되었다.



\### 3. RoomPrefabDatabase\_Main 에셋 생성



Unity Project 창에 RoomPrefabDatabase\_Main ScriptableObject 에셋을 생성하였다.



생성 위치는 다음과 같다.



\- Assets/ScriptableObject/RoomSystem/RoomPrefabDatabase\_Main



해당 에셋에는 Forest, Beach, Desert, Snowfield, ServerRoom 총 5개 지역에 대한 프리팹 세트를 등록하였다.



\### 4. 지역별 프리팹 세트 구성



RoomPrefabDatabase\_Main의 Region Prefab Sets 크기를 5로 설정하고, 각 Element를 지역별로 구성하였다.



구성한 지역은 다음과 같다.



\- Forest

\- Beach

\- Desert

\- Snowfield

\- ServerRoom



현재는 Forest 지역을 기준으로 기존 테스트 방 프리팹을 연결하였다.



아직 Beach, Desert, Snowfield, ServerRoom 전용 방 프리팹은 제작하지 않았기 때문에, 임시로 Forest에서 사용하던 방 프리팹을 연결하여 방 생성 오류가 나지 않도록 구성하였다.



\### 5. Forest 지역 방 프리팹 연결



Forest 지역에는 기존에 제작한 방 프리팹들을 연결하였다.



연결한 프리팹은 다음과 같다.



\- Room\_Test\_01

\- Room\_Normal\_01

\- Room\_Normal\_02

\- Room\_Elite\_01

\- Room\_Event\_01

\- Room\_Rest\_01



방 타입별 연결은 다음과 같다.



\- Start Room Prefabs: Room\_Test\_01

\- Normal Battle Room Prefabs: Room\_Normal\_01, Room\_Normal\_02

\- Elite Battle Room Prefabs: Room\_Elite\_01

\- Event Room Prefabs: Room\_Event\_01

\- Rest Room Prefabs: Room\_Rest\_01

\- Mid Boss Room Prefabs: 임시로 Room\_Elite\_01

\- Final Boss Room Prefabs: 임시로 Room\_Elite\_01



\### 6. 다른 지역 임시 프리팹 연결



Beach, Desert, Snowfield, ServerRoom 지역은 아직 전용 방 프리팹이 없으므로 Forest 지역의 방 프리팹을 임시로 연결하였다.



이를 통해 현재 층이 Forest 이후 지역으로 넘어가더라도 방 프리팹이 비어 있어서 오류가 발생하는 문제를 방지하였다.



이후 각 지역별 전용 방 프리팹을 제작하면 RoomPrefabDatabase\_Main에서 해당 지역의 프리팹만 교체하면 된다.



\### 7. RoomMapManager에 현재 지역 반환 함수 추가



RoomSceneController가 현재 지역 정보를 가져올 수 있도록 RoomMapManager에 GetCurrentRegionType 함수를 추가하였다.



이 함수는 현재 FloorData의 regionType을 반환한다.



이를 통해 현재 층이 Forest인지, Beach인지, Desert인지에 따라 RoomPrefabDatabase에서 다른 프리팹 세트를 가져올 수 있게 되었다.



\### 8. RoomSceneController 구조 수정



RoomSceneController에서 기존 방 타입별 프리팹 배열을 제거하고, RoomPrefabDatabase를 사용하는 구조로 수정하였다.



수정 전 구조는 다음과 같았다.



RoomSceneController

→ Start Room Prefabs 직접 보유

→ Normal Battle Room Prefabs 직접 보유

→ Elite Battle Room Prefabs 직접 보유

→ Event Room Prefabs 직접 보유

→ Rest Room Prefabs 직접 보유



수정 후 구조는 다음과 같다.



RoomSceneController

→ RoomPrefabDatabase 연결

→ 현재 RegionType 확인

→ 현재 RoomType 확인

→ RoomPrefabDatabase에서 후보 프리팹 배열 가져오기

→ RoomData.roomPrefabIndex 기준으로 프리팹 선택

→ 방 생성



\### 9. fallback 구조 유지



RoomPrefabDatabase에 프리팹이 비어 있거나 특정 지역, 특정 방 타입의 프리팹이 누락된 경우를 대비하여 fallback 구조를 유지하였다.



fallback 우선순위는 다음과 같다.



1\. 해당 지역의 Fallback Room Prefab

2\. RoomPrefabDatabase의 Global Fallback Room Prefab

3\. RoomSceneController의 Default Room Prefab



이를 통해 데이터 연결이 일부 비어 있어도 기본 방을 생성할 수 있게 하였다.



\### 10. 기존 방 프리팹 인덱스 유지



27일차에 추가했던 RoomData.roomPrefabIndex 구조는 그대로 유지하였다.



같은 방에 다시 들어갔을 때 매번 다른 프리팹이 생성되지 않고, 처음 선택된 프리팹이 유지되도록 하였다.



현재 흐름은 다음과 같다.



처음 방 입장

→ 현재 지역과 방 타입 확인

→ 후보 프리팹 배열 가져오기

→ 랜덤 인덱스 선택

→ RoomData.roomPrefabIndex 저장

→ 방 프리팹 생성



같은 방 재입장

→ 저장된 roomPrefabIndex 사용

→ 이전과 같은 방 프리팹 생성



\### 11. 기존 시스템 정상 작동 확인



RoomPrefabDatabase 구조로 변경한 뒤에도 기존 시스템이 정상적으로 작동하는지 확인하였다.



확인한 기능은 다음과 같다.



\- 문 자동 이동

\- 미니맵 갱신

\- 방 타입별 프리팹 선택

\- 일반 전투방 적 스폰

\- 정예 전투방 적 스폰

\- 전투 중 문 잠금

\- 문 색상 변경

\- 방 클리어 처리

\- Event / Rest 방 비전투 처리

\- 같은 방 재입장 시 같은 프리팹 유지



\## 수정 및 추가된 주요 파일



| 파일 또는 오브젝트 | 작업 내용 |

|---|---|

| RoomPrefabSet.cs | 지역 하나의 방 타입별 프리팹 목록 관리 클래스 추가 |

| RoomPrefabDatabase.cs | 지역과 방 타입 기준 프리팹을 반환하는 ScriptableObject 추가 |

| RoomMapManager.cs | 현재 지역을 반환하는 GetCurrentRegionType 함수 추가 |

| RoomSceneController.cs | 방 프리팹 직접 배열 관리 방식에서 RoomPrefabDatabase 사용 방식으로 수정 |

| RoomPrefabDatabase\_Main | 지역별 방 프리팹 관리 에셋 생성 |

| Room\_Test\_01 | Global / 지역 fallback 방으로 연결 |

| Room\_Normal\_01 / Room\_Normal\_02 | 일반 전투방 프리팹으로 연결 |

| Room\_Elite\_01 | 정예 및 임시 보스방 프리팹으로 연결 |

| Room\_Event\_01 | 이벤트방 프리팹으로 연결 |

| Room\_Rest\_01 | 휴식방 프리팹으로 연결 |



\## 테스트 내용



\### RoomPrefabDatabase 에셋 생성 테스트



RoomPrefabDatabase\_Main 에셋이 정상적으로 생성되고 Inspector에서 지역별 프리팹 세트를 설정할 수 있는지 확인하였다.



\### Forest 지역 프리팹 선택 테스트



현재 Forest 지역에서 Start, NormalBattle, EliteBattle, Event, Rest 방 타입에 맞는 프리팹이 정상적으로 선택되는지 확인하였다.



\### 일반 전투방 테스트



NormalBattle 방에 들어갔을 때 Room\_Normal\_01 또는 Room\_Normal\_02가 생성되는지 확인하였다.



\### 정예 전투방 테스트



EliteBattle 방에 들어갔을 때 Room\_Elite\_01이 생성되는지 확인하였다.



\### 이벤트방 테스트



Event 방에 들어갔을 때 Room\_Event\_01이 생성되고, 적이 스폰되지 않는지 확인하였다.



\### 휴식방 테스트



Rest 방에 들어갔을 때 Room\_Rest\_01이 생성되고, 전투가 시작되지 않는지 확인하였다.



\### fallback 테스트



특정 방 타입의 프리팹이 비어 있더라도 fallback 프리팹을 통해 기본 방이 생성될 수 있는 구조를 확인하였다.



\### 기존 전투 시스템 유지 테스트



RoomPrefabDatabase 구조로 변경한 뒤에도 전투방 입장, 적 스폰, 문 잠금, 방 클리어 처리가 정상적으로 작동하는지 확인하였다.



\### 재입장 테스트



같은 방에 다시 들어갔을 때 RoomData.roomPrefabIndex에 저장된 프리팹이 유지되는지 확인하였다.



\## 완료 결과



28일차 작업을 통해 Project Q의 방 프리팹 관리 구조가 RoomSceneController 중심의 직접 배열 방식에서 RoomPrefabDatabase 기반 데이터 관리 방식으로 변경되었다.



이제 RoomSceneController는 방 프리팹 목록을 직접 관리하지 않고, 현재 지역과 방 타입을 기준으로 RoomPrefabDatabase에서 알맞은 프리팹을 받아와 방을 생성한다.



이번 구조 덕분에 이후 Forest, Beach, Desert, Snowfield, ServerRoom 지역별 방 프리팹을 쉽게 추가할 수 있게 되었으며, 보스방과 다음 층 이동 구조를 구현할 때도 지역별 방 구분을 자연스럽게 확장할 수 있는 기반이 마련되었다.



\## 추가로 논의한 개선 방향



현재 방 이동 방식은 기존 방을 삭제하고 새 방을 생성한 뒤 플레이어를 EntryPoint로 이동시키는 방식이다.



이 방식은 기능적으로는 작동하지만, 실제로 옆방으로 이동한다기보다 방에 새로 소환되는 느낌이 강하다.



이를 개선하기 위해 아이작의 번제처럼 방이 실제 월드 좌표상에 나란히 배치되는 방식으로 변경하는 방향을 정리하였다.



개선 방향은 다음과 같다.



\- RoomData.gridPosition을 실제 월드 좌표로 변환

\- 방 하나의 크기를 roomWorldSize로 관리

\- 오른쪽 방은 실제로 오른쪽 위치에 생성

\- 왼쪽 방은 실제로 왼쪽 위치에 생성

\- 위쪽 방은 실제로 위쪽 위치에 생성

\- 아래쪽 방은 실제로 아래쪽 위치에 생성

\- 문 이동 시 방을 새로 소환하는 것이 아니라 옆방으로 카메라가 이동

\- 카메라가 부드럽게 슬라이드되어 아이작의 번제 같은 방 이동 연출 구현



이 작업은 다음 일차에서 진행할 예정이다.



\## 다음 개발 방향



29일차에는 실제 월드 좌표 기반 방 배치와 카메라 슬라이드 이동을 구현한다.



주요 작업은 다음과 같다.



\- RoomData.gridPosition을 월드 좌표로 변환

\- 방을 실제 월드 좌표에 배치

\- 현재 방과 인접 방을 미리 생성

\- 이미 생성된 방은 Dictionary로 관리

\- 문 이동 시 새 방 소환이 아니라 실제 옆방으로 이동

\- 카메라가 현재 방에서 다음 방으로 부드럽게 슬라이드

\- 플레이어 조작 잠금 처리

\- 현재 방 주변만 유지하고 먼 방은 정리

