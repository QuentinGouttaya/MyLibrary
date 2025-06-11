# Branches à utiliser
- main
- SoloQuentin
- SoloSirine
- SoloOumou

# 📚 Projet API REST .NET - Gestion de Livres

Ce projet est une API REST construite avec **ASP.NET Core** permettant de gérer une collection de **livres**. L'API utilise **Entity Framework Core** pour interagir avec une base de données, à travers un `DbContext` nommé `MediaDb`.

---

## 📦 Technologies

- ASP.NET Core
- Entity Framework Core
- C#
- SQL Server / SQLite (selon configuration)

---

## 📁 Structure principale

- `Controllers/LivresController.cs` : Contrôleur principal exposant les endpoints pour manipuler les livres.
- `Models/Media.cs` : Modèle représentant un livre.
- `MediaDb.cs` : Contexte de base de données EF Core.

---

## 🔌 Endpoints de l'API

### ➕ Ajouter un livre

`POST /livres`

- Reçoit un objet JSON représentant un livre (`Media`)
- Valide les données et ajoute le livre à la base
- Réponse : `201 Created` avec les détails du livre ajouté

---

### 📚 Obtenir tous les livres

`GET /livres`

- Récupère tous les livres de la base de données
- Réponse : `200 OK` avec la liste complète

---

### 🔎 Filtrer les livres par auteur ou titre

`GET /livres/filter?start=xxx&end=yyy`

- Permet de filtrer les livres dont le **titre ou l’auteur commence** (`start`) ou **se termine** (`end`) par une chaîne donnée
- Les filtres sont optionnels et insensibles à la casse
- Réponse : `200 OK` avec les livres correspondants (titres et auteurs en minuscules)

---

### 📖 Obtenir un livre par ID

`GET /livres/{id}`

- Recherche un livre par son `id`
- Réponse :
  - `200 OK` avec les informations du livre
  - `404 Not Found` si l’ID n’existe pas

---

### ✏️ Mettre à jour un livre

`PUT /livres/{id}`

- Reçoit un objet `Media` avec les nouvelles valeurs
- Met à jour les informations du livre existant
- Réponse :
  - `200 OK` si la mise à jour est réussie
  - `404 Not Found` si le livre n'existe pas

---

### ❌ Supprimer un livre

`DELETE /livres/{id}`

- Supprime un livre de la base par son ID
- Réponse :
  - `200 OK` si supprimé
  - `404 Not Found` si le livre n'existe pas

---

## 🧪 Exemple de modèle `Media`

Voici un exemple d'objet JSON que l'on peut envoyer dans les requêtes `POST` ou `PUT` :

```json
{
  "id": 1,
  "title": "Le Petit Prince",
  "author": "Antoine de Saint-Exupéry"
}
