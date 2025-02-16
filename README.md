# Boulder Dash - README

## 1. Comment jouer ?

### Principe de gameplay

Boulder Dash est un jeu d'arcade où le joueur contrôle Rockford, un personnage qui doit collecter des diamants tout en évitant les éboulements de rochers et de diamants. 
L'objectif est de récupérer tout les diamants du niveau.

Commandes

> Le joueur peut se déplacer grâce aux touches directionnelles.

Comment gagner ?

> Collecter tous les diamants.

Comment perdre ?

> Se faire écraser par un rocher ou un diamant qui tombe.

## 2. Le code source

### Structure

Le projet suit une architecture orientée objet. Voici les principales classes et leur rôle :

- *Game.cs* : Gère la boucle principale du jeu.
- *Rockford.cs* : Contrôle le personnage du joueur.
- *Tilemap.cs* : Gère la grille du jeu et les interactions avec les objets.
- *Entity.cs* : Classe de base pour toutes les entités (Rochers, Diamants, Dirt, etc.).
- *Boulder.cs* : Gère le comportement des rochers (chute, collision).
- *Diamond.cs* : Définit les règles des diamants (chute, collecte).
- *GameScene.cs* : Gère l'ensemble des entités et leur mise à jour.
- *AssetsManager.cs* : Permet la gestion de toute les assets du jeu (textures, sons, etc).
- *ScenesManager.cs* : Permet de gérer les différents scènes du jeu.

### Algorithmes de grilles :

Le jeu repose sur une grille où chaque cellule correspond à un élément du décor (vide, dirt, rocher, diamant).
- Lors des déplacement de Rockford : 
> - On vérifie si la case cible est vide pour autoriser le déplacement.
> - Si il y a de la terre on peut se déplacer et creuser. (On fait disparaitre la terre)
> - Si il y a un diamants on le ramasse et on le décompte des diamants restants.
> - On vérifie si un rocher peut être pousser.

- Chute des objets :
> - Un rocher ou un diamant tombe s’il y a du vide en dessous.
> - Un rocher peut rouler sur les côtés si il n'y a aucun obstacle.
> - Un rocher ou un diamant peut provoquer un "éboulement" si il tomber sur un autre rocher ou diamant et que l'espace à côté et vide.
> - Un rocher ou un diamant qui tombe sur Rockford entraîne sa mort.

### Service Locator :

Le Service Locator centralise l'accès aux services du jeu, comme les textures ou l'audio mais également les différentes scènes. Cela évite de devoir passer ces services manuellement dans le constructeur de chaque objet.

- Services mis en oeuvre :
> - Gestion des assets (textures, sons, etc).
> - Un ScenesManager pour permet de créer et d'accèder facilement à différentes scènes (Menu, Jeu, etc).

- Utilité :
> - Simplifie l'accès aux ressources sans les instancier à plusieurs endroits.
> - Facilite la création de nouvelles scènes.

- Exemple d'utilisation :

> Lorsque l'on crée l'assetsManager, on l'ajoute directement aux gestionnaire de services :

    public  AssetsManager()    
    {    
	    Services.Register<IAssetsManager>(this);    
    }

> On peut ensuite créer dans notre classe Game, par exemple, un textureSet pour les entitées :

    assetsManager.AddTextureSet("Entity",  new  List<(int  id,  string  name,  string  path)>
    {    
        (1,  "Dirt",  "Assets/Images/Dirt.png"),    
        (2,  "Boulder",  "Assets/Images/Boulder.png"),    
        (3,  "BoulderFalling",  "Assets/Images/Boulder.png"),    
        (4,  "Rockford",  "Assets/Images/Rockford.png"),    
        (5,  "Diamond",  "Assets/Images/Diamond.png"),    
        (6,  "DiamondFalling",  "Assets/Images/Diamond.png"),    
    });

> On peut ensuite facilement récupérer un élément de ce textureSet pour l'affichage, par exemple pour Rockford :

    Raylib.DrawTexture(assets.GetTextureFromSet("Entity",  (int)IdTile.Rockford),  (int)posInWorld.X,  (int)posInWorld.Y,  Color.White);

> On peut également charger, par exemple, une musique :

    assetsManager.Load<Music>("Menu", "Assets/Musics/menu.wav");
    
> Et l'utiliser à tout moment dans une autre classe, ici MenuScene :

    menuMusic  =  assets.Get<Music>("Menu");
