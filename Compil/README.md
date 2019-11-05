# Project
C# Compilateur project

# Auteurs
CAPODANO Thomas
HAMEL Ludovic

# Opérations fonctionnelles
 - Lecture du fichier de test (.c)
 - Analyseur lexical:
      - Découpage des tokens
 - Analyseur syntaxique :
    - Création de l'arbre.
 - Analyseur sémantique
 - Génération de code :
    - opérations arithmétiques (+, -, *, /)
    - constantes
    - opérateurs unaires (+, -)
    - blocs
    - expressions
    - conditions (if, else) : imbriquée les une dans les autres aussi + if sans else
    - comparaison ( =, !=, <, >, <=, >=)
 - Boucles :
    - While
    - For (attention, la déclaration de l'itérateur doit être faite a l'exterieur de l'entête du for!!)

# Utilisation
Pour lancer l'application, ouvrir un invite de commande et lancer : 
Compil.exe [nomFichierC]
Exemple :
Compil.exe c:\test.c