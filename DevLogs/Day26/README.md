\# Project Q 개발일지 - Day26



\## 개발 주제



전투방 베리어, 적 스폰, 방 클리어 처리 구현



\## 개발 목표



26일차의 목표는 방에 입장했을 때 전투가 시작되고, 적을 모두 처치하기 전까지 다른 방으로 이동하지 못하도록 만드는 것이었다.



25일차까지는 랜덤 방 데이터 생성, 미니맵 표시, Tilemap 기반 Room\_Test\_01 생성, 문 자동 이동, 구조물 랜덤 배치까지 구현하였다.  

이번 작업에서는 전투방에 들어갔을 때 문이 잠기고, 적이 스폰되며, 적을 모두 처치하면 방이 클리어되고 다시 이동할 수 있는 구조를 추가하였다.



\## 구현 내용



\### 1. RoomMapManager 설정 변경



전투방 테스트를 위해 RoomMapManager의 테스트용 자동 클리어 기능을 비활성화하였다.



변경한 설정은 다음과 같다.



\- Enable Keyboard Test Movement 비활성화

\- Auto Clear Entered Room For Test 비활성화



이제 방 이동은 방향키 테스트가 아니라 실제 문 이동으로만 진행되고, 전투방에 입장해도 자동으로 클리어되지 않는다.



\### 2. 미니맵 갱신 함수 공개



RoomData의 클리어 상태가 변경된 뒤 미니맵을 다시 갱신할 수 있도록 RoomMapManager의 RefreshMinimap 함수를 public으로 수정하였다.



이를 통해 전투방 클리어 후 RoomData.isCleared 값이 true로 변경되면 미니맵에도 즉시 반영될 수 있게 하였다.



\### 3. RoomBattleController 구현



방 전투를 관리하기 위한 RoomBattleController 스크립트를 추가하였다.



RoomBattleController의 주요 기능은 다음과 같다.



\- 현재 방 데이터 확인

\- 전투방 여부 확인

\- 이미 클리어한 방인지 확인

\- 전투방 입장 시 문 잠금

\- EnemySpawnPoints 위치에 적 생성

\- 살아있는 적 목록 관리

\- 적이 모두 사라졌는지 주기적으로 확인

\- 전투 종료 시 방 클리어 처리

\- RoomData.isCleared 갱신

\- 문 잠금 해제

\- 미니맵 갱신



\### 4. 방 타입별 적 스폰 구조 추가



RoomBattleController에서 RoomType에 따라 다른 적 프리팹과 적 수를 사용할 수 있도록 구성하였다.



현재 처리 대상은 다음과 같다.



\- NormalBattle

\- EliteBattle

\- MidBoss

\- FinalBoss



각 타입별로 프리팹 배열과 생성 개수를 따로 둘 수 있도록 구성하였다.



현재 테스트 단계에서는 기존 일반 적 프리팹을 NormalBattle과 EliteBattle에 연결하여 전투 흐름을 확인하였다.



\### 5. EnemySpawnPoints 연결



Room\_Test\_01 프리팹 안에 적 생성 위치를 관리하는 EnemySpawnPoints를 연결하였다.



구성은 다음과 같다.



\- SpawnPoints

&#x20; - EnemySpawnPoints

&#x20;   - EnemySpawn\_01

&#x20;   - EnemySpawn\_02

&#x20;   - EnemySpawn\_03



RoomBattleController는 이 위치들을 읽어 방 타입에 맞는 적을 생성한다.



\### 6. EnemyParent 추가



생성된 적들을 정리하기 위해 Room\_Test\_01에 EnemyParent를 추가하였다.



전투방에서 생성되는 적들은 모두 EnemyParent 아래에 생성된다.



이를 통해 방이 삭제되거나 새 방이 생성될 때 기존 적 오브젝트를 함께 정리하기 쉬워졌다.



\### 7. RoomController와 전투 시스템 연결



RoomController에 RoomBattleController 참조를 추가하였다.



RoomController.Initialize가 실행될 때 현재 RoomData를 가져오고, RoomBattleController.Initialize를 호출하도록 수정하였다.



수정 후 흐름은 다음과 같다.



Room\_Test\_01 생성

→ RoomController.Initialize 실행

→ 문 연결 상태 설정

→ 구조물 랜덤 배치

→ 현재 RoomData 확인

→ RoomBattleController.Initialize 실행

→ 전투방이면 전투 시작



\### 8. 문 잠금 기능 추가



전투 중에는 플레이어가 문을 통해 다른 방으로 이동하지 못하도록 문 잠금 기능을 추가하였다.



RoomController에 SetDoorsLocked 함수를 추가하여 상하좌우 문을 한 번에 잠그거나 해제할 수 있도록 하였다.



전투 시작 시에는 모든 문이 잠기고, 전투 클리어 후에는 다시 문이 열린다.



\### 9. Barrier 방식 대신 문 자체 잠금 방식으로 수정



처음에는 문 아래에 Barrier 오브젝트를 두고, 전투 중 Barrier를 활성화하여 문을 막는 방식을 사용하려 했다.



하지만 이후 문 자체가 잠금 상태를 가지고, 잠금 상태에 따라 색상 또는 이미지가 바뀌는 방식이 더 적합하다고 판단하였다.



최종적으로는 Barrier 표시 방식 대신 다음 구조로 정리하였다.



\- 문이 열려 있을 때: 초록색 표시

\- 문이 잠겼을 때: 빨간색 표시

\- 이후에는 열린 문 이미지 / 닫힌 문 이미지로 교체 가능

\- DoorBlocker Collider를 사용해 잠긴 문은 물리적으로 통과할 수 없게 처리



