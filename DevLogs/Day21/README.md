\# Project Q 개발일지 - Day21



\## 개발 주제



메인 메뉴 구현 및 게임 흐름 연결



\## 개발 목표



21일차의 목표는 게임을 바로 MapScene이나 GameScene에서 시작하는 구조가 아니라, MainMenuScene에서 시작하는 실제 게임 형태로 바꾸는 것이었다.



기존에는 개발 테스트를 위해 MapScene 또는 GameScene을 직접 실행하는 방식으로 진행했지만, 이번 작업을 통해 게임 실행 후 메인 메뉴가 먼저 표시되고, Start Game 버튼을 통해 새 런을 시작하는 흐름을 구현하였다.



또한 게임 오버나 챕터 클리어 이후 다시 메인 메뉴로 돌아갈 수 있도록 전체 게임 흐름을 정리하였다.



\## 구현 내용



\### 1. MainMenuScene 생성



새로운 MainMenuScene을 생성하였다.



MainMenuScene은 게임 시작 화면 역할을 하며, 게임 제목과 주요 버튼을 배치하였다.



구성은 다음과 같다.



\- Main Camera

\- Canvas\_MainMenu

\- Panel\_Background

\- Text\_GameTitle

\- Button\_StartGame

\- Button\_Continue

\- Button\_Settings

\- Button\_Quit

\- ===Managers===

\- GameFlowManager

\- MainMenuController

\- EventSystem



\### 2. MainMenu UI 구성



Canvas\_MainMenu 아래에 메인 메뉴 UI를 구성하였다.



메인 메뉴에는 다음 버튼을 배치하였다.



\- Start Game

\- Continue

\- Settings

\- Quit



Start Game은 실제 게임 시작 버튼으로 사용하고, Continue는 아직 저장 시스템이 구현되지 않았기 때문에 비활성화 상태로 두었다.



Settings는 아직 설정 UI가 구현되지 않았으므로 임시 로그 출력 방식으로 처리하였다.



Quit 버튼은 빌드 환경에서 게임 종료가 가능하도록 Application.Quit 구조를 연결하였다.



\### 3. MainMenuController 구현



메인 메뉴 버튼 처리를 담당하는 MainMenuController 스크립트를 추가하였다.



MainMenuController는 각 버튼의 클릭 이벤트를 코드에서 자동으로 연결한다.



주요 기능은 다음과 같다.



\- Start Game 버튼 클릭 시 새 런 시작

\- Continue 버튼 자리 준비

\- Settings 버튼 임시 처리

\- Quit 버튼으로 게임 종료 처리



Start Game 버튼을 누르면 GameFlowManager의 StartNewRun 함수를 호출하여 기존 맵 데이터를 초기화하고 새로운 노드맵을 생성한 뒤 MapScene으로 이동하도록 구현하였다.



\### 4. GameFlowManager 연결



MainMenuScene의 ===Managers=== 아래에 GameFlowManager 오브젝트를 배치하였다.



이를 통해 메인 메뉴에서 새 런을 시작할 수 있도록 구성하였다.



GameFlowManager는 씬 이동 후에도 유지되는 구조이므로, MainMenuScene에서 시작해 MapScene과 GameScene으로 이어지는 전체 진행 흐름을 관리한다.



\### 5. Build Settings 씬 순서 정리



Build Settings에 주요 씬을 등록하고 순서를 정리하였다.



등록 순서는 다음과 같다.



1\. MainMenuScene

2\. MapScene

3\. GameScene



MainMenuScene을 0번에 배치하여, 빌드 실행 시 메인 메뉴가 가장 먼저 표시되도록 하였다.



\### 6. Chapter Clear UI의 Main Menu 복귀 연결



보스 처치 후 표시되는 Chapter Clear UI에서 Main Menu 버튼을 사용할 수 있도록 정리하였다.



Chapter Clear UI의 Main Menu 버튼을 누르면 Time.timeScale을 1로 복구한 뒤 MainMenuScene으로 이동하도록 구성하였다.



이를 통해 챕터 클리어 이후 게임을 다시 시작하거나 종료할 수 있는 기본 흐름이 마련되었다.



\### 7. Game Over UI의 Main Menu 버튼 추가



플레이어가 사망했을 때 표시되는 Game Over UI에도 Main Menu 버튼을 추가하였다.



