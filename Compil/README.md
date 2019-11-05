# Project
C# Compilateur project

# Auteurs
CAPODANO Thomas
HAMEL Ludovic

Classes et code source disponibles dans le dossier Compil/Compil/
Projet de tests unitaires disponible dans le dossier Compil/CompilTest

Lors des tests, un affichage de l'arbre syntaxique sera donné en premier.
Pour poursuivre et afficher le code compilé, merci d'appuyer sur une touche.
Le fichier de code machine est également généré dans le dossier Compil/Compil/bin/Debug/code.txt par défaut (si compilation du code source du projet en "Debug").

# Opérations fonctionnelles
 - Lecture d'un fichier contenant du code en langage C pour nos tests (.c)
    - Chemin du fichier pour tests par défaut que nous utilisons : Compil/Tests/test.c
    
 - Analyseur lexical:
      - Découpage des tokens fonctionnel
      
 - Analyseur syntaxique (ces points traitent de la construction de l'arbre syntaxique uniquement) :
    - Création de l'arbre et affichage sur la console.
    - Expressions fonctionnelles (arithmétiques et logiques).
    - Gestion des opérateurs unaires.
    - Gestion des boucles "loop" infinies.
    - Gestion des conditions IF/ELSE/ELSEIF au sein de l'arbre.
    - Gestion des boucles WHILE utilisant la notion de boucle LOOP et de condition IF (avec noeud BREAK).
    - Gestion des boucles FOR utilisant également la notion de boucle LOOP et de conditions IF.
    - Gestion des blocs de code.
    - Gestion des affectations et déclarations variables.
    - Exceptions verbeuses levées en cas de crash ("primaire attendue", "missing variable x", etc).
    
 - Analyseur sémantique
    - Table des symboles implémentée.
    - Scope des variables gérées.
    - Exceptions levées ("variable already declared in this scope", etc).
    
 - Génération de code :
    - Reservation de l'espace mémoire
    - opérations arithmétiques (+, -, *, /)
    - constantes
    - opérateurs unaires (+, -) intégrés
    - blocs
    - expressions
    - conditions (if, else) : imbriquée les unes dans les autres + if sans else + else if implémentés
    - comparaison ( =, !=, <, >, <=, >=)
    - Boucles :
       - While
       - For (attention, la déclaration de l'itérateur doit être faite a l'exterieur de l'entête du for!!)
       Exemple: var i; for(i = 0; i < 10; i++) {}
         Il s'agit d'un choix de conception et non d'une erreur !

- Ce qui n'est PAS implémenté :
    - DO WHILE
    - SWITCH/CASE

- Futur :
    - Programmation des tests unitaire en cours.
    - Optimisation déclaration des variables

# Utilisation
Pour lancer l'application, ouvrir un invite de commande et lancer : 
Compil.exe [nomFichierC]
Exemple :
Compil.exe c:\test.c