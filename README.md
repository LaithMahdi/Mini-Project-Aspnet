# School Management System

Système de gestion scolaire ASP.NET Core avec Razor Pages permettant la création et la gestion d'utilisateurs (étudiants et enseignants).

## 🚀 Fonctionnalités

- ✅ Création d'utilisateurs (étudiants et enseignants)
- ✅ Formulaire dynamique avec sections conditionnelles
- ✅ Validation des données côté client et serveur
- ✅ Interface utilisateur responsive avec Tailwind CSS
- ✅ Gestion des rôles et des informations spécifiques par profil
- ✅ Authentification et autorisations utilisateur
- ✅ Gestion des classes, sections et salles de classe
- ✅ Gestion des matières et sessions

## 📋 Prérequis

- .NET 10 SDK ou supérieur
- Visual Studio 2022 ou VS Code
- Git
- SQL Server (LocalDB ou Express)

## 🔧 Installation

### 1. Cloner le dépôt

```bash
git clone https://github.com/LaithMahdi/mini-project-aspnet.git
cd mini-project-aspnet
```

### 2. Restaurer les dépendances

```bash
dotnet restore
```

### 3. Configurer la base de données

```bash
dotnet ef database update
```

### 4. Lancer l'application

```bash
dotnet run
```

L'application sera accessible sur `https://localhost:5001`

## 📁 Structure du projet

```
school/
├── Controllers/
│   ├── UsersController.cs          # Gestion des utilisateurs
│   ├── TeachersController.cs       # Gestion des enseignants
│   ├── StudentsController.cs       # Gestion des étudiants
│   ├── ClassesController.cs        # Gestion des classes
│   ├── SubjectsController.cs       # Gestion des matières
│   ├── SectionsController.cs       # Gestion des sections
│   ├── RoomsController.cs          # Gestion des salles
│   ├── SessionsController.cs       # Gestion des sessions
│   └── AccountController.cs        # Authentification
├── Models/
│   ├── User.cs                     # Modèle utilisateur
│   ├── Teacher.cs                  # Modèle enseignant
│   ├── Student.cs                  # Modèle étudiant
│   ├── Classe.cs                   # Modèle classe
│   ├── Subject.cs                  # Modèle matière
│   ├── Section.cs                  # Modèle section
│   ├── Room.cs                     # Modèle salle
│   ├── Session.cs                  # Modèle session
│   ├── Enums.cs                    # Énumérations
│   ├── Junctionentities.cs         # Entités de jonction
│   └── ApplicationDbContext.cs     # Contexte EF Core
├── ViewModels/
│   ├── UserCreateViewModel.cs      # Modèle de vue utilisateur
│   └── LoginViewModel.cs           # Modèle de vue connexion
├── Views/
│   ├── Users/
│   │   ├── Create.cshtml           # Formulaire création utilisateur
│   │   ├── Index.cshtml            # Liste des utilisateurs
│   │   ├── Edit.cshtml             # Édition utilisateur
│   │   ├── Details.cshtml          # Détails utilisateur
│   │   └── Delete.cshtml           # Confirmation suppression
│   ├── Subjects/
│   │   └── Create.cshtml           # Création matière
│   ├── Account/
│   │   └── Login.cshtml            # Formulaire connexion
│   └── Shared/
│       ├── _Layout.cshtml          # Layout principal
│       └── _ViewStart.cshtml       # Démarrage vues
├── Migrations/
│   └── [Migration files]           # Migrations Entity Framework
├── Seeding/
│   └── AppSeeder.cs                # Données initiales
├── Styles/
│   └── tailwind.input.css          # Configuration Tailwind
├── wwwroot/
│   ├── css/                        # Fichiers CSS compilés
│   ├── js/                         # Fichiers JavaScript
│   └── images/                     # Images
├── Program.cs                      # Configuration de l'application
├── appsettings.json                # Paramètres de configuration
├── tailwind.config.js              # Configuration Tailwind CSS
├── package.json                    # Dépendances npm
└── README.md                       # Ce fichier
```

## 🐛 Correctifs récents

### Fix: Section 'Scripts' définie deux fois
- **Problème** : Exception `System.InvalidOperationException: Section 'Scripts' is already defined`
- **Cause** : Le contenu du fichier `Create.cshtml` était dupliqué
- **Solution** : Suppression du contenu dupliqué et de la section `@section Scripts` en doublon

## 📝 Utilisation

### Créer un nouvel utilisateur

1. Accéder à `/users/create`
2. Remplir les informations communes :
   - Nom complet
   - Email
   - Numéro de téléphone
   - Nom d'utilisateur
   - Mot de passe
3. Sélectionner le type d'utilisateur (Étudiant ou Enseignant)
4. Les champs spécifiques s'affichent automatiquement
5. Soumettre le formulaire

### Types d'utilisateurs

#### 👨‍🏫 Enseignant
- Spécialisation (ex: Mathématiques, Physique)
- Date d'embauche
- Salaire

#### 👨‍🎓 Étudiant
- Date de naissance
- Numéro d'identité (CIN)
- Numéro de téléphone secondaire (parent/tuteur)
- Date d'inscription
- Adresse

## 🛠️ Technologies utilisées

