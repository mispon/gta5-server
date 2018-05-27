CLIENT
------------

The client part is written in Typescript and is compiled by Webpack into a single build.js file in the public folder.
That is, to collect the client part, you need to install Node.js
Otherwise, it's a regular client that accepts events from the server, does something and throws events back to the server.
All events that exist on the server are declared in two classes: ServerEvent.cs and ClientEvent.cs in the Server / Constants / folder.
In the ui folder you will find the interface, speedometer, authorization and character customization windows. They are made on pure html + js + css, there everything is very badly written, so I advise
The user interface is immediately done using Webpack and less / sass. If you try, you can easily do it on React.

DATABASE
------------

This is a wrapper over a database that is based on linq2db. I used sql server, for me it was very convenient. The structure of the tables I saved and zakomitil in the root directory gta-mp-server.bak.
You will simply need to install Sql Management Studio, deploy the backup of this file and in ConnectionSettings.cs specify your data for the connection.

TRANSPORT TYPES
------------

This is the general data for the database, server and site. I collected them as a local nuget-package. This was convenient, since I was working on the server and the site at the same time. But you can not be bothered and just create
direct link to this package wherever required.

SERVER
------------

Finally, the most interesting. In general, I think that folder names display the essence of the content in them quite well.
An important point: the project uses Ninject and all the base and server bindings are stored in the IoC folder. If you are not familiar with Ninject and what it is for, it will be helpful to find out.

In short, what is implemented on the server:

- 14 different works with sublevels from 1 to 5. Working levels affect salary and experience. List of works: loader, farmer, tractor driver, forklift driver, food courier, van driver with food, tow truck, trucker, fisherman, builder, pilot, policeman, taxi driver, bus driver;
- Game command events that start automatically every hour: a sniper duel, a prison riot, and so on;
- 4 kinds of races;
- Voice chat;
"A gang system, I call it clans." Total made 3 clans: Michael, Trevor and Frank. Every day for the clans a battle starts for the territory (A marker appears in one of the districts and it must be captured and held).
  If the clan owns the territory, then it receives income from all stores, gas stations, etc.
  Also, clans have missions that are started and executed jointly by clan members. The essence of the missions is that you need to take a van, somewhere to get there, load the van with valuable cargo and successfully take it to the base.
  Leaders of gangs can take various tasks, like quests: steal a supercar, take drugs to dealers or replace documents in a police station;
- Shops, gas stations, clothing stores and car dealerships;
- Houses with wardrobes, storages and garages. To put your transport in the garage, it's enough just to drop into it and exit. Exactly as in GTA Online;
- Public parking and a parking lot, where the car gets to, if you just left it;
- Driving school;
- More about the work of the cop: when real players fight or kill each other, all police officers receive an appropriate call and go to the scene of the crime.
  If everything is clean, then the server automatically creates patrol tasks for policemen. Police officers can check the search for players, handcuffs, fine, forcefully put in a police car and take them to prison.
  But only if the player has a search, otherwise the cops get fines. Also the cops have a system of ranks, with the rise of which you get a new weapon;
- Player's inventory and trunk of a personal vehicle. Each thing has its own weight, the player has a limited payload, and the car also has a limited capacity. Weight restrictions are not only in the storehouse of the house;
- A mobile phone with the ability to call other players who have it, too;
- Weapons;
- Tuning of personal transport;
- Street fights without rules 1 on 1;
- Skydiving;
- Hospitals
- Rent scooters;

And, in fact, there are a lot of small things that you can write about for a long time. But all the most important and most importantly, I think, numbered.
