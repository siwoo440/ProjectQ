\# Project Q 개발일지 - Day25



\## 개발 주제



방 내부 구조물 랜덤 배치 구현



\## 개발 목표



25일차의 목표는 Tilemap 기반 테스트 방인 Room\_Test\_01 안에 구조물이 랜덤으로 배치되도록 만드는 것이었다.



24일차까지는 Room\_Test\_01 프리팹 생성, 문 자동 이동, EntryPoint 배치, 미니맵 갱신까지 구현하였다.  

이번 작업에서는 같은 방 프리팹을 사용하더라도 방에 들어갈 때마다 내부 구조물 배치가 달라지도록 하여, 반복되는 방 이동의 단조로움을 줄이는 기반을 만들었다.



\## 구현 내용



\### 1. RoomObjects 프리팹 폴더 생성



방 내부에 배치할 구조물 프리팹을 관리하기 위해 RoomObjects 폴더를 생성하였다.



생성 위치는 다음과 같다.



\- Assets/Prefabs/RoomObjects



이 폴더에는 방 안에 랜덤으로 생성될 구조물 프리팹들을 저장하였다.



\### 2. 테스트용 구조물 프리팹 제작



랜덤 배치 테스트를 위해 기본 구조물 프리팹을 제작하였다.



제작한 구조물은 다음과 같다.



\- Obstacle\_Box

\- Obstacle\_Pillar

\- Obstacle\_Rock



각 구조물은 Sprite Renderer를 사용한 단순한 사각형 형태로 만들었고, 플레이어가 통과하지 못하도록 BoxCollider2D를 추가하였다.



\### 3. 구조물 Collider 설정



구조물 프리팹에는 충돌 처리를 위해 BoxCollider2D를 추가하였다.



설정은 다음과 같다.



\- Is Trigger 비활성화

\- 플레이어가 통과하지 못하는 장애물로 사용

\- 방 내부 랜덤 배치 테스트용으로 활용



이를 통해 구조물이 단순 장식이 아니라 실제 이동을 막는 오브젝트로 동작하도록 하였다.



\### 4. Room\_Test\_01에 ObstacleParent 추가



Room\_Test\_01 프리팹 내부에 생성된 구조물을 정리하기 위한 ObstacleParent 오브젝트를 추가하였다.



구조는 다음과 같다.



\- Room\_Test\_01

&#x20; - ObstacleParent



방이 생성될 때 랜덤으로 배치된 구조물들은 모두 ObstacleParent 아래에 생성되도록 구성하였다.



이를 통해 생성된 구조물들을 Hierarchy에서 쉽게 확인하고 관리할 수 있게 되었다.



\### 5. ObstacleSpawnPoints 배치



Room\_Test\_01 안에 구조물 생성 위치를 미리 지정하기 위해 ObstacleSpawnPoints를 구성하였다.



구조는 다음과 같다.



\- SpawnPoints

&#x20; - ObstacleSpawnPoints

&#x20;   - ObstacleSpawn\_01

&#x20;   - ObstacleSpawn\_02

&#x20;   - ObstacleSpawn\_03

&#x20;   - ObstacleSpawn\_04

&#x20;   - ObstacleSpawn\_05

&#x20;   - ObstacleSpawn\_06

&#x20;   - ObstacleSpawn\_07

&#x20;   - ObstacleSpawn\_08



구조물은 완전한 랜덤 좌표가 아니라, 미리 지정한 SpawnPoint 중 일부를 랜덤으로 선택하여 생성되도록 하였다.



이 방식은 문 앞이나 플레이어 입장 위치가 막히는 문제를 방지하기 위한 구조이다.



\### 6. 구조물 배치 금지 위치 정리



구조물 SpawnPoint는 다음 위치를 피해서 배치하였다.



\- 문 바로 앞

\- EntryPoint 근처

\- 플레이어가 방을 이동하는 주요 경로

\- 방 이동을 막을 수 있는 좁은 통로



자동 문 이동 구조를 사용하고 있기 때문에 문 앞에 구조물이 생성되면 방 이동이 불편해질 수 있다.  

따라서 구조물은 방 중앙부와 측면 여유 공간 위주로 배치되도록 구성하였다.



\### 7. RoomObjectRandomizer 구현



방 내부 구조물 랜덤 배치를 담당하는 RoomObjectRandomizer 스크립트를 추가하였다.



RoomObjectRandomizer의 주요 기능은 다음과 같다.



\- ObstacleSpawnPoints 목록 수집

\- 구조물 생성 개수 랜덤 결정

\- SpawnPoint 순서 랜덤 섞기

\- 같은 위치에 중복 생성 방지

\- 구조물 프리팹 중 하나를 랜덤 선택

\- 선택된 위치에 구조물 생성

\- 생성된 구조물을 ObstacleParent 아래에 정리



