# NEVE toolkit
![Reinforcement learning model learning to find a burrow](readme_gif.gif)

# About

Neuroecology virtual environments (NEVE) is a toolkit to allow researchers to build virtual environments to use with animals in behavioural and physiological experiments or reinforcement learning models in a simulation. NEVE leverages the highly developed [Unity](https://unity.com/) engine to simulate and display stimuli. It provides the ability to simulate environments in a closed-loop fashion, providing visual feed-back to animal movement and/or behaviour.

NEVE uses the [Unity Python API](https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Python-API.md) and [Unity Machine Learning Agents Toolkit](https://github.com/Unity-Technologies/ml-agents) to record data, modify experimental conditions and introduce animals or reinforcement learning agents into simulations. A number of pre-built objects and environments are provided to allow researchers to quickly build, test and deploy experiments on conventional computer monitors using custom animal inputs (e.g. movement recorded on a trackball).

Our examples have been applied to certain animals, specifically fiddler crabs and deep-sea hyperiid amphipods, however, the use of these tools are not species specific. We have created experiments previously time-intensive or infeasible with conventional approaches, such as those with physical objects or simple computer simulation tools (e.g. Psychtoolbox). We have also recreated simpler traditional physiological experiments that are routinely conducted on multiple species of animals, including humans. Our tools and examples are highly applicable to most species with visual systems and are designed to be easily modifiable if required.

# Getting started
## Experiments with animals
[Running a pre-built experiment](NEVE_python/README.md)
[Creating a custom experiment](Docs/creating_custom_experiment.md)

## Experiments with machine learning models
[Running a pre-built experiment for reinforcement learning](Docs/running_prebuilt_experiment_for_reinforcement_learning.md)

# Project structure

NEVE is seperated into two components:

1) NEVE_unity: A Unity project containing the files and tools needed to build a experiment.
2) NEVE_python: A Python project used to control built experiments for 
