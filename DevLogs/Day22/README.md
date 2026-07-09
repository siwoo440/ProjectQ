\# Project Q 개발일지 - Day22



\## 개발 주제



SettingsScene 방식의 설정 화면 구현



\## 개발 목표



22일차의 목표는 MainMenuScene 안에서 Panel을 띄우는 방식이 아니라, 별도의 SettingsScene으로 이동하여 설정을 변경하는 구조를 구현하는 것이었다.



21일차에서 MainMenuScene을 구현하고 게임 시작, 재시작, 메인 메뉴 복귀 흐름을 정리했기 때문에, 이번 작업에서는 메인 메뉴의 Settings 버튼을 실제 설정 화면으로 연결하였다.



\## 구현 내용



\### 1. SettingsScene 생성



새로운 SettingsScene을 생성하였다.



SettingsScene은 게임 설정을 담당하는 전용 씬이며, MainMenuScene에서 Settings 버튼을 클릭하면 이동하도록 구성하였다.



기본 흐름은 다음과 같다.



MainMenuScene

→ Settings 버튼 클릭

→ SettingsScene 이동

→ 설정값 변경

→ Back 버튼 클릭

→ MainMenuScene 복귀



\### 2. SettingsScene UI 구성



SettingsScene에 설정 전용 UI를 구성하였다.



구성은 다음과 같다.



\- Main Camera

\- Canvas\_Settings

\- Panel\_Background

\- Text\_Title

\- Text\_BGMVolume

\- Slider\_BGMVolume

\- Text\_BGMValue

\- Text\_SFXVolume

\- Slider\_SFXVolume

\- Text\_SFXValue

\- Button\_Back

\- ===Managers===

\- SettingsController

\- EventSystem



BGM과 SFX는 각각 Slider를 통해 0%부터 100%까지 조절할 수 있도록 만들었다.



\### 3. BGM Volume 설정값 저장



BGM Volume Slider의 값을 PlayerPrefs에 저장하도록 구현하였다.



저장 키는 다음과 같다.



\- BGM\_VOLUME



슬라이더 값을 변경하면 즉시 PlayerPrefs에 저장되고, 화면의 퍼센트 텍스트도 함께 갱신되도록 하였다.



\### 4. SFX Volume 설정값 저장



SFX Volume Slider의 값을 PlayerPrefs에 저장하도록 구현하였다.



저장 키는 다음과 같다.



\- SFX\_VOLUME



SFX 값도 BGM과 동일하게 Slider 조작 시 즉시 저장되고, 퍼센트 텍스트가 갱신되도록 구성하였다.



\### 5. SettingsController 구현



SettingsScene의 설정 기능을 관리하는 SettingsController 스크립트를 추가하였다.



SettingsController의 주요 역할은 다음과 같다.



\- 저장된 BGM Volume 값 불러오기

\- 저장된 SFX Volume 값 불러오기

\- Slider 값 변경 감지

\- PlayerPrefs에 설정값 저장

\- 퍼센트 텍스트 갱신

\- Back 버튼 클릭 시 MainMenuScene으로 이동



이번 단계에서는 실제 AudioMixer나 AudioManager에 연결하지 않고, 설정값 저장 구조만 먼저 구현하였다.



\### 6. MainMenuController 수정



MainMenuScene의 Settings 버튼이 기존에는 임시 로그만 출력하던 구조였지만, 이번 작업을 통해 SettingsScene으로 이동하도록 수정하였다.



수정 후 흐름은 다음과 같다.



Settings 버튼 클릭

→ Time.timeScale 복구

→ SettingsScene 로드



이를 통해 메인 메뉴에서 설정 화면으로 자연스럽게 이동할 수 있게 되었다.



\### 7. Back 버튼 구현



SettingsScene의 Back 버튼을 누르면 MainMenuScene으로 돌아가도록 구현하였다.



Back 버튼 클릭 시 Time.timeScale을 1로 복구한 뒤 MainMenuScene을 로드한다.



\### 8. Build Settings 등록



SettingsScene을 Build Settings에 추가하였다.



최종 씬 순서는 다음과 같이 정리하였다.



1\. MainMenuScene

2\. SettingsScene

3\. MapScene

4\. GameScene



MainMenuScene은 게임 시작 화면이므로 0번에 유지하고, SettingsScene은 메인 메뉴에서 이동하는 설정 화면으로 1번에 배치하였다.



\## 수정 및 추가된 주요 파일



| 파일 또는 오브젝트 | 작업 내용 |

|---|---|

| SettingsScene | 설정 전용 씬 생성 |

| SettingsController.cs | 설정값 저장, 불러오기, UI 갱신, Back 버튼 처리 |

| MainMenuController.cs | Settings 버튼 클릭 시 SettingsScene 이동 |

| Canvas\_Settings | 설정 화면 UI 구성 |

| Slider\_BGMVolume | BGM 볼륨 조절 UI |

| Slider\_SFXVolume | SFX 볼륨 조절 UI |

| Build Settings | SettingsScene 추가 및 씬 순서 정리 |



\## 테스트 내용



\### SettingsScene 직접 실행 테스트



SettingsScene을 직접 실행하여 제목, BGM Slider, SFX Slider, Back 버튼이 정상적으로 표시되는지 확인하였다.



\### Slider 조작 테스트



BGM Slider와 SFX Slider를 움직였을 때 각각의 퍼센트 텍스트가 변경되는지 확인하였다.



\### PlayerPrefs 저장 테스트



BGM과 SFX 값을 변경한 뒤 다시 SettingsScene에 들어갔을 때 이전 설정값이 유지되는지 확인하였다.



\### MainMenuScene 이동 테스트



MainMenuScene에서 Settings 버튼을 클릭했을 때 SettingsScene으로 이동하는지 확인하였다.



\### Back 버튼 테스트



SettingsScene에서 Back 버튼을 클릭했을 때 MainMenuScene으로 복귀하는지 확인하였다.



\### Build Settings 테스트



Build Settings에 MainMenuScene, SettingsScene, MapScene, GameScene이 정상적으로 등록되어 있는지 확인하였다.



\## 완료 결과



22일차 작업을 통해 Project Q에 설정 화면의 기본 구조가 추가되었다.



이제 메인 메뉴에서 SettingsScene으로 이동할 수 있고, BGM과 SFX 볼륨 값을 Slider로 조절한 뒤 PlayerPrefs에 저장할 수 있다.



아직 실제 사운드 크기에는 적용되지 않았지만, 이후 AudioManager와 AudioMixer를 연결하기 위한 설정값 저장 기반이 마련되었다.



\## 다음 개발 방향



23일차에는 AudioManager 기본 구조를 구현한다.



주요 작업은 다음과 같다.



\- AudioManager 생성

\- 씬이 바뀌어도 유지되는 사운드 관리 구조 구현

\- BGM AudioSource 추가

\- SFX AudioSource 추가

\- PlayerPrefs에 저장된 BGM/SFX 값을 실제 볼륨에 적용

\- SettingsScene에서 변경한 값이 실제 소리에 반영되도록 연결

