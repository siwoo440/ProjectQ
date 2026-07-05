\# Project Q 개발 기록 - 2일차



\## 1. 개발 목표



2일차의 목표는 플레이어와 적이 서로 공격할 수 있는 기본 전투 구조를 구현하는 것이다.



1일차에서 플레이어 이동, 회피, HP/MP 구조를 만들었고, 2일차에서는 카드 발사, 적 피격, 적 탄환, 플레이어 피격 구조를 추가하였다.



\---



\## 2. 구현한 기능



| 구분 | 구현 내용 |

|---|---|

| HP/MP UI | 화면 좌상단에 HP, MP, Shield 표시 |

| 플레이어 탄환 | PlayerBullet 프리팹 생성 |

| 기본 카드 | 좌클릭으로 픽셀 샷 발사 |

| MP 소비 | 카드 사용 시 MP 1 감소 |

| 카드 쿨타임 | 1초 동안 재사용 제한 |

| 적 더미 | EnemyDummy 생성 |

| 적 체력 | EnemyHealth.cs로 적 HP 관리 |

| 적 피격 | 플레이어 탄환이 적에게 닿으면 HP 감소 |

| 적 사망 | 적 HP가 0이 되면 삭제 |

| 적 탄환 | EnemyBullet 프리팹 생성 |

| 적 공격 | EnemyShooter.cs로 플레이어 방향 탄환 발사 |

| 플레이어 피격 | 적 탄환에 맞으면 Shield 또는 HP 감소 |

| 회피 무적 | Shift 회피 중 탄환 피해 무시 |

| 적 이동 | EnemyMovement.cs로 플레이어에게 접근 후 거리 유지 |



\---



\## 3. 추가한 스크립트



| 파일명 | 위치 | 역할 |

|---|---|---|

| PlayerHUD.cs | Assets/Scripts/UI | HP, MP, Shield UI 표시 |

| Bullet.cs | Assets/Scripts/Bullet | 플레이어 탄환 이동과 적 피격 처리 |

| EnemyHealth.cs | Assets/Scripts/Enemy | 적 체력, 피해, 사망 처리 |

| CardManager.cs | Assets/Scripts/Card | 픽셀 샷 카드 사용 처리 |

| EnemyBullet.cs | Assets/Scripts/Bullet | 적 탄환 이동과 플레이어 피격 처리 |

| EnemyShooter.cs | Assets/Scripts/Enemy | 적이 플레이어를 향해 탄환 발사 |

| EnemyMovement.cs | Assets/Scripts/Enemy | 적이 플레이어에게 접근 후 거리 유지 |



\---



\## 4. 수정한 스크립트



| 파일명 | 수정 내용 |

|---|---|

| PlayerStats.cs | HP, MP, Shield 표시와 MP 사용 함수 정리 |

| PlayerDodge.cs | 외부 스크립트에서 무적 상태를 확인할 수 있도록 IsInvincible 추가 |



\---



\## 5. 추가한 프리팹



| 프리팹 | 설명 |

|---|---|

| PlayerBullet.prefab | 플레이어가 발사하는 기본 탄환 |

| EnemyBullet.prefab | 적이 발사하는 기본 탄환 |

| EnemyDummy.prefab | 테스트용 기본 적 |



\---



\## 6. 현재 가능한 플레이 흐름



```text

플레이어 이동

→ Shift로 회피

→ 좌클릭으로 픽셀 샷 발사

→ MP 감소

→ 탄환이 적에게 충돌

→ 적 HP 감소

→ 적 HP 0이면 삭제

