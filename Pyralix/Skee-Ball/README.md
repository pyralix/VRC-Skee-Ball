# VRC Skee-Ball

<p align="center"><i>Many thanks to Theycallhimcake, Burnadette, and Coffepot for the inspiration to create in VRChat, to Varneon for their expertise refactoring and optimizing, and to the udon-questions channel for helping me fumble through learning Unity.</i></p>

![Header](https://raw.githubusercontent.com/pyralix/VRC-Skee-Ball/main/.github/images/skeeball.PNG)


This is the first prefab that I've made for VRChat, and I hope you find it as enjoyable to use and edit to your liking as I did creating it. It was largely a vehicle for learning Unity, Udon, and the VRC Networking functionalities. I appreciate positive and negative feedback alike and plan to improve it based on the feedback I receive!

This prefab is using object sync and manual udon sync exclusively in order to be as network efficient as I can make it with my current understanding. 

Generally, you use the game like this:
1. Press start and the game lights turn on.
2. After 9 balls the game ends and the lights turn off.
3. If you have the highest score your name and score will be saved.
4. If you need help, turn the booster on and your ball will get a push at the end.
5. Use the reset button to return the machine to its starting state.
6. If in VR, try standing a bit further away from the machine (or go nuts and run up the ramp like you did irl that one time XD)

You can find me at Pyralix#3552 on Discord and of course on VRChat!

# Installation

Requirements:

1. Latest VRChat SDK3
2. Latest [UdonSharp](https://github.com/MerlinVR/UdonSharp)

# How to use:

1. [Download the latest unitypackage](https://github.com/pyralix/VRC-Skee-Ball/releases/download/v1.3/Skee-ball.v1.3.unitypackage).
2. Import Skee-ball v1.3.unitypackage into your project that already has the SDK and UdonSharp loaded.
3. Drag prefab in the Skee-Ball v#.# folder into your world or open the example scene.
4. Consider disabling the audio sources on the balls if you have more than 1 machine as they use up many audio slots due to being not well optimized yet.

# Crediting

I want to convert my time into enjoyment for others! To enrich the VR experience of others is my goal, so I'm providing my prefabs for free. I hope that you'll decide to leave my name on the prefab as a small way to provide credits. But hey, once you have it it's all yours to edit to your heart's content! If you'd like to support me you can at https://pyralix.booth.pm/
