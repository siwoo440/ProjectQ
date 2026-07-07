Project Q 개발 기록 - 16일차

1\. 개발 목표



16일차의 목표는 기존의 선형 전투 반복 구조를 수정하여, Slay the Spire 방식의 Map Scene 기반 진행 구조를 구현하는 것이다.



기존에는 전투를 클리어한 뒤 보상을 선택하고, Next Battle 버튼을 눌러 다음 전투로 넘어가는 구조였다. 하지만 이 방식은 플레이어가 직접 경로를 선택하는 로그라이크 진행감이 부족했다.



이번 작업에서는 전투 승리 후 바로 다음 전투로 넘어가는 방식이 아니라, 별도의 MapScene으로 돌아가고, 플레이어가 랜덤으로 생성된 노드맵에서 연결된 다음 노드를 선택하는 구조로 변경하였다.



2\. 변경 전 구조



기존 진행 구조는 다음과 같았다.



전투 시작

→ 전투 클리어

→ 보상 선택

→ Next Battle 버튼 클릭

→ 다음 전투 시작



이 구조는 구현은 단순하지만, 플레이어가 직접 경로를 선택하는 요소가 없었다.



3\. 변경 후 구조



16일차 이후 진행 구조는 다음과 같이 변경되었다.



MapScene

→ 노드 선택

→ GameScene 전투 시작

→ 전투 클리어

→ 보상 선택

→ MapScene 복귀

→ 연결된 다음 노드 선택

→ 반복

→ Boss 노드 도달

→ 챕터 클리어



이제 플레이어는 전투 후 자동으로 다음 전투를 진행하지 않고, 맵으로 돌아와 다음 경로를 직접 선택한다.



4\. 구현한 핵심 기능

구분	구현 내용

MapScene 추가	전투 진행과 분리된 전용 맵 씬 생성

랜덤 노드맵 생성	층과 라인 구조를 기반으로 노드 자동 생성

노드 타입 구분	일반 전투, 엘리트 전투, 휴식, 이벤트, 보스 노드 구분

노드 선택 기능	현재 위치에서 연결된 노드만 선택 가능

GameFlowManager 추가	맵 데이터, 현재 위치, 선택 노드, 씬 이동 관리

GameScene 연동	선택한 노드 타입에 따라 전투 씬에서 전투 시작

BattleType 전달	Normal, Elite, Boss 전투 타입을 EnemySpawner에 전달

보상 후 맵 복귀	보상 선택 후 Next Battle이 아니라 MapScene으로 복귀

연결선 표시	노드 사이의 이동 가능한 경로를 UI 선으로 표시

경로 색상 구분	선택 가능 경로, 클리어 경로, 잠긴 경로 색상 구분

5\. 새로 추가한 씬

씬 이름	역할

MapScene	노드맵 표시 및 다음 노드 선택

GameScene	실제 전투 진행



Build Settings에는 다음 순서로 등록하였다.



순서	씬

0	MapScene

1	GameScene

6\. 새로 추가한 스크립트

파일명	위치	역할

MapNodeData.cs	Assets/Scripts/Map	맵 노드 하나의 데이터 저장

GameFlowManager.cs	Assets/Scripts/Managers	전체 맵 진행, 노드 선택, 씬 이동 관리

MapNodeUI.cs	Assets/Scripts/UI	맵 노드 버튼 UI 표시

MapSceneUI.cs	Assets/Scripts/UI	맵 전체 UI 생성 및 갱신

BattleSceneStarter.cs	Assets/Scripts/Managers	GameScene 진입 시 선택된 노드 정보로 전투 시작

7\. 수정한 스크립트

파일명	수정 내용

RewardManager.cs	보상 선택 후 NextBattleUI 표시 대신 MapScene으로 복귀하도록 수정

EnemySpawner.cs	BattleType에 따라 적 수와 체력을 다르게 계산하도록 수정

StageManager.cs	기존 자동 진행 구조와 충돌하지 않도록 비활성화 또는 자동 시작 해제

MapSceneUI.cs	노드 사이 연결선을 표시하도록 추가 수정

8\. MapNodeData 구현



MapNodeData는 MapScene에 표시되는 노드 하나의 정보를 저장한다.



변수	설명

nodeId	노드 고유 번호

floor	노드가 위치한 층

lane	노드의 가로 위치

nodeType	노드 타입

battleNumber	전투 씬에 전달할 전투 번호

normalizedPosition	MapArea 안에서의 위치

connectedNodeIds	다음으로 이동 가능한 노드 ID 목록

isCleared	해당 노드 클리어 여부



이를 통해 각 노드가 어디에 위치하는지, 어떤 노드로 연결되는지, 이미 클리어했는지를 관리할 수 있게 되었다.



9\. GameFlowManager 구현



GameFlowManager는 16일차의 핵심 관리자이다.



기능	설명

맵 데이터 유지	씬이 바뀌어도 랜덤 맵 정보를 유지

랜덤 맵 생성	층과 라인 수에 따라 노드 생성

노드 연결 생성	현재 층 노드에서 다음 층 노드로 연결 생성

선택 가능 여부 판단	현재 위치에서 갈 수 있는 노드만 선택 가능

선택 노드 저장	선택한 노드 타입, 전투 타입, 전투 번호 저장

씬 이동 관리	MapScene과 GameScene 사이 이동 처리

노드 클리어 처리	전투 종료 후 현재 노드를 클리어 처리

보스 노드 처리	보스 노드 클리어 시 챕터 클리어 로그 출력

10\. 랜덤 맵 생성 구조



현재 맵은 다음 규칙으로 생성된다.



항목	현재 설정

층 수	6

가로 라인 수	3

첫 번째 층	일반 전투 노드

중간 층	일반 전투, 이벤트, 휴식, 엘리트 노드 랜덤 생성

보스 직전 층	휴식 또는 엘리트 노드

마지막 층	보스 노드 1개

연결 개수	각 노드에서 다음 층 노드 1\~2개 연결



현재는 완전한 Slay the Spire식 맵 생성은 아니지만, 플레이어가 경로를 선택하며 진행할 수 있는 기본 구조가 완성되었다.



11\. MapScene UI 구현



MapScene의 UI 구조는 다음과 같이 구성하였다.



오브젝트	역할

Canvas\_Map	맵 UI 전체 캔버스

Panel\_Map	맵 화면 배경 패널

Text\_Title	챕터 맵 제목 표시

MapArea	노드와 연결선이 배치되는 영역

LineParent	노드 간 연결선 생성 위치

NodeParent	노드 버튼 생성 위치



Hierarchy 구조는 다음과 같다.



Canvas\_Map

└ Panel\_Map

　├ Text\_Title

　└ MapArea

　　├ LineParent

　　└ NodeParent



중요한 점은 LineParent를 NodeParent보다 위에 두어, 연결선이 노드 버튼 뒤에 보이도록 구성한 것이다.



12\. MapNodeUI 구현



MapNodeUI는 노드 버튼 하나를 담당한다.



노드에는 다음 정보가 표시된다.



표시 상태	의미

Select	현재 선택 가능한 노드

Locked	아직 선택할 수 없는 노드

Clear	이미 클리어한 노드

Current	마지막으로 클리어한 현재 위치 노드



노드 타입은 다음과 같이 표시된다.



노드 타입	표시 문구

NormalBattle	Battle

EliteBattle	Elite

Rest	Rest

Event	Event

BossBattle	Boss

13\. 노드 선택 규칙



현재 노드 선택 규칙은 다음과 같다.



상황	선택 가능 조건

아직 클리어한 노드가 없음	첫 번째 층 노드만 선택 가능

전투를 1개 이상 클리어함	마지막으로 클리어한 노드와 연결된 다음 노드만 선택 가능

이미 클리어한 노드	선택 불가

연결되지 않은 노드	선택 불가

진행 중인 노드가 있음	다른 노드 선택 불가



이를 통해 플레이어가 아무 노드나 선택하지 못하고, 연결된 경로를 따라 진행하도록 만들었다.



14\. 노드 타입별 처리



노드 선택 시 타입에 따라 다음과 같이 처리된다.



노드 타입	처리

NormalBattle	GameScene으로 이동 후 BattleType.Normal 전투 시작

EliteBattle	GameScene으로 이동 후 BattleType.Elite 전투 시작

BossBattle	GameScene으로 이동 후 BattleType.Boss 전투 시작

Rest	현재는 임시 처리 후 바로 클리어

Event	현재는 임시 처리 후 바로 클리어



Rest와 Event는 아직 상세 UI가 없기 때문에 16일차에서는 임시로 클리어 처리한다. 이후 휴식 UI와 이벤트 선택지를 구현할 수 있다.



15\. BattleSceneStarter 구현



BattleSceneStarter는 GameScene에 진입했을 때 전투를 자동으로 시작하는 역할을 한다.



기존에는 StageManager 또는 BattleProgressManager가 전투를 시작했지만, MapScene 구조에서는 선택한 노드 정보가 필요하다. 따라서 GameScene에 진입하면 GameFlowManager가 저장해둔 값을 읽어 전투를 시작한다.



