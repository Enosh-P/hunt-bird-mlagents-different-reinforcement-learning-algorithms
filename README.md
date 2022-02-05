# Bird-Hunt using mlagents in Unity

Making an agent to shoot bird using RL and Imitation learning using mlagents in unity. Here I have trained the agent on Bird-Hunt game to shoot the target using following Reinforcement Learning Algorithms:

1. PPO Algorithm
2. GAIL
3. Behaviour Clonning

### Game Environment Complexities:

1. There are 3 birds Yellow Bird(Target), Red Bird(Bonus Target) and Black Duck(Bomb shell)
2. The agent is provided with a ammo pallet containing 10 ammo initially.
3. Hitting the Bonus Target provides an extra ammo
4. Reloads when there is no bullet.
5. Added No Shoot Zone.

### Agent Learns the follwing:

1. Hit 2 Yellow Bird first.
2. Hit a Red Bonus Bird after 2 Yellow Bird (Provide more priority).
3. Observes the ammo availability as well.
4. When there is no ammo, go to No Shoot Zone(NSZ).
5. When there is no bird on the screen, go to NSZ as well.

### Reward Function:

1. Hitting taget provides +1
2. Hitting Bomb shell provides -0.5
3. Missing a hit provides -0.5
4. Hitting Bonus Target provides +1.4
5. Not hitting anything when reload provides +1

### Expert Playing:

![alt text](https://github.com/Enosh-P/hunt-bird-mlagents-different-reinforcement-learning-algorithms/blob/main/Expert_demo.gif?raw=true)

## Graphs:
