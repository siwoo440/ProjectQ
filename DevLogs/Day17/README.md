17일차 개발 기록

개발 내용



17일차에는 기존 전투 중심의 맵 노드 구조를 확장하여 Rest 노드와 Event 노드를 MapScene 안에서 처리하는 구조를 구현하였다. 기존에는 Rest 노드나 Event 노드가 임시로 바로 클리어되거나 단순 처리되는 상태였지만, 이번 작업을 통해 각각 별도의 UI를 띄우고 선택지를 통해 효과를 적용하는 방식으로 변경하였다.



Rest 노드는 전투 씬으로 이동하지 않고 MapScene 안에서 Rest UI가 표시되도록 수정하였다. Rest UI에는 Heal, Gain Shield, Upgrade Card 선택지를 배치하였고, 선택한 효과는 즉시 적용하지 않고 GameFlowManager에 pending 효과로 저장되도록 구성하였다. 이후 다음 전투가 시작될 때 BattleSceneStarter가 pending 효과를 읽어 체력 회복, 보호막 획득, 카드 강화 효과를 적용하도록 연결하였다.



Event 노드는 Slay the Spire의 이벤트 방처럼 큰 이벤트 전용 화면이 뜨는 방식으로 방향을 수정하였다. Event 노드를 클릭하면 GameScene으로 이동하지 않고 MapScene 위에 EventRoom UI가 표시되며, 왼쪽에는 이벤트 이미지 영역, 오른쪽에는 이벤트 설명, 아래에는 선택지 버튼이 나타나도록 구성하였다. 선택지를 누르면 선택한 효과가 pending 효과로 저장되고, Event 노드가 클리어된 뒤 다시 맵 진행이 가능하도록 처리하였다.



또한 MapScene의 전체 구조도 개선하였다. 기존에는 중앙의 작은 고정 패널 안에서 맵이 표시되었지만, 이번 작업을 통해 전체 화면 배경을 두고 중앙에는 세로로 긴 스크롤 가능한 맵 영역을 배치하였다. 이를 통해 Slay the Spire처럼 좌우에는 배경이 보이고, 중앙 지도 영역은 위아래로 스크롤하며 노드를 확인할 수 있는 구조로 변경하였다.



구현한 주요 기능



. Rest 노드 클릭 시 GameScene으로 이동하지 않고 Rest UI 표시

. Rest 선택지 Heal, Gain Shield, Upgrade Card 구현

. Rest 효과를 GameFlowManager의 pending 효과로 저장

. 다음 전투 시작 시 BattleSceneStarter에서 pending 효과 적용

. Event 노드 클릭 시 큰 EventRoom UI 표시

. EventRoom UI에 제목, 이미지, 설명, 선택지 구조 적용

. Event 선택지 선택 후 효과 저장 및 노드 클리어 처리

. MapScene을 전체 화면 배경 + 중앙 스크롤 맵 구조로 개선

. ScrollRect\_Map, Content\_Map, LineParent, NodeParent 기반 스크롤 맵 구성

. 맵 영역의 좌우 폭을 확장하여 노드 배치 공간 개선



수정 및 추가된 주요 스크립트

파일	작업 내용

GameFlowManager.cs	Rest/Event 노드 분기 처리, pending 효과 저장, 노드 클리어 처리

RestNodeUIController.cs	Rest UI 표시, 버튼 선택 처리

EventRoomUIController.cs	EventRoom UI 표시, 선택지 버튼 처리

BattleSceneStarter.cs	다음 전투 시작 시 pending 효과 적용

MapSceneUI.cs	스크롤 맵 구조에 맞게 맵 표시 영역 연결

완료 결과



17일차 작업을 통해 맵 노드 진행 구조가 단순 전투 반복에서 벗어나 전투 / 휴식 / 이벤트가 분기되는 로그라이크식 진행 구조에 가까워졌다. 특히 Rest와 Event가 전투 씬으로 이동하지 않고 MapScene 안에서 처리되도록 바뀌면서, 이후 상점 노드, 보스 보상, 랜덤 이벤트 데이터 확장 등의 기능을 추가하기 쉬운 기반이 마련되었다.

