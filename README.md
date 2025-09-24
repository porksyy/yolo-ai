# YOLO: Your Life, Your Rules
The project aims to simulate the experience of raising a digital character through different stages of life by placing the player in the position of a caretaker. This study proposes a system that uses Reinforcement Learning (RL), to create a dynamic and context-aware virtual character.

## Project Overview
YOLO simulates the caregiving experience by placing players in the role of a caretaker for a digital baby. The infant has needs-based systems for:
🍼 Hunger
😴 Sleep
🧷 Diaper Cleanliness

These needs are tracked by virtual sensors, influencing whether the baby cries or stays content. Using Unity ML-Agents and reinforcement learning, the baby learns to associate states ) with actions and adapts its communication strategies over time.

## Data & Results

We trained the baby agent across three age groups:

- **0–3 months**  
- **4–6 months**  
- **6–12 months**  

### Behavioral Metrics

| Age Group   | Feed Count (Mean ± SD) | Sleep Hours (Mean ± SD) | Correct Cries (%) | False Cries (%) |
|-------------|-------------------------|--------------------------|-------------------|-----------------|
| 0–3 months  | 7.5 ± 0.3              | 14.8 ± 0.5              | 89.2              | 4.3             |
| 4–6 months  | 6.0 ± 0.5              | 13.2 ± 0.8              | 85.5              | 7.0             |
| 6–12 months | 4.8 ± 0.7              | 12.0 ± 1.0              | 79.8              | 9.1             |

Feeding frequency decreased with age, sleep duration gradually shortened, and crying accuracy declined slightly as needs became more complex.  

### Policy & Learning Metrics

| Age Group   | Policy Entropy (Mean) | Extrinsic Value Estimate (Mean) | No Action Count (Mean) |
|-------------|------------------------|----------------------------------|-------------------------|
| 0–3 months  | 0.32                  | 0.58                             | 4.1                     |
| 4–6 months  | 0.34                  | 0.61                             | 3.2                     |
| 6–12 months | 0.38                  | 0.65                             | 2.7                     |

Entropy values showed healthy exploration, value estimates improved steadily, and indecisiveness (no action counts) decreased over time.  

### Conclusion  
The baby agent successfully learned **feeding, sleeping, and communication behaviors** that evolved across developmental stages.

## Tech Stack

- Game Engine: Unity

- AI Training: Unity ML-Agents Toolkit (PPO algorithm)

- Programming: C#, Python

- Concepts: Reinforcement Learning, Needs-Based Motivation