처리 흐름은 다음과 같다.



GameScene 진입

→ GameFlowManager에서 selectedBattleType 확인

→ EnemySpawner.SetBattleType 호출

→ GameFlowManager에서 selectedBattleNumber 확인

→ BattleManager.StartBattle 호출



이를 통해 맵에서 선택한 노드 타입이 전투에 반영되도록 만들었다.



16\. 전투 타입 분리



16일차에서는 전투 타입을 다음 3가지로 분리하였다.



BattleType	설명

Normal	일반 전투

Elite	엘리트 전투

Boss	보스 전투



EnemySpawner는 BattleType에 따라 적 수와 적 체력을 다르게 계산한다.



전투 타입	적 수 배율	적 체력 배율

Normal	1.0	1.0

Elite	1.3	1.5

Boss	0.7	2.5



현재 Boss는 아직 전용 보스 프리팹이 없기 때문에, 임시로 적 수는 줄이고 체력은 높이는 방식으로 처리하였다.



17\. EnemySpawner 수정



EnemySpawner에는 다음 기능을 추가하였다.



함수	역할

SetBattleType	현재 전투 타입 저장

ApplyBattleTypeEnemyCount	전투 타입에 따라 적 수 조정

ApplyBattleTypeEnemyHealth	전투 타입에 따라 적 체력 조정

GetEnemyCount	전투 번호와 전투 타입을 반영한 최종 적 수 반환

GetEnemyHealth	전투 번호와 전투 타입을 반영한 최종 적 체력 반환



수정 후 전투 시작 시 Console에서 다음 로그를 확인할 수 있다.



Normal 노드에서는 EnemySpawner Battle Type Set : Normal

Elite 노드에서는 EnemySpawner Battle Type Set : Elite

Boss 노드에서는 EnemySpawner Battle Type Set : Boss



18\. 보상 후 MapScene 복귀 구조



기존 보상 선택 후 흐름은 다음과 같았다.



보상 선택

→ NextBattleUI.Show

→ Next Battle 버튼 표시



16일차 이후 흐름은 다음과 같이 변경되었다.



보상 선택

→ GameFlowManager.CompleteActiveNodeAndReturnToMap

→ 현재 노드 클리어 처리

→ MapScene으로 복귀



즉, 이제 Next Battle 버튼을 통해 다음 전투로 이동하지 않는다. 전투 후에는 항상 MapScene으로 돌아가며, 플레이어가 다음 노드를 직접 선택한다.



19\. 노드 연결선 표시 추가



초기 MapScene 구현에서는 노드 버튼만 표시되어 있었기 때문에, 현재 노드가 어떤 노드로 이어지는지 알기 어려웠다.



이를 해결하기 위해 MapSceneUI에 노드 간 연결선 표시 기능을 추가하였다.



연결선은 UI Image를 얇은 직사각형으로 만들고, 두 노드 사이의 거리와 각도에 맞춰 배치하는 방식으로 구현하였다.



처리 방식은 다음과 같다.



시작 노드 위치 계산

→ 도착 노드 위치 계산

→ 두 점의 중간 위치 계산

→ 두 점 사이 거리 계산

→ 두 점 사이 각도 계산

→ Image 오브젝트 생성

→ 길이와 회전을 적용하여 선처럼 표시



20\. 연결선 UI 구조



연결선과 노드 버튼이 겹치지 않도록 부모 오브젝트를 분리하였다.



MapArea

├ LineParent

└ NodeParent



오브젝트	역할

LineParent	연결선 UI가 생성되는 부모

NodeParent	노드 버튼 UI가 생성되는 부모



LineParent를 먼저 배치하고, NodeParent를 그 아래에 배치하여 노드 버튼이 연결선보다 앞에 보이도록 하였다.



21\. 연결선 색상 구분



연결선은 현재 진행 상태에 따라 색상이 다르게 표시된다.



연결선 상태	설명

선택 가능 경로	현재 위치에서 이동 가능한 노드로 이어지는 선

클리어 경로	이미 지나간 노드 사이의 선

잠긴 경로	아직 이동할 수 없는 연결선



이를 통해 플레이어가 현재 어떤 경로로 이동할 수 있는지 시각적으로 확인할 수 있게 되었다.



22\. 연결선 클릭 방해 방지



연결선은 UI Image로 생성되기 때문에 기본 상태에서는 버튼 클릭을 방해할 수 있다.



이를 방지하기 위해 생성된 연결선 Image에 다음 설정을 적용하였다.



Raycast Target = false



