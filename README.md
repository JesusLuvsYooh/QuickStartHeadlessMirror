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