기존 Game Over UI는 Retry 중심이었지만, 이번 작업을 통해 사망 후에도 메인 메뉴로 돌아갈 수 있도록 확장하였다.



Game Over UI 구성은 다음과 같이 확장하였다.



\- Text\_GameOver

\- Button\_Retry

\- Button\_MainMenu



\### 8. Game Over Retry 흐름 정리



Game Over UI의 Retry 버튼은 단순히 GameScene을 다시 로드하는 방식이 아니라, GameFlowManager.StartNewRun을 호출하여 새 런을 시작하는 방식으로 정리하였다.



이를 통해 사망 후 Retry를 누르면 기존 전투를 반복하는 것이 아니라, 새 노드맵을 생성하고 처음부터 다시 진행할 수 있게 되었다.



\### 9. 전체 게임 흐름 연결



이번 작업을 통해 다음 흐름이 연결되었다.



MainMenuScene

→ Start Game

→ MapScene

→ 노드 선택

→ GameScene

→ Game Over 또는 Chapter Clear

→ Retry Run 또는 Main Menu 복귀



이제 Project Q는 단순한 전투 테스트 프로젝트가 아니라, 메인 메뉴에서 시작해 게임 진행과 종료 후 복귀까지 가능한 기본 게임 흐름을 갖추게 되었다.



\## 수정 및 추가된 주요 파일



| 파일 또는 오브젝트 | 작업 내용 |

|---|---|

| MainMenuScene | 메인 메뉴 씬 생성 |

| MainMenuController.cs | 메인 메뉴 버튼 처리 구현 |

| GameFlowManager | MainMenuScene에서 새 런 시작 가능하도록 배치 |

| ChapterClearUIController.cs | Main Menu 복귀 흐름 정리 |

| GameOverUI.cs | Main Menu 버튼 및 Retry 흐름 정리 |

| Build Settings | MainMenuScene, MapScene, GameScene 순서 등록 |



\## 테스트 내용



\### MainMenuScene 실행 테스트



MainMenuScene을 실행했을 때 게임 제목과 메뉴 버튼이 정상적으로 표시되는지 확인하였다.



\### Start Game 테스트



Start Game 버튼을 눌렀을 때 GameFlowManager.StartNewRun이 실행되고 MapScene으로 이동하는지 확인하였다.



\### Continue 버튼 테스트



저장 시스템이 아직 없기 때문에 Continue 버튼이 비활성화 상태인지 확인하였다.



\### Settings 버튼 테스트



Settings 버튼 클릭 시 임시 로그가 출력되는지 확인하였다.



\### Quit 버튼 테스트



Quit 버튼 클릭 시 종료 로그가 출력되고, 빌드 환경에서 게임 종료가 가능한 구조인지 확인하였다.



\### Chapter Clear Main Menu 테스트



보스 처치 후 Chapter Clear UI에서 Main Menu 버튼을 눌렀을 때 MainMenuScene으로 이동하는지 확인하였다.



\### Game Over Main Menu 테스트



플레이어 사망 후 Game Over UI에서 Main Menu 버튼을 눌렀을 때 MainMenuScene으로 이동하는지 확인하였다.



\### Retry Run 테스트



Game Over 또는 Chapter Clear 상태에서 Retry 버튼을 눌렀을 때 새 런이 시작되고 MapScene으로 이동하는지 확인하였다.



\## 완료 결과



21일차 작업을 통해 Project Q의 기본 게임 시작과 복귀 흐름이 완성되었다.



이제 게임은 MainMenuScene에서 시작하여 새 런을 생성하고, MapScene과 GameScene을 거쳐 Game Over 또는 Chapter Clear 이후 다시 메인 메뉴로 돌아올 수 있다.



이를 통해 Project Q는 전투 기능과 노드맵 기능을 넘어, 실제 게임 형태에 가까운 전체 흐름을 갖추게 되었다.



\## 다음 개발 방향



22일차에는 Settings UI 기본 구조를 구현한다.



주요 작업은 다음과 같다.



\- Settings 버튼 클릭 시 설정 패널 표시

\- BGM Volume Slider 추가

\- SFX Volume Slider 추가

\- Back 버튼 구현

\- 설정값 PlayerPrefs 저장 준비

