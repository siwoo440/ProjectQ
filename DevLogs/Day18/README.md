\# Project Q 개발일지 - Day18



\## 개발 주제



보스 전투 기본 구조 구현



\## 개발 목표



18일차의 목표는 MapScene의 BossBattle 노드를 실제 보스 전투와 연결하는 것이었다.



기존에는 BossBattle 노드가 존재하더라도 일반 전투와 비슷한 방식으로 처리될 수 있었기 때문에, 이번 작업에서는 BossBattle에 진입했을 때 일반 적이 아니라 보스 1마리만 등장하도록 EnemySpawner 구조를 수정하였다.



또한 보스 전투임을 명확하게 보여주기 위해 Boss HP Bar UI를 추가하고, 보스가 생성될 때 해당 UI가 표시되도록 연결하였다.



\## 구현 내용



\### 1. BossBattle 전투 분기 구현



EnemySpawner에서 현재 전투 타입이 Boss인지 확인하는 분기를 추가하였다.



BossBattle일 경우 일반 적 생성 로직을 실행하지 않고, BossPrefab을 BossSpawnPoint 위치에 1마리만 생성하도록 수정하였다.



이를 통해 BossBattle 노드에서는 기존 일반 적 여러 마리가 등장하지 않고, 보스 전투만 진행되도록 변경하였다.



\### 2. BossSpawnPoint 추가



GameScene에 BossSpawnPoint 오브젝트를 추가하였다.



BossSpawnPoint는 보스가 등장할 위치를 지정하는 역할을 하며, EnemySpawner의 Boss Spawn Point 항목에 연결하였다.



\### 3. Boss\_Test 프리팹 생성



기존 Enemy 프리팹을 기반으로 Boss\_Test 프리팹을 만들었다.



보스는 기존 EnemyHealth 구조를 그대로 사용하도록 하여, 기존 피격 처리와 사망 처리 구조를 유지하면서 보스 전투를 테스트할 수 있도록 구성하였다.



\### 4. EnemySpawner 보스 생성 구조 수정



EnemySpawner에 Boss Settings 항목을 추가하였다.



추가된 주요 항목은 다음과 같다.



\- Boss Prefab

\- Boss Spawn Point

\- Boss Max Health

\- Boss Display Name

\- Boss Health UI Controller



BossBattle 진입 시 SpawnBoss 함수를 호출하고, 생성된 보스를 BattleManager에 등록하도록 구현하였다.



\### 5. Boss HP Bar UI 구현



GameScene의 Canvas\_Battle 아래에 Boss HP UI를 추가하였다.



구성은 다음과 같다.



\- Panel\_BossHealth

\- Text\_BossName

\- Slider\_BossHealth

\- Text\_BossHPValue

\- BossHealthUI\_Controller



BossHealthUIController 스크립트를 통해 보스 체력 UI를 관리하도록 하였다.



보스가 생성되면 Boss HP Bar가 표시되고, 보스의 현재 체력과 최대 체력을 기준으로 Slider와 체력 텍스트가 갱신된다.



\### 6. BossHealthUIController 연결



BossHealthUIController는 EnemyHealth를 추적하여 매 프레임 보스 체력바를 갱신한다.



보스가 없거나 제거된 경우 UI를 숨기도록 구성하였다.



\### 7. Unity 최신 함수 경고 수정



EnemySpawner에서 사용하던 FindObjectOfType 함수가 Unity 최신 버전에서 사용되지 않는 함수로 경고가 발생하였다.



이를 해결하기 위해 FindFirstObjectByType 함수로 변경하였다.



\## 수정 및 추가된 주요 파일



| 파일 | 작업 내용 |

|---|---|

| EnemySpawner.cs | BossBattle 분기, 보스 생성, Boss HP UI 연결 |

| BossHealthUIController.cs | 보스 체력 UI 표시 및 갱신 |

| GameScene | BossSpawnPoint, Boss HP UI 추가 |

| Boss\_Test Prefab | 보스 테스트 프리팹 생성 |



\## 테스트 내용



\### BossBattle 진입 테스트



MapScene에서 BossBattle 노드에 진입했을 때 GameScene으로 이동하는지 확인하였다.



\### 보스 생성 테스트



BossBattle에서 일반 적 여러 마리가 아니라 보스 1마리만 생성되는지 확인하였다.



\### Boss HP Bar 표시 테스트



보스가 생성될 때 Boss HP Bar가 표시되는지 확인하였다.



\### 보스 체력 감소 테스트



보스를 공격했을 때 Slider와 체력 텍스트가 감소하는지 확인하였다.



\### 일반 전투 영향 확인



NormalBattle에서는 Boss HP Bar가 뜨지 않고, 기존 일반 적 생성 구조가 유지되는지 확인하였다.



\## 완료 결과



18일차 작업을 통해 BossBattle 노드가 실제 보스 전투로 연결되었다.



이제 Project Q의 노드맵 진행 구조는 일반 전투, 휴식, 이벤트에 이어 보스 전투까지 연결되었으며, 1챕터 진행 구조의 핵심 기반이 완성되었다.



다음 개발에서는 보스 전용 탄막 패턴, 보스 공격 방식, 보스 클리어 후 챕터 클리어 UI를 구현할 예정이다.



\## 다음 개발 방향



19일차에는 보스 전용 탄막 패턴과 보스 전투 UI를 개선한다.



주요 작업은 다음과 같다.



\- 보스 전용 공격 패턴 구현

\- 원형 탄막 패턴 추가

\- 부채꼴 탄막 패턴 추가

\- 보스 체력에 따른 패턴 변화 준비

\- Boss HP Bar 디자인 개선

