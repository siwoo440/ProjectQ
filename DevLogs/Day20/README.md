\# Project Q 개발일지 - Day20



\## 개발 주제



보스 처치 후 챕터 클리어 구조 구현



\## 개발 목표



20일차의 목표는 BossBattle에서 보스를 처치했을 때 기존 Reward UI로 넘어가지 않고, Chapter Clear UI가 표시되도록 게임 흐름을 수정하는 것이었다.



19일차까지는 보스 등장, Boss HP Bar, 보스 전용 탄막 패턴까지 구현되어 있었다.  

이번 작업에서는 보스 처치 이후의 결과 흐름을 추가하여 1챕터의 끝을 표현할 수 있도록 구성하였다.



\## 구현 내용



\### 1. Chapter Clear UI 제작



GameScene의 Canvas\_Battle 아래에 Chapter Clear 전용 UI를 추가하였다.



구성은 다음과 같다.



\- Panel\_ChapterClear

\- Dim\_Background

\- Window\_ChapterClear

\- Text\_ChapterTitle

\- Text\_ChapterDescription

\- Text\_Result

\- Button\_RetryRun

\- Button\_BackToMap

\- Button\_MainMenu



Panel\_ChapterClear는 처음에는 비활성화 상태로 두고, 보스 클리어 시 코드에서 활성화되도록 설정하였다.



\### 2. ChapterClearUIController 추가



Chapter Clear UI를 관리하기 위한 ChapterClearUIController 스크립트를 추가하였다.



이 스크립트는 챕터 클리어 패널을 표시하고, Retry Run, Back To Map, Main Menu 버튼의 동작을 관리한다.



현재 MainMenuScene은 아직 본격적으로 구현되지 않았기 때문에, Main Menu 버튼은 이후 메인 메뉴 작업과 함께 확장할 예정이다.



\### 3. BattleManager 보스 클리어 분기 추가



BattleManager의 전투 클리어 처리 구조를 수정하였다.



기존에는 모든 전투 클리어 후 Reward UI가 표시되었지만, BossBattle 클리어 시에는 Reward UI 대신 Chapter Clear UI가 표시되도록 분기하였다.



수정 후 흐름은 다음과 같다.



NormalBattle 또는 EliteBattle:

적 전부 처치

→ Battle Clear 표시

→ Reward UI 표시

→ 보상 선택 후 MapScene 복귀



BossBattle:

보스 처치

→ Chapter Clear 표시

→ Chapter Clear UI 표시

→ Retry / Back To Map / Main Menu 선택



\### 4. 남은 탄환 정리 구조 추가



보스 처치 후 화면에 남아 있는 플레이어 탄환, 적 탄환, 보스 탄환을 정리하는 구조를 추가하였다.



이를 통해 Chapter Clear UI가 표시된 뒤에도 탄환이 화면에 남아 움직이는 문제를 방지하였다.



\### 5. GameScene Hierarchy 정리



GameScene의 오브젝트 수가 많아져 관리가 어려워졌기 때문에 Hierarchy를 정리하였다.



다음과 같은 정리용 빈 오브젝트를 추가하였다.



\- ===Core===

\- ===Managers===

\- ===Spawn Points===

\- ===UI===



정리 후 주요 구조는 다음과 같다.



\- ===Core===

&#x20; - Main Camera

&#x20; - Player



\- ===Managers===

&#x20; - CardManager

&#x20; - BattleManager

&#x20; - RewardManager

&#x20; - BattleProgressManager

&#x20; - RelicManager

&#x20; - StageManager

&#x20; - NextBattleUI\_Controller

&#x20; - BattleSceneStarter

&#x20; - BossHealthUI\_Controller

&#x20; - ChapterClearUI\_Controller



\- ===Spawn Points===

&#x20; - EnemySpawner

&#x20; - BossSpawnPoint



\- ===UI===

&#x20; - Canvas\_GameHUD

&#x20; - Canvas\_Battle



이를 통해 GameScene의 구조가 더 보기 쉬워졌고, 이후 기능 추가 시 필요한 오브젝트를 더 빠르게 찾을 수 있게 되었다.



\## 수정 및 추가된 주요 파일



| 파일 또는 오브젝트 | 작업 내용 |

|---|---|

| ChapterClearUIController.cs | Chapter Clear UI 표시 및 버튼 처리 |

| BattleManager.cs | BossBattle 클리어 시 Chapter Clear UI 표시 분기 |

| GameScene | Panel\_ChapterClear UI 추가 |

| GameScene Hierarchy | Core, Managers, Spawn Points, UI 그룹 정리 |



\## 테스트 내용



\### BossBattle 클리어 테스트



BossBattle에 진입한 뒤 보스를 처치했을 때 Chapter Clear UI가 표시되는지 확인하였다.



\### Reward UI 분기 테스트



BossBattle 클리어 시 기존 Reward UI가 표시되지 않는지 확인하였다.



\### 일반 전투 보상 테스트



NormalBattle과 EliteBattle에서는 기존처럼 Reward UI가 정상적으로 표시되는지 확인하였다.



\### 버튼 동작 테스트



Retry Run 버튼과 Back To Map 버튼이 정상적으로 작동하는지 확인하였다.



\### Hierarchy 정리 후 연결 테스트



Manager 오브젝트들을 정리용 오브젝트 아래로 이동한 뒤에도 Inspector 연결이 유지되는지 확인하였다.



\## 완료 결과



20일차 작업을 통해 Project Q의 1챕터 종료 흐름이 구현되었다.



이제 보스 전투는 단순히 강한 적을 처치하는 전투가 아니라, 챕터 클리어 화면으로 이어지는 게임 진행의 마무리 지점이 되었다.



또한 GameScene의 Hierarchy를 정리하여 이후 MainMenu, SaveManager, SettingsUI 같은 시스템을 추가하기 쉬운 구조로 개선하였다.



\## 다음 개발 방향



21일차에는 MainMenuScene을 구현한다.



주요 작업은 다음과 같다.



\- MainMenuScene 생성

\- Start Game 버튼 구현

\- Continue 버튼 자리 준비

\- Settings 버튼 자리 준비

\- Quit 버튼 구현

\- 새 게임 시작 시 MapScene으로 이동

