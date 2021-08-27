# NEVE toolkit
![](readme_gif.gif)
# About

Neuroecology virtual environments (NEVE) is a toolkit to allow researchers to build virtual environments to use with animals behavioural and physiological experiments or reinforcement learning models in a simulation. NEVE leverages the highly developed [Unity](https://unity.com/) engine to simulate and display stimuli. It provides the ability to simulate environments in a closed-loop fashion, providing visual feed-back to animal movement and/or behaviour.

NEVE uses the [Unity Python API](https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Python-API.md) and [Unity Machine Learning Agents Toolkit](https://github.com/Unity-Technologies/ml-agents) to record data, modify experimental conditions and introduce animals or reinforcement learning agents into simulations. A number of pre-built objects and environments are provided to allow researchers to quickly build, test and deploy experiments on conventional computer monitors using custom animal inputs (e.g. movement recorded on a trackball).

Our examples have been applied to animals, specifically fiddler crabs and deep-sea hyperiid amphipods, however, the use of these tools are not species specific. We have created experiments previously time-intensive or infeasible with conventional approaches, such as those with physical objects or simple computer simulation tools (e.g. Pscychtoolbox). We have also recreated simpler traditional physiological experiments that are routinely conducted on multiple species of animals, including humans. Our tools and examples are highly applicable to most species with visual systems and are designed to be easily modifiable if required.

# Getting started
[Setting up unity](Docs/getting_started/unity_setup.md)

[Running a build from python](NEVE_python/README.md)

[Getting started controlling unity from python for development](Docs/getting_started/controlling_from_python.md)

(c) 2021 Jake Manger
