## Unity Prototype Color Cube Wars

![alt tag](https://raw.github.com/TPen/Unity_Prototype_CCW/master/Example_Image.jpg)
Weplayer of the prototype: http://portfolio.felixkate.net/CCW_prototype.html


A small one man prototype made with unity in about 22h. I decided to upload this to give an insight into my workflow (and learn about manipulating an objects vertex colors during runtime). This is uploaded under the MIT license so feel free to use what you want for your own projects.

 <br />
#### About this project:
If you know about Nintendos current game Splatoon this game has a similiar goal. Up to 4 players have to color the game board into their selected colors. Winner of course is the one that colored most of it once the timer runs out.
The pawns for this purpose are cubes which roll over the tiles of the game board while leaving a trail of color behind. <br />
If the cubes avoid coliding or stopping they will increase their speed over time. Every second there is a small chance that an item spawns. Currently there are 3 types of items: The blue one will increase the cubes speed; the green one will send out a shockwave that resets the speed of all other players within a range; lastly the orange one will color every tile in a certain radius into the color of the player that collected it.
 <br />
There's also a few changeable settings. Those include game-time, spawnrate and the size of the game board.

 <br />
#### About my work on this:
I spent less than the first 8 hours to create the actual gameplay. This includes the game board (mesh generations as well as the vertex color mechanic), the player movement, the chest object (mechanics and particles) as well as most of the non UI shaders. <br />
The UI was probably the thing that took longest here. The amount of buttons (implementation over script) and the actual layout took quite a while. <br />
I can't really say how long changes and fixes took scince I did most of them together with the previous tasks.

 <br />
Just a few small ideas what could be added if I would try to make this into a game
- Network over Unity or Photon
- Use the vertex coloring and replace the setting (For example every person plays a season of the year; the most powerful one will change the scenery: winter would make a snowfloor, the background would change and it would begin to snow etc)
- Maybe convert the controls to mobile ones (this would mean max 2 players though or an online feature)
- Make a single player where the player has to defend an area of colored tiles against different moving enemies that color everything black (attack in waves)

 <br />
Font used in the game:     Montserrat  (http://www.fontsquirrel.com/fonts/montserrat) <br />
Prototype sounds:          sfxr (http://www.drpetter.se/project_sfxr.html) <br />
Engine:                    Unity Engine (https://unity3d.com/)
