\# Project Q 개발일지 - Day24



\## 개발 주제



Tilemap 기반 테스트 방 제작 및 실제 문 이동 구현



\## 개발 목표



24일차의 목표는 23일차에 구현한 랜덤 방 맵 데이터와 미니맵 시스템을 실제 게임 화면의 방 이동 구조와 연결하는 것이었다.



이전까지는 방향키 입력으로 데이터상의 현재 방 좌표만 이동하는 테스트 방식이었다.  

이번 작업에서는 Tilemap 기반 테스트 방 프리팹을 만들고, 플레이어가 실제 방 안에서 문으로 이동하면 옆방으로 넘어가는 구조를 구현하였다.



\## 구현 내용



\### 1. Tilemap 기반 Room\_Test\_01 제작



테스트용 방 프리팹인 Room\_Test\_01을 제작하였다.



Room\_Test\_01은 Tilemap 기반으로 구성하였으며, 방의 기본 바닥, 벽, 장식, 충돌 영역을 분리할 수 있도록 구조를 잡았다.



구성은 다음과 같다.



\- Room\_Test\_01

&#x20; - Grid

&#x20;   - Tilemap\_Floor

&#x20;   - Tilemap\_Wall

&#x20;   - Tilemap\_Decoration

&#x20;   - Tilemap\_Collision

&#x20; - Doors

&#x20; - EntryPoints

&#x20; - SpawnPoints



Tilemap\_Collision에는 Tilemap Collider 2D를 추가하여 보이지 않는 충돌 영역으로 사용할 수 있도록 준비하였다.



\### 2. 문 오브젝트 구성



Room\_Test\_01에 상하좌우 문 오브젝트를 추가하였다.



구성은 다음과 같다.



\- Door\_Top

\- Door\_Bottom

\- Door\_Left

\- Door\_Right



각 문에는 BoxCollider2D를 추가하고 Is Trigger를 활성화하였다.



처음에는 문이 빈 오브젝트라 Game 화면에서 위치 확인이 어려웠기 때문에, 각 문 아래에 Visual\_Door 오브젝트를 추가하여 테스트 중 문 위치를 확인할 수 있도록 하였다.



\### 3. EntryPoint 구성



방 이동 후 플레이어가 나타날 위치를 지정하기 위해 EntryPoint를 추가하였다.



구성은 다음과 같다.



\- Entry\_Default

\- Entry\_FromTop

\- Entry\_FromBottom

\- Entry\_FromLeft

\- Entry\_FromRight



처음에는 Entry\_Default 위치가 잘못 저장되어 플레이어가 0,0이 아닌 먼 좌표로 이동하는 문제가 있었지만, Room\_Test\_01 프리팹 내부의 EntryPoint 위치를 수정하여 해결하였다.



\### 4. RoomDirection 추가



방 이동 방향을 코드에서 관리하기 위해 RoomDirection enum을 추가하였다.



방향은 다음과 같다.



\- Up

\- Down

\- Left

\- Right



또한 RoomDirectionUtility를 통해 방향을 Vector2Int 좌표로 변환하거나, 반대 방향을 구할 수 있도록 하였다.



\### 5. RoomController 구현



Room\_Test\_01 프리팹 내부의 문과 EntryPoint를 관리하기 위해 RoomController를 추가하였다.



RoomController는 현재 방 기준으로 상하좌우에 연결된 방이 있는지 확인하고, 연결된 방향의 문만 활성화하도록 구성하였다.



또한 플레이어가 어느 방향에서 들어왔는지에 따라 알맞은 EntryPoint 위치로 배치되도록 하였다.



\### 6. RoomSceneController 구현



실제 방 프리팹 생성과 교체를 담당하는 RoomSceneController를 추가하였다.



RoomSceneController의 주요 기능은 다음과 같다.



\- 현재 RoomData에 맞는 방 프리팹 생성

\- 기존 방 프리팹 삭제

\- 새 방 프리팹 생성

\- RoomController 초기화

\- 플레이어를 EntryPoint 위치로 이동

\- RoomMapManager와 연결하여 미니맵 갱신



24일차에서는 아직 방 종류별 프리팹 선택은 구현하지 않고, 모든 방에서 Room\_Test\_01을 생성하는 방식으로 테스트하였다.



\### 7. RoomController 누락 오류 수정



처음 실행 시 다음 오류가 발생하였다.



RoomController is missing on room prefab.



원인은 Room\_Test\_01 프리팹의 루트 오브젝트에 RoomController가 붙어 있지 않았기 때문이었다.



Room\_Test\_01 루트 오브젝트에 RoomController를 추가하고, Door와 EntryPoint를 연결하여 문제를 해결하였다.



\### 8. 문 위치 확인용 Visual 추가



문이 빈 오브젝트로만 존재하면 Game 화면에서 위치와 작동 여부를 확인하기 어려웠다.



이를 해결하기 위해 각 문에 시각 표시용 Sprite 오브젝트를 추가하였다.



구성 예시는 다음과 같다.



\- Door\_Top

&#x20; - Visual\_Door\_Top

\- Door\_Bottom

&#x20; - Visual\_Door\_Bottom

\- Door\_Left

&#x20; - Visual\_Door\_Left

\- Door\_Right

