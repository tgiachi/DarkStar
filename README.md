# Dark Star MMO

![Dark Star Logo](/imgs/DarkStarLogoSmall.png)

# Milestones: 0.11.0
In this month I got to work on the project in my spare time. We are starting to see the project take shape!

Breaking changes:
- I refactored the script engine from LUA to Javascript (js is much softer for anonymous functions)
- I created a repository for scripts and example world generation (you can find it at this [LINK](https://github.com/tgiachi/DarkStar.Scripts)). It is a typescript project with types support
- I created the client (initially it had to be in webgl but in the end I created it with Avalonia, I feel much more confident)
- I have optimized the code and all the classes
- It's possible (it's an experimental feature and if someone wants to help me do it, it's welcome) to create the definitions in typescript (types.d.ts) in order to have a minimum of development comfort (yes you have the possibility to have autocomplete to create the world!)
- Two example npc's have been implemented, the cat, the mushroom seeker and the mushroom!
- Thanks to the advice received on reddit, I implemented SignalR as a communication protocol between the client and the server

# Yes, all good, but how to test it?
Ensure you have installed `docker`

```bash
  docker pull tgiachi/darkstar.server
  git clone git@github.com:tgiachi/DarkStar.Scripts.git darkstar_root
  docker run -p 5000:5000 --env DARKSTAR_ROOT_DIRECTORY="data/" -v $(pwd)darkstar_root/:/data tgiachi/darkstar.server
```
  Then compile `DarkStar.Client` and run it with username `test@test.com` and password `1234!`



# ScreenShots

![Screenshot 1](/imgs/screenshot_1.png)
![Screenshot 2](/imgs/screenshot_2.png)
![Screenshot 3](/imgs/screenshot_3.png)

 **!!! README being updated !!!**


# Preface

Hi, my name is Tommaso and I've always been passionate about fantasy and mmorpg games, especially when (back in 1999) I discovered Ultima OnLine.

I am a very curious person and as such every year I set myself challenges in order to grow professionally and the challenge of 2023 is to create a 2d MMORPG in C#

You can find me on linkedin [Tommaso Giachi | LinkedIn](https://www.linkedin.com/in/tgiachi/)



# What is Dark Star?:

**The project is still in the early stage**

Dark Star is a fantasy roguelike game where the entire game world is procedurally generated. In this game, the player has to go through the underworld of the game,

encountering monsters and exploring procedurally generated dungeons. The game is known for its difficulty, as the player has to fight for survival in a hostile world, where even one wrong choice can lead to the death of his character.

In Dark Star, the player has the choice between different character classes, each with their own unique abilities and characteristics. Furthermore, the game features a progression system that allows the player to upgrade his character as he progresses through the game.




# Used .net core libraries

- GoRogue (https://github.com/Chris3606/GoRogue)
- TinyCsv (https://github.com/fmazzant/TinyCsv) thanks Federico
- ProtoBuf C#
- SignalR




# Milestone 0.5.0

- ~~Management of character and account creation~~
- ~~Creation of the AI dummy (cat and mushroom finder human)~~
- ~~Management of the movements of the character~~
- Basic inventory management
- ~~Random map creation (city [ which has 10 dungeons ] )~~


# How collaborate?
- Catch me in https://discord.gg/DkNBQg8Etv
- Open a PR


# Inspirations

For the creation of Dark Star I was inspired by

- NetHack
- Tales of Maj'Eyal
- Cataclysm Dark Days Ahead

## Star History

[![Star History Chart](https://api.star-history.com/svg?repos=tgiachi/DarkStar&type=Date)](https://star-history.com/#tgiachi/DarkStar&Date)


[![Stargazers repo roster for @tgiachi/DarkStart](https://reporoster.com/stars/tgiachi/DarkStar)](https://github.com/stars/tgiachi/DarkStar/stargazers)

[![Forkers repo roster for @stars/tgiachi/DarkStar](https://reporoster.com/forks/tgiachi/DarkStar)](https://github.com/tgiachi/DarkStar/network/members)

By the way, thank you so much for [![Stars](https://img.shields.io/github/stars/tgiachi/DarkStar?style=social)](https://github.com/IntelligenzaArtificiale/Free-AUTOGPT-with-NO-API/stargazers) and all the support!!
