\# Project Q 개발 기록 - 1Day



\## 1. 개발 목표



오늘의 목표는 Project Q의 1차 프로토타입 기반을 만드는 것이었다.



전체 게임 시스템을 한 번에 구현하지 않고, 먼저 플레이어 조작의 핵심이 되는 이동, 회피, 기본 스탯 구조를 구현하였다.



\## 2. 오늘 구현한 내용



\### 2.1 Unity 프로젝트 기본 정리



\- Unity 2D 프로젝트를 기준으로 작업을 시작하였다.

\- GameScene을 생성하였다.

\- 프로젝트 폴더 구조를 정리하였다.



생성한 주요 폴더는 다음과 같다.



```text

Assets

├── Scenes

├── Scripts

│   ├── Player

│   ├── Enemy

│   ├── Bullet

│   ├── Card

│   ├── Boss

│   ├── Managers

│   └── UI

├── Prefabs

├── Sprites

├── UI

└── ScriptableObjects# Project Q 개발 기록 - 1Day



\## 1. 개발 목표



오늘의 목표는 Project Q의 1차 프로토타입 기반을 만드는 것이었다.



전체 게임 시스템을 한 번에 구현하지 않고, 먼저 플레이어 조작의 핵심이 되는 이동, 회피, 기본 스탯 구조를 구현하였다.



\## 2. 오늘 구현한 내용



\### 2.1 Unity 프로젝트 기본 정리



\- Unity 2D 프로젝트를 기준으로 작업을 시작하였다.

\- GameScene을 생성하였다.

\- 프로젝트 폴더 구조를 정리하였다.



생성한 주요 폴더는 다음과 같다.



```text

Assets

├── Scenes

├── Scripts

│   ├── Player

│   ├── Enemy

│   ├── Bullet

│   ├── Card

│   ├── Boss

│   ├── Managers

│   └── UI

├── Prefabs

├── Sprites

├── UI

└── ScriptableObjects



2.2 플레이어 오브젝트 생성

Player 오브젝트를 생성하였다.

임시 그래픽으로 Square Sprite를 사용하였다.

Rigidbody2D를 추가하였다.

CircleCollider2D를 추가하였다.

Rigidbody2D의 Gravity Scale을 0으로 설정하였다.

회전 방지를 위해 Freeze Rotation Z를 설정하였다.

2.3 플레이어 기본 스탯 구현



PlayerStats.cs를 작성하였다.



구현한 기능은 다음과 같다.



최대 체력

현재 체력

최대 마나

현재 마나

초당 마나 회복

보호막 수치

피해 처리

마나 사용 처리

사망 처리 로그



현재 기본 수치는 다음과 같다.



항목	값

HP	100

MP	5

MP 회복량	초당 1

Shield	0

2.4 플레이어 이동 구현



PlayerController.cs를 작성하였다.



구현한 기능은 다음과 같다.



W 입력 시 위로 이동

A 입력 시 왼쪽 이동

S 입력 시 아래로 이동

D 입력 시 오른쪽 이동

대각선 이동 시 속도가 과하게 빨라지지 않도록 normalized 처리

Rigidbody2D를 이용한 물리 기반 이동

2.5 플레이어 회피 구현



PlayerDodge.cs를 작성하였다.



구현한 기능은 다음과 같다.



Left Shift 입력 시 회피

마지막 이동 방향으로 회피

회피 중 이동 입력 제한

회피 중 무적 상태 적용

회피 쿨타임 적용



현재 회피 수치는 다음과 같다.



항목	값

회피 속도	12

회피 시간	0.2초

무적 시간	0.3초

회피 쿨타임	1.2초

3\. 발생한 문제와 해결

3.1 플레이어가 움직이지 않는 문제



처음에는 코드 오류가 없는 것처럼 보였지만, 플레이어가 움직이지 않았다.



디버그 로그를 추가하여 확인한 결과, Input.GetKey() 입력이 정상적으로 들어오지 않는 상태였다.



3.2 원인



Unity 프로젝트의 입력 방식이 New Input System만 사용하도록 설정되어 있었지만, 작성한 코드는 구형 입력 방식인 UnityEngine.Input.GetKey()를 사용하고 있었다.



Console에 다음 오류가 발생하였다.



InvalidOperationException: You are trying to read Input using the UnityEngine.Input class,

but you have switched active Input handling to Input System package in Player Settings.

3.3 해결 방법



Unity 설정에서 입력 방식을 변경하였다.



Edit > Project Settings > Player > Other Settings > Active Input Handling



값을 아래와 같이 변경하였다.



Both



설정 변경 후 Unity를 재시작하였고, WASD 이동 입력이 정상적으로 작동하였다.



4\. 현재 완료 상태



현재까지 완료된 기능은 다음과 같다.



Player 오브젝트 생성 완료

WASD 이동 구현 완료

Shift 회피 구현 완료

회피 중 무적 상태값 구현 완료

HP / MP 기본 구조 구현 완료

마나 자동 회복 구현 완료

Unity Input System 문제 해결 완료