&#x20; - Visual\_Door\_Right



RoomDoor의 Visual Root에 각 Visual 오브젝트를 연결하여, 연결된 방향의 문만 보이도록 구성하였다.



\### 9. F키 상호작용 방식에서 자동 이동 방식으로 변경



처음에는 문 근처에서 F키를 눌러 옆방으로 이동하는 방식으로 구현하였다.



하지만 실제 플레이 감각을 자연스럽게 만들기 위해, 플레이어가 문 Trigger에 닿으면 자동으로 옆방으로 넘어가는 방식으로 수정하였다.



변경 전 흐름은 다음과 같다.



플레이어가 문 근처로 이동

→ F키 입력

→ 옆방 이동



변경 후 흐름은 다음과 같다.



플레이어가 문에 닿음

→ 자동으로 옆방 이동

→ 새 방 생성

→ 플레이어가 반대쪽 입구에 배치



\### 10. 중복 방 이동 방지 처리



문 Trigger에 닿는 순간 이동이 여러 번 실행될 수 있기 때문에, RoomSceneController에 isChangingRoom 변수를 추가하였다.



이를 통해 방 이동 중에는 추가 이동 입력이나 Trigger 처리를 무시하도록 하였다.



또한 방 이동 후 짧은 대기 시간을 두어 연속 이동이 발생하지 않도록 처리하였다.



\### 11. 기존 방향키 테스트 이동 비활성화



23일차에는 WASD 또는 방향키로 미니맵상의 현재 방을 이동하는 테스트 기능을 사용하였다.



24일차부터는 실제 문 이동 방식으로 전환하였기 때문에 RoomMapManager의 키보드 테스트 이동 기능을 비활성화하였다.



이제 방 이동은 문 Trigger를 통해서만 이루어진다.



\## 수정 및 추가된 주요 파일



| 파일 또는 오브젝트 | 작업 내용 |

|---|---|

| RoomDirection.cs | 방 이동 방향 enum 및 방향 변환 함수 추가 |

| RoomDoor.cs | 문 Trigger 감지 및 자동 방 이동 처리 |

| RoomController.cs | 방 내부의 문과 EntryPoint 관리 |

| RoomSceneController.cs | 현재 방 프리팹 생성, 교체, 플레이어 배치 처리 |

| Room\_Test\_01 Prefab | Tilemap 기반 테스트 방 제작 |

| Door\_Top / Bottom / Left / Right | 상하좌우 문 오브젝트 추가 |

| EntryPoints | 방 이동 후 플레이어 위치 지정 |

| GameScene | RoomSceneController 및 RoomParent 추가 |



\## 테스트 내용



\### Room\_Test\_01 생성 테스트



GameScene 실행 시 RoomSceneController가 Room\_Test\_01 프리팹을 정상적으로 생성하는지 확인하였다.



\### RoomController 연결 테스트



Room\_Test\_01 프리팹의 루트 오브젝트에 RoomController가 정상적으로 연결되어 있는지 확인하였다.



\### 문 표시 테스트



각 문에 Visual 오브젝트를 추가하여 Game 화면에서 문 위치를 확인할 수 있도록 하였다.



\### 연결 방향 문 활성화 테스트



RoomMapManager의 방 데이터 기준으로 연결된 방향의 문만 활성화되는지 확인하였다.



\### EntryPoint 테스트



게임 시작 시 플레이어가 Entry\_Default 위치에 배치되는지 확인하였다.



방 이동 후에는 이동 방향에 따라 반대쪽 EntryPoint에 플레이어가 배치되는지 확인하였다.



\### 자동 문 이동 테스트



플레이어가 문 Trigger에 닿았을 때 F키 입력 없이 자동으로 옆방으로 이동하는지 확인하였다.



\### 미니맵 갱신 테스트



문을 통해 방을 이동했을 때 RoomMapManager의 현재 방 좌표가 변경되고, 미니맵의 현재 방 표시가 갱신되는지 확인하였다.



\## 완료 결과



24일차 작업을 통해 Project Q의 방 이동 시스템이 데이터 테스트 단계에서 실제 플레이 방식으로 확장되었다.



이제 플레이어는 GameScene 안에서 Room\_Test\_01 방을 기준으로 이동하며, 연결된 문을 통과하면 옆방으로 자동 이동할 수 있다.



아직 모든 방은 동일한 Room\_Test\_01 프리팹으로 생성되지만, 방 생성, 문 활성화, 플레이어 입장 위치 조정, 미니맵 갱신까지 연결되었다.



이를 통해 이후 여러 방 프리팹 확장, 구조물 랜덤 배치, 전투방 베리어, 적 스폰 시스템을 붙일 수 있는 기반이 마련되었다.



\## 다음 개발 방향



25일차에는 방 내부 구조물 랜덤 배치를 구현한다.



주요 작업은 다음과 같다.



\- RoomObjectRandomizer 생성

\- ObstacleSpawnPoints 중 일부를 랜덤 선택

\- 구조물 프리팹 랜덤 생성

\- 같은 Room\_Test\_01이라도 입장할 때마다 내부 구조가 조금씩 달라지도록 구현

\- 문 앞과 EntryPoint 근처에는 구조물이 생성되지 않도록 배치 규칙 정리

