# Production-ShootingGame
求职作品-射击类



//敌人AI行为树
    Root Selector
│
└── Combat Selector 1f
│   │
│   ├── Attack Sequence 0.5f
│   │   ├── CanSeePlayer
|   |   ├── Attack  --update里会判断canSeePlayer和弹匣是否还有子弹
|   |
|   ├── Reload
│   │
│   └── Investigate Selector -0f
|       ├── Chase    --前往玩家上次出现位置，中途发现玩家Success回到Combat Selector，没有发现玩家到达目的地后Failure，前往下一个子节点Search Selector
|       |
|       ├── Search Selector -0f
|           ├──SearchLeaf   --向左扫视，发现玩家Success，回到Combat Selector，扫视结束Failure，前往下一个子节点SearchRight
|           ├──SearchRight  --向右扫视，发现玩家Success，回到Combat Selector，扫视结束Failure，回到Root Selector，前往下一个子节点Patrol
│           
│
│
└── Patrol Sequence
    │
    ├── MoveToPatrolPoint
    └── Wait