이 설정으로 인해 연결선이 노드 버튼 클릭을 막지 않도록 처리하였다.



23\. 현재 16일차 이후 게임 흐름



현재 프로젝트의 진행 흐름은 다음과 같다.



MapScene 시작

→ 랜덤 노드맵 생성

→ 첫 층 노드 중 하나 선택

→ GameScene 이동

→ 선택한 BattleType으로 전투 시작

→ 전투 클리어

→ 보상 선택

→ MapScene 복귀

→ 클리어한 노드 기준 연결 노드만 선택 가능

→ 선택한 다음 노드 진행

→ Boss 노드 도달

→ Boss 전투 진행



이제 Project Q는 단순 전투 반복 구조에서 벗어나, 로그라이크식 경로 선택 구조를 갖추게 되었다.



24\. 테스트 결과

테스트 항목	결과

MapScene 생성	정상

랜덤 노드 생성	정상

노드 위치 표시	정상

노드 선택 가능 여부 구분	정상

선택 불가 노드 비활성화	정상

노드 선택 시 GameScene 이동	정상

선택한 노드 타입 저장	정상

GameScene에서 선택한 BattleType으로 전투 시작	정상

전투 클리어 후 보상 표시	정상

보상 선택 후 MapScene 복귀	정상

클리어한 노드 기준 다음 노드 선택 가능	정상

노드 간 연결선 표시	정상

연결선이 노드 뒤에 표시	정상

연결선이 노드 클릭을 방해하지 않음	정상

기존 Next Battle 진행 구조 제거	정상

Console 빨간 오류	없음

25\. 발생한 문제와 해결

문제	원인	해결

노드가 어떻게 연결됐는지 알기 어려움	노드 버튼만 있고 연결선이 없었음	MapSceneUI에 연결선 생성 기능 추가

선이 노드 위에 표시될 가능성	선과 노드가 같은 부모에 생성됨	LineParent와 NodeParent를 분리

연결선이 노드 클릭을 막을 가능성	연결선 Image가 Raycast를 받을 수 있음	raycastTarget을 false로 설정

전투 후 기존 Next Battle 버튼 흐름이 남아 있음	RewardManager가 NextBattleUI.Show를 호출함	GameFlowManager.CompleteActiveNodeAndReturnToMap으로 교체

기존 StageManager와 충돌 가능성	StageManager가 자동으로 노드를 시작할 수 있음	StageManager 비활성화 또는 Start Run On Play 해제

26\. 현재 완료된 핵심 시스템

시스템	상태

플레이어 이동 / 회피	완료

HP / MP / Shield	완료

카드 발사	완료

다중 카드 선택	완료

카드 상세 UI	완료

보상 시스템	완료

보상 등급 확률	완료

유물 시스템	완료

유물 UI	완료

카드 전용 유물	완료

카드-유물 조합 효과	완료

신규 카드 3종	완료

관통 / 폭발 / 유도 탄환	완료

적 스폰	완료

적 탄막 패턴	완료

Game Over / Retry	완료

BattleType 분리	완료

MapScene	완료

랜덤 노드 생성	완료

노드 선택 진행	완료

전투 후 MapScene 복귀	완료

노드 연결선 표시	완료

27\. 다음 개발 목표



다음 개발에서는 MapScene의 기본 진행 구조 위에 실제 노드 기능을 확장하는 것이 좋다.



우선순위	다음 작업

1	Rest 노드 전용 UI 구현

2	Rest 노드에서 체력 회복 / 보호막 획득 / 카드 강화 선택

3	Event 노드 기본 선택지 구현

4	Boss 전용 적 또는 Boss 프리팹 구현

5	보스 전투 패턴 구현

6	보스 클리어 후 Chapter Clear 화면 구현

28\. 16일차 정리



16일차 작업을 통해 Project Q는 기존의 선형 전투 반복 구조에서 벗어나, Slay the Spire 방식의 맵 선택형 로그라이크 구조로 전환되었다.



이번 작업의 핵심 변화는 다음과 같다.



기존 구조는 전투, 보상, Next Battle, 다음 전투로 이어지는 방식이었다.

변경 후 구조는 MapScene, 노드 선택, 전투, 보상, MapScene 복귀, 연결된 다음 노드 선택으로 이어지는 방식이다.



추가로 노드 사이 연결선을 표시하여, 플레이어가 현재 경로와 이동 가능한 노드를 더 쉽게 파악할 수 있게 되었다.



아직 Rest 노드, Event 노드, Boss 전용 전투는 임시 처리 상태이지만, 전체적인 게임 진행 구조는 로그라이크 맵 기반 구조로 확장되었다.