| Technologie | Version | Utilisation |
|-------------|---------|-------------|
| ASP.NET Core | 10 | Framework web |
| C# | 14.0 | Langage principal |
| Entity Framework Core | 10 | ORM base de données |
| Razor Pages | - | Architecture UI/Pages |
| MVC Controllers | - | Logique applicative |
| Tailwind CSS | - | Styling et design |
| JavaScript | ES6+ | Interactions côté client |
| SQL Server | LocalDB | Base de données |

## 📊 Modèles principaux

### UserCreateViewModel

```csharp
public class UserCreateViewModel
{
    // Informations communes
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Role { get; set; }
    public string Gender { get; set; }
    public bool IsActive { get; set; }
    
    // Champs enseignant
    public string Specialization { get; set; }
    public DateTime? HireDate { get; set; }
    public decimal? Salary { get; set; }
    
    // Champs étudiant
    public DateTime? DateOfBirth { get; set; }
    public string CinNumber { get; set; }
    public string SecondPhoneNumber { get; set; }
    public DateTime? EnrollmentDate { get; set; }
    public string Address { get; set; }
}
```

### Modèles principaux

- **User** : Classe de base pour les utilisateurs
- **Teacher** : Hérite de User, informations spécifiques enseignant
- **Student** : Hérite de User, informations spécifiques étudiant
- **Classe** : Représente une classe scolaire
- **Section** : Représente une section/division
- **Subject** : Représente une matière
- **Room** : Représente une salle de classe
- **Session** : Représente une session/période scolaire

## 🎨 Interface utilisateur

### Formulaire réactif
- Affichage/masquage automatique des sections selon le rôle sélectionné
- Validation en temps réel
- Messages d'erreur contextuels
- Design responsive (mobile, tablette, desktop)

### Palette de couleurs
- **Primaire** : Indigo (`#4F46E5`)
- **Enseignants** : Bleu (`#1E40AF`)
- **Étudiants** : Vert (`#15803D`)
- **Erreurs** : Rouge (`#DC2626`)
- **Neutres** : Slate (`#64748B`)

## 🚀 Déploiement

### Build Release

```bash
dotnet build -c Release
```

### Publication

```bash
dotnet publish -c Release -o ./publish
```

### Déploiement sur Azure

```bash
# Créer un profil de publication
dotnet publish -c Release --self-contained
```

## 📦 Dépendances principales

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="10.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Mvc" Version="10.0.0" />
```

## 🔐 Sécurité

- ✅ Validation CSRF activée (Anti-Forgery tokens)
- ✅ Hachage des mots de passe avec Identity
- ✅ Validation des données côté serveur
- ✅ Protection contre l'injection SQL via EF Core
- ✅ HTTPS forcé en production
- ✅ Authentification par formulaire
- ✅ Autorisation basée sur les rôles

## 📋 Checklist de développement

- [x] Formulaire de création utilisateur
- [x] Validation dynamique par rôle
- [x] Interface Tailwind CSS
- [x] Gestion des erreurs
- [x] Authentification utilisateur
- [x] Gestion des classes
- [x] Gestion des matières
- [x] Gestion des sections
- [ ] Tests unitaires
- [ ] Tests d'intégration
- [ ] Pagination de liste
- [ ] Édition d'utilisateur
- [ ] Suppression d'utilisateur

## 🐛 Signaler un bug

Créez une issue GitHub avec :
- Description du problème
- Étapes pour reproduire
- Comportement attendu vs réel
- Screenshots si applicable

## 📝 Contribuer

1. Fork le projet
2. Créer une branche (`git checkout -b feature/AmazingFeature`)
3. Commit vos changements (`git commit -m 'Add some AmazingFeature'`)
4. Push vers la branche (`git push origin feature/AmazingFeature`)
5. Ouvrir une Pull Request

## 📄 Licence

Ce projet est sous licence MIT. Voir le fichier `LICENSE` pour plus de détails.

## 👨‍💻 Auteur

**Laith Mahdi**
- GitHub : [@LaithMahdi](https://github.com/LaithMahdi)
- Projet : [mini-project-aspnet](https://github.com/LaithMahdi/mini-project-aspnet)

## 📞 Support

Pour les questions ou les problèmes :
- 📧 Ouvrir une issue sur [GitHub Issues](https://github.com/LaithMahdi/mini-project-aspnet/issues)
- 💬 Consulter la documentation du projet

## 📚 Ressources utiles

- [Documentation ASP.NET Core](https://docs.microsoft.com/aspnet/core)
- [Razor Pages Documentation](https://docs.microsoft.com/aspnet/core/razor-pages)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Tailwind CSS](https://tailwindcss.com)
- [ASP.NET Core Security](https://docs.microsoft.com/aspnet/core/security)

## 🔄 Flux de travail Git

```bash
# Voir l'état
git status

# Ajouter les fichiers
git add .

# Commit
git commit -m "feat: Description de la fonctionnalité"

# Push
git push origin main

# Pull les dernières modifications
git pull origin main
```

## 🔨 Commandes utiles

```bash
# Restaurer les dépendances
dotnet restore

# Build le projet
dotnet build

# Lancer en debug
dotnet run

# Migrations EF Core
dotnet ef migrations add NomMigration
dotnet ef database update
dotnet ef database drop

# Tests
dotnet test
```

---

**Dernière mise à jour** : 2024  
**Version** : 1.0.0  
**Statut** : En développement 🚧
