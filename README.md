# Obstacle-course-Machine-Learning
I coded a simple neural network in C# from scratch to control each individual in order to make them learn to drive in a track, I used the game engine Unity 3D to create the environment.

# Machine learning
In the process of learning, I used genetic algorithms to evolve the neural network. The fitness of each individual is calculated depending on the time that the individual managed to drive without colliding,
and the first indivuduals to cross a checkpoint or the finish line get extra fitness score. I also added a neural network representation of the individual with the highest current fitness score, as well as the input values of the neural network and the output value.

# Map maker
I coded a map maker, that I integrated in the project, to be easier to create diferent maps. The map can be modified during training (remove or add obstacles). The map can be generated using one of three primitive blocks, hexagons, squares or triangles.

Map maker example:
![](https://github.com/JotaToiro/Obstacle-course-Machine-Learning/blob/main/example1.gif)

Training example:
![](https://github.com/JotaToiro/Obstacle-course-Machine-Learning/blob/main/example2.gif)
