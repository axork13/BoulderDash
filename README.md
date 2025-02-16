Boulder Dash - README

1. Comment jouer ?

Principe de gameplay

Boulder Dash est un jeu d'arcade où le joueur contrôle Rockford, un personnage qui doit collecter des diamants tout en évitant les éboulements de rochers et de diamants. 
L'objectif est de récupérer tout les diamants du niveau.

Commandes

Le joueur peut se déplacer grâce aux touches directionnelles.

Comment gagner ?

Collecter tous les diamant.

Comment perdre ?

Se faire écraser par un rocher ou un diamant qui tombe.

2. Votre code source

Structure

Le projet suit une architecture orientée objet. Voici les principales classes et leur rôle :

Game.cs : Gère la boucle principale du jeu.
Rockford.cs : Contrôle le personnage du joueur.
Tilemap.cs : Gère la grille du jeu et les interactions avec les objets.
Entity.cs : Classe de base pour toutes les entités (Rochers, Diamants, Dirt, etc.).
Boulder.cs : Gère le comportement des rochers (chute, collision).
Diamond.cs : Définit les règles des diamants (chute, collecte).
GameScene.cs : Gère l'ensemble des entités et leur mise à jour.
AssetsManager.cs : Permet la gestion de toute les assets du jeu (textures, sons, etc).
ScenesManager.cs : Permet de gérer les différents scènes du jeu.




