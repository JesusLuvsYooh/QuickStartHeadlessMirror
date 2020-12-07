# QuickStart V6 - Headless - Mirror
 The public quickstart guide, modified to be headless server and client.
 
 The quickstart is how it sounds, to get a user up and running with a multiplayer scene.
 It is client authority based, contains very few comments or explanations, and doesnt follow 'best practises', for example firing physical objects rather than raycasting.
 But it serves its purpose of being a template with basic mirror multiplayer features.  :)

 Original Guide located here:
 https://mirror-networking.com/docs/Articles/CommunityGuides/MirrorQuickStartGuide/index.html
 

# Running headless server or client

Inside terminal/cmd console, add the complete line, type 0 for default or none.

File Location - Client/Sever - frame rate - host IP - host port - traffic
      
Example of server arguements:
C:\Users\location\QuickStartHeadless s 30 0 0 0

Example of client arguements"
C:\Users\location\QuickStartHeadless c 30 123.123.123.123 0 0 3

# Traffic explanation
- 0 = none  (just initial player setup such as name and colour)
- 1 = light  (card game) (traffic: 0 + some cmd/rpcs every few seconds)
- 2 = active  (social game) (traffic: 1 increased + player rotation only + rigidbody sphere projectile)
- 3 = heavy  (mmo) (traffic: 2 increased + player movement)
- 4 = frequent (fps) (traffic: 3 increased)

# Other
Port argument section for headless has been temporary disabled, whilst doing the many transport tests.
Directly change port to 7777 on NetworkManagers Transport.

# Walkthrough Guide
1: Download the QuickStart project, open with Unity (made with 2019)

2: Import Mirror from Asset Store or Github Releases<br/>

3a: Check the Player Prefab has NetworkTransform on it<br/>
![Build Settings](https://user-images.githubusercontent.com/57072365/101388283-a1d20b80-38b7-11eb-9adf-28a24ad1a63a.jpg)

3b: Check MyScene NetworkManager has a Transport, you can use any (Telepathy, LiteNet, Asio, Ignorance etc)<br/>
![Build Settings](https://user-images.githubusercontent.com/57072365/101388278-a0084800-38b7-11eb-8462-bcb47933e91a.jpg)

3c: Check the Build Settings has the 2 main scenes, make sure GamesList is set as the first scene.<br/>
Also check that these are set in NetworkManager (GamesList - offline map and MyScene - online map)
![Build Settings](https://user-images.githubusercontent.com/57072365/101388267-9da5ee00-38b7-11eb-9193-f4b5cea34c0d.jpg)

