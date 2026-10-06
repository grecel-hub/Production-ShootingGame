# Production-ShootingGame
求职作品-射击类

一些难点：
    1.换弹动画左手位置偏差太大。方案：在换弹动画控制左手IK Target的Transform。新问题：该动画只使用双臂Mask，但是预览状态下全身都在动导致动画里添加的左手IK Target的Transform与实际有偏差。

    -0.051795       -0.085204       0.029274        -0.055063
    0.084531        0.074868        0.070783        0.001397
    -0.14282        -0.074807       -0.074721       -0.16233
    11.748          24.663          19.719          -33.594
    88.531          81.46           77.836          34.808
    -9.613          8.755           3.627           -28.096


    Root Selector
│
└── Combat Selector 1f
│   │
│   ├── Attack Sequence 1f
│   │   ├── CanSeePlayer
|   |   ├── AimPlayer --未完成
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
    └── Sequence
        ├── MoveToPatrolPoint
        └── Wait --未完成