구조물 생성 개수는 최소값과 최대값 사이에서 랜덤으로 결정되도록 하였다.



\### 8. 구조물 생성 개수 설정



RoomObjectRandomizer에서 구조물 생성 개수를 설정할 수 있도록 하였다.



현재 기준은 다음과 같다.



\- Min Obstacle Count: 2

\- Max Obstacle Count: 5



이를 통해 방이 너무 비어 보이지 않으면서도, 구조물이 지나치게 많아 이동이 막히지 않도록 조절하였다.



\### 9. RoomController와 Randomizer 연결



Room\_Test\_01의 RoomController에 RoomObjectRandomizer 참조를 추가하였다.



방이 생성되고 RoomController.Initialize가 실행될 때 RoomObjectRandomizer.RandomizeObjects를 호출하도록 수정하였다.



수정 후 흐름은 다음과 같다.



Room\_Test\_01 생성

→ RoomController.Initialize 실행

→ 문 연결 상태 설정

→ RoomObjectRandomizer 실행

→ 구조물 랜덤 배치



\### 10. 방 이동 시 구조물 재배치 확인



문을 통해 다른 방으로 이동하면 기존 Room\_Test\_01이 삭제되고 새 Room\_Test\_01이 생성된다.



이때 RoomObjectRandomizer가 다시 실행되기 때문에, 방을 이동할 때마다 구조물 배치가 새롭게 바뀌는 것을 확인하였다.



현재는 같은 Room\_Test\_01 프리팹을 반복해서 사용하지만, 구조물 배치가 달라지기 때문에 방마다 약간 다른 내부 형태를 가지는 것처럼 보이게 되었다.



\## 수정 및 추가된 주요 파일



| 파일 또는 오브젝트 | 작업 내용 |

|---|---|

| RoomObjectRandomizer.cs | 방 내부 구조물 랜덤 배치 기능 구현 |

| RoomController.cs | RoomObjectRandomizer 연결 및 방 초기화 시 실행 |

| Obstacle\_Box Prefab | 테스트용 구조물 프리팹 생성 |

| Obstacle\_Pillar Prefab | 테스트용 기둥 구조물 프리팹 생성 |

| Obstacle\_Rock Prefab | 테스트용 바위 구조물 프리팹 생성 |

| Room\_Test\_01 Prefab | ObstacleParent 및 ObstacleSpawnPoints 추가 |

| Assets/Prefabs/RoomObjects | 구조물 프리팹 저장 폴더 생성 |



\## 테스트 내용



\### 구조물 프리팹 생성 테스트



Obstacle\_Box, Obstacle\_Pillar, Obstacle\_Rock 프리팹이 정상적으로 생성되었는지 확인하였다.



\### Collider 테스트



각 구조물에 BoxCollider2D가 적용되어 플레이어가 통과하지 못하는지 확인하였다.



\### 구조물 랜덤 생성 테스트



GameScene 실행 시 Room\_Test\_01 내부에 구조물이 랜덤으로 생성되는지 확인하였다.



\### ObstacleParent 정리 테스트



생성된 구조물들이 Room\_Test\_01(Clone)의 ObstacleParent 아래에 정리되는지 확인하였다.



\### 중복 위치 방지 테스트



같은 SpawnPoint에 여러 구조물이 겹쳐 생성되지 않는지 확인하였다.



\### 방 이동 후 재배치 테스트



문을 통해 다른 방으로 이동했을 때 새 Room\_Test\_01이 생성되고, 구조물 배치가 다시 랜덤으로 바뀌는지 확인하였다.



\### 문 이동 방해 여부 테스트



구조물이 문 앞이나 EntryPoint 근처에 생성되지 않아 방 이동을 방해하지 않는지 확인하였다.



\## 완료 결과



25일차 작업을 통해 Project Q의 방 내부 랜덤 구조물 배치 기능이 구현되었다.



이제 Room\_Test\_01이 생성될 때마다 ObstacleSpawnPoints 중 일부가 선택되고, 선택된 위치에 구조물이 랜덤으로 생성된다.



아직 방 프리팹은 하나만 사용하고 있지만, 구조물 배치가 매번 달라지기 때문에 같은 방이라도 조금씩 다른 형태로 느껴지게 되었다.



이번 작업을 통해 이후 여러 방 프리팹 제작, 지역별 방 구성, 전투방 베리어, 적 스폰 시스템을 붙일 수 있는 기반이 강화되었다.



\## 다음 개발 방향



26일차에는 전투방 베리어와 적 스폰, 방 클리어 처리를 구현한다.



주요 작업은 다음과 같다.



\- 전투방 입장 시 문 베리어 활성화

\- 전투 중 방 이동 금지

\- RoomData의 방 타입에 따라 적 스폰

\- 적을 모두 처치하면 방 클리어 처리

\- RoomData.isCleared 갱신

\- 방 클리어 후 문 베리어 해제

