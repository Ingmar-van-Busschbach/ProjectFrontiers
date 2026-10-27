# ProjectFrontiers
Team Assignment for Saxion first semester of the first year. We worked on this with a team of 7, over the span of 3 weeks. Release page: https://ingmar-van-busschbach.itch.io/spells-and-shells

To run this project:
1. Download the project
2. Install Unity Hub and Unity 6000.0.66f2
3. Add the project to Unity Hub
4. Run the project

Some problems the engineers solved in this project:
1. We created a weapon system with configurable stats, including animations, weapon accuracy/bloom, camera recoil (but no weapon recoil), "multishot" for shotguns, audio, particle effects, aiming down sights, range, aim assist, bullet homing, magazine stats, burst fire support, and more.
2. We created an enemy with AI controlled tank turrets, with configurable firing angles and rotation speed. These turrets each have a weapon attached to them from the weapon system.
3. We created a voiced dialogue system that allows NPCs to speak to the player.
4. We created an AI enemy system that patrols through different checkpoints, and attacks the player when the player comes within line of sight, using a cone-shaped set of raycasts.

Issues with the project:
Transparent materials sadly stopped working near the end of production. We tried various solutions to fix the issue, but they did not work. All important materials have been made opaque because otherwise they would not render.
