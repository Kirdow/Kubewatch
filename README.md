# Kubewatch <a href="https://github.com/Kirdow/Kubewatch/blob/master/LICENSE"><img src="https://img.shields.io/badge/license-AGPL%203.0-green.svg"></a>
Rubik's Cube timer made to run on both desktop and mobile devices, with timer spam protection and other useful features.

## About Kubewatch
Kubewatch started after a frustration with another speedcube timer website where after a PB, nerves kicked in and I jittered on the spacebar causing me to miss the screenshot opportunity on the new PB. The PB was a first in 5 years of inactivity going from 23 seconds to 20 seconds. This made me want to make my own timer with my own features, with a cooldown on the spacebar after you stop the timer.

## Features
Currently Kubewatch is early in development, but it got some neat features.<br>
Current features are checked while planned features are unchecked.<br>
May not update immediately after a feature commit.

- [x] Scramble sequence at the top of the screen
- [x] Animated 3d cube showing the scramble sequence over the course of 2 seconds
- [x] Big timer during a solve
- [x] Small stopped timer between solves showing last time
- [x] Cooldown for timer start after timer has stopped
  - [x] This cooldown is set to 2 seconds
  - [x] The small stopped timer is green while this cooldown is active
  - [ ] Configurable timer cooldown color
- [ ] Timer history
  - [ ] Saving timer history to disk for persistent use
  - [ ] Saving the scramble sequence used for that solve
  - [ ] Ability to recall the result screen of that solve to main screen
  - [ ] Ability to remove individual solves from the history list
- [ ] Ability to rotate the 3d cube to validate all sides of the scramble
- [ ] Ability to use multiple Rubik's Cubes
  - [ ] 2x2 support
  - [x] 3x3 support
  - [ ] 4x4 support
  - [ ] 5x5 support
  - [ ] 6x6 support
  - [ ] 7x7 support
  - [ ] Megaminx support
  - [ ] Kilominx support
  - [ ] Gigaminx support
- [ ] Ability to score past solves into different categories
  - [ ] Single best
  - [ ] Average
  - [ ] Avg. of 5
  - [ ] Avg. of 10
  - [ ] Avg. 3 of 5
  - [ ] Avg. 10 of 12
- [ ] Ability to configure limited inspection time
  - [ ] Ability to play sound after inspection time
  - [ ] 3 seconds
  - [ ] 5 seconds
  - [ ] 8 seconds
  - [ ] 10 seconds
  - [ ] 15 seconds
  - [x] No inspection time

# License
Kubewatch is licensed under [GNU Affero General Public License v3.0](https://github.com/Kirdow/Kubewatch/blob/master/LICENSE)