이를 위해 RoomDoor 스크립트를 수정하였다.



\### 10. RoomDoor 잠금 상태 로직 수정



RoomDoor에 문 잠금 상태를 직접 관리하는 로직을 추가하였다.



RoomDoor의 주요 기능은 다음과 같다.



\- 연결된 방이 있는 문만 활성화

\- 문이 열려 있으면 플레이어가 닿았을 때 자동으로 방 이동

\- 문이 잠겨 있으면 자동 이동 차단

\- 잠금 상태에 따라 문 색상 변경

\- 잠금 상태에 따라 DoorBlocker Collider 활성화

\- 이후 문 Sprite가 준비되면 열린 문 / 닫힌 문 이미지로 교체 가능



RoomDoor에는 다음 항목을 추가하였다.



\- Visual Root

\- Door Visual Renderer

\- Unlocked Color

\- Locked Color

\- Unlocked Sprite

\- Locked Sprite

\- Locked Collider



\### 11. DoorBlocker 방식 적용



문이 잠겼을 때 단순히 이동 로직만 막으면 플레이어가 문 밖 공간으로 밀려나갈 수 있기 때문에, DoorBlocker Collider를 사용하였다.



DoorBlocker는 문 자체의 잠금 충돌 판정으로 사용된다.



동작 방식은 다음과 같다.



문 열림

→ DoorBlocker Collider 비활성화

→ 문에 닿으면 옆방으로 자동 이동



문 잠김

→ DoorBlocker Collider 활성화

→ 문 색상 빨간색

→ 문에 닿아도 방 이동 불가



\### 12. 기존 EnemySpawner 비활성화 유지



현재 방 전투는 RoomBattleController가 적을 생성한다.



기존 GameScene의 EnemySpawner와 BattleSceneStarter가 함께 작동하면 적이 중복 생성될 수 있기 때문에, 이번 단계에서는 기존 전투 자동 시작 구조를 비활성화 상태로 유지하였다.



이후 기존 전투 시스템은 새 방 전투 구조에 맞게 정리하거나 제거할 예정이다.



\## 수정 및 추가된 주요 파일



| 파일 또는 오브젝트 | 작업 내용 |

|---|---|

| RoomBattleController.cs | 방 전투 시작, 적 스폰, 방 클리어 처리 구현 |

| RoomDoor.cs | 문 잠금 상태, 색상 변경, DoorBlocker Collider 제어 추가 |

| RoomController.cs | RoomBattleController 연결 및 모든 문 잠금/해제 함수 추가 |

| RoomMapManager.cs | RefreshMinimap 함수 public 처리 |

| Room\_Test\_01 Prefab | EnemyParent, EnemySpawnPoints, DoorBlocker 연결 |

| Door\_Top / Bottom / Left / Right | 잠금 상태에 따른 색상 변경 및 Collider 제어 적용 |



\## 테스트 내용



\### 전투방 입장 테스트



시작 방에서 전투방으로 이동했을 때 RoomBattleController가 실행되고 전투가 시작되는지 확인하였다.



\### 적 스폰 테스트



EnemySpawnPoints 위치에 적이 정상적으로 생성되는지 확인하였다.



\### 문 잠금 테스트



전투방에 입장했을 때 문이 잠기고, 플레이어가 전투 중에 다른 방으로 이동할 수 없는지 확인하였다.



\### 문 색상 변경 테스트



문이 열려 있을 때는 초록색으로 표시되고, 전투 중 잠겼을 때는 빨간색으로 바뀌는지 확인하였다.



\### DoorBlocker Collider 테스트



문이 잠겼을 때 DoorBlocker Collider가 활성화되어 플레이어가 문을 통과하지 못하는지 확인하였다.



\### 방 클리어 테스트



방 안의 적을 모두 처치했을 때 Room Battle Clear 로그가 출력되고, RoomData.isCleared가 true로 변경되는지 확인하였다.



\### 문 잠금 해제 테스트



방 클리어 후 문 색상이 다시 열림 상태로 바뀌고, 문을 통해 다른 방으로 이동할 수 있는지 확인하였다.



\### 재입장 테스트



이미 클리어한 방에 다시 들어갔을 때 적이 다시 스폰되지 않고, 문도 잠기지 않는지 확인하였다.



\### 미니맵 갱신 테스트



방 클리어 후 미니맵에 클리어 상태가 반영되는지 확인하였다.



\## 완료 결과



26일차 작업을 통해 Project Q의 방 이동 시스템에 전투방 구조가 연결되었다.



이제 전투방에 들어가면 적이 스폰되고, 전투가 끝나기 전까지 문이 잠겨 다른 방으로 이동할 수 없다.  

적을 모두 처치하면 방이 클리어 처리되고, 문이 다시 열리며 미니맵에도 클리어 상태가 반영된다.



또한 문 잠금 표현을 Barrier 오브젝트 방식이 아니라 문 자체의 상태 변화 방식으로 정리하여, 이후 열린 문 이미지와 닫힌 문 이미지를 적용하기 쉬운 구조가 되었다.



\## 다음 개발 방향



27일차에는 방 프리팹 여러 개 확장과 방 타입별 프리팹 선택 구조를 구현한다.



주요 작업은 다음과 같다.



\- Room\_Test\_01을 복제해 여러 테스트 방 제작

\- 일반 전투방 / 정예 전투방 / 이벤트방 / 휴식방 구분 준비

\- RoomSceneController가 현재 RoomData의 RoomType에 맞는 방 프리팹을 선택하도록 수정

\- 이후 RoomPrefabDatabase로 확장할 기반 